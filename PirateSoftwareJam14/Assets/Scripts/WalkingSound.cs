using Cinemachine;
using NaughtyAttributes.Test;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class WalkingSound : MonoBehaviour
{
    public AudioClip[] FootstepSFX;
    public AudioClip[] Footstep2SFX;

    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    void FootstepEvent()
    {
        AudioClip clip = FootstepSFX[(int)Random.RandomRange(0,FootstepSFX.Length)];
        source.clip = clip;
        source.pitch = .8f;
        source.Play();

    }
    void FootstepEvent2()
    {
        AudioClip clip = Footstep2SFX[(int)Random.RandomRange(0, FootstepSFX.Length)];
        source.clip = clip;
        source.pitch = .8f;
        source.Play();

    }
    void SprintpEvent()
    {
        AudioClip clip = FootstepSFX[(int)Random.RandomRange(0, FootstepSFX.Length)];
        source.clip = clip;
        source.pitch = 1;
        source.Play();

    }
    void SprintpEvent2()
    {
        AudioClip clip = Footstep2SFX[(int)Random.RandomRange(0, FootstepSFX.Length)];
        source.clip = clip;
        source.pitch = 1;
        source.Play();

    }
}
