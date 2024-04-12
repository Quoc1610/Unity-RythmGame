using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using Quoc;
using System;

namespace Quoc_Core
{
    public class SaveManager : SingletonMono<SaveManager>
    {

        private float gameSaveInterval = 3f;
        private string gameSaveFileName = "/game-save.json.";

        private float timeSinceSave;
        private GameSave gameSave;

        private void Awake()
        {
            DontDestroyOnLoad(this);

        }

        private void Update()
        {
            if(Quoc_GameManager.Instance&& gameSave != null)
            {
                timeSinceSave += Time.deltaTime;
                if (timeSinceSave >= gameSaveInterval)
                {
                    timeSinceSave = 0;
                    //savegame
                    SaveGame();

                }
            }
        }

        public GameSave Init()
        {
            try
            {
                if (gameSave == null)
                {
                    string gameSavePath = GetGameSavePath();
                    Debug.Log(gameSavePath);
                    if (File.Exists(gameSavePath))
                    {
                        Debug.Log("Loading game save: " + gameSavePath);
                        var s = FileHelper.LoadFileWithPassword(gameSavePath, "", true);
                        try {
                            gameSave = JsonConvert.DeserializeObject<GameSave>(s);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError("Parse Game Save error:" + e.Message);
                        }
                   }
                    else
                    {
                        Debug.Log("<color=blue>Game save not found, starting a new game </color>");
                        if (gameSave == null)
                        {
                           gameSave = new GameSave();
                       }

                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to load saved game due to:" + e.Message);
            }
            return gameSave;
        }

        private string GetGameSavePath()
        {
            //"D:\Unity\RhythmGame\Assets\Resources\game-save.json"
           return "D:\\Unity\\RhythmGame\\Assets\\Resources\\game-save.json";
           //return Path.Combine( "E:\\FileSave",gameSaveFileName);
        }

        public GameSave LoadGame()
        {
           
            if (gameSave == null)
            {
                Init();
            }

            gameSave.Init(Application.version);
            Debug.Log("Load Game");
            return gameSave;
        }
        public void SaveGame()
        {
           
            string gameSavePath = GetGameSavePath();
            Debug.Log("Save Game "+gameSavePath);
            string content = JsonConvert.SerializeObject(gameSave, Formatting.Indented);
            File.WriteAllText(gameSavePath, content);
        }
       
        private void OnApplicationQuit()

        {
                gameSave.isFirstOpen = true;
        }
    }
}
