using System;
using UnityEngine;


namespace UF {
    public class PauseSystemEvents {
        public Action OnPauseContinueHandle;
        public void OnContinue() {
            if (OnPauseContinueHandle != null) {
                OnPauseContinueHandle();
            }
        }

        public Action OnPauseIllustratedHandle;
        public void OnIllustrated() {
            if (OnPauseIllustratedHandle != null) {
                OnPauseIllustratedHandle();
            }
        }

        public Action OnPauseExitGameHandle;

        public void OnExitGame() {
            if (OnPauseExitGameHandle != null) {
                OnPauseExitGameHandle();
            }
        }

        #region BackSelection
        public Action OnCloseWarningHandle;
        public void OnCloseWarning() {
            if (OnCloseWarningHandle != null) {
                OnCloseWarningHandle();
            }
        }

        public Action OnContinueWarningHandle;
        public void OnContinueWarning() {
            if (OnContinueWarningHandle != null) {
                OnContinueWarningHandle();
            }
        }

        public Action OnResetWarningHandle;
        public void OnResetWarning() {
            if (OnResetWarningHandle != null) {
                OnResetWarningHandle();
            }
        }

        public Action OnQuitWarningHandle;
        public void OnQuitWarning() {
            if (OnQuitWarningHandle != null) {
                OnQuitWarningHandle();
            }
        }
        #endregion

        #region Train Back
        public Action OnTrainBackCloseClickHandle;
        public void OnTrainBackCloseClick() {
            if (OnTrainBackCloseClickHandle != null) {
                OnTrainBackCloseClickHandle();
            }
        }

        public Action OnTrainBackContinueClickHandle;
        public void OnTrainBackContinueClick() {
            if (OnTrainBackContinueClickHandle != null) {
                OnTrainBackContinueClickHandle();
            }
        }

        public Action OnTrainBackQuitClickHandle;
        public void OnTrainBackQuitClick() {
            if (OnTrainBackQuitClickHandle != null) {
                OnTrainBackQuitClickHandle();
            }
        }
        #endregion
    }
}