using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quoc;
using UnityEngine.UI;
using Quoc_Core;

namespace Quoc
{
	public class SkinItem : MonoBehaviour
	{
		[SerializeField] private GameObject goPrice;

		[SerializeField] private Image imgSkin;

		private Quoc_UISkin parent;

		//configskin
		private ConfigSkinData configSkinData;
		private bool isBought;

		private bool isSkinGirl;

		[SerializeField] private Sprite spriteDeselect;
		[SerializeField] private Sprite spriteSelect;

		[SerializeField] private Sprite spriteGirlDeselect;
		private const int valueBoughtSkin = 1000;

        private void Awake()
        {
			GetComponent<Button>().onClick.AddListener(() => OnSkin_Clicked());

			//goPrice = transform.Find("btnBuy").gameObject;
			goPrice.GetComponent<Button>().onClick.AddListener(() => OnBuy_Clicked());
			//imgSkin = transform.Find("imgGirl").GetComponent<Image>();
        }

		public void OnSkin_Clicked()
        {
			Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.Click);
			if (!goPrice.activeSelf)
            {
				GetComponent<Image>().sprite = spriteSelect;

				if (isSkinGirl)
				{
					//change shin girl
					parent.OnChangeSkinGirl(configSkinData.spriteSkin, this);
				}
				else
				{
					//chang boy
					parent.OnChangeSkinBoy(configSkinData.spriteSkin, this);

				}
			}
		
		}
		public void OnSkin_Clicked(bool isGirl)
        {
			GetComponent<Image>().sprite = spriteSelect;
            if (isGirl)
            {
				//change shin girl
				parent.OnChangeSkinGirl(configSkinData.spriteSkin, this);

			}
			else
			{
				//chang boy
				parent.OnChangeSkinBoy(configSkinData.spriteSkin, this);

			}
		}
		public void OnDeselect()
        {
			GetComponent<Image>().sprite= spriteDeselect;
        }
		public void OnSetup(Quoc_UISkin parent,ConfigSkinData configSkinData,bool isBought,bool isSkinGirl = true)
        {
			this.parent = parent;
			this.isBought = isBought;
			this.isSkinGirl = isSkinGirl;
			this.configSkinData = configSkinData;


			GetComponent<Image>().sprite = spriteDeselect;

			if(configSkinData.coin==0||isBought)
            {
				goPrice.SetActive(false);
				imgSkin.sprite = configSkinData.spriteSkin;
            }
            else
            {
				imgSkin.sprite = spriteGirlDeselect;
				goPrice.SetActive(true);
            }
        }
		public void OnBuy_Clicked()
        {
			Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.Click);
            if (Quoc_GameManager.Instance.GameSave.Coin >= valueBoughtSkin && !isBought)
            {
				isBought = true;
				Quoc_GameManager.Instance.GameSave.Coin -= valueBoughtSkin;
				goPrice.SetActive(false);
                if (isSkinGirl)
                {
					int idSkinGirl = configSkinData.id;
					//id config
					Quoc_GameManager.Instance.GameSave.GirlSkinBoughts.Add(idSkinGirl);
                }
                else
                {
					int idSkinBoy = configSkinData.id;
					//id config
					Quoc_GameManager.Instance.GameSave.GirlSkinBoughts.Add(idSkinBoy);
				}
				parent.UpdateTextCoin();

				imgSkin.sprite = configSkinData.spriteSkin;
            }
			OnSkin_Clicked();
        }
    }
}