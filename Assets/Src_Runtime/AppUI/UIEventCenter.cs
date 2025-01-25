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
    }
}