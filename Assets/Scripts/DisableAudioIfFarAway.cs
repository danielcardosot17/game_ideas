using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAudioIfFarAway : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioListener audioListener;
    private float distanceFromPlayer;
    // Start is called before the first frame update
    void Start()
    {
        audioListener = FindObjectOfType<AudioListener>();
        audioSource = GetComponent<AudioSource>();
        DisableAudioSource();
        InvokeRepeating("CheckDistance", 1, 1);
    }

    private void CheckDistance()
    {
        distanceFromPlayer = Vector2.Distance(audioListener.transform.position, transform.position);
        if(distanceFromPlayer > audioSource.maxDistance)
        {
            if(audioSource.isPlaying) DisableAudioSource();
        }
        else
        {
            if(!audioSource.isPlaying) EnableAudioSource();
        }
    }

    private void EnableAudioSource()
    {
        audioSource.enabled = true;
        audioSource.Play();
    }

    private void DisableAudioSource()
    {
        audioSource.Pause();
        audioSource.enabled = false;
    }
}
