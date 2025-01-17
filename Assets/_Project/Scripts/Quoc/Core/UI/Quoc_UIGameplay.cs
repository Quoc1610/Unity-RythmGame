using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Quoc_Core;
using TMPro;
using Quoc;

public class Quoc_UIGameplay : BaseUI
{
    [Header("----------------Text--------------------------------")]
    [SerializeField] private TextMeshProUGUI txtSong;
    [SerializeField] private TextMeshProUGUI txtMiss;
    [SerializeField] private TextMeshProUGUI txtScore;
    [SerializeField] private TextMeshProUGUI txtTimer;

    [Header("----------------Image--------------------------------")]
    [SerializeField] private Image imgReady;
    [SerializeField] private Image imgDifficulty;

    [SerializeField] private Image imgIconBoss;
    [SerializeField] private Image imgIconBoy;

    [SerializeField] private List<Image> lsArrowTops = new List<Image>();
    [SerializeField] private List<Image> lsImgBtns = new List<Image>();

    [Header("----------------Transform & GameObject--------------------------------")]
    [SerializeField] private Transform transTarget;
    public List<Transform> lsTransSpawnArrowBot = new List<Transform>();

    [SerializeField] private List<GameObject> lsGoEffectArrows = new List<GameObject>();
    [SerializeField] private List<GameObject> lsGoTexts = new List<GameObject>();

    [Header("-----------------Sprite----------------------------")]
    [SerializeField] private List<Sprite> lsSpriteArrowNormals = new List<Sprite>();

    [SerializeField] private List<Sprite> lsSpriteArrowCorrects = new List<Sprite>();

    [SerializeField] private List<Sprite> lsSpriteDifficults = new List<Sprite>();

    [SerializeField] private List<Sprite> lsSpriteCountdowns = new List<Sprite>();

    [SerializeField] private List<Sprite> lsSpriteBtnOns = new List<Sprite>();
    [SerializeField] private List<Sprite> lsSpriteBtnOffs = new List<Sprite>();



    [SerializeField] private Sprite spriteBoyNormal;
    [SerializeField] private Sprite spriteBoyLose;

    private Sprite spriteEnemyNormal;
    private Sprite spriteEnemyLose;

    [Header("------------------Variable--------------------------------")]
    private GamePlayParam gameplayParam;

    [SerializeField] private Slider sliderHP;
    [SerializeField] private Vector3 defaultScaleBtn = new Vector3(0.9f, 0.9f, 1);
    private GameState gameState;

    public override void OnInit()
    {
        base.OnInit();
    }
    public override void OnSetup(UIParam param = null)
    {
        base.OnSetup(param);
        gameplayParam = (GamePlayParam)param;
        Debug.Log("Param " + gameplayParam.nameSong);
        txtSong.text = gameplayParam.nameSong;
        imgDifficulty.sprite = lsSpriteDifficults[(int)gameplayParam.difficult];
        imgDifficulty.SetNativeSize();

        sliderHP.maxValue = gameplayParam.maxValueSlider;
        sliderHP.value = gameplayParam.maxValueSlider / 2;

        imgReady.sprite = lsSpriteCountdowns[0];
        imgReady.SetNativeSize();

        for(int i = 0; i < lsGoTexts.Count; i++)
        {
            lsGoTexts[i].SetActive(false);
        }

        imgIconBoy.sprite = spriteBoyNormal;
        imgIconBoy.SetNativeSize();

        for(int i = 0; i < lsImgBtns.Count; i++)
        {
            lsImgBtns[i].sprite = lsSpriteBtnOffs[i];
            lsImgBtns[i].SetNativeSize();

            lsImgBtns[i].transform.localScale = defaultScaleBtn;
        }

        StartCoroutine(CountdownToStart(1f));
    }

