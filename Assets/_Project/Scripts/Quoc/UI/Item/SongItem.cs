using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quoc;
using UnityEngine.UI;
using TMPro;
using Quoc_Core;
namespace Quoc
{
	public class SongItem : MonoBehaviour
	{
		[SerializeField] private Image imgIcon;
		[SerializeField] private Image imgDifficult;
		private Image imgFrame;

		[SerializeField] private List<Sprite> lsSpriteDifficults = new List<Sprite>();
		[SerializeField] private List<Sprite> lsSpriteFrames = new List<Sprite>();

		[SerializeField] private TextMeshProUGUI txtScore;
		[SerializeField] private TextMeshProUGUI txtNameSong;

		[SerializeField] private GameObject goPlay;
		[SerializeField] private GameObject gobuySong;

		private int indexDifficult;
		private int indexSong;
		private int indexWeek;
		private int indexMode;

		private int price;


		private Quoc_UIMainMenu parent;

        private void Awake()
        {
			imgFrame = GetComponent<Image>();
        }

        public void OnSetupSongitem(Quoc_UIMainMenu parent,int indexWeek, int indexSong,
			int indexMode, Quoc_GameplaySongData gamePlaySongData, int score, bool isBought)
        {
			int indexFrame = indexSong % lsSpriteFrames.Count;
			imgFrame.sprite = lsSpriteFrames[indexFrame];

			this.parent = parent;
			this.indexMode = indexMode;
			this.indexWeek = indexWeek;
			this.indexSong = indexSong;
			this.price = gamePlaySongData.price;
			this.price = gamePlaySongData.price;

			txtNameSong.text = gamePlaySongData.nameSong;
			txtScore.text = score.ToString();

			indexDifficult = 0;
			imgDifficult.sprite = lsSpriteDifficults[indexDifficult];
			imgDifficult.SetNativeSize();

			imgIcon.sprite = gamePlaySongData.spriteCharacter;
			imgIcon.SetNativeSize();
			imgIcon.transform.localScale = new Vector3(0.6f, 0.6f, 1);

            if (isBought)
            {
				gobuySong.SetActive(false);
				goPlay.SetActive(true);

            }
            else
            {
				gobuySong.SetActive(true);
				goPlay.SetActive(false);
            }
		}

		public void OnDifficult_Clicked(int index)
        {
			Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.Click);
			indexDifficult += index;
            if (indexDifficult < 0)
            {
				indexDifficult = lsSpriteDifficults.Count-1;

            }
            else
            {
                if (indexDifficult >= lsSpriteDifficults.Count)
                {
					indexDifficult = 0;
                }
            }
			imgDifficult.sprite = lsSpriteDifficults[indexDifficult];
			imgDifficult.SetNativeSize();
        }

		public void OnBuySongItem_Clicked()
        {
			Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.Click);
            if (Quoc_GameManager.Instance.GameSave.Coin >= price)
            {
				Quoc_GameManager.Instance.GameSave.Coin -= price;
				SaveManager.Instance.SaveGame();

				gobuySong.SetActive(false);
				goPlay.SetActive(true);

				//save song data item
				parent.OnSaveSongData(indexMode, indexWeek, indexSong);

            }
        }

		public void OnPlay_Clicked()
        {
			Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.Click);
            if (Quoc_GameManager.Instance.GameSave.isFirstOpen)
            {
				//show ad
            }
			UIManager.Instance.HideUI(parent);
			Quoc_GameManager.Instance.SetupGameplay(indexMode, indexWeek, indexSong,
				(Difficult)indexDifficult);
			
        }

		public void OnRewardAds_Clicked()
        {
			Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.Click);
			//show reward ads

			Timer.DelayedCall(0.5f, () =>
			{
				 UIManager.Instance.HideUI(parent);
				 Quoc_GameManager.Instance.SetupGameplay(indexMode, indexWeek, indexSong, 
					 (Difficult)indexDifficult);
			},this);
        }
	}
}