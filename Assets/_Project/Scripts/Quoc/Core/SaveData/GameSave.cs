using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quoc;
using System;

namespace Quoc
{
	[Serializable]
	public class SongProperty
    {
		public int IndexSong;
		public string NameSong;
		public bool IsUnlocked;
		public int Price;
		
		public SongProperty(int indexSong,string nameSong,bool isUnlocked,int price)
        {
			IndexSong = indexSong;
			NameSong = nameSong;
			IsUnlocked = isUnlocked;
			Price = price;
        } 
    }
	[Serializable] 
	public class ModeSave
    {
		public List<WeekSave>WeekSaves  = new List<WeekSave>();
		public bool IsDownloadAssetBundle;
	}

	[Serializable]
	public class WeekSave
    {
		public List<SongSave> SongSaves = new List<SongSave>();

    }

	[Serializable]
	public class SongSave
	{
		public int IndexSong;
		public bool IsBought;
		public int Score;
	}

	[Serializable]
	public class HotNewSong
	{
		public int IndexMode;
		public int IndexWeek;
		public int IndexSong;
	}

	[Serializable]
	public class GameSave
	{
		public bool isFirstOpen;
		public bool OptionUpScroll;
		public bool VibreateOn = true;

		public bool ShowRating = true;
		public bool ShowRatingNewHot = true;

		public double CountDownLuckyDraw;

		public int CurrentDayLogin;
		public int CurrentDayOfWeekLogin;

		public List<SongProperty> SongProperties = new List<SongProperty>();

		public int Coin;

		public string FirstVersion = "";

		public int CurrentIndexBoy;
		public int CurrentIndexGirl;

		public List<int> GirlSkinBoughts = new List<int>();
		
		public List<int> BoySkinBoughts = new List<int>();

		public List<ModeSave> ModeSaves = new List<ModeSave>();

		public int CurrentDay;

		public List<HotNewSong> HotNewSongs = new List<HotNewSong>();

		public void Init(string version)
		{
			if (FirstVersion == "")
            {
				FirstVersion = version;
            }
			

			int firstOpen = PlayerPrefs.GetInt("Firstopen", 0);
			Debug.Log("Init: "+ firstOpen + " " + FirstVersion);
            if (firstOpen == 0)
            {
				ModeSaves.Clear();
				for(int i = 0; i < Quoc_ConfigGameplay.GetModeLength(); i++)
                {
					ModeSave modeSave = new ModeSave();
					modeSave.IsDownloadAssetBundle = i == 0;
					ModeSaves.Add(modeSave);

					for (int j = 0; j < Quoc_ConfigGameplay.GetWeekLength(i); j++)
                    {
						ModeSaves[i].WeekSaves.Add(new WeekSave());

						for(int k = 0; k < Quoc_ConfigGameplay.GetSongLength(i, j); k++)
                        {
							SongSave songSave = new SongSave();
							songSave.IndexSong = k;
                            if (i == 0 && j <= 1)
                            {
								songSave.IsBought = true;

                            }
                            else
                            {
								songSave.IsBought = false;

                            }

							songSave.Score = 0;
							ModeSaves[i].WeekSaves[j].SongSaves.Add(songSave); 
                        }
                    }
				}

				CountDownLuckyDraw = TimeSpan.FromTicks(DateTime.Now.Ticks).TotalSeconds - 7200;

				OptionUpScroll = false;
				VibreateOn = true;

				ShowRating = true;
				ShowRatingNewHot = true;

				CurrentDay = DateTime.Now.DayOfYear - 1;
				CurrentDayOfWeekLogin = DateTime.Now.DayOfYear;

				Coin = 0;
				CurrentDay = 0;
				BoySkinBoughts.Add(0);
				GirlSkinBoughts.Add(0);

				isFirstOpen =false;
			}
			for (int i = 0; i < ModeSaves.Count; i++)
			{
				if (ModeSaves[i].WeekSaves.Count == 0)
				{
					ModeSaves.RemoveAt(i);
				}
			}

			if (ModeSaves.Count < Quoc_ConfigGameplay.GetModeLength())
			{
				for (int i = ModeSaves.Count; i < Quoc_ConfigGameplay.GetModeLength(); i++)
				{
					ModeSave modeSave = new ModeSave();
					modeSave.IsDownloadAssetBundle = false;
					ModeSaves.Add(modeSave);

					for (int j = 0; j < Quoc_ConfigGameplay.GetWeekLength(i); j++)
					{
						ModeSaves[i].WeekSaves.Add(new WeekSave());

						for (int k = 0; k < Quoc_ConfigGameplay.GetSongLength(i, j); k++)
						{
							SongSave songSave = new SongSave();
							songSave.IndexSong = k;
							songSave.IsBought = false;

							songSave.Score = 0;
							ModeSaves[i].WeekSaves[j].SongSaves.Add(songSave);
						}
					}
				}
			}


			firstOpen++;
			PlayerPrefs.SetInt("FirstOpen", firstOpen);
		}
	}
}