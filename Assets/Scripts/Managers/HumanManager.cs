using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class HumanManager : MonoBehaviour
{
    public Transform spawnPoint;
    ObjectPool<BaseHuman> _pool;
    public GameObject humanPrefab;
    public Transform humanParent;

    public float spawnTimeGap;
    private float _timer;
    private void Start()
    {
        SetPool();
    }

    void SetPool()
    {
        _pool = new ObjectPool<BaseHuman>(() => CreateHuman(),
            human => OnHumanGet(human),
            human => OnHumanRelease(human), 
            human => OnHumanDestroy(human),
            false,50,100);

    }
    public void Spawn()
    {
        var temp = _pool.Get();
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


    private void OnEnable()
    {
        EventManager.HumanHitTheEnd += HumanHitTheEnd;
    }

    private void OnDisable()
    {
        EventManager.HumanHitTheEnd -= HumanHitTheEnd;
    }

    private void HumanHitTheEnd(BaseHuman obj)
    {
        _pool.Release(obj);
    }

    public BaseHuman CreateHuman()
    { 
        return Instantiate(humanPrefab).GetComponent<BaseHuman>();
    }

    public void OnHumanGet(BaseHuman human)
    {
         human.gameObject.SetActive(true);
    }
    public void OnHumanRelease(BaseHuman human)
    {
        human.gameObject.SetActive(false);
    }
    public void OnHumanDestroy(BaseHuman human)
    {
        Destroy(human.gameObject); 
    }
    
}
