using System;
using UnityEngine;

namespace UF {

    public class GameSettingModel {

        public bool isResolutionFullScreen;//是否全屏
        public int resolutionWidth;//分辨率宽
        public int resolutionHeight;//分辨率高

        float sfxVolume;//音效音量
        public void SetSFXVolume(float volume) => sfxVolume = volume;
        public float GetSFXVolume() => sfxVolume;

        float musicVolume;//背景音乐音量
        public void SetMusicVolume(float volume) => musicVolume = volume;
        public float GetMusicVolume() => musicVolume;

        public GameSettingModel() {
            isResolutionFullScreen = true;
            resolutionWidth = Screen.width;
            resolutionHeight = Screen.height;
            sfxVolume = 1;
            musicVolume = 1;
        }

    }
}