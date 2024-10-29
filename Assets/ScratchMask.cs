using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScratchMask : MonoBehaviour
{
    public GameObject waterEffectGO;
    public Animator waterEffectAnimtor;

    public void DestroyWaterEffect() 
    {
        Destroy(waterEffectGO);
    }
}
