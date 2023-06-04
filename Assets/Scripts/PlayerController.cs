using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject currentEffector;
    private int layer_mask;


    private void OnEnable()
    {
        EventManager.PickerItemAmountIsZero += obj =>
        {
            if (obj == currentEffector) currentEffector = null;
        };
        EventManager.PickerItemChoosed += obj => currentEffector = obj;
    }

    private void OnDisable()
    {
        EventManager.PickerItemAmountIsZero -= obj =>
        {
            if (obj == currentEffector) currentEffector = null;
        };
        EventManager.PickerItemChoosed -= obj => currentEffector = obj;
    }

    private void Start()
    {
        layer_mask = LayerMask.GetMask("Ground","Effector");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && currentEffector != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer_mask))
            {
                if (hit.transform.CompareTag("Ground"))
                {
                    
                var effector = Instantiate(currentEffector, hit.point, currentEffector.transform.rotation);
                effector.transform.rotation =Quaternion.Euler(effector.transform.rotation.eulerAngles.x,
                    effector.transform.rotation.eulerAngles.y + Camera.main.transform.rotation.eulerAngles.y,
                    effector.transform.rotation.eulerAngles.z);
                effector.GetComponent<BaseEffector>().EffectorPlaced();
                EventManager.EffectorPlaced(currentEffector);
                }

            }
        }
    }
}