using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quoc;
using System;

namespace Quoc
{
	[CreateAssetMenu(fileName ="Config Skin",menuName ="Config/Config Skin",order =5)]
	public class ConfigSkin : ScriptableObject
	{
		public ConfigSkinData[] dataBoy;
		public ConfigSkinData[] dataGirl;

		private static ConfigSkin Instance;

		public static ConfigSkinData GetConfigSkinDataBoy(int index)
        {
			Instance = Resources.Load<ConfigSkin>("Configs/Config Skin");
			ConfigSkinData result = null;
			foreach(var go in Instance.dataBoy)
            {
                if (go.id == index){
					result = go;
					break;
                }
            }
            if (result == null)
            {
				result = Instance.dataBoy[0];
            }
			return result;
        }
		public static ConfigSkinData GetConfigSkinDataGirl(int index)
		{
			Instance = Resources.Load<ConfigSkin>("Configs/Config Skin");
			ConfigSkinData result = null;
			foreach (var go in Instance.dataGirl)
			{
				if (go.id == index)
				{
					result = go;
					break;
				}
			}
			if (result == null)
			{
				result = Instance.dataGirl[0];
			}
			return result;
		}
		public static int GetBoySkinDataLength()
        {
			Instance = Resources.Load<ConfigSkin>("Configs/Config skin");
			return Instance.dataBoy.Length;
        }

		public static int GetGirlSkinDataLength()
		{
			Instance = Resources.Load<ConfigSkin>("Configs/Config skin");
			return Instance.dataGirl.Length;
		}
	}
	[Serializable] 
	public class ConfigSkinData
    {
		public int id;
		public RuntimeAnimatorController skinAnimator;
		public int coin;
		public Sprite spriteSkin;
    }
}