using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MusicMaster : MonoBehaviour
{
    [SerializeField] private  List<Sound> musicList;
    private AudioSource audioSource;
    private int musicIndexToPlay = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StopMusic();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void PlayMusic(Sound sound)
    {
        audioSource.clip = sound.clip;
        audioSource.priority = sound.priority;
        audioSource.volume = sound.volume;
        audioSource.pitch = sound.pitch;
        audioSource.Play();
    }

    public void PlayNextMusic()
    {
        if(musicIndexToPlay >= musicList.Count) musicIndexToPlay = 0;
        if(!audioSource.isPlaying)
        {
            var sound = musicList[musicIndexToPlay];
            audioSource.clip = sound.clip;
            audioSource.priority = sound.priority;
            audioSource.volume = sound.volume;
            audioSource.pitch = sound.pitch;
            audioSource.Play();
            musicIndexToPlay++;
        }
    }

}
