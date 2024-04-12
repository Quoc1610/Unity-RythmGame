using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Quoc;
using Quoc_Core;

namespace Quoc
{
    public class BaseUI : MonoBehaviour
    {
        public UIIndex uiIndex;
        private RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public virtual void OnInit()
        {

        }

        public virtual void OnSetup(UIParam param=null)
        {

        }
        public virtual void OnShow(UIParam param = null)
        {
            
        }
        public virtual void OnHide()
        {

        }
        public void ShowUI(UIParam param = null, Action callback = null)
        {
            gameObject.SetActive(true);
            rectTransform.SetAsLastSibling();
            OnSetup(param);
            OnShow();

            if (callback != null)
            {
                callback();
            }
        }
        public void HideUI(Action callback = null)
        {
            OnHide();
            gameObject.SetActive(false);
            if (callback != null)
            {
                callback();
            }
        }

        public virtual void OnCloseClick()
        {
            Quoc_SoundManager.Instance.PlaySoundFX(SoundFxIndex.Click);
            //Hide UI from UIMANAGER
            UIManager.Instance.HideUI(this);
        }
    }
}