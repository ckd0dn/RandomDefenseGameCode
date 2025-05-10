using CWLib;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UnitInfoPopupUI : UIBase
{
    [SerializeField] private Image _unitImage;
    [SerializeField] private TextMeshProUGUI _unitNameText;
    [SerializeField] private TextMeshProUGUI _unitSpeciesText;
    [SerializeField] private TextMeshProUGUI _unitAttackDamageText;
    [SerializeField] private TextMeshProUGUI _unitAttackEnhanceDamageText;
    [SerializeField] private TextMeshProUGUI _unitAttackDelayText;
    [SerializeField] private TextMeshProUGUI _unitDescriptionText;

    public void SetInfo(Unit unit)
    {
        _unitImage.sprite = Managers.Resource.Load<Sprite>(unit.PrefabName + ".png");
        
        _unitNameText.text = Managers.Data.UnitDic[unit.PrefabName].Name; 
        _unitSpeciesText.text = Managers.Data.UnitDic[unit.PrefabName].GetSpeciesName(); 
        _unitAttackDamageText.text = Managers.Data.UnitDic[unit.PrefabName].Damage.ToString("F0"); 
        _unitAttackDelayText.text = Managers.Data.UnitDic[unit.PrefabName].AttackDelay.ToString(); 
        _unitDescriptionText.text = Managers.Data.UnitDic[unit.PrefabName].Description;
        _unitAttackEnhanceDamageText.text = "+" + unit.EnhanceDamage;
    }
} 
