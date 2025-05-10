using System.Collections;
using System.Text;
using BackEnd;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackendManager
{
    public static UserData userData;
    public static BackendReturnObject bro;
    private string _gameDataRowInDate = string.Empty;
    
    public void Init()
    {
        bro = Backend.Initialize(); // 뒤끝 초기화

        // 뒤끝 초기화에 대한 응답값
        if(bro.IsSuccess()) {
            Debug.Log("초기화 성공 : " + bro); // 성공일 경우 statusCode 204 Success
        } else {
            Debug.LogError("초기화 실패 : " + bro); // 실패일 경우 statusCode 400대 에러 발생
        }
            
    }


    private void GetHash()
    {
        Backend.Utils.GetGoogleHash();
        
        //example
        string googlehash = Backend.Utils.GetGoogleHash();

        Debug.Log("구글 해시 키 : " + googlehash);
    }

    public void GuestLogin()
    {
        Debug.Log("게스트 로그인에 시도");
     
        BackendReturnObject bro = Backend.BMember.GuestLogin("게스트 로그인으로 로그인함");
        if(bro.IsSuccess())
        {
            Debug.Log("게스트 로그인에 성공했습니다");
            SceneManager.LoadScene("LobbyScene");;
            InitGameData();
        }
        else
        {
            Debug.Log("게스트 로그인에 실패했습니다.");
            Debug.Log(bro);
            var ui = Managers.UI.ShowPopup<MessagePopupUI>();
            ui.Show($"게스트 로그인에 실패했습니다.\n{bro}");

        }
    }

    public void InitGameData()
    {
        GetGameData();

        if (userData == null)
        {
            InsertGameData();
        }
        
    }

    public void InsertGameData()
    {
        if (userData == null)
        {
            userData = new UserData();
        }
        
        userData.Level = 0;
        userData.Gold = 0;
        userData.Diamond = 0;
        
        Param param = new Param();
        param.Add("Level", userData.Level);
        param.Add("Gold", userData.Gold);
        param.Add("Diamond", userData.Diamond);
        
        bro = Backend.GameData.Insert("USER_DATA", param);
        
        if (bro.IsSuccess())
        {
            Debug.Log("게임 정보 데이터 삽입에 성공했습니다. : " + bro);

            //삽입한 게임 정보의 고유값입니다.  
            var gameDataRowInDate = bro.GetInDate();
        }
        else
        {
            Debug.LogError("게임 정보 데이터 삽입에 실패했습니다. : " + bro);
        }
    }
    
    public void GetGameData()
    {
        Debug.Log("게임 정보 조회 함수를 호출합니다.");
        var bro = Backend.GameData.GetMyData("USER_DATA", new Where());
        if (bro.IsSuccess())
        {
            Debug.Log("게임 정보 조회에 성공했습니다. : " + bro);

            LitJson.JsonData gameDataJson = bro.FlattenRows(); // Json으로 리턴된 데이터를 받아옵니다.  

            // 받아온 데이터의 갯수가 0이라면 데이터가 존재하지 않는 것입니다.  
            if (gameDataJson.Count <= 0)
            {
                Debug.LogWarning("데이터가 존재하지 않습니다.");
            }
            else
            {
                _gameDataRowInDate = gameDataJson[0]["inDate"].ToString(); //불러온 게임 정보의 고유값입니다.  

                userData = new UserData();

                userData.Level = int.Parse(gameDataJson[0]["Level"].ToString());
                userData.Gold = int.Parse(gameDataJson[0]["Gold"].ToString());
                userData.Diamond = int.Parse(gameDataJson[0]["Diamond"].ToString());

                // foreach (string itemKey in gameDataJson[0]["inventory"].Keys)
                // {
                //     userData.inventory.Add(itemKey, int.Parse(gameDataJson[0]["inventory"][itemKey].ToString()));
                // }
                //
                // foreach (LitJson.JsonData equip in gameDataJson[0]["equipment"])
                // {
                //     userData.equipment.Add(equip.ToString());
                // }

                Debug.Log(userData.ToString());
            }
        }
        else
        {
            Debug.LogError("게임 정보 조회에 실패했습니다. : " + bro);
        }
    }
    
    public void UpdateGameData()
    {
        if (userData == null)
        {
            Debug.LogError("서버에서 다운받거나 새로 삽입한 데이터가 존재하지 않습니다. Insert 혹은 Get을 통해 데이터를 생성해주세요.");
            return;
        }

        Param param = new Param();
        param.Add("Level", userData.Level);
        param.Add("Gold", userData.Gold);
        param.Add("Diamond", userData.Diamond);
        
        if (string.IsNullOrEmpty(_gameDataRowInDate))
        {
            Debug.Log("내 제일 최신 게임 정보 데이터 수정을 요청합니다.");

            bro = Backend.GameData.Update("USER_DATA", new Where(), param);
        }
        else
        {
            Debug.Log($"{_gameDataRowInDate}의 게임 정보 데이터 수정을 요청합니다.");

            bro = Backend.GameData.UpdateV2("USER_DATA", _gameDataRowInDate, Backend.UserInDate, param);
        }

        if (bro.IsSuccess())
        {
            Debug.Log("게임 정보 데이터 수정에 성공했습니다. : " + bro);
        }
        else
        {
            Debug.LogError("게임 정보 데이터 수정에 실패했습니다. : " + bro);
        }
    }

    public void WithdrawAccount()
    {
        CoroutineRunner.Instance.StartCoroutine(WithdrawAndDelay());
    }

    private IEnumerator WithdrawAndDelay()
    {
        Backend.BMember.WithdrawAccount();
    
        var ui = Managers.UI.ShowPopup<MessagePopupUI>();
        ui.Show("계정이 탈퇴되었습니다.");

        yield return new WaitForSeconds(2f); 

        SceneManager.LoadScene("StartScene");
    }
}

// Data
public class UserData
{
    public int Level;
    public int Gold;
    public int Diamond;
    // public string info = string.Empty;
    // public Dictionary<string, int> inventory = new Dictionary<string, int>();
    // public List<string> equipment = new List<string>();

    // 데이터를 디버깅하기 위한 함수입니다.(Debug.Log(UserData);)
    public override string ToString()
    {
        StringBuilder result = new StringBuilder();
        result.AppendLine($"Level : {Level}");
        result.AppendLine($"Gold : {Gold}");
        result.AppendLine($"Diamond : {Diamond}");

        // result.AppendLine($"inventory");
        // foreach (var itemKey in inventory.Keys)
        // {
        //     result.AppendLine($"| {itemKey} : {inventory[itemKey]}개");
        // }
        //
        // result.AppendLine($"equipment");
        // foreach (var equip in equipment)
        // {
        //     result.AppendLine($"| {equip}");
        // }

        return result.ToString();
    }
}
