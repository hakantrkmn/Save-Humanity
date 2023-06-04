using System;
using UnityEngine;


public static class EventManager
{
    
    

    #region InputSystem
    public static Func<Vector2> GetInput;
    public static Func<Vector2> GetInputDelta;
    public static Action InputStarted;
    public static Action InputEnded;
    public static Func<bool> IsTouching;
    public static Func<bool> IsPointerOverUI;
    #endregion

    public static Action<BaseHuman> HumanHitTheEnd;
    public static Action<GameObject> PickerItemChoosed;
    public static Action<GameObject> EffectorPlaced;
    public static Action<GameObject> PickerItemAmountIsZero;
    public static Action<Directions,EffectorTypes> EffectorClicked;
    public static Action<BaseCollectable> CollectableHit;
    public static Action<float> SetPercent;

    public static Func<BaseHuman> GetHumanFromPool;



}