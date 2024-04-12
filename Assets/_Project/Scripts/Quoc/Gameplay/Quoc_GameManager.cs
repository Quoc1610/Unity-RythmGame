using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Quoc_Core;
using Newtonsoft.Json;
using UnityEngine.Serialization;
using Quoc;

public class Quoc_GameManager : SingletonMono<Quoc_GameManager>
{
    public GameSave GameSave;
    public Quoc_UIGameplay uiGameplay;

    [Header("-------------------------Transform and Game Object-------------------")]

    [SerializeField]
    private List<GameObject> lsPrefabArrows = new List<GameObject>();

    [SerializeField]
    private List<Transform> lsContainSpawnArrow = new List<Transform>();

    [SerializeField]
    private List<Transform> lsContainSpawnEnemyArrow = new List<Transform>();
    
    [SerializeField]
    private List<Transform> lsTransTargetArrows = new List<Transform>();

    [SerializeField]
    private List<Transform> lsPositionSpawnArrows = new List<Transform>();

    public GameObject goGameContent;


    [Header("------------------------------------Animator-------------------------")]
    [SerializeField] private Quoc_CharacterDataBinding boyDataBinding;
    [SerializeField] private Quoc_CharacterDataBinding girlDataBinding;
    [SerializeField] private Quoc_CharacterDataBinding bossDatabinding;
    private Quoc_CharacterDataBinding enemyDataBinding;

    [Header("--------------------------------------Data-------------------------------")]
    private List<Quoc_ArrowDataItem> lsArrowDataItems = new List<Quoc_ArrowDataItem>();

    [Header("-------------------------------------Variable--------------------------------")]
    private float prevTimeArrow = 0;
    private float distanceMoveArrow = 0;
    private int curIndexArrow = 0;

    private float timeMoveArrow = 0;

    [Range(1, 3)]
    [SerializeField]
    private float defaultTimeMoveArrow = 1.8f;

    

    public GameState gameState;
    private float timerSong;
    public float TimerSong
    {
        get
        {
            return timerSong;
        }
        set
        {
            timerSong = value;
            if (uiGameplay != null)
            {
                uiGameplay.UpdateTimerText(timerSong);
            }
        }
    }
    private float deltaTime;


    public string nameSong;

    public int miss;
    public int Miss
    {
        get
        {
            return miss;
        }
        set
        {
            miss = value;
            if (uiGameplay != null)
            {
                uiGameplay.UpdataMissText(miss);
            }
        }
    }

