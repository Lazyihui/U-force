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

    }
}