    IEnumerator CountdownToStart(float timer)
    {
        gameState = GameState.Ready;
        imgReady.gameObject.SetActive(true);
        for(int i = 0; i < lsSpriteCountdowns.Count; i++)
        {
            imgReady.sprite = lsSpriteCountdowns[i];
            imgReady.SetNativeSize();

            Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.One + i);

            yield return new WaitForSeconds(timer);
        }
        imgReady.gameObject.SetActive(false);
        Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.Go);

        yield return new WaitForSeconds(timer);
        gameState = GameState.Playing;
        Quoc_SoundManager.Instance.PlaySoundBGM();
    }

    public float GetDistanceMoveArrow()
    {
        return Vector2.Distance(transTarget.position, lsTransSpawnArrowBot[0].position);
    }
   
    public List<Transform> GetListTransformSpawnArrow()
    {
        return lsTransSpawnArrowBot;
    }
    public List<Transform> GetListTargetArrow()
    {
        List<Transform> lsTransTarget = new List<Transform>();
        for(int i = 0; i < lsArrowTops.Count; i++)
        {
            lsTransTarget.Add(lsArrowTops[i].transform);
        }
        return lsTransSpawnArrowBot;
    }
    public void OnButtonArrowClick(int index)
    {
        Quoc_GameManager.Instance.OnButtonClickDown(index);
    }

    public void OnButtonClickUp(int index)
    {
        lsImgBtns[index].sprite = lsSpriteBtnOffs[index];
        lsImgBtns[index].SetNativeSize();

        lsImgBtns[index].transform.localScale=defaultScaleBtn;
        Quoc_GameManager.Instance.OnButtonClickUp(index);
    }

    public void OnButtonClickDown(int index)
    {
        lsImgBtns[index].sprite = lsSpriteBtnOns[index];
        lsImgBtns[index].SetNativeSize();

        lsImgBtns[index].transform.localScale = Vector3.one;

        Quoc_GameManager.Instance.OnButtonClickDown(index);
    }

    private void Update()
    {

        //key down 

        if (Input.GetKeyDown(KeyCode.A)){
            OnButtonClickDown(0);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            OnButtonClickDown(1);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            OnButtonClickDown(2);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            OnButtonClickDown(3);
        }

        //key up

        if (Input.GetKeyDown(KeyCode.A))
        {
            OnButtonClickUp(0);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            OnButtonClickUp(1);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            OnButtonClickUp(2);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            OnButtonClickUp(3);
        }
    }

    public void SetCorrectArrow(int index)
    {
        StartCoroutine(PlayAnimationCorrectArrow(index));
    }
    IEnumerator PlayAnimationCorrectArrow(int index)
    {
        lsArrowTops[index].sprite = lsSpriteArrowCorrects[index];
        lsArrowTops[index].SetNativeSize();

        GameObject goEffect = Instantiate(lsGoEffectArrows[index]);
        goEffect.transform.position = lsArrowTops[index].transform.position;
        goEffect.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        lsArrowTops[index].sprite = lsSpriteArrowNormals[index];
        lsArrowTops[index].SetNativeSize();
    }

    public void UpdateScoreText(int score)
    {
        txtScore.text = score.ToString();
    }

    public void UpdataMissText(int miss)
    {
        txtMiss.text = string.Format("|   Miss: {0}  |", miss);

    }

    public void UpdateTimerText(float timer)
    {
        txtTimer.text = Mathf.FloorToInt(timer / 60).ToString("00") + " : " +
                        Mathf.FloorToInt(timer % 60).ToString("00");
    }

    public void ShowTextCorrect(int index)
    {
        lsGoTexts[index].SetActive(true);
    }
    public void SetSliderHP(float delta)
    {
        sliderHP.value += delta;
        if (sliderHP.value > sliderHP.maxValue)
        {
            imgIconBoy.sprite = spriteBoyLose;
            imgIconBoss.sprite = spriteEnemyNormal;
        }
        else
        {
            imgIconBoy.sprite = spriteBoyNormal;
            imgIconBoss.sprite = spriteEnemyLose;
        }

        if (sliderHP.value >= sliderHP.maxValue)
        {
            //show game lose
            Quoc_GameManager.Instance.ShowGameLose();
        }
    }

    public bool CheckGameWin()
    {
        return sliderHP.value <= sliderHP.maxValue / 2;
    }


    public void SetSpriteIconboss(Sprite spriteBossLose, Sprite spriteBossNormal) 
    {
        Debug.Log("Set Sprite: ");
        this.spriteEnemyLose = spriteBossLose;
        this.spriteEnemyNormal = spriteBossNormal;
        SetSliderHP(0);
    }

    public void OnPauseGame_Clicked()
    {
        //play click SFX
        Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.Click);
        Quoc_SoundManager.Instance.PauseSoundBGM();
        gameState = GameState.PauseGame;
        UIManager.Instance.ShowUI(UIIndex.UIPause);
    }
}
