using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quoc;
using Quoc_Core;
namespace Quoc
{
	public class Quoc_UIPause : BaseUI
	{
		public override void OnInit()
        {
            base.OnInit();
        }
        public override void OnSetup(UIParam param = null)
        {
            base.OnSetup(param);
            Time.timeScale = 0;
        }

        public void OnResume_Clicked()
        {
            Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.Click);
            //resume game
            Quoc_GameManager.Instance.ResumeGame();
            //show inter ads
            UIManager.Instance.HideUI(this);
        }
        public void OnHome_Clicked()
        {
            Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.Click);
            //end game
            Quoc_GameManager.Instance.GoToHome();
            UIManager.Instance.HideUI(this);
            UIManager.Instance.ShowUI(UIIndex.UIMainMenu);
        }
        public void OnRestart_Clicked()
        {
            Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.Click);
            UIManager.Instance.HideUI(this);
            UIManager.Instance.HideUI(UIIndex.UIGameplay);
            // show inter ads
            //restart game
            Quoc_GameManager.Instance.RestartGame();
        }

    }
}