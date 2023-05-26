using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpEffector : BaseEffector
{
    public float jumpPower;
    
    
    public override void OnMouseDown()
    {
        EventManager.EffectorClicked(Directions.Down,EffectorTypes.Jump);
        base.OnMouseDown();

    }
}
