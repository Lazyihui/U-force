using System;
using UnityEngine;

namespace UF {

    public class GameSystemEvents {

        #region  GamePause
        public Action OnGamePauseHandle;
        public void OnGamePause() {
            if (OnGamePauseHandle != null) {
                OnGamePauseHandle();
            }
        }
        #endregion

        #region  GameDefend_Buttin
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

        public Action OnGameOverHandle;
        public void OnGameOver() {
            if (OnGameOverHandle != null) {
                OnGameOverHandle();
            }
        }
    }
}
