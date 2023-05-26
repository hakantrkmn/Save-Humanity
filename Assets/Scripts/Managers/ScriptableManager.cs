using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class ScriptableManager : MonoBehaviour
{
    [SerializeField] GameData gameData;
    [SerializeField] PlayerMovementSettings PlayerMovementSettings;
    [SerializeField] EffectorHolder effectorHolder;


    //-------------------------------------------------------------------
    void Awake()
    {
        SaveManager.LoadGameData(gameData);

        Scriptable.GameData = GetGameData;
        Scriptable.PlayerSettings = GetPlayerMovementSettings;
        Scriptable.EffectorHolder = GetEffectorHolder;

    }


    //-------------------------------------------------------------------
    GameData GetGameData() => gameData;
    EffectorHolder GetEffectorHolder() => effectorHolder;


    //-------------------------------------------------------------------
    PlayerMovementSettings GetPlayerMovementSettings() => PlayerMovementSettings;

}



public static class Scriptable
{
    public static Func<GameData> GameData;
    public static Func<PlayerMovementSettings> PlayerSettings;
    public static Func<EffectorHolder> EffectorHolder;

}