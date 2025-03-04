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
        public int stageCurType;//当前关卡类型
        public int GetStageCurType() => stageCurType;
        public MapEntity mapEntity;
        public int unLockStageCount;//解锁的关卡 
        public int GetUnLockStageCount() => unLockStageCount;
        public int chapterCurID;//当前章节
        public int GetChapterCurID() => chapterCurID;
        public int unLockChapterID;//解锁的章节
        public int GetUnLockChapterID() => unLockChapterID;

        public int slotMax;//卡槽最大数量 也是按钮数量
        public int GetSlotMax() => slotMax;
        public void Set_slotMax(int slotMax) => this.slotMax = slotMax;

        public int starAllCount;//总星数
        public int GetstarAllCount() => starAllCount;

        // 结算信息
        public int ownerGetHurt;
        public void SetOwnerGetHurt(int value) => ownerGetHurt = value;
        public int GetRoleGetHurt() => ownerGetHurt;
        public int shootTimes;//射击次数
        public void SetHitTimes(int value) => shootTimes = value;
        public int HitTimes;//命中次数
        public void SetHitNumberMax(int value) => HitTimes = value;

        public float GetRoleHitRate() {
            if (HitTimes == 0) {
                return 0;
            }
            return (float)HitTimes / shootTimes;
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
            Set_stage_AllStar();
            return starCount;
        }
        public void Set_stage_PassedStar(int stageID, int star) {
            stage_PassedStar[stageID] = star;
        }

        public void Set_stage_AllStar() {
            int len = stage_PassedStar.Count;
            starAllCount = 0;

            for (int i = 0; i < len; i++) {
                i += 1;
                int star = stage_PassedStar[i];
                starAllCount += star;
            }
        }

        public PlayerEntity() {
            stageCurID = 0;
            stageCurType = 0;
            chapterCurID = 1;//当前章节
            unLockChapterID = 2;//解锁的章节
            unLockStageCount = 10;//最大就是10

            EnemyTypeIDs = new List<int>();
            stage_Passed = new HashSet<int>();
            stage_PassedStar = new Dictionary<int, int>();
            stage_reached = new HashSet<int>();
        }

        public void Clear() {
            stageCurType =  0;
            chapterCurID = 1;
            ownerGetHurt = 0;
            shootTimes = 0;
            HitTimes = 0;
        }
        public void Reset() {
            stageCurID = 0;
            stageCurType = 0;
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

        public void SetStageCurType(int stageType) {
            stageCurType = stageType;
        }//设置当前关卡类型

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