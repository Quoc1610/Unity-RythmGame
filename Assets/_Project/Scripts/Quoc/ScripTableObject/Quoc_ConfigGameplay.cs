using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName ="Config Gameplay", menuName ="Config/Config Gameplay",order =4)]
public class Quoc_ConfigGameplay : ScriptableObject
{
    public Quoc_GameplayModeData[] data;
    private static Quoc_ConfigGameplay Instance;
    public static Quoc_GameplayModeData GameplayModeData(int index)
    {
        Instance= Resources.Load<Quoc_ConfigGameplay>("Configs/Config Gameplay");
        Quoc_GameplayModeData result = null;

        if (Instance.data.Length > index)
        {
            result = Instance.data[index];
        }

        if (result == null)
        {
            result = Instance.data[0];
        }

        return result;
    }

    public static Quoc_GameplayWeekData GameplayWeekData(int indexMode, int indexWeek)
    {
        Instance = Resources.Load<Quoc_ConfigGameplay>("Configs/Config Gameplay");

        Quoc_GameplayWeekData result = null;

        if (Instance.data.Length > indexMode && Instance.data[indexMode].gameplayWeekDatas.Count>indexWeek)
        {
            result = Instance.data[indexMode].gameplayWeekDatas[indexWeek];
        }

        if (result == null)
        {
            result = Instance.data[0].gameplayWeekDatas[0];
        }

        return result;
    }
    public static Quoc_GameplaySongData ConfigSongData(int indexMode, int indexWeek,int indexSong)
    {
        Instance = Resources.Load<Quoc_ConfigGameplay>("Configs/Config Gameplay");

        Quoc_GameplaySongData result = null;

        if (Instance.data.Length > indexMode && Instance.data[indexMode].gameplayWeekDatas.Count > indexWeek
            &&Instance.data[indexMode].gameplayWeekDatas[indexWeek].gameplaySongDatas.Count>indexSong)
        {
            result = Instance.data[indexMode].gameplayWeekDatas[indexWeek].gameplaySongDatas[indexSong];
        }

        /*if (result == null)
        {
            result = Instance.data[0].gameplayWeekDatas[0].gameplaySongDatas[0];
        }*/

        return result;
    }

    public static int GetModeLength()
    {
        Instance = Resources.Load<Quoc_ConfigGameplay>("Configs/Config Gameplay");
        return Instance.data.Length;
    }

    public static int GetWeekLength(int indexMode)
    {
        Instance = Resources.Load<Quoc_ConfigGameplay>("Configs/Config Gameplay");
        return Instance.data[indexMode].gameplayWeekDatas.Count;
    }

    public static int GetSongLength(int indexMode,int indexWeek)
    {
        Instance = Resources.Load<Quoc_ConfigGameplay>("Configs/Config Gameplay");
        return Instance.data[indexMode].gameplayWeekDatas[indexWeek].gameplaySongDatas.Count;
    }

    public static int GetAllSongInMode(int indexMode)
    {
        int countSong = 0;
        for(int i = 0; i < GetWeekLength(indexMode); i++)
        {
            countSong += GetSongLength(indexMode, i);

        }
        return countSong;
    }
}

[Serializable]
public class Quoc_GameplayModeData
{
    public string nameMode;
    public List<Quoc_GameplayWeekData> gameplayWeekDatas = new List<Quoc_GameplayWeekData>();
}

[Serializable]
public class Quoc_GameplayWeekData
{
    public string nameWeek;
    public List<Quoc_GameplaySongData> gameplaySongDatas = new List<Quoc_GameplaySongData>();
}

[Serializable]
public class Quoc_GameplaySongData
{
    public string nameSong;
    public Sprite spriteBG;
    public Sprite spriteSubBG;
    public RuntimeAnimatorController enemyAnimator;
    public Sprite spriteIcon;
    public Sprite spriteIconLose;
    public Sprite spriteCharacter;
    public int price = 200;
    public Vector3 localScaleBG = Vector3.one;
    public Vector3 localPositionBG = Vector3.zero;
    public Vector3 localScaleSubBG = Vector3.one;
    public Vector3 localPositionSubBG = Vector3.zero;
}
