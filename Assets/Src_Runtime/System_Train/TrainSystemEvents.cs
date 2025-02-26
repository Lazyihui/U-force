using System;
using UnityEngine;

namespace UF {
    public class TrainSystemEvents {
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

        public Action OnTrainOverResetClickHandle; // 结束页面重新开始
        public void OnTrainOverResetClick() {
            if (OnTrainOverResetClickHandle != null) {
                OnTrainOverResetClickHandle();
            }
        }

        public Action OnTrainOverBackClickHandle; // 结束页面退出
        public void OnTrainOverBackClick() {
            if (OnTrainOverBackClickHandle != null) {
                OnTrainOverBackClickHandle();
            }
        }
    }
}