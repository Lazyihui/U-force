using System;
using System.Collections.Generic;
using UnityEngine;


namespace UF {

    public class GameSaveModel {


        #region  要存储的数据
        public int slotID;// 存档ID

        // 关卡信息
        public HashSet<int> stage_Passed;//通关的关卡
        public HashSet<StarSignature> stage_PassedStar;//通关关卡的星级
        public HashSet<int> stage_UnLock;//到达的关卡

        public int unLockChapterID;//解锁的章节2
        public int starAllCount;//总星数

        public int GetUnLockChapterID() => unLockChapterID;
        public int GetstarAllCount() => starAllCount;

        #endregion
        public int unLockStageCount;//解锁的关卡数量

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
            Add_Stage_PassedStar(stageCurID, starCount);
            Set_stage_AllStar();
            return starCount;
        }
        public void Add_Stage_PassedStar(int stageID, int star) {
            // 创建一个临时的 StarSignature 用于查找
            StarSignature temp = new StarSignature(stageID, 0);

            // 查找是否已存在相同 stageID 的记录
            bool found = false;
            foreach (var item in stage_PassedStar) {
                if (item.stageID == stageID) {
                    // 找到后更新星级
                    temp = item;
                    if (temp.starCount >= star) return;
                    temp.starCount = star; // 修改星级
                    stage_PassedStar.Remove(item); // 移除旧记录
                    stage_PassedStar.Add(temp); //   添加更新后的记录
                    found = true;
                    Debug.Log("更新关卡的星级: " + stageID + " 新星级: " + star);
                    break;
                }
            }

            // 如果没有找到，则添加新记录
            if (!found) {
                stage_PassedStar.Add(new StarSignature(stageID, star));
                Debug.Log("添加新关卡的星级: " + stageID + " 星级: " + star);
            }
        }
        public void Set_stage_AllStar() {
            starAllCount = 0;
            foreach (var stars in stage_PassedStar) {
                starAllCount += stars.starCount;
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
            stage_PassedStar = new HashSet<StarSignature>();
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

        public int GetStagePassedCount() {
            return stage_Passed.Count;
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
            return stage_UnLock.Contains(stageID);
        }

        public int GetStagePassedStar(int stageID) {
            foreach (var stars in stage_PassedStar) {
                if (stars.stageID == stageID) {
                    return stars.starCount;
                }
            }
            return 0;
        }
    }
}