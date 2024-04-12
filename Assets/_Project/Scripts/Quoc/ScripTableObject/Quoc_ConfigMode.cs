using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Config Mode", menuName = "Config/Config Mode", order = 3)]
public class Quoc_ConfigMode : ScriptableObject
{
    public Quoc_ConfigModeData[] data;
    private static Quoc_ConfigMode Instance;

    public static Quoc_ConfigModeData ConfigModeData(int index)
    {
        Instance = Resources.Load<Quoc_ConfigMode>("Configs/Config Mode");
        Quoc_ConfigModeData result = null;
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
    public static Quoc_ConfigWeekData ConfigWeekData(int indexMode, int indexWeek)
    {
        Instance = Resources.Load<Quoc_ConfigMode>("Configs/Config Mode");
        Quoc_ConfigWeekData result = null;
        if (Instance.data.Length > indexMode && Instance.data[indexMode].configWeekDatas.Count > indexWeek)
        {
            result = Instance.data[indexMode].configWeekDatas[indexWeek];
        }
        else
        {
            result = Instance.data[0].configWeekDatas[0];
        }
        return result;
    }

    public static Quoc_ConfigSongData ConfigSongData(int indexMode, int indexWeek, int indexSong)
    {
        Instance = Resources.Load<Quoc_ConfigMode>("Configs/Config Mode");
        Quoc_ConfigSongData result = null;
        if (Instance.data.Length > indexMode && Instance.data[indexMode].configWeekDatas.Count > indexWeek
            && Instance.data[indexMode].configWeekDatas[indexWeek].configSongDatas.Count > indexSong)
        {
            result = Instance.data[indexMode].configWeekDatas[indexWeek].configSongDatas[indexSong];
        }
        else
        {
            result = Instance.data[0].configWeekDatas[0].configSongDatas[0];
        }

        return result;
    }
}

[Serializable]
public class Quoc_ConfigSongData
{
    public string nameSong;
    public string nameJson;
}

[Serializable]
public class Quoc_ConfigWeekData
{
    public string name;
    public List<Quoc_ConfigSongData> configSongDatas = new List<Quoc_ConfigSongData>();

}

[Serializable]
public class Quoc_ConfigModeData
{
    public string nameMode;
    public string nameAssetBundle;
    public List<Quoc_ConfigWeekData> configWeekDatas = new List<Quoc_ConfigWeekData>();
}