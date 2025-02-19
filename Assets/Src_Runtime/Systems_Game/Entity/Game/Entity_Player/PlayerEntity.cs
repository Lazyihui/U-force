using System;
using System.Collections.Generic;
using UnityEngine;


namespace UF {

    public class PlayerEntity {

        // public int slotID;
        // 关卡信息

        HashSet<int> stage_Passed;//通关的关卡
        Dictionary<int, int> stage_PassedStar;//通关关卡的星级

        HashSet<int> stage_reached;//到达的关卡
        public int stageCurID;//当前关卡
        public int unLockStageCount;//解锁的关卡 
        public int chapterCurID;//当前章节
        public int unLockChapterID;//解锁的章节


        public PlayerEntity() {
            stageCurID = 0;
            chapterCurID = 1;//当前章节
            unLockChapterID = 2;//解锁的章节
            unLockStageCount = 10;//最大就是10


            stage_Passed = new HashSet<int>();
            stage_PassedStar = new Dictionary<int, int>();
            stage_reached = new HashSet<int>();
        }

        public void Reset() {
            stageCurID = 0;
            chapterCurID = 0;
            unLockChapterID = 0;

            stage_Passed.Clear();
            stage_PassedStar.Clear();
            stage_reached.Clear();
        }



        public void SetStagePassed(int stageID) {
            stage_Passed.Add(stageID);
        }

        public void SetStagePassedStar(int stageID, int star) {
            stage_PassedStar[stageID] = star;
        }

        public void SetStageReached(int stageID) {
            stage_reached.Add(stageID);
        }

        public void SetStageCurID(int stageID) {
            stageCurID = stageID;
        }

        public void SetChapterCurID(int chapterID) {
            chapterCurID = chapterID;
        }

        public bool IsStagePassed(int stageID) {
            return stage_Passed.Contains(stageID);
        }

        public bool IsStageReached(int stageID) {
            return stage_reached.Contains(stageID);
        }

        public int GetStageCurID() {
            return stageCurID;
        }

        public int GetUnLockStageCount() {
            return unLockStageCount;
        }

        public int GetChapterCurID() {
            return chapterCurID;
        }

        public int GetUnLockChapterID() {
            return unLockChapterID;
        }

        public int GetStagePassedStar(int stageID) {
            if (stage_PassedStar.TryGetValue(stageID, out int star)) {
                return star;
            }
            return 0;
        }

    }
}