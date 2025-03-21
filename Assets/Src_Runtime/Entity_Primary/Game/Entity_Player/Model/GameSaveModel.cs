using System;
using System.Collections.Generic;
using UnityEngine;


namespace UF {

    public class GameSaveModel {
        #region  要存储的数据
        public int slotID;// 存档ID
        public Dictionary<int, StageInfoModel> stages;

        // 关卡信息
        public int unLockChapterID;//解锁的章节2
        public int GetUnLockChapterID() => unLockChapterID;
        public int GetstarAllCount() {
            int count = 0;
            foreach (var stage in stages.Values) {
                count += stage.starCount;
            }
            return count;
        }
        #endregion
        
        public void FromDTO(in GameSaveDTO dto) {
            slotID = dto.slotID;
            unLockChapterID = dto.unLockChapterID;
            stages.Clear();
            foreach (var stageDTO in dto.all) {
                StageInfoModel stage = new StageInfoModel();
                stage.FromDTO(stageDTO);
                stages.Add(stage.stageID, stage);
            }
        }

        public GameSaveDTO ToDTO() {
            GameSaveDTO dto;
            dto.slotID = slotID;
            dto.unLockChapterID = unLockChapterID;
            dto.all = new List<StageInfoDTO>();
            foreach (var stage in stages.Values) {
                dto.all.Add(stage.ToDTO());
            }
            return dto;
        }

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

        // 资源战结算
        int energyCount; // 能量棒
        public void SetEnergyCount(int value) => energyCount = value;
        public int GetEnergyCount() => energyCount;

        public int eliShootTimes; // 资源战射击次数
        public int GetEliShootTimes() => eliShootTimes;

        public float eliTakeTimes; // 耗时
        public int GetEliTakeTimes() => (int)eliTakeTimes;

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
            Add_Stage_PassedStar(stageCurID, starCount, StagePassStatus.Passed);
            return starCount;
        }

        public void Add_Stage_PassedStar(int stageID, int starCount, StagePassStatus passStatus) {
            bool has = stages.TryGetValue(stageID, out var stage);
            if (has) {
                stage.starCount = starCount;
                stage.passStatus = passStatus;
                stages[stageID] = stage;
            } else {
                stage = new StageInfoModel {
                    stageID = stageID,
                    starCount = starCount,
                    passStatus = passStatus,
                };
                stages.Add(stageID, stage);
            }
        }

        public GameSaveModel() {
            slotID = 0;
            stages = new Dictionary<int, StageInfoModel>();


            stageCurID = 0;
            stageCurType = 0;
            chapterCurID = 1;//当前章节
            unLockChapterID = 2;//解锁的章节
            unLockStageCount = 10;//最大就是10

            EnemyTypeIDs = new List<int>();

            // // // 第一关先进去
            // AddStageReached(1);
            // AddStagePassed(1);

            // 第一关先进去
            AddStageAll();

        }

        public void AddStageAll() {
            StageInfoModel stage1 = new StageInfoModel {
                stageID = 1,
                starCount = 0,
                passStatus = StagePassStatus.Reached,
            };
            stages.Add(stage1.stageID, stage1);
        }

        public void Clear() {

            stageCurType = 0;
            chapterCurID = 1;
            ownerGetHurt = 0;
            shootTimes = 0;
            HitTimes = 0;
            energyCount = 0;
            eliShootTimes = 0;
            eliTakeTimes = 0;
        }

        public void Reset() {
            stageCurID = 0;
            stageCurType = 0;
            chapterCurID = 1;
            unLockChapterID = 0;

            stages.Clear();
        }

        public void AddStageReached(int stageID) {
            StageInfoModel stage = new StageInfoModel {
                stageID = stageID,
                starCount = 0,
                passStatus = StagePassStatus.Reached,
            };

            stages.Add(stageID, stage);
        }

        public void ClearEnemyTypeID() {
            EnemyTypeIDs.Clear();
        }

        public int GetRandomEnemyTypeID() {
            int index = UnityEngine.Random.Range(0, EnemyTypeIDs.Count);
            return EnemyTypeIDs[index];
        }

        public void AddStagePassed(int stageID, int starCount) {
            StageInfoModel stage = new StageInfoModel {
                stageID = stageID,
                starCount = starCount,
                passStatus = StagePassStatus.Passed,
            };
            stages.Add(stageID, stage);
        }

        public int GetStagePassedCount() {
            int PassedCount = 0;
            foreach (var stage in stages.Values) {
                if (stage.passStatus == StagePassStatus.Passed) {
                    PassedCount++;
                }
            }
            return PassedCount;
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

        public bool IsStageReached(int stageID) {
            bool isReach = false;
            foreach (var stage in stages.Values) {
                if (stage.stageID == stageID) {
                    if (stage.passStatus == StagePassStatus.Reached) {
                        isReach = true;
                        return isReach;
                    }
                }
            }
            return isReach;
        }

        public int GetStagePassedStar(int stageID) {
            foreach (var stage in stages.Values) {
                if (stage.stageID == stageID) {
                    return stage.starCount;
                }
            }
            return 0;
        }
    }
}