    public int score;
    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            if (uiGameplay != null)
            {
                uiGameplay.UpdateScoreText(score);
            }
        }
    }

    private int indexSongofWeek;
    private int indexMode;
    private int indexWeek;

    private Difficult difficult;

    private Quoc_ConfigWeekData configWeekData;
    private Quoc_ConfigSongData configSongData;
    private Quoc_GameplaySongData gameplaySongData;


    public List<Quoc_TargetArrow> lsTargetArrows = new List<Quoc_TargetArrow>();
    private void Awake()
    {
        Input.multiTouchEnabled = true;
        goGameContent.SetActive(false);
        GameSave = SaveManager.Instance.LoadGame();
        UIManager.Instance.InitUI(() =>
        {
            UIManager.Instance.ShowUI(UIIndex.UILoading);
        });
    }
    private IEnumerator Start()
    {

        //get list poss spawn arrow
        lsPositionSpawnArrows = uiGameplay.GetListTransformSpawnArrow();

        //get list target arrow
        lsTransTargetArrows = uiGameplay.GetListTargetArrow();

        yield return new WaitForSeconds(0.1f);
       // SetupGameplay(0,1,0,Difficult.Easy);

    }

    public void SetupGameplay(int indexMode, int indexWeek,int indexSong,Difficult difficult)
    {
        this.indexMode = indexMode;
        this.indexWeek = indexWeek;
        this.indexSongofWeek = indexSong;
        this.difficult = difficult;

        configWeekData = Quoc_ConfigMode.ConfigWeekData(indexMode, indexWeek);
        configSongData = Quoc_ConfigMode.ConfigSongData(indexMode, indexWeek, indexSongofWeek);

        gameState = GameState.None;

        goGameContent.SetActive(true);

        GetSongGameplay(indexMode,configSongData.nameJson);
        float lengthSong = Quoc_SoundManager.Instance.GetLengthBGM();
        Quoc_SoundManager.Instance.PlaySoundBGM();
        
        SetUpGamePlayUI(lengthSong);    
        GameAnalyticsManager.Instance.PlayStart(configSongData.nameSong);
    }
    public void SetUpGamePlayUI(float lengthSong)
    {
        gameState = GameState.None;

        prevTimeArrow = 0;
        distanceMoveArrow = uiGameplay.GetDistanceMoveArrow();
        timeMoveArrow = defaultTimeMoveArrow * 1;
       

        //get data json
        Quoc_RootItem rootItem =
            JsonConvert.DeserializeObject<Quoc_RootItem>(Resources.Load<TextAsset>("Jsons/"+ configSongData.nameJson +"-easy").text);
        Debug.Log("json: " + rootItem.ToString());
        Quoc_SongItem songItem = rootItem.song;

        lsArrowDataItems.Clear();

        for(int i = 0; i < songItem.notes.Count; i++)
        {
            for (int j = 0; j < songItem.notes[i].sectionNotes.Count; j++)
            {
                Quoc_ArrowDataItem arrowDataItem = new Quoc_ArrowDataItem(songItem.notes[i].sectionNotes[j][0],
                    (int)(songItem.notes[i].sectionNotes[j][1] % 4)
                    , songItem.notes[i].sectionNotes[j][2]
                    , songItem.notes[i].mustHitSection);
                lsArrowDataItems.Add(arrowDataItem);                
            }
        }

        lsArrowDataItems.Sort(SortByTimeAppear);

        TimerSong = lengthSong;
        deltaTime = timeMoveArrow -0.1f;

        Quoc_SoundManager.Instance.StopSoundFX(SoundFxIndex.SoundMenu);
        UIManager.Instance.ShowUI(UIIndex.UIGameplay, new GamePlayParam()
        {
            difficult = difficult,
            maxValueSlider = 50,
            nameSong = configSongData.nameSong
        });
        gameplaySongData = Quoc_ConfigGameplay.ConfigSongData(indexMode, indexWeek, indexSongofWeek);
        uiGameplay.SetSpriteIconboss(gameplaySongData.spriteIconLose, gameplaySongData.spriteIcon);
        Miss = 0;
        Score = 0;
        SetupCharacter();

        gameState = GameState.Playing;
    }

    public void SetupCharacter()
    {
        if (configSongData.nameJson == "tutorial")
        {
            enemyDataBinding = girlDataBinding;
            bossDatabinding.gameObject.SetActive(false);
        }
        else
        {
            bossDatabinding.gameObject.SetActive(true);
            girlDataBinding.SetAnimationCharacter(0);
            enemyDataBinding = bossDatabinding;
        }
    }
    private void Update()
    {
        if(gameState == GameState.Playing &&timerSong >= 0)
        {
            ShowTimerSong();
            if (name == "tutorial")
            {
                LoadNotenew(Quoc_SoundManager.Instance.GetCurrentTimeSoundBGM() + deltaTime);

            }
            else
            {
                CalculateCreateArrow(Quoc_SoundManager.Instance.GetCurrentTimeSoundBGM() + deltaTime);
            }
        }
    }

    private void ShowTimerSong()
    {
        TimerSong -= Time.deltaTime;
        if (TimerSong <= 0)
        {
            //Show timer text
            uiGameplay.UpdateTimerText(0);

            //chjeck win lose
            if (uiGameplay.CheckGameWin())
            {
                ShowGameWin();
            }
            else
            {
                goGameContent.SetActive(false);
                ShowGameLose();
            }
        }
    }
    private int SortByTimeAppear(Quoc_ArrowDataItem obj1, Quoc_ArrowDataItem obj2)
    {
        return obj1.timeAppear.CompareTo(obj2.timeAppear);
    }

    public void LoadNotenew(float time)
    {
        if (curIndexArrow == lsArrowDataItems.Count - 1)
        {

            //if (lsArrowDataItems[curIndexArrow-1].timeAppear>time*1000)
           // {
            //    return;
           // }
           // else
            {
                if (((lsArrowDataItems[curIndexArrow].timeAppear / 1000) - time) < -0.001f &&
                    ((lsArrowDataItems[curIndexArrow].timeAppear / 1000) - time) >= -0.15f)
                {
                    Debug.Log("arrow: " + curIndexArrow + " " + lsArrowDataItems.Count + " " + lsArrowDataItems[curIndexArrow].timeAppear);

                    //create arrow
                    CreateArrow();
                    return;
                }
            }
        }
        else
        {
            if (curIndexArrow < lsArrowDataItems.Count - 1)
            {
                if (lsArrowDataItems[curIndexArrow].timeAppear == 0 || lsArrowDataItems[curIndexArrow].timeAppear < 1000)
                {
                    //create arrow
                    CreateArrow();
                    return;
                }
                else
                {
                    if (lsArrowDataItems[curIndexArrow].timeAppear > time * 1000)
                    {
                        return;
                    }
                    else
                    {
                        if (lsArrowDataItems[curIndexArrow + 1].timeAppear > time * 1000)
                        {
                            //create arrow
                            CreateArrow();
                        }
                    }
                }
            }
            
        }
    }

    private void CalculateCreateArrow(float time)
    {
        if(curIndexArrow>= lsArrowDataItems.Count)
        {
            return;
        }
        if((lsArrowDataItems[curIndexArrow].timeAppear/1000)<time&& (time - prevTimeArrow > 0.1f))
        {
            Debug.Log("arrow: " + curIndexArrow + " " + lsArrowDataItems[curIndexArrow].timeAppear + " " + lsArrowDataItems.Count);
            CreateArrow();
            prevTimeArrow = time;
        }
    }
    private void CreateArrow()
    {
        if(lsArrowDataItems[curIndexArrow] != null)
        {
            int indexArrowClone = lsArrowDataItems[curIndexArrow].indexArrow;
            int sumArrow = lsArrowDataItems.Count;
            //Debug.Log("dataa: "+ curIndexArrow + " " + lsArrowDataItems[curIndexArrow].indexArrow + " " + lsArrowDataItems[curIndexArrow].timeAppear);
            if (lsArrowDataItems[curIndexArrow].mustHit)
            {
                //create arrow from list
                GameObject goArrow= Instantiate(lsPrefabArrows[indexArrowClone],lsContainSpawnArrow[indexArrowClone]);
                goArrow.transform.localPosition = lsPositionSpawnArrows[indexArrowClone].position;
                Quoc_Arrow arrowMove = goArrow.GetComponent<Quoc_Arrow>();
                //setup arrow
                arrowMove.SetupArrow(timeMoveArrow,lsArrowDataItems[curIndexArrow].timeTail/1000,lsArrowDataItems[curIndexArrow].indexArrow, 
                    lsArrowDataItems[curIndexArrow].mustHit, distanceMoveArrow, curIndexArrow,sumArrow);
            }
            else
            {
                //create arrow from list
                GameObject goArrow = Instantiate(lsPrefabArrows[indexArrowClone], lsContainSpawnEnemyArrow[indexArrowClone]);
                goArrow.transform.localPosition = lsPositionSpawnArrows[indexArrowClone].position;
                Quoc_Arrow arrowMove = goArrow.GetComponent<Quoc_Arrow>();
                //setup arrow
                arrowMove.SetupArrow(timeMoveArrow, lsArrowDataItems[curIndexArrow].timeTail / 1000, lsArrowDataItems[curIndexArrow].indexArrow,
                  lsArrowDataItems[curIndexArrow].mustHit, distanceMoveArrow, curIndexArrow, sumArrow);
            }
            curIndexArrow++;
        }
    }
    private void GetSongGameplay(int indexMod, string nameSong)
    {
        if (indexMod == 0)
        {
            AudioClip songAudioClip = Resources.Load("Sounds/Inst-" + nameSong) as AudioClip;
            Quoc_SoundManager.Instance.AddSoundBGM(songAudioClip);
        }
        else
        {
            //Get from Asset Bundle
        }
    }

    public void OnButtonClickDown(int index)
    {
        lsTargetArrows[index].IsPress = true;
    }
    public void OnButtonClickUp(int index)
    {
        lsTargetArrows[index].IsPress = false;
        for(int i = 0; i < lsContainSpawnArrow[index].childCount; i++)
        {
            lsContainSpawnArrow[index].GetChild(i).GetComponent<Quoc_Arrow>().IsPress = false;
        }
    }
    public void SetAnimationBoy(float index,float timeLoop = 0)
    {
        boyDataBinding.SetAnimationCharacter(index);
        if (timeLoop == 0)
        {
            timeLoop = 0.5f;
        }
        float speedMove = distanceMoveArrow / timeMoveArrow;
        float newTimeMove = (timeLoop * 10) / speedMove;
        CancelInvoke("DelayFinishAnimBoy");
        Invoke("DelayFinishAnimBoy",newTimeMove);
    }
    public void DelayFinishAnimBoy()
    {
        boyDataBinding.SetAnimationCharacter(0);
    }
    public void SetAnimationEnemy(float index,float timeLoop = 0) 
    {
        if (enemyDataBinding == null || !enemyDataBinding.gameObject.activeSelf)
        {
            return;
        }
        enemyDataBinding.SetAnimationCharacter(index);
        if (timeLoop == 0)
        {
            timeLoop = 0.5f;
        }
        float speedMove = distanceMoveArrow / timeMoveArrow;
        float newTimeMove = (timeLoop * 10) / speedMove;
        CancelInvoke("DelayFinishAnimEnemy");
        Invoke("DelayFinishAnimEnemy", newTimeMove);
    }

    public void DelayFinishAnimEnemy()
    {
        enemyDataBinding.SetAnimationCharacter(0);
    }
    public void ShowGameLose()
    {
        Debug.Log("ShowGameLose");
        if (gameState != GameState.EndGame)
        {
            Quoc_SoundManager.Instance.StopSoundBGM();
            //clear all arrow
            ClearAllArrow();
            UIManager.Instance.HideUI(UIIndex.UIGameplay);
            gameState = GameState.EndGame;
            GameAnalyticsManager.Instance.PlayEnd(configSongData.nameSong,"lose",score,miss,timerSong);
            UIManager.Instance.ShowUI(UIIndex.UILose);
        }
    }

    public void AddScore()
    {
        //Show Combo Text Correct
        Score+=100;

        uiGameplay.SetSliderHP(-1);
    }

    public void SubScore()
    {
        Miss++;
        uiGameplay.SetSliderHP(1);
    }
    public void ShowGameWin()
    {
        GameSave.ModeSaves[indexMode].WeekSaves[indexWeek].SongSaves[indexSongofWeek].Score = score;
        gameState = GameState.EndGame;
        GameAnalyticsManager.Instance.PlayEnd(configSongData.nameSong,"win",score,miss,timerSong);
        UIManager.Instance.ShowUI(UIIndex.UIWin,new WinParam() {coinReward=100});
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        gameState = GameState.EndGame;
        SetupGameplay(indexMode, indexWeek, indexSongofWeek,difficult);
        Quoc_SoundManager.Instance.StopSoundBGM();
        ClearAllArrow();
    }

    public void GoToHome()
    {
        Time.timeScale = 1;
        gameState = GameState.EndGame;
        UIManager.Instance.HideUI(UIIndex.UIGameplay);
        Quoc_SoundManager.Instance.StopSoundBGM();
        //hideGame Object
        goGameContent.SetActive(false);
        ClearAllArrow();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        gameState = GameState.Playing;
        Quoc_SoundManager.Instance.ResumeSoundBGM();
    }
    private void ClearAllArrow()
    {
        for(int i = 0; i < lsContainSpawnArrow.Count; i++)
        {
            if (lsContainSpawnArrow[i].childCount > 0)
            {
                for(int j = 0; j < lsContainSpawnArrow[i].childCount; j++)
                {
                    lsContainSpawnArrow[i].GetChild(j).GetComponent<Quoc_Arrow>().DestroySelf();
                }
            }
        }
        for(int i = 0; i < lsContainSpawnEnemyArrow.Count; i++)
        {
            if (lsContainSpawnEnemyArrow[i].childCount > 0)
            {
                for(int j = 0; j < lsContainSpawnEnemyArrow[i].childCount; j++)
                {
                    lsContainSpawnEnemyArrow[i].GetChild(j).GetComponent<Quoc_Arrow>().DestroySelf();
                }
            }
        }
    }
}


