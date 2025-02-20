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

        #region CardSelection

        public Action OnSelectHeroClickHandle;

        public void OnSelectHeroClick() {
            if (OnSelectHeroClickHandle != null) {
                OnSelectHeroClickHandle();
            }
        }

        public Action OnSelectAgrClickHandle;//选择农业

        public void OnSelectAgrClick() {
            if (OnSelectAgrClickHandle != null) {
                OnSelectAgrClickHandle();
            }
        }

        public Action OnSelectIndClickHandle;//选择工业

        public void OnSelectIndClick() {
            if (OnSelectIndClickHandle != null) {
                OnSelectIndClickHandle();
            }
        }

        public Action OnSelectSciClickHandle;//选择科技

        public void OnSelectSciClick() {
            if (OnSelectSciClickHandle != null) {
                OnSelectSciClickHandle();
            }
        }

        public Action OnSelectEnterClickHandle;//选择进入

        public void OnSelectEnterClick() {
            if (OnSelectEnterClickHandle != null) {
                OnSelectEnterClickHandle();
            }
        }

        public Action OnSelectBackClickHandle;//选择返回

        public void OnSelectBackClick() {
            if (OnSelectBackClickHandle != null) {
                OnSelectBackClickHandle();
            }
        }

        public Action<int> OnSelectionCardClickHandle;//选择卡片

        public void OnSelectionCardClick(int a) {
            if (OnSelectionCardClickHandle != null) {
                OnSelectionCardClickHandle(a);
            }
        }

        public Action<int> OnConfirmClickHandle;//确认选择

        public void OnConfirmClick(int a) {
            if (OnConfirmClickHandle != null) {
                OnConfirmClickHandle(a);
            }
        }

        public Action<int> OnCardSelectedClickHandle;// 点击的卡片
        public void OnCardSelectedClick(int a) {
            if (OnCardSelectedClickHandle != null) {
                OnCardSelectedClickHandle(a);
            }
        }

        #endregion

        #region BackSelection
        public Action OnCloseWarningClickHandle;
        public void OnCloseWarningClick() {
            if (OnCloseWarningClickHandle != null) {
                OnCloseWarningClickHandle();
            }
        }

        public Action OnContinueWarningClickHandle;
        public void OnContinueWarningClick() {
            if (OnContinueWarningClickHandle != null) {
                OnContinueWarningClickHandle();
            }
        }

        public Action OnResetWarningClickHandle;
        public void OnResetWarningClick() {
            if (OnResetWarningClickHandle != null) {
                OnResetWarningClickHandle();
            }
        }

        public Action OnQuitWarningClickHandle;
        public void OnQuitWarningClick() {
            if (OnQuitWarningClickHandle != null) {
                OnQuitWarningClickHandle();
            }
        }
        #endregion

        #region WatchTower
        public Action OnViewClickHandle;
        public void OnViewClick() {
            if (OnViewClickHandle != null) {
                OnViewClickHandle();
            }
        }

        public Action<int> OnSelectionWatchCardClickHandle;
        public void OnSelectionWatchCardClick(int typeID) {
            if (OnSelectionWatchCardClickHandle != null) {
                OnSelectionWatchCardClickHandle(typeID);
            }
        }
        #endregion

        #region WatchDetail
        public Action OnDetailCloseHandle;
        public void OnDetailClose() {
            if (OnDetailCloseHandle != null) {
                OnDetailCloseHandle();
            }
        }
        #endregion

        #region Training
        public Action OnPlayer1ReadyClickHandle;
        public void OnPlayer1ReadyClick() {
            if (OnPlayer1ReadyClickHandle != null) {
                OnPlayer1ReadyClickHandle();
            }
        }

        public Action OnPlayer2ReadyClickHandle;
        public void OnPlayer2ReadyClick() {
            if (OnPlayer2ReadyClickHandle != null) {
                OnPlayer2ReadyClickHandle();
            }
        }

        public Action OnTrainTipClickHandle;
        public void OnTrainTipClick() {
            if (OnTrainTipClickHandle != null) {
                OnTrainTipClickHandle();
            }
        }

        public Action<int> OnSelectionTrainCardClickHandle;
        public void OnSelectionTrainCardClick(int a) {
            if (OnSelectionTrainCardClickHandle != null) {
                OnSelectionTrainCardClickHandle(a);
            }
        }

        public Action OnTrainTipCloseClickHandle;
        public void OnTrainTipCloseClick() {
            if (OnTrainTipCloseClickHandle != null) {
                OnTrainTipCloseClickHandle();
            }
        }

        // 游戏界面
        public Action OnTrainPauseClickHandle; // 暂停
        public void OnTrainPauseClick() {
            if (OnTrainPauseClickHandle != null) {
                OnTrainPauseClickHandle();
            }
        }

        public Action OnTrainContinueClickHandle; // 继续
        public void OnTrainContinueClick() {
            if (OnTrainContinueClickHandle != null) {
                OnTrainContinueClickHandle();
            }
        }

        public Action OnTrainIllustratedClickHandle; // 图鉴
        public void OnTrainIllustratedClick() {
            if (OnTrainIllustratedClickHandle != null) {
                OnTrainIllustratedClickHandle();
            }
        }

        public Action OnTrainQuitClickHandle; // 退出
        public void OnTrainQuitClick() {
            if (OnTrainQuitClickHandle != null) {
                OnTrainQuitClickHandle();
            }
        }
        #endregion
    }
}