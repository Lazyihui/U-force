using System;
using UnityEngine;

namespace UF {

    public class GameSettingModel {

        public bool isResolutionFullScreen;//是否全屏
        public int resolutionWidth;//分辨率宽
        public int resolutionHeight;//分辨率高
        public float sfxVolume;//音效音量
        public float bgmVolume;//背景音乐音量

        public GameSettingModel() {
            isResolutionFullScreen = true;
            resolutionWidth = Screen.width;
            resolutionHeight = Screen.height;
            sfxVolume = 1;
            bgmVolume = 1;
        }

    }
}