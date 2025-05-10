using System;
using TMPro;
using UnityEngine;

public class ResourceUI : MonoBehaviour
{
   [SerializeField] public TextMeshProUGUI MoneyText; 
   [SerializeField] public TextMeshProUGUI GemText;

   private void Start()
   {
       UpdateResourceUI();
       GameScene.Instance.OnResourceChangedCallback += UpdateResourceUI;
   }

   public void UpdateResourceUI()
   {
      MoneyText.text = GameScene.Instance.Money.ToString();
      GemText.text = GameScene.Instance.Gem.ToString();
   }
}
