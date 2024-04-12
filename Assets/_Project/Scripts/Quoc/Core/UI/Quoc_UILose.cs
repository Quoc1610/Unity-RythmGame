using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quoc;
using Quoc_Core;

namespace Quoc
{
	public class Quoc_UILose : BaseUI
	{
        [SerializeField] private GameObject goBtns;
		public override void OnInit()
        {
            base.OnInit();
        }
        public override void OnSetup(UIParam param = null)
        {
            base.OnSetup(param);
            goBtns.SetActive(false);
            Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.GameOver);
            Timer.DelayedCall(2, () =>
            {
                //show UI btns
                goBtns.SetActive(true);

            }, this);
            AdManager.Instance.ShowInterstitialAds(() =>
            {
                Debug.Log("Show inter ads UI lose");
            });
        }
        public void OnHome_Clicked()
        {
            Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.Click);
            Quoc_SoundManager.Instance.StopSoundFX(SoundFxIndex.GameOver);

            UIManager.Instance.HideUI(this);
            UIManager.Instance.ShowUI(UIIndex.UIMainMenu);

            //show inter ads

        }
        public void OnRestart_Clicked()
        {
            Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.Click);
            Quoc_SoundManager.Instance.StopSoundFX(SoundFxIndex.GameOver);

            UIManager.Instance.HideUI(this);
            // Game Manager restart func
            Quoc_GameManager.Instance.RestartGame();

        }
    }
}