using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PickerItem : MonoBehaviour , IPointerClickHandler
{
   public EffectorTypes type;
   [ShowIf("type", EffectorTypes.Turn)] public Directions direction;
   public TextMeshProUGUI amountText;
   private int amount;
   public GameObject spawnPrefab;

   private void OnEnable()
   {
      EventManager.EffectorPlaced += EffectorPlaced;
   }

   private void OnDisable()
   {
      EventManager.EffectorPlaced -= EffectorPlaced;
   }

   public void ReOpenItem()
   {
      amount++;
      amountText.text = amount.ToString();
   }

   private void EffectorPlaced(GameObject obj)
   {
      if (spawnPrefab==obj)
      {
         amount--;
         amountText.text = amount.ToString();
         if (amount==0)
         {
            EventManager.PickerItemAmountIsZero(spawnPrefab);
            gameObject.SetActive(false);
         }
      }
   }

   public void SetItem(int maxAmount,GameObject prefab)
   {
      spawnPrefab = prefab;
      amount = maxAmount;
      amountText.text = amount.ToString();
   }

   public void OnPointerClick(PointerEventData eventData)
   {
      EventManager.PickerItemChoosed(spawnPrefab);
   }
}
