using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;

public class UnitManager
{
    public void SpawnUnit(int enhanceLevel)
    {
        Tile emptyTile = Managers.Object.Tiles.FirstOrDefault(t => t.Unit == null);

        if (emptyTile == null)
        {
            Debug.Log("더 이상 유닛을 소환할 수 없습니다.");
            return;
        }
        
        // have money

        if (GameScene.Instance.Money < GameScene.Instance.SpawnUnitMoney)
        {
            Debug.Log("돈이 부족합니다.");
            return;
        }
        
        GameScene.Instance.Money -= GameScene.Instance.SpawnUnitMoney;
        GameScene.Instance.SpawnUnitMoney += 1;

        string unitkey = PickRandomUnit(enhanceLevel);
        
        string key = unitkey + ".prefab" ;
        
        Unit unit = Managers.Object.Spawn<Unit>(key);
        unit.transform.position = emptyTile.transform.position;
        emptyTile.Unit = unit;
        unit.Tile = emptyTile;
    }

    public void SpawnGradeUnit(UnitGrade grade)
    {
        Tile emptyTile = Managers.Object.Tiles.FirstOrDefault(t => t.Unit == null);

        if (emptyTile == null)
        {
            Debug.Log("더 이상 유닛을 소환할 수 없습니다.");
            return;
        }
        
        string unitkey =  GetGradeUnit(grade);
        
        string key = unitkey + ".prefab" ;
        
        Unit unit = Managers.Object.Spawn<Unit>(key);
        unit.transform.position = emptyTile.transform.position;
        emptyTile.Unit = unit;
        unit.Tile = emptyTile;
    }
    
    private string PickRandomUnit(int enhanceLevel)
    {
        // 1. 현재 강화 레벨에 해당하는 등급 확률 리스트를 가져오기
        var gradeProbabilityDatas = Managers.Data.GradeProbList.FindAll(x => x.EnhanceLevel == enhanceLevel);

        // 2. 확률에 따라 랜덤하게 등급 뽑기
        UnitGrade selectedGrade = PickRandomGrade(gradeProbabilityDatas);

        return GetGradeUnit(selectedGrade);
    }

    private string GetGradeUnit(UnitGrade grade)
    {
        // 3. 선택된 등급에 해당하는 유닛 목록 만들기
        var unitList = new List<Datas.UnitData>();
        foreach (var unit in Managers.Data.UnitDic.Values)
        {
            if (unit.Grade == grade)
            {
                unitList.Add(unit);
            }
        }

        // 4. 유닛 중 랜덤으로 하나 선택
        int randomIndex = Random.Range(0, unitList.Count);
        return unitList[randomIndex].PrefabName;
    }

    private UnitGrade PickRandomGrade(List<Datas.GradeProbabilityData> candidates)
    {
        float randomPoint = Random.Range(0f, 100f);

        float cumulative = 0f;
        foreach (var c in candidates)
        {
            cumulative += c.Probability;
            if (randomPoint <= cumulative)
            {
                return c.Grade;
            }
               
        }
        
     
        
        return candidates[^1].Grade;
    }

    public void MergeUnit(Unit unit)
    {
        //  Despawn 3Same Unit 
        var matchingUnits = Managers.Object.Units
            .Where(targeUnit => targeUnit.Name == unit.Name)
            .Take(3)
            .ToList();
        
        if (matchingUnits.Count == 3)
        {
            foreach (var matchingUnit in matchingUnits)
            {
                Managers.Object.Despawn(matchingUnit);
            }
        }
        
        // Next Grade Unit  

        UnitGrade nextGrade = (UnitGrade)((int)unit.Grade + 1);
        
        SpawnGradeUnit(nextGrade);
    }

   
}
