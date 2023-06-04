using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class HumanManager : MonoBehaviour
{
    public Transform humanParent;
    
    public float spawnTimeGap;
    private float _timer;
    public void Spawn()
    {
        var temp = EventManager.GetHumanFromPool();
        temp.transform.parent = humanParent;
        temp.transform.localPosition = Vector3.zero + new Vector3(Random.Range(-.5f,.5f),0,0);
        temp.SetHuman();
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer>spawnTimeGap)
        {
            _timer = 0;
            Spawn();
        }
    }

   
    
}
