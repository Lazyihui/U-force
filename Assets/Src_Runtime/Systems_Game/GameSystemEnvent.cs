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

        public Action<bool> OnGameContinueOverHandle;
        public void OnGameOver(bool isWin) {
            if (OnGameContinueOverHandle != null) {
                OnGameContinueOverHandle(isWin);
            }
        }

        #region 图鉴
        public Action OnGameIllusCloseClickHandle;
        public void OnGameIllusCloseClick() {
            if (OnGameIllusCloseClickHandle != null) {
                OnGameIllusCloseClickHandle();
            }
        }

        public Action<int> OnGameIllusCardClickHandle;
        public void OnGameIllusCardClick(int a) {
            if (OnGameIllusCardClickHandle != null) {
                OnGameIllusCardClickHandle(a);
            }
        }

        public Action<int> OnGameIllusStuffCardClickHandle;
        public void OnGameIllusStuffCardClick(int a) {
            if (OnGameIllusStuffCardClickHandle != null) {
                OnGameIllusStuffCardClickHandle(a);
            }
        }
        #endregion
    }
}
