using System;
using UnityEngine;

public class AudioModule : MonoBehaviour {
    [SerializeField] AudioSource musicSource; // 音乐
    public AudioSource Get_MusicSource() { return musicSource; }
    [SerializeField] AudioSource sfxSource; // 音效
    public AudioSource Get_SfxSource() { return sfxSource; }

    [SerializeField] AudioClip loginMusic;
    [SerializeField] AudioClip gameMusic;
    [SerializeField] AudioClip trainMusic;

    public void Ctor() {
        musicSource.loop = true;
        sfxSource.loop = false;
    }

    public void SetMusicVolume(float value) {
        musicSource.volume = value;
    }

    public void SetSFXVolume(float value) {
        sfxSource.volume = value;
    }

    public void PlayLoginMusic() {
        if (musicSource.clip == loginMusic && musicSource.isPlaying) { return; }

        musicSource.clip = loginMusic;
        // musicSource.Play();
    }

    public void PlayGameMusic() {
        if (musicSource.clip == gameMusic && musicSource.isPlaying) { return; }

        musicSource.clip = gameMusic;
        // musicSource.Play();
    }

    public void PlayTrainMusic() {
        if (musicSource.clip == trainMusic && musicSource.isPlaying) { return; }

        musicSource.clip = trainMusic;
        // musicSource.Play();
    }

    public void PauseMusic() {
        musicSource.Pause();
    }

    public void ContinueMusic() {
        musicSource.UnPause();
    }

    public void PlaySfx(AudioClip clip) {
        // sfxSource.PlayOneShot(clip);
    }
}