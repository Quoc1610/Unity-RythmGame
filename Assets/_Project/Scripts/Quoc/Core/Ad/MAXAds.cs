using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quoc_Core;
using System;

public class MAXAds : IGameAd
{
    private string sdkKey;

    private bool hasRewarded = false;

    private Action<string, double> rewardCallback;

    private Action customRewardCallback = null;

    private Action watchFailed = null;
    private Action openedCallback = null;
    private Action closedCallback = null;

    private double rewardAmount;
    private string rewardType;
    private string bannerAdUnitID = "";
    private string intertitialAdUnitID = "";
    private string rewardedAdUnitID = "";

    private int intertitialRetryAttempt;
    private int rewardedRetryAttempt;

    private MonoBehaviour target = null;

    public MAXAds(MonoBehaviour target,string sdkKey,string banneriOSAds,string bannerAdroidAds,
        string interiOSAds,string interAndriodAds,string rewardiOSAds,string rewardAndriodAds,
        Action<string,double> rewardCallback,Action openedCallback,Action closedCallback)
    {
        this.target = target;
        this.sdkKey = sdkKey;
#if UNITY_ANDROID
        bannerAdUnitID = bannerAdroidAds;
        intertitialAdUnitID = interAndriodAds;
        rewardedAdUnitID = rewardAndriodAds;
#else
        bannerAdUnitID=banneriOSads;
        interstitialAdUnitID=interiOSAds;
        rewardedAdUnitID=rewardiOSAds;
#endif
        this.rewardCallback = rewardCallback;
        this.openedCallback = openedCallback;
        this.closedCallback = closedCallback;
    }
  
    public void Init()
    {
        MaxSdkCallbacks.OnSdkInitializedEvent += configuration =>
        {
            //init banner
            InitBanner();
            //init iter
            InitInterstitial();
            //init reward
            InitRewardedVideo();
        };
        MaxSdk.SetSdkKey(sdkKey);
        MaxSdk.InitializeSdk();
    }
    #region Init

    void InitBanner()
    {
        MaxSdkCallbacks.Banner.OnAdLoadedEvent += BannerOnOnAdLoadedEvent;
        MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += BannerOnOnAdLoadFailedEvent;
        if (!string.IsNullOrEmpty(bannerAdUnitID))
        {
            MaxSdk.CreateBanner(bannerAdUnitID, MaxSdkBase.BannerPosition.BottomCenter);
            ShowBanner();
        }
    }

    void InitInterstitial()
    {
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += IntertitialOnOnAdLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent+= IntertitialOnOnAdLoadFailEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent+= IntertitialOnOnAdClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += InterstitialOnAdHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += InterstitialOnAdDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += InterstitialOnAdDisplayFailedEvent;

        LoadInterstial();
    }
    void InitRewardedVideo()
    {
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += RewardedOnAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += RewardedOnAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += RewardedOnAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += RewardedOnAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += RewardedOnAdDisplayFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += RewardedOnAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += RewardedOnAdReceivedRewardEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += RewardedOnAdRevenuePaidEvent;
        
        LoadRewardedVideo();
        
    }

    
    void IGameAd.Update()
    {
        Update();
    }

    #region Banner
    public void ShowBanner()
    {
        if (!string.IsNullOrEmpty(bannerAdUnitID))
        {
            MaxSdk.ShowBanner(bannerAdUnitID);
        }
    }
    public void HideBanner()
    {
        if (!string.IsNullOrEmpty(bannerAdUnitID))
        {
            MaxSdk.HideBanner(bannerAdUnitID);
        }
    }
    #endregion

    #region Intertitial
    public bool IsIntertitialReady()
    {
        return MaxSdk.IsInterstitialReady(intertitialAdUnitID);
    }
    public void LoadInterstial()
    {
        if (!string.IsNullOrEmpty(intertitialAdUnitID))
        {
            MaxSdk.LoadInterstitial(intertitialAdUnitID);
        }
    }
    public void ShowIntertitial(Action finished)
    {
        if (!string.IsNullOrEmpty(intertitialAdUnitID))
        {
            MaxSdk.ShowInterstitial(intertitialAdUnitID);
        }
    }
    #endregion

