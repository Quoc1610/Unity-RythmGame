using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Quoc_CharacterDataBinding : MonoBehaviour
{
    [SerializeField] private Animator characterAnimator;
    [SerializeField] private bool isBoy = false;

    private void Awake()
    {
        characterAnimator = GetComponent<Animator>();
    }
    public void SetAnimationCharacter(float index)
    {
        characterAnimator.SetFloat("Index", index);
        if (isBoy && index >= 5)
        {
            //delay back to idle animation
            CancelInvoke("DelayFinishAnimationCharacter");
            Invoke("DelayFinishAnimationCharacter", 0.5f);
        }
    }
    private void DelayFinishAnimationmCharacter()
    {
        characterAnimator.SetFloat("index", 0);
    }
}
