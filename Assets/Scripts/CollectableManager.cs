using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableManager : MonoBehaviour
{
    public List<BaseCollectable> collectables;
    private int collectableAmount;
    public float collectPercent;
    private void OnValidate()
    {
        collectables.Clear();
        foreach (var collectable in GetComponentsInChildren<BaseCollectable>())
        {
            collectables.Add(collectable);
        }

        collectableAmount = collectables.Count;
    }
    
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
        foreach (var collectable in collectables)
        {
            if (collectable==obj)
            {
                collectables.Remove(collectable);
                collectable.gameObject.SetActive(false);
                collectPercent = 1 - (((float)collectables.Count) / (float)collectableAmount);
                EventManager.SetPercent(collectPercent);
                return;
            }
        }
    }
}
