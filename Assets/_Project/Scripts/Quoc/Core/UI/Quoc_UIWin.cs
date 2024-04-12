using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quoc;
using TMPro;
using UnityEngine.UI;
using Quoc_Core;

namespace Quoc
{

    public class WinParam : UIParam
    {
        public int coinReward;
    }
    public class Quoc_UIWin : BaseUI
	{
        [SerializeField] private TextMeshProUGUI txtCoinReward;
        [SerializeField] private TextMeshProUGUI txtCoin;

        [SerializeField] private Button btn5X;
        private bool isWatchAds;
        private int valueCoinReward;

        public override void OnInit()
        {
            base.OnInit();
        }
        public override void OnSetup(UIParam param = null)
        {
            base.OnSetup(param);
            //get coin from save
            WinParam winParam = (WinParam)param;
            txtCoinReward.text = "+" + winParam.coinReward;
            Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.Victory);
            
            btn5X.gameObject.SetActive(true);
            isWatchAds = false;
            AdManager.Instance.ShowInterstitialAds(() =>
            {
                Debug.Log("Show inter ads UI lose");
            });
        }

        public void OnHome_Clicked()
        {
            Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.Click);
            Quoc_SoundManager.Instance.StopSoundFX(SoundFxIndex.Victory);

            //show inter ads

            UIManager.Instance.HideUI(UIIndex.UIGameplay);
            UIManager.Instance.HideUI(this);
            UIManager.Instance.ShowUI(UIIndex.UIMainMenu);

            //disable game content
            Quoc_GameManager.Instance.goGameContent.SetActive(false);
            if (!isWatchAds)
            {
                //add coin reward value to save
                Quoc_GameManager.Instance.GameSave.Coin += valueCoinReward;
                SaveManager.Instance.SaveGame();
            }
        }

        public void OnX5_Clicked()
        {
            Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.Click);
            AdManager.Instance.ShowRewardedAds(() =>
            {
                UIManager.Instance.HideUI(UIIndex.UIGameplay);
                //UIManager.Instance.HideUI(this);

                //show reward ads
                valueCoinReward *= 5;
                //add coin reward to save
                btn5X.gameObject.SetActive(false);
            });
           
        }
	}
}