using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class HumanPoolManager : MonoBehaviour
{
    private ObjectPool<BaseHuman> _pool;
    public GameObject humanPrefab;

    private void Start()
    {
        SetPool();
    }

    void SetPool()
    {
        _pool = new ObjectPool<BaseHuman>(CreateHuman,
            OnHumanGet,
            OnHumanRelease,
            OnHumanDestroy,
            false, 50, 100);
    }


    private void OnEnable()
    {
        EventManager.GetHumanFromPool += GetHumanFromPool;
        EventManager.HumanHitTheEnd += HumanHitTheEnd;
    }

    private BaseHuman GetHumanFromPool()
    {
        return _pool.Get();
    }

    private void OnDisable()
    {
        EventManager.GetHumanFromPool -= GetHumanFromPool;
        EventManager.HumanHitTheEnd -= HumanHitTheEnd;
    }

    private void HumanHitTheEnd(BaseHuman obj)
    {
        _pool.Release(obj);
    }

    BaseHuman CreateHuman()
    {
        return Instantiate(humanPrefab).GetComponent<BaseHuman>();
    }

    void OnHumanGet(BaseHuman human)
    {
        human.gameObject.SetActive(true);
    }
    void OnHumanRelease(BaseHuman human)
    {
        human.gameObject.SetActive(false);
    }

    void OnHumanDestroy(BaseHuman human)
    {
        Destroy(human.gameObject);
    }
}