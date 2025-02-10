using System;
using System.Collections.Generic;
using UnityEngine;

namespace UF {
    public class UIEventCenter {

        #region Login
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
        #endregion

        public Action OnQuitHandle;
        public void OnQuit() {
            if (OnQuitHandle != null) {
                OnQuitHandle();
            }
        }

        #region Game
        public Action OnGamePauseHandle;
        public void OnGamePause() {
            if (OnGamePauseHandle != null) {
                OnGamePauseHandle();
            }
        }

        public Action OnAttackHandle;
        public void OnAttack() {
            if (OnAttackHandle != null) {
                OnAttackHandle();
            }
        }

        public Action OnTechHandle;
        public void OnTech() {
            if (OnTechHandle != null) {
                OnTechHandle();
            }
        }

        public Action OnWeap1Handle;
        public void OnWeap1() {
            if (OnWeap1Handle != null) {
                OnWeap1Handle();
            }
        }

        public Action OnWeap2Handle;
        public void OnWeap2() {
            if (OnWeap2Handle != null) {
                OnWeap2Handle();
            }
        }

        public Action OnWeap3Handle;
        public void OnWeap3() {
            if (OnWeap3Handle != null) {
                OnWeap3Handle();
            }
        }

        public Action OnWeap4Handle;
        public void OnWeap4() {
            if (OnWeap4Handle != null) {
                OnWeap4Handle();
            }
        }

        #endregion


        #region Pause
        public Action OnContinueHandle;
        public void OnContinue() {
            if (OnContinueHandle != null) {
                OnContinueHandle();
            }
        }

        public Action OnIllustratedHandle;
        public void OnIllustrated() {
            if (OnIllustratedHandle != null) {
                OnIllustratedHandle();
            }
        }

        public Action OnPauseExitGameHandle;

        public void OnPauseExitGame() {
            if (OnPauseExitGameHandle != null) {
                OnPauseExitGameHandle();
            }
        }

        #endregion

        #region GameOver
        public Action OnGameOverHandle;
        public void OnGameOver() {
            if (OnGameOverHandle != null) {
                OnGameOverHandle();
            }
        }
        #endregion

        #region StageSelection

        public Action<int> OnStageClickHandle;
        public void OnStageClick(int a) {
            if (OnStageClickHandle != null) {
                OnStageClickHandle(a);
            }
        }

        public Action<int> OnChapterClickHandle;

        public void OnChapterClick(int a) {
            if (OnChapterClickHandle != null) {
                OnChapterClickHandle(a);
            }
        }

        #endregion
    }
}