using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quoc;
using UnityEngine.UI;
using TMPro;
using System;
using Random = UnityEngine.Random;
using DG.Tweening;
using Quoc_Core;

namespace Quoc
{
	public class Quoc_UILuckyDraw : BaseUI
	{
        [SerializeField] private Image imgDraw;
        [SerializeField] private TextMeshProUGUI txtCountDown;
        [SerializeField] private List<TextMeshProUGUI> lsTextCoins = new List<TextMeshProUGUI>();


        [SerializeField] private Button btnFree;

        private double timerCountdown;
        private const double ValueTimerCountDown = 7199;
        private bool isShowCountdown = false;
        private bool isAds;
		public override void OnInit()
        {
            base.OnInit();
        }
        public override void OnSetup(UIParam param = null)
        {
            base.OnSetup(param);
            timerCountdown = (Quoc_GameManager.Instance.GameSave.CountDownLuckyDraw -
                TimeSpan.FromTicks(DateTime.Now.Ticks).TotalSeconds);

            if (timerCountdown > 0)
            {
                isShowCountdown = true;
            }
            else
            {
                isShowCountdown = false;
                txtCountDown.text = "FREE";
            }

            btnFree.interactable = !isShowCountdown;

            //set text coin from config Lucky Draw

            for(int i = 0; i < lsTextCoins.Count; i++)
            {
                ConfigLuckyDrawData luckyDrawData = ConfigLuckyDraw.GetConfigLuckyDrawData(i);
                lsTextCoins[i].text = luckyDrawData.coin.ToString();
            }

            imgDraw.transform.eulerAngles = new Vector3(0, 0, -30);

        }

        private void Spin()
        {
            float randTimer = Random.Range(3.5f, 5f);
            Transform transWheelCircle = imgDraw.transform;
            transWheelCircle.eulerAngles = new Vector3(0, 0, -30);

            float pieceAngles = 360 / lsTextCoins.Count;
            float halfPieceAngle = pieceAngles / 2;
            float halfPieceAngleWithPadding = halfPieceAngle - (halfPieceAngle / 4f);

            int randIndex = Random.Range(0, lsTextCoins.Count);
            Debug.Log("random:" + randIndex + " " + lsTextCoins[randIndex].text);
            float angle = -(pieceAngles * randIndex);

            float rightOffset = (angle - halfPieceAngleWithPadding) % 360;
            float leftOffset = (angle + halfPieceAngleWithPadding) % 360;

            float randomAngle = randIndex*60-30;
            randomAngle = randIndex * 60 + 30;

            Vector3 targetRotation = Vector3.back * (randomAngle + 2 * 360 * 5);
            //Debug.Log("Rotation: " + targetRotation);
            float prevAngle, curAngle;
            prevAngle = curAngle = transWheelCircle.eulerAngles.z;

            transWheelCircle.DORotate(targetRotation, randTimer,RotateMode.FastBeyond360).SetEase(Ease.InOutQuart)
                .OnUpdate(()=>
                {
                    float diff = Mathf.Abs(prevAngle - curAngle);

                    if (diff >= halfPieceAngle)
                    {
                        prevAngle = curAngle;
                    }

                    curAngle = transWheelCircle.eulerAngles.z;
                }).OnComplete(()=>
                {
                    Timer.DelayedCall(1, () =>
                    {
                        int indexLuckyDraw = Mathf.Abs(Mathf.RoundToInt(transWheelCircle.eulerAngles.z / 60));
                       // Debug.Log("index: " + indexLuckyDraw + " " + lsTextCoins[indexLuckyDraw].text);
                        //get config depend Index
                        ConfigLuckyDrawData luckyDrawData = ConfigLuckyDraw.GetConfigLuckyDrawData(randIndex);
                        //show reward
                        UIManager.Instance.ShowUI(UIIndex.UIReward, new RewardParam()
                        {
                            valueCoin = luckyDrawData.coin
                        });
                        if (!isAds)
                        {
                            timerCountdown = ValueTimerCountDown;
                            isShowCountdown = true;
                            //show timer
                            ShowTimer();
                        }
                    }, this);
                });
        }
        private void ShowTimer()
        {
            int second = (int)(timerCountdown % 60);
            int minutes = (int)(timerCountdown / 60) % 60;
            int hour = (int)((timerCountdown / 60) / 60) % 60;
            txtCountDown.text = string.Format("{0:0}:{1:00}:{2:00}", hour, minutes, second);
        }

        private void Update()
        {
            if (isShowCountdown)
            {
                timerCountdown -= Time.deltaTime;
                ShowTimer();
                if (timerCountdown <= 0)
                {
                    timerCountdown = 0;
                    isShowCountdown = false;
                    txtCountDown.text = "FREE";
                    btnFree.interactable = true;
                }
            }
        }

        public void OnFree_Clicked()
        {
            isAds = false;
            Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.Click);
            Quoc_GameManager.Instance.GameSave.CountDownLuckyDraw = TimeSpan.FromTicks(DateTime.Now.Ticks).TotalSeconds + ValueTimerCountDown;
            btnFree.interactable = false;
            Spin();

        }

        public void OnAds_Clicked()
        {
            
            Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.Click);
            //show ad
            AdManager.Instance.ShowRewardedAds(() =>
            {
                isAds = true;
                Spin();
            });
            
        }

        public override void OnCloseClick()
        {
            Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.Click);
            base.OnCloseClick();
            //show inter ads'
            if (UIManager.Instance.FindUIVisible(UIIndex.UIMainMenu) != null)
            {
                Quoc_UIMainMenu uIMainMenu = (Quoc_UIMainMenu)UIManager.Instance.FindUIVisible(UIIndex.UIMainMenu);
                //update cur
            } 
        }
    }
}