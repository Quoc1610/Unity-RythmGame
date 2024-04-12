using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quoc;
using TMPro;
using UnityEngine.UI;
using Quoc_Core;
using System;
using Random = UnityEngine.Random;

namespace Quoc
{
	public class Quoc_UIMainMenu : BaseUI
	{

        [SerializeField] private List<SongItem> lsSongItems = new List<SongItem>();
        [SerializeField] private TextMeshProUGUI txtCoin;

        [SerializeField] private Image imgBoy;
        [SerializeField] private Image imgGirl;

        [SerializeField] private Image imgVibrate;
        [SerializeField] private Sprite spriteVibrateOn;
        [SerializeField] private Sprite spriteVibrateOff;

        [SerializeField] private Animator boyAnimator;
        [SerializeField] private Animator girlAnimator;

        [SerializeField] private List<GameObject> lsGoSelectModes = new List<GameObject>();

        private GameObject goCurSelectMode;

        private const int NumberSongNewHot = 20;
        private GameSave gameSave;

        public override void OnInit()
        {
            base.OnInit();
            for (int i = 0; i < lsGoSelectModes.Count; i++)
            {
                lsGoSelectModes[i].SetActive(false);
            }

        }
        public override void OnSetup(UIParam param = null)
        {
            base.OnSetup(param);
            //Check setting vibrate
            gameSave = Quoc_GameManager.Instance.GameSave;
            bool VirtbrateOn = gameSave.VibreateOn;
            if (VirtbrateOn)
            {
                imgVibrate.sprite = spriteVibrateOn;
            }
            else
            {
                imgVibrate.sprite = spriteVibrateOff;
            }
            imgVibrate.SetNativeSize();
            /*if (!Quoc_SoundManager.Instance.CheckSoundFXAvailable(SoundFxIndex.SoundMenu))
            {
                Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.SoundMenu);
            }*/

            //select 1st mode

            Timer.DelayedCall(0.5f, () =>
            {
                imgBoy.SetNativeSize();
                imgGirl.SetNativeSize();
                //get coin from save
                UpdateTextCoin();

            }, this);
            OnSelectMode(0);
            if(gameSave.CurrentDay!= DateTime.Now.DayOfYear)
            {
                gameSave.CurrentDay = DateTime.Now.DayOfYear;
                gameSave.HotNewSongs.Clear();
                int countSong = 0;
                while (countSong < NumberSongNewHot)
                {
                    int randMode = Random.Range(1, Quoc_ConfigGameplay.GetModeLength());
                    int randWeek = Random.Range(0, Quoc_ConfigGameplay.GetWeekLength(randMode));
                    int randSong = Random.Range(0, Quoc_ConfigGameplay.GetSongLength(randMode, randWeek));
                    HotNewSong hotNewSong = new HotNewSong
                    {
                        IndexMode = randMode,
                        IndexWeek = randWeek,
                        IndexSong = randSong
                    };
                    if (!CheckAvailableSong(hotNewSong))
                    {
                        gameSave.HotNewSongs.Add(hotNewSong);
                        countSong++;
                    }
                }
            }
            AdManager.Instance.ShowBanner();
        }
        public void OnOption_Clicked()
        {
            Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.Click);
            UIManager.Instance.ShowUI(UIIndex.UIOption);
        }

        public void OnRate_Clicked()
        {
            Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.Click);
            UIManager.Instance.ShowUI(UIIndex.UIRate);
        }

        public void OnSkin_Clicked()
        {
            Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.Click);
            UIManager.Instance.ShowUI(UIIndex.UISkin);
        }

        public void OnRewardLogin_Clicked()
        {
            Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.Click);
            UIManager.Instance.ShowUI(UIIndex.UIRewardLogin);
        }

        public void OnLuckDraw_Clicked()
        {
            Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.Click);
            UIManager.Instance.ShowUI(UIIndex.UILuckyDraw);
        }

        public void OnVibrate_Clicked()
        {
            Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.Click);
            //Save Vibrate Setting
            gameSave.VibreateOn = !gameSave.VibreateOn;
            bool vibrateOn =gameSave.VibreateOn;
            if (vibrateOn)
            {
                imgVibrate.sprite = spriteVibrateOn;
            }
            else
            {
                imgVibrate.sprite = spriteVibrateOff;
            }
            imgVibrate.SetNativeSize();
        }

        public void UpdateTextCoin()
        {
            //set coin from save to Txtcoin
            int coin =gameSave.Coin;
            txtCoin.text = coin.ToString();
        }

        public void OnSaveSongData(int indexMode,int indexWeek,int indexSong)
        {
            gameSave.ModeSaves[indexMode].WeekSaves[indexWeek].SongSaves[indexSong].
                IsBought = true;
            UpdateTextCoin();

        }
        private bool CheckAvailableSong(HotNewSong hotNewSong)
        {
            if (gameSave.HotNewSongs.Contains(hotNewSong))
            {
                return true;

            }
            else
            {
                return false;
            }
        }

        public void OnNewHot_Clicked()
        {
            Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.Click);
            for(int i = 0; i < lsSongItems.Count; i++)
            {
                lsSongItems[i].gameObject.SetActive(i < NumberSongNewHot);
            }

            for(int i = 0; i < gameSave.HotNewSongs.Count; i++)
            {
                HotNewSong hotNewSong = gameSave.HotNewSongs[i];
                
                SongSave songSave = gameSave.ModeSaves[hotNewSong.IndexMode].WeekSaves[hotNewSong.IndexWeek]
                    .SongSaves[hotNewSong.IndexSong];
                Quoc_GameplaySongData gameplaySongData = Quoc_ConfigGameplay.ConfigSongData(
                    hotNewSong.IndexMode, hotNewSong.IndexWeek, hotNewSong.IndexSong);

                lsSongItems[i].OnSetupSongitem(this, hotNewSong.IndexMode, hotNewSong.IndexWeek, hotNewSong.IndexSong
                    , gameplaySongData, songSave.Score, songSave.IsBought);
            }
        }

        public void OnSelectMode(int index)
        {
            Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.Click);
            int countSong = Quoc_ConfigGameplay.GetAllSongInMode(index);
            for (int i= 0; i < lsSongItems.Count; i++)
            {
                lsSongItems[i].gameObject.SetActive(i < countSong);
            }

            int count = 0;
            for(int i = 0; i < Quoc_ConfigGameplay.GetWeekLength(index); i++)
            {
                for(int j = 0; j < Quoc_ConfigGameplay.GetSongLength(index, i); j++)
                {
                    SongSave songSave = gameSave.ModeSaves[index].WeekSaves[i].SongSaves[j];
                    Quoc_GameplaySongData gameplaySongData = Quoc_ConfigGameplay.ConfigSongData(index, i, j);
                    lsSongItems[count].OnSetupSongitem(this, index, i, j,
                        gameplaySongData, songSave.Score, songSave.IsBought);
                    count++;
                }
            }
        }
        public void ChanggeUISelectModeBtn(int indexMode)
        {
            if (goCurSelectMode != null)
            {
                goCurSelectMode.SetActive(false);
            }
            goCurSelectMode = lsGoSelectModes[indexMode];
            goCurSelectMode.SetActive(true);
        }
    }
}