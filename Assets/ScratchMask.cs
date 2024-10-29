using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScratchMask : MonoBehaviour
{
    public GameObject waterEffectGO;
    public Animator waterEffectAnimtor;

    private void OnEnable()
    {
        //StartCoroutine(DestroyWaterEffectWithDelay());   
    }

    //public IEnumerator DestroyWaterEffectWithDelay() 
    //{
        //Color transparentColor = new Color(0f, 0f, 0f,0f);
        //waterEffectSR.enabled = true;
        //waterEffectSR.color = transparentColor;
        //yield return new WaitForSeconds(0.25f);
        //waterEffectSR.color = transparentColor;
        //waterEffectSR.enabled = false;
        //Destroy(waterEffectGO);
    //}
}
