using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace Quoc_Core
{
    public class AdManager : SingletonMono<AdManager>
    {
        private IGameAd maxApplovin = null;
        public bool maxApplovinSupport;
        public string maxSDKKey;
        public string maxAndroidBannerID;
        public string maxiOSBannerID;
        public string maxAndroidInterID;
        public string maxiOSInterID;
        public string maxAndroidRewardID;
        public string maxiOSRewardID;
        
        
        
        private void Awake()
        {
            if (maxApplovinSupport)
            {
                maxApplovin = new MAXAds(this, maxSDKKey, maxiOSBannerID, maxAndroidBannerID,
                    maxiOSInterID, maxAndroidInterID, maxiOSRewardID, maxAndroidRewardID,
                    RewardCallback, OpenedCallback, ClosedCallback);
                maxApplovin.Init();
                ShowBanner();
            }
        }

        private void ClosedCallback()
        {
            
        }
        private void OpenedCallback()
        {
            
        }
        private void RewardCallback(string arg1,double arg2)
        {
            
        }

        public void ShowBanner()
        {
            if (maxApplovinSupport && maxApplovin != null)
            {
                maxApplovin.ShowBanner();
            }
        }
        public void HideBanner()
        {
            if (maxApplovinSupport && maxApplovin != null)
            {
                maxApplovin.HideBanner();
            }
        }

        public bool IsInterstitialReady()
        {
            if (maxApplovinSupport && maxApplovin != null)
            {
                maxApplovin.IsIntertitialReady();
            }

            return false;
        }

        public void LoadInterstitialAds()
        {
            if (maxApplovinSupport && maxApplovin != null)
            {
                maxApplovin.LoadInterstial();
            }
        }
        public void ShowInterstitialAds(Action finished)
        {
            if (maxApplovinSupport && maxApplovin != null)
            {
                maxApplovin.ShowIntertitial(finished);
            }
            else
            {
                if (finished != null)
                {
                    //show
                }
                
            }
        }

        public bool IsRewardedReady()
        {
            if (maxApplovinSupport && maxApplovin != null)
            {
                return maxApplovin.IsRewardedReady();
            }

            return Application.isEditor;
        }
        public void LoadRewardedAds()
        {
            if (maxApplovinSupport && maxApplovin != null)
            {
                maxApplovin.LoadRewardedVideo();
            }
        }

        public void ShowRewardedAds(Action finished, Action watchFailed = null)
        {
            if (maxApplovinSupport && maxApplovin != null)
            {
                if (IsRewardedReady())
                {
                    maxApplovin.ShowRewardedVideo(finished,watchFailed);
                }
                else
                {
                    //show UI no internet
                }
            }
        }
    }
}

