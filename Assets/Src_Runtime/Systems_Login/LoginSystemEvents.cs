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

        public Action OnDetailCloseClickHandle;
        public void OnDetailCloseClick() {
            if (OnDetailCloseClickHandle != null) {
                OnDetailCloseClickHandle();
            }
        }
        #endregion
    }
}