    #region Rewarded Video
    public bool IsRewardedReady()
    {
        return MaxSdk.IsRewardedAdReady(rewardedAdUnitID);
    }
    public void LoadRewardedVideo()
    {
        if (!string.IsNullOrEmpty(rewardedAdUnitID))
        {
            MaxSdk.LoadRewardedAd(rewardedAdUnitID);
        }
    }
    public void ShowRewardedVideo(Action finished, Action watchFailed)
    {
        if (!string.IsNullOrEmpty(rewardedAdUnitID))
        {
            customRewardCallback = finished;
            this.watchFailed = watchFailed;
            if (IsRewardedReady())
            {
                MaxSdk.ShowRewardedAd(rewardedAdUnitID);
            }
           
        }
    }
    #endregion


    #endregion

    #region callback
    private void BannerOnOnAdLoadedEvent(string arg1, MaxSdkBase.AdInfo arg2)
    {
        Debug.Log("BannerOnOnAdLoadedEvent");
    }

    private void BannerOnOnAdLoadFailedEvent(string arg1, MaxSdkBase.ErrorInfo arg2)
    {
        Debug.Log("BannerOnOnAdLoadFailedEvent: " + arg2.Message);

    }
    private void RewardedOnAdReceivedRewardEvent(string arg1, MaxSdkBase.Reward arg2, MaxSdkBase.AdInfo arg3)
    {
        hasRewarded = true;

    }

    private void RewardedOnAdRevenuePaidEvent(string arg1, MaxSdkBase.AdInfo arg2)
    {
        Debug.Log("Rewarded_OnAdRevenuePaidEvent");
    }

    private void RewardedOnAdHiddenEvent(string arg1, MaxSdkBase.AdInfo arg2)
    {
        if (hasRewarded)
        {
            if (customRewardCallback != null)
            {
                customRewardCallback();
                customRewardCallback = null;
            }
            else
            {
                rewardCallback(rewardType, rewardAmount);
            }

            hasRewarded = false;
        }
        else
        {
            watchFailed?.Invoke();
            
        }

        if (closedCallback != null)
        {
            closedCallback();
        }
        LoadRewardedVideo();
    }

    private void RewardedOnAdDisplayFailedEvent(string arg1, MaxSdkBase.ErrorInfo arg2, MaxSdkBase.AdInfo arg3)
    {
        LoadRewardedVideo();
    }

    private void RewardedOnAdDisplayedEvent(string arg1, MaxSdkBase.AdInfo arg2)
    {
        if (openedCallback != null)
        {
            openedCallback();
        }
    }

    private void RewardedOnAdClickedEvent(string arg1, MaxSdkBase.AdInfo arg2)
    {
        
    }

    private void RewardedOnAdLoadFailedEvent(string arg1, MaxSdkBase.ErrorInfo arg2)
    {
        Debug.Log("Rewarded_OnAdLoadFailedEvent");
        rewardedRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, rewardedRetryAttempt));
        if (target != null)
        {
            target.Invoke("LoadRewardedVideo", (float)retryDelay);
        }
    }

    private void RewardedOnAdLoadedEvent(string arg1, MaxSdkBase.AdInfo arg2)
    {
        Debug.Log("RewardedOnOnAdLoadedEvent");
        rewardedRetryAttempt = 0;
    }

    private void InterstitialOnAdDisplayFailedEvent(string arg1, MaxSdkBase.ErrorInfo arg2, MaxSdkBase.AdInfo arg3)
    {
        LoadInterstial();
    }

    private void InterstitialOnAdDisplayedEvent(string arg1, MaxSdkBase.AdInfo arg2)
    {
        if (openedCallback != null)
        {
            openedCallback();
        }
    }

    private void IntertitialOnOnAdLoadedEvent(string arg1, MaxSdkBase.AdInfo arg2)
    {
        intertitialRetryAttempt = 0;
    }

    private void IntertitialOnOnAdLoadFailEvent(string arg1, MaxSdkBase.ErrorInfo arg2)
    {
        intertitialRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, intertitialRetryAttempt));
        if (target)
        {
            target.Invoke("LoadIntertitial", (float)retryDelay);
        }
    }

    private void IntertitialOnOnAdClickedEvent(string arg1, MaxSdkBase.AdInfo arg2)
    {
        throw new NotImplementedException();
    }

    private void InterstitialOnAdHiddenEvent(string arg1, MaxSdkBase.AdInfo arg2)
    {
        LoadInterstial();
        if (closedCallback != null)
        {
            closedCallback();
        }
        if (customRewardCallback != null)
        {
            customRewardCallback();
            customRewardCallback = null;
        }
    }

    #endregion


    public void Update()
    {
        throw new NotImplementedException();
    }
}
