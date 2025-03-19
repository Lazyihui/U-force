using System;
using System.Collections;
using UnityEngine;

namespace UF {
    public class LoopModel {

        public FrameLoopFlag loopFlag;

        // maintain 和 triggerInterval 是两个不同的概念(是分开用的)
        float maintainSec;//持续时间
        float maintainTimer;//倒计时
            
        float triggerIntervalSec;//触发间隔
        float triggerIntervalTimer;
        float triggerTimes;//触发时间
        float triggerTimesMax;

        int hitCount;//击中次数
        public void SetHitCount(int count) {
            hitCount += count;
        }

        public bool isDone; //是否结束 

        public LoopModel() { }

        public void Reset() {
            triggerTimes = 0;
            triggerIntervalTimer = 0;

            maintainTimer = maintainSec;

            hitCount = 0;

            isDone = false;
        }

        public void FromTM(LoopTM tm) {

            maintainSec = tm.maintainFrame * 1 / 24f;//LOGIC_FPS
            maintainTimer = tm.maintainFrame * 1 / 24f;//LOGIC_FPS

            if (!tm.loopFlag.HasFlag(FrameLoopFlag.LoopWhenTimeEnd) && tm.triggerIntervalSec == 0) {
                triggerIntervalSec = 1 * 24f;
            } else {
                triggerIntervalSec = tm.triggerIntervalSec;
            }
            triggerIntervalTimer = 0;
            triggerTimes = 0;
            triggerTimesMax = tm.triggerTimesMax;
            hitCount = 0;
            isDone = false;

            loopFlag = tm.loopFlag;
        }

        public void Tick(float dt, out bool istrigger, out bool out_isDone) {
            istrigger = false;
            out_isDone = false;

            bool isCurFrameEnd = maintainTimer <= 0;
            if (!isCurFrameEnd) {
                if (triggerIntervalTimer <= 0) {
                    triggerIntervalTimer += triggerIntervalSec;
                    triggerTimes += 1;
                    istrigger = true;
                }

            } else {
                triggerIntervalTimer = 0;
                out_isDone = true;
                istrigger = true;
            }
            maintainTimer -= dt;
            triggerIntervalTimer -= dt;
        }

        public void CheckDone() {
            if (isDone) {
                return;
            }

            if (loopFlag == FrameLoopFlag.Once) {
                Debug.Log("结束Once");
                isDone = true;
            }

            if (loopFlag.HasFlag(FrameLoopFlag.LoopByTriggerTimesEnd)) {
                if (triggerTimes >= triggerTimesMax) {
                    Debug.Log("结束triggerTimes" + triggerTimes);
                    isDone = true;
                }
            }

            if (loopFlag.HasFlag(FrameLoopFlag.LoopWhenTimeEnd)) {
                if (maintainTimer <= 0) {
                    Debug.Log("结束maintainTimer" + maintainTimer);
                    isDone = true;
                }
            }

            if (loopFlag.HasFlag(FrameLoopFlag.HitEnd)) {
                if (hitCount > 0) {
                    Debug.Log("结束hitCount" + hitCount);
                    isDone = true;
                }
            }
        }

    }
}