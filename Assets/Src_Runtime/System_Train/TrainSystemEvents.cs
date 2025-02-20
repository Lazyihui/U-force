using System;
using UnityEngine;

namespace UF
{
    public class TrainSystemEvents
    {
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
    }
}