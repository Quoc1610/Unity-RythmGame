using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quoc;
using UnityEngine.UI;
using TMPro;
using Quoc_Core;

namespace Quoc
{
	public class Quoc_UISkin : BaseUI
	{
        [SerializeField] private Image imgGirl;
        [SerializeField] private Image imgBoy;

        [SerializeField] private List<SkinItem>lsSkinBoyItem=new List<SkinItem>();
        [SerializeField] private List<SkinItem> lsSkinGirlItem = new List<SkinItem>();
        private SkinItem curBoySkinItem;
        private SkinItem curGirlSkinItem;

        [SerializeField] private GameObject goScrollRectBoy;
        [SerializeField] private GameObject goScrollRectGirl;

        [SerializeField] private Button btnAds;
        [SerializeField] private Button btnBoys;
        [SerializeField] private Button btnGirls;

        [SerializeField] private Sprite spritebtnBoySelect;
        [SerializeField] private Sprite spritebtnBoyDeselect;

        [SerializeField] private Sprite spritebtnGirlSelect;
        [SerializeField] private Sprite spritebtnGirlDeselect;

        [SerializeField] private TextMeshProUGUI txtCoin;

        [SerializeField] private Animator boyAnim;
        [SerializeField] private Animator girlAnim;

        public override void OnInit()
        {
            base.OnInit();
        }
        public override void OnSetup(UIParam param = null)
        {
            base.OnSetup(param);
            for(int i = 0; i < ConfigSkin.GetBoySkinDataLength(); i++)
            {
                ConfigSkinData configSkinData = ConfigSkin.GetConfigSkinDataBoy(i);
                bool isBought = Quoc_GameManager.Instance.GameSave.BoySkinBoughts.IndexOf(i) >= 0;
                lsSkinBoyItem[i].OnSetup(this, configSkinData, isBought, false);
            }
            for (int i = 0; i < ConfigSkin.GetGirlSkinDataLength(); i++)
            {
                ConfigSkinData configSkinData = ConfigSkin.GetConfigSkinDataGirl(i);
                bool isBought = Quoc_GameManager.Instance.GameSave.GirlSkinBoughts.IndexOf(i) >= 0;
                lsSkinGirlItem[i].OnSetup(this, configSkinData, isBought, true);
                Debug.Log("girl: " + i + " " + configSkinData.id);
            }
            txtCoin.text = Quoc_GameManager.Instance.GameSave.Coin.ToString();
            lsSkinBoyItem[Quoc_GameManager.Instance.GameSave.CurrentIndexBoy].OnSkin_Clicked(false);
            lsSkinGirlItem[Quoc_GameManager.Instance.GameSave.CurrentIndexGirl].OnSkin_Clicked();
            //show Boy Skin
            OnShowBoySkinClick();
        }
        public void OnChangeSkinGirl(Sprite spriteGirl,SkinItem skinItem)
        {
            if(curGirlSkinItem!=null&& curGirlSkinItem != skinItem)
            {
                curGirlSkinItem.OnDeselect();
            }
            Debug.Log("Skin girl " + skinItem.name);
            curGirlSkinItem = skinItem;
            int indexSkin = lsSkinGirlItem.IndexOf(skinItem);
            girlAnim.SetFloat("Index", indexSkin);
            imgGirl.sprite = spriteGirl;
            imgGirl.SetNativeSize();

            bool isBought = Quoc_GameManager.Instance.GameSave.GirlSkinBoughts.IndexOf(indexSkin) >= 0;
            if (isBought)
            {
                Quoc_GameManager.Instance.GameSave.CurrentIndexGirl = indexSkin;
            }
        }
        public void OnChangeSkinBoy(Sprite spriteBoy, SkinItem skinItem)
        {
            if (curBoySkinItem != null && curBoySkinItem != skinItem)
            {
                curBoySkinItem.OnDeselect();
            }

            curBoySkinItem = skinItem;
            int indexSkin = lsSkinBoyItem.IndexOf(skinItem);
            boyAnim.SetFloat("Index", indexSkin);
            imgBoy.sprite = spriteBoy;
            imgBoy.SetNativeSize();

            bool isBought = Quoc_GameManager.Instance.GameSave.BoySkinBoughts.IndexOf(indexSkin) >= 0;
            if (isBought)
            {
                Quoc_GameManager.Instance.GameSave.CurrentIndexBoy = indexSkin;
            }
        }
        public void OnShowBoySkinClick()
        {
            Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.Click);

            btnBoys.enabled = false;
            btnBoys.image.sprite = spritebtnBoySelect;

            btnGirls.enabled = true;
            btnGirls.image.sprite = spritebtnGirlDeselect;

            goScrollRectBoy.SetActive(true);
            goScrollRectGirl.SetActive(false);
        }

        public void OnShowGirlClick()
        {
            Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.Click);
            btnGirls.enabled = false;
            btnGirls.image.sprite = spritebtnGirlSelect;

            btnBoys.enabled = true;
            btnBoys.image.sprite = spritebtnBoyDeselect;

            goScrollRectBoy.SetActive(false);
            goScrollRectGirl.SetActive(true);
        }
        public void OnAdsClick()
        {
            Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.Click);
            //show ad
            AdManager.Instance.ShowRewardedAds(() =>
            {
                Debug.Log(("finished reward ad"));
                Quoc_GameManager.Instance.GameSave.Coin += 100;
                txtCoin.text = Quoc_GameManager.Instance.GameSave.Coin.ToString();
            });
            
        }
        public void UpdateTextCoin()
        {
            txtCoin.text = Quoc_GameManager.Instance.GameSave.Coin.ToString();
        }
        public override void OnCloseClick()
        {
            base.OnCloseClick();
            UIManager.Instance.ShowUI(UIIndex.UIMainMenu);
            Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.Click);
            if (Quoc_GameManager.Instance.GameSave.isFirstOpen)
            {
                //show ad
            }
        }
    }
}