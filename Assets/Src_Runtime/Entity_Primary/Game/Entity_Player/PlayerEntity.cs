using System;
using System.Collections.Generic;
using UnityEngine;


namespace UF {

    public class PlayerEntity {

        // 可以生成怪物的TypeID
        public List<int> EnemyTypeIDs;

        // 关卡信息
        HashSet<int> stage_Passed;//通关的关卡
        Dictionary<int, int> stage_PassedStar;//通关关卡的星级
        HashSet<int> stage_reached;//到达的关卡
        public int stageCurID;//当前关卡
        public int GetStageCurID() => stageCurID;
        public MapEntity mapEntity;
        public int unLockStageCount;//解锁的关卡 
        public int GetUnLockStageCount() => unLockStageCount;
        public int chapterCurID;//当前章节
        public int GetChapterCurID() => chapterCurID;
        public int unLockChapterID;//解锁的章节
        public int GetUnLockChapterID() => unLockChapterID;

        public int slotMax;//卡槽最大数量 也是按钮数量
        public int GetSlotMax() => slotMax;

        // 结算信息
        public int ownerGetHurt;
        public void SetOwnerGetHurt(int value) => ownerGetHurt = value;
        public int GetRoleGetHurt() => ownerGetHurt;
        public int hitTimes;//射中次数
        public void SetHitTimes(int value) => hitTimes = value;
        public int shootAllTimes;//射击所有次数
        public void SetHitNumberMax(int value) => shootAllTimes = value;

        public float GetRoleHitRate() {
            if (shootAllTimes == 0) {
                return 0;
            }
            return (float)hitTimes / shootAllTimes;
        }

        public int GetStarCount() {
            int starCount = 1;
            if (ownerGetHurt == 0) {
                starCount++;
            }

            if (GetRoleHitRate() >= 0.7f) {
                starCount++;
            }

            // 保存星级
            Set_stage_PassedStar(stageCurID, starCount);
            return starCount;
        }
        public void Set_stage_PassedStar(int stageID, int star) {
            stage_PassedStar[stageID] = star;
        }

        public PlayerEntity() {
            stageCurID = 0;
            chapterCurID = 1;//当前章节
            unLockChapterID = 2;//解锁的章节
            unLockStageCount = 10;//最大就是10
            slotMax = 3;//卡槽最大数量

            EnemyTypeIDs = new List<int>();
            stage_Passed = new HashSet<int>();
            stage_PassedStar = new Dictionary<int, int>();
            stage_reached = new HashSet<int>();
        }

        public void Reset() {
            stageCurID = 0;
            chapterCurID = 1;
            unLockChapterID = 0;

            stage_Passed.Clear();
            stage_PassedStar.Clear();
            stage_reached.Clear();
        }

        public void AddEnemyTypeID(int typeID) {
            EnemyTypeIDs.Add(typeID);
        }

        public void ClearEnemyTypeID() {
            EnemyTypeIDs.Clear();
        }

        public int GetRandomEnemyTypeID() {
            int index = UnityEngine.Random.Range(0, EnemyTypeIDs.Count);
            return EnemyTypeIDs[index];
        }

        public void AddStagePassed(int stageID) {
            stage_Passed.Add(stageID);
        }

        public void AddStagePassedStar(int stageID, int star) {
            stage_PassedStar[stageID] = star;
        }

        public void AddStageReached(int stageID) {
            stage_reached.Add(stageID);
        }

        public void SetStageCurID(int stageID) {
            stageCurID = stageID;
        }//设置当前关卡

        public void SetChapterCurID(int chapterID) {
            chapterCurID = chapterID;
        }//设置当前章节

        public void SetUnLockChapterID(int chapterID) {
            unLockChapterID = chapterID;
        }//设置解锁的章节

        public bool IsStagePassed(int stageID) {
            return stage_Passed.Contains(stageID);
        }

        public bool IsStageReached(int stageID) {
            return stage_reached.Contains(stageID);
        }

        public int GetStagePassedStar(int stageID) {
            if (stage_PassedStar.TryGetValue(stageID, out int star)) {
                return star;
            }
            return 0;
        }

    }
}