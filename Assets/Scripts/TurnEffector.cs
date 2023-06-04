using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnEffector : BaseEffector
{
   public Directions directionType;
   public Vector3 direction;
   public Transform arrowImage;

   public override void EffectorPlaced()
   {

         UpdateDirection();
      
   }

   public void UpdateDirection()
   {
      direction = transform.forward;
      arrowImage.forward = direction;
   }

   public override void OnMouseDown()
   {
      EventManager.EffectorClicked(directionType,EffectorTypes.Turn);
      base.OnMouseDown();

   }
}