[Serializable]
public class Quoc_NoteSongItem
{
    public int lengthInStep;
    public bool mustHitSection;
    public List<float[]> sectionNotes = new List<float[]>();

    public Quoc_NoteSongItem(List<float[]> sectionNotes, int lengthInStep, bool mustHitSection)
    {
        this.sectionNotes = sectionNotes;
        this.lengthInStep = lengthInStep;
        this.mustHitSection = mustHitSection;
    }
}

[Serializable]
public class Quoc_ArrowDataItem
{
    public float timeAppear;
    public int indexArrow;
    public float timeTail;
    public bool mustHit;

    public Quoc_ArrowDataItem(float timeAppear, int indexArrow, float timeTail,bool mustHit)
    {
        this.timeAppear = timeAppear;
        this.indexArrow = indexArrow;
        this.timeTail = timeTail;
        this.mustHit = mustHit;
    }

}

[Serializable]

public class Quoc_SongItem
{
    public List<Quoc_NoteSongItem> notes = new List<Quoc_NoteSongItem>();
}

[Serializable]

public class Quoc_RootItem
{
    public Quoc_SongItem song;
}

public enum GameState
{
    None=0,
    PauseGame=1,
    Playing=2,
    Ready=3,
    EndGame=4
}

public enum Difficult
{
    Easy=0,
    Normal = 1, 
    Hard = 2
}