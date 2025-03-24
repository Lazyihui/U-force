using System;
using UnityEngine;

public class AudioModule : MonoBehaviour {
    AudioSource musicSource; // 音乐
    public AudioSource Get_MusicSource() { return musicSource; }
    AudioSource sfxSource; // 音效
    public AudioSource Get_SfxSource() { return sfxSource; }

    public void Ctor() {
        var audioArray = gameObject.GetComponents<AudioSource>();
        musicSource = audioArray[0];
        sfxSource = audioArray[1];

        musicSource.loop = true;
        sfxSource.loop = false;
    }

    public void PlayMusic(AudioClip clip) {
        musicSource.PlayOneShot(clip);
    }

    public void PlaySfx(AudioClip clip) {
        sfxSource.PlayOneShot(clip);
    }
}