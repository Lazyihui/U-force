using System;
using System.Collections;
using UnityEngine;

namespace UF {
    public class LoopModel {

        public FrameLoopFlag loopFlag;

        public float maintainSec;
        public float maintainTimer;

        public float triggerIntervalSec;//触发间隔
        public float triggerIntervalTimer;
        public float triggerTimes;//触发时间
        public float triggerTimesMax;

        public bool isDone; //是否结束 

        public LoopModel() { }

        public void Reset() {
            triggerTimes = 0;
            triggerIntervalTimer = 0;

            maintainTimer = maintainSec;

            isDone = false;
        }

        public void FromTM(LoopTM tm) {

            // maintainSec = tm.maintainFrame * 1 / 24f;//LOGIC_FPS
            // maintainTimer = tm.maintainFrame * 1 / 24f;//LOGIC_FPS
            // // // Debug.Log(maintainSec + maintainTimer);
            maintainSec = tm.maintainFrame;
            maintainTimer = tm.maintainFrame;

            if (!tm.loopFlag.HasFlag(FrameLoopFlag.LoopWhenTimeEnd) && tm.triggerIntervalSec == 0) {
                triggerIntervalSec = 1 * 24f;
            } else {
                triggerIntervalSec = tm.triggerIntervalSec;
            }
            triggerIntervalTimer = 0;
            triggerTimes = 0;
            triggerTimesMax = tm.triggerTimesMax;

            isDone = false;
        }

        public void Tick(float dt, out bool istrigger, out bool isNextFrame) {
            istrigger = false;
            isNextFrame = false;

            maintainTimer -= dt;
            triggerIntervalTimer -= dt;

            bool isCurFrameEnd = maintainTimer <= 0;

            if (!isCurFrameEnd) {
                if (triggerIntervalTimer <= 0) {
                    triggerIntervalTimer += triggerIntervalSec;
                    istrigger = true;
                }
                
            } else {
                triggerIntervalTimer = 0;
                isNextFrame = true;
                istrigger = true;
            }

        }

        public void CheckDone() {
            if (isDone) {
                return;
            }
            if (loopFlag == FrameLoopFlag.Once) {
                isDone = true;
            }
            if (loopFlag.HasFlag(FrameLoopFlag.LoopByTriggerTimesEnd)) {
                triggerTimes += 1;
                if (triggerTimes >= triggerTimesMax) {
                    isDone = true;
                    maintainTimer = 0;
                }
            }
             if (loopFlag.HasFlag(FrameLoopFlag.LoopWhenTimeEnd)) {
                if (maintainTimer <= 0) {
                    isDone = true;
                }
            }
        }

    }
}