using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickerController : MonoBehaviour
{
    public GridLayoutGroup gridLayoutGroup;
    public RectTransform rectTransform;
    public List<PickerElement> elements;
    public List<PickerItem> pickers;
    private void Start()
    {
        var cellSize = new Vector3(rectTransform.rect.height * .8f, rectTransform.rect.height * .8f, 0);
        gridLayoutGroup.cellSize = cellSize;
        CreatePickerItem();
    }


    private void OnEnable()
    {
        EventManager.EffectorClicked+= EffectorClicked;
    }

    private void EffectorClicked(Directions arg1, EffectorTypes arg2)
    {
        foreach (var picker in pickers)
        {
            if (picker.type==arg2)
            {
                if (arg2==EffectorTypes.Turn)
                {
                    if (arg1==picker.direction)
                    {
                        picker.gameObject.SetActive(true);
                        picker.ReOpenItem();
                    }
                }
                else
                {
                    picker.gameObject.SetActive(true);
                    picker.ReOpenItem();
                }
            }
        }
    }

    private void OnDisable()
    {
        EventManager.EffectorClicked-= EffectorClicked;
    }

   


    void CreatePickerItem()
    {
        var effectorHolder = Scriptable.EffectorHolder();
        foreach (var element in elements)
        {
            foreach (var effector in effectorHolder.effectors)
            {
                if (element.type == EffectorTypes.Turn)
                {
                    if (element.type== effector.type && element.direction== effector.direction)
                    {
                        var item = Instantiate(effector.pickerPrefab);
                        item.transform.parent = gridLayoutGroup.transform;
                        item.GetComponent<PickerItem>().SetItem(element.amount,effector.scenePrefab);
                        pickers.Add(item.GetComponent<PickerItem>());
                    }
                }
                else if (element.type == EffectorTypes.Jump)
                {
                    if (element.type== effector.type )
                    {
                        var item = Instantiate(effector.pickerPrefab);
                        item.transform.parent = gridLayoutGroup.transform;
                        item.GetComponent<PickerItem>().SetItem(element.amount,effector.scenePrefab);
                        pickers.Add(item.GetComponent<PickerItem>());
                    }
                }
            }
        }
    }
}
