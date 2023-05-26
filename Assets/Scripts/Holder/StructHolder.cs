
using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public struct PickerElement
{
    public EffectorTypes type;
    public int amount;
    [ShowIf("type", EffectorTypes.Turn)] public Directions direction;
}


[Serializable]
public struct Effector
{
    public EffectorTypes type;
    public GameObject pickerPrefab;
    public GameObject scenePrefab;
    [ShowIf("type", EffectorTypes.Turn)] public Directions direction;
}