using System;
using System.Collections.Generic;
using UnityEngine;

namespace UF {
    public class UIEventCenter {

        //Login
        public Action OnLoginStartHandle;
        public void OnLoginStart() {
            if (OnLoginStartHandle != null) {
                OnLoginStartHandle();
            }
        }
        //Game_Defend
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
    }
}