using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireSound : MonoBehaviour
{
    public AudioSource FireSFX;
    public AudioSource PutOutSFX;
    public GameObject FireOBJ;

    
    private void OnEnable()
    {
        FireSFX.enabled = true; 
    }
    private void OnDisable()
    {
        FireSFX.enabled = false;
        PutOutSFX.Play();

    }
}
