using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quoc;
using TMPro;
using Quoc_Core;

namespace Quoc
{
    public class RewardParam : UIParam
    {
        public int valueCoin;
    }
	public class Quoc_UIReward : BaseUI
	{
        public TextMeshProUGUI txtCoin;
		public override void OnInit()
        {
            base.OnInit();
        }
        public override void OnSetup(UIParam param = null)
        {
            base.OnSetup(param);
            RewardParam rewardParam = (RewardParam)param;

            txtCoin.text = "+" + rewardParam.valueCoin;
            Quoc_GameManager.Instance.GameSave.Coin += rewardParam.valueCoin;
            SaveManager.Instance.SaveGame();

            Quoc_UIMainMenu uIMainMenu = (Quoc_UIMainMenu)UIManager.Instance.FindUIVisible(UIIndex.UIMainMenu);
            uIMainMenu.UpdateTextCoin();
        }
	}
}