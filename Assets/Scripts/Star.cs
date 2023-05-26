using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : BaseCollectable
{
    private void OnEnable()
    {
        EventManager.CollectableHit+= CollectableHit;
    }

    private void OnDisable()
    {
        EventManager.CollectableHit -= CollectableHit;
    }

    private void CollectableHit(BaseCollectable obj)
    {
        
    }
}
