using System;
using System.Collections.Generic;
using GameFunctions;
using UnityEngine;

namespace UF {

    public static class GameMainDomain {
        // 赋值存档
        public static void Game_Load(GameSaveModel model, SaveDTO save) {
            // 给 GameSaveModel 赋值
            model.slotID = save.slotID;

            model.stage_Passed = new HashSet<int>();
            foreach (Int32 u32 in save.stage_Passed) {
                model.stage_Passed.Add(u32);
            }

            model.stage_PassedStar = new Dictionary<int, int>();
            for (int i = 0; i < save.stage_PassedStar.Count; i++) {
                int value = save.stage_PassedStar[i];
                model.stage_PassedStar.Add(i, value);
            }

            model.stage_UnLock = new HashSet<int>();
            foreach (Int32 u32 in save.stage_UnLock) {
                model.stage_UnLock.Add(u32);
            }

            model.unLockStageCount = save.unLockStageCount;
            model.unLockChapterID = save.unLockChapterID;
            model.starAllCount = save.starAllCount;
        }

        // 加载存档
        public static void Slot_LoadGame(GameSaveModel models, out SaveCore saveCore, int slotID) {
            saveCore = new SaveCore();
            // 找到存档
            bool has = saveCore.Slot_TryLoad(slotID, out var saveData);
            if (has) {
                Debug.Log("加载成功: " + slotID);
            } else {
                Debug.LogError("加载失败: " + slotID);
            }
            // 将存档里面的数值赋值给 GameSaveModel
            Game_Load(models, saveData);
        }

        // public static void Slot_SaveCurrent(GameSaveModel model, SaveCore saveCore) {
        //     //TODO: 这里要把他存到Main的里面
        //     saveCore.Slot_Save(model);
        // }

    }
}