using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quoc;
using System;

namespace Quoc
{
	public class Quoc_UIRewardLogin : BaseUI
	{
        [SerializeField] private List<SlotItem> lsSlotItems = new List<SlotItem>();

		public override void OnInit()
        {
            base.OnInit();
        }
        public override void OnSetup(UIParam param = null)
        {
            base.OnSetup(param);
            for(int i = 0; i < lsSlotItems.Count; i++)
            {
                //get config 
                ConfigDailyLoginData configDailyLoginData = ConfigDailyLogin.GetDailyLoginData(i);
                Debug.Log("login: " + Quoc_GameManager.Instance.GameSave.CurrentDayLogin + " "
                    + Quoc_GameManager.Instance.GameSave.CurrentDayOfWeekLogin);
                int currentDayLogin = Quoc_GameManager.Instance.GameSave.CurrentDayLogin;
                int currentWeekLogin = Quoc_GameManager.Instance.GameSave.CurrentDayOfWeekLogin;
                int coin = configDailyLoginData.coin;
                if (DateTime.Now.DayOfYear - currentWeekLogin == currentDayLogin)
                {
                    //get coin
                    
                    lsSlotItems[i].OnSetup(i, coin, i >= currentDayLogin, i == currentDayLogin);
                }
                else
                {
                    if(DateTime.Now.DayOfYear - currentWeekLogin < currentDayLogin)
                    {
                        lsSlotItems[i].OnSetup(i, coin, i >= currentDayLogin, false);
                    }
                    else
                    {
                        lsSlotItems[i].OnSetup(i, coin, i >= currentDayLogin, i==currentDayLogin);
                    }
                }

            }
        }
        public override void OnCloseClick()
        {
            base.OnCloseClick();
            Quoc_UIMainMenu uIMainMenu = (Quoc_UIMainMenu)UIManager.Instance.FindUIVisible(UIIndex.UIMainMenu);
            uIMainMenu.UpdateTextCoin();
            //show ads
        }

    }
}