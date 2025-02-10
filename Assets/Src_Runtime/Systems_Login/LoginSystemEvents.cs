using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UF {
    public class LoginSystemEvents {
        public Action OnLoginStartHandle;
        public void OnLoginStart() {
            if (OnLoginStartHandle != null) {
                OnLoginStartHandle();
            }
        }

        public Action OnTrainingHandle;
        public void OnTraining() {
            if (OnTrainingHandle != null) {
                OnTrainingHandle();
            }
        }

        public Action OnSettingHandle;
        public void OnSetting() {
            if (OnSettingHandle != null) {
                OnSettingHandle();
            }
        }

        public Action OnQuitHandle;
        public void OnQuit() {
            if (OnQuitHandle != null) {
                OnQuitHandle();
            }
        }

        #region  StageSelection

        public Action<int> OnStageClickHandle;
        public void OnStageClick(int id) {
            if (OnStageClickHandle != null) {
                OnStageClickHandle(id);
            }
        }

        public Action<int> OnChapterClickHandle;
        public void OnChapterClick(int id) {
            if (OnChapterClickHandle != null) {
                OnChapterClickHandle(id);
            }
        }

        #endregion
    }
}