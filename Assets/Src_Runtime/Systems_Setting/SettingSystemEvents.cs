using System;
using System.Collections.Generic;
using UnityEngine;

namespace UF {
    public class SettingSystemEvents {
        #region Setting
        public Action OnSettingBackClickHandle;
        public void OnSettingBackClick() {
            if (OnSettingBackClickHandle != null) {
                OnSettingBackClickHandle();
            }
        }

        // 分辨率
        public Action OnSettingReselustion1920ClickHandle;
        public void OnSettingReselustion1920Click() {
            if (OnSettingReselustion1920ClickHandle != null) {
                OnSettingReselustion1920ClickHandle();
            }
        }

        public Action OnSettingReselustion1600ClickHandle;
        public void OnSettingReselustion1600Click() {
            if (OnSettingReselustion1600ClickHandle != null) {
                OnSettingReselustion1600ClickHandle();
            }
        }

        public Action OnSettingReselustion640ClickHandle;
        public void OnSettingReselustion640Click() {
            if (OnSettingReselustion640ClickHandle != null) {
                OnSettingReselustion640ClickHandle();
            }
        }
        public Action<bool> OnSettingFullScreenToggleHandle;
        public void OnSettingFullScreenToggle(bool isOn) {
            if (OnSettingFullScreenToggleHandle != null) {
                OnSettingFullScreenToggleHandle(isOn);
            }
        }

        // 声音
        public Action<float> OnSettingMusicSliderChangeHandle;
        public void OnSettingMusicSliderChange(float value) {
            if (OnSettingMusicSliderChangeHandle != null) {
                OnSettingMusicSliderChangeHandle(value);
            }
        }

        public Action<float> OnSettingSFXSliderChangeHandle;
        public void OnSettingSFXSliderChange(float value) {
            if (OnSettingSFXSliderChangeHandle != null) {
                OnSettingSFXSliderChangeHandle(value);
            }
        }
        #endregion
    }
}