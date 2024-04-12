using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quoc;
using TMPro;
using Quoc_Core;
using UnityEngine.UI;

namespace Quoc
{
	public class SlotItem : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI txtCoin;
		private Button btnClick;

		[SerializeField] private GameObject goFade;

		private int valueCoin;
		private int indexLogin;

		[SerializeField] private Sprite spriteOn;
		[SerializeField] private Sprite spriteOff;

        private void Awake()
        {
			btnClick = GetComponent<Button>();
			btnClick.onClick.AddListener(() =>
			{
				OnLogin_Clicked();
			});
        }
		public void OnSetup(int index,int coin,bool getReward,bool isToday)
        {
			valueCoin = coin;
			indexLogin = index;
			txtCoin.text = valueCoin.ToString();
			GetComponent<Image>().sprite = spriteOff;
            if (!getReward)
            {
				DisableSlot();
            }
            else
            {
				btnClick.enabled = isToday;
                if (isToday)
                {
					GetComponent<Image>().sprite = spriteOn;
                }
            }
        }
        private void OnLogin_Clicked()
        {
			Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.Click);
			DisableSlot();
			UIManager.Instance.ShowUI(UIIndex.UIReward, new RewardParam() { valueCoin = valueCoin });
			indexLogin++;

            if (indexLogin >= 7)
            {
				indexLogin = 0;
            }

			Quoc_GameManager.Instance.GameSave.CurrentDayLogin = indexLogin;
        }

        private void DisableSlot()
        {
			GetComponent<Image>().sprite = spriteOff;
			goFade.SetActive(true);
			btnClick.interactable = false;
        }
    }
}