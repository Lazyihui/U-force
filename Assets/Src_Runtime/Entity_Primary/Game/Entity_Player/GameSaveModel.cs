using System;
using System.Collections.Generic;
using UnityEngine;


namespace UF {

    public class GameSaveModel {


        #region  要存储的数据
        public int slotID;// 存档ID

        // 关卡信息
        public HashSet<int> stage_Passed;//通关的关卡
        public Dictionary<int, int> stage_PassedStar;//通关关卡的星级
        public HashSet<int> stage_UnLock;//到达的关卡

        public int unLockStageCount;//解锁的关卡 
        public int unLockChapterID;//解锁的章节
        public int starAllCount;//总星数

        public int GetUnLockChapterID() => unLockChapterID;
        public int GetUnLockStageCount() => unLockStageCount;
        public int GetstarAllCount() => starAllCount;

        #endregion
        public int stageCurID;//当前关卡
        public int GetStageCurID() => stageCurID;
        public int stageCurType;//当前关卡类型
        public int GetStageCurType() => stageCurType;
        public MapEntity mapEntity;
        public int chapterCurID;//当前章节
        public int GetChapterCurID() => chapterCurID;

        public int slotMax;//卡槽最大数量 也是按钮数量
        public int GetSlotMax() => slotMax;
        public void Set_slotMax(int slotMax) => this.slotMax = slotMax;

        // 可以生成怪物的TypeID
        public List<int> EnemyTypeIDs;

        // 结算信息
        public int ownerGetHurt;
        public void SetOwnerGetHurt(int value) => ownerGetHurt = value;
        public int GetRoleGetHurt() => ownerGetHurt;
        public int shootTimes;//射击次数
        public void SetShootTimes(int value) => shootTimes = value;
        public int HitTimes;//命中次数
        public void SetHitTimes(int value) => HitTimes = value;

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
            starAllCount = 0;
            foreach (var stars in stage_PassedStar.Values) {
                starAllCount += stars;
            }
        }

        public GameSaveModel() {
            slotID = 0;
            stageCurID = 0;
            stageCurType = 0;
            chapterCurID = 1;//当前章节
            unLockChapterID = 2;//解锁的章节
            unLockStageCount = 10;//最大就是10

            EnemyTypeIDs = new List<int>();
            stage_Passed = new HashSet<int>();
            stage_PassedStar = new Dictionary<int, int>();
            stage_UnLock = new HashSet<int>();

            // 第一关先进去
            AddStageReached(1);
            AddStagePassed(1);

        }

        public void Clear() {

            stageCurType = 0;
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
            stage_UnLock.Clear();
        }

        public void AddStageReached(int stageID) {
            stage_UnLock.Add(stageID);
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

        public void FeachStagePassed() {
            // 遍历所有的关卡
            foreach (var stageID in stage_UnLock) {
                if (stage_Passed.Contains(stageID)) {
                    Debug.Log("通关的关卡: " + stageID);
                    continue;
                }
                // 如果没有通关就跳出
                break;
            }
        }

        public int GetStagePassedCount() {
            return stage_Passed.Count;
        }

        public void AddStagePassedStar(int stageID, int star) {
            stage_PassedStar[stageID] = star;
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
            return stage_UnLock.Contains(stageID);
        }

        public int GetStagePassedStar(int stageID) {
            if (stage_PassedStar.TryGetValue(stageID, out int star)) {
                return star;
            }
            return 0;
        }

    }
}