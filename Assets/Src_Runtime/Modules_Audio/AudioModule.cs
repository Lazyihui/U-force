using System;
using UnityEngine;

public class AudioModule : MonoBehaviour {
    [SerializeField] AudioSource musicSource; // 音乐
    public AudioSource Get_MusicSource() { return musicSource; }
    [SerializeField] AudioSource sfxSource; // 音效
    public AudioSource Get_SfxSource() { return sfxSource; }

    public void Ctor() {
        
    }

    public void SetMusicVolume(float value) {
        musicSource.volume = value;
    }

    public void SetSFXVolume(float value) {
        sfxSource.volume = value;
    }

    public void PlayMusic(AudioClip clip) {
        musicSource.PlayOneShot(clip);
    }

    public void PlaySfx(AudioClip clip) {
        sfxSource.PlayOneShot(clip);
    }
}