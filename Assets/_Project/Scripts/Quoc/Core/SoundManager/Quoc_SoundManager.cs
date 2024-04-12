using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Hellmade.Sound;

namespace Quoc_Core
{   
    public class Quoc_SoundManager : SingletonMono<Quoc_SoundManager>
    {

        public bool isMute = false;

        [SerializeField] 
        private List<Quoc_SoundItem> soundFxItems = new List<Quoc_SoundItem>();
        
        private Dictionary<SoundFxIndex, Quoc_SoundItem> dicSoundFxs = new Dictionary<SoundFxIndex, Quoc_SoundItem>();

        [SerializeField] private List<AudioClip> lsBGMs = new List<AudioClip>();
        private AudioSource bgmSource;

        private void Awake()
        {
            for(int i = 0; i < soundFxItems.Count; i++)
            {
                Debug.Log("dict: " + soundFxItems[i].soundFxIndex);
                dicSoundFxs.Add(soundFxItems[i].soundFxIndex, soundFxItems[i]);
            }
            bgmSource = GetComponent<AudioSource>();
        }

        #region BGM
        public void AddSoundBGM(AudioClip bgmClip)
        {
            if (isMute)
            {
                return;
            }
            lsBGMs.Clear();
            lsBGMs.Add(bgmClip);

            //Get clip length
        }

        public float GetLengthBGM()
        {
            //Debug.Log("count: " + lsBGMs.Count);
            
            if (lsBGMs.Count > 0)
            {
                //Debug.Log("length: " + lsBGMs[0].length);
                return lsBGMs[0].length;
            }
            return 0;
        }

        public void PlaySoundBGM(float volume = 1, bool isLoop = false)
        {
            bgmSource.clip = lsBGMs[0];
            bgmSource.Play();
            bgmSource.volume = 0;
            bgmSource.DOFade(volume, 0.25f);
        }

        public void PauseSoundBGM()
        {
            bgmSource.Pause();

        }

        public void ResumeSoundBGM()
        {
            bgmSource.Play();
        }

        public void StopSoundBGM()
        {
            bgmSource.Stop();
        }

        public float GetCurrentTimeSoundBGM()
        {
            if (bgmSource.clip != null)
            {
                return bgmSource.time;
            }
            return 0;
        }

        #endregion

        #region SFX

        public void PlaySoundFX(SoundFxIndex soundIndex, bool isLoop = false)
        {
            if (isMute)
                return;

            EazySoundManager.PlaySound(dicSoundFxs[soundIndex].soundFxClip, isLoop);
        }

        public void StopSoundFX(SoundFxIndex soundFxIndex)
        {
            Audio audio=EazySoundManager.GetAudio(dicSoundFxs[soundFxIndex].soundFxClip);
            if (audio != null)
            {
                audio.Stop();
            }
        }

        public void StopAllSoundFx()
        {
            EazySoundManager.StopAllUISounds();
        }

        public bool CheckSoundFXAvailable(SoundFxIndex soundIndex)
        {
            Audio audio = EazySoundManager.GetAudio(dicSoundFxs[soundIndex].soundFxClip);
            if (audio != null && audio.IsPlaying)
            {
                return true;
            }
            return false;
        }
        #endregion

        public void Mute()
        {
            isMute = true;
            StopSoundBGM();
            //stop all fx
            StopAllSoundFx();
        }

        public void Unmute()
        {
            for(SoundFxIndex i = SoundFxIndex.Click; i < SoundFxIndex.COUNT; i++)
            {
                StopSoundFX(i);
            }

            isMute = false;
        }
    }

    public enum SoundFxIndex
    {
        Click = 0,
        One,
        Two,
        Three,
        Go,
        SoundMenu,
        GameOver,
        MissNote,
        ConfirmMenu,
        Victory,
        COUNT
    }
    [Serializable]
    public class Quoc_SoundItem
    {
        public SoundFxIndex soundFxIndex;
        public AudioClip soundFxClip;
    }
}
