using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quoc;
using System;

namespace Quoc_Core
{
	public interface IGameAd 
	{
		void Init();

		void Update();
		
		void ShowBanner();
		
		void HideBanner();

		bool IsRewardedReady();

		void LoadRewardedVideo();

		void ShowRewardedVideo(Action finished, Action watchFailed);
		
		bool IsIntertitialReady();

		void LoadInterstial();

		void ShowIntertitial(Action finished);
	}
}