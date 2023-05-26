using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image progress;

    private void OnEnable()
    {
        EventManager.SetPercent += percent => progress.DOFillAmount(percent, .1f);
    }

    private void OnDisable()
    {
        EventManager.SetPercent -= percent => progress.DOFillAmount(percent, .1f);
    }
}
