using System;
using System.Collections.Generic;
using GameFunctions;
using UnityEngine;

namespace UF {

    public static class GameMainDomain {
        // 赋值存档
        public static void Game_Load(GameSaveModel model, SaveDTO save) {
            // 给 GameSaveModel 赋值
            Debug.Log("存档ID: " + save.slotID);
            model.slotID = save.slotID;

            model.stage_Passed = new HashSet<int>();
            foreach (Int32 u32 in save.stage_Passed) {
                model.stage_Passed.Add(u32);
            }

            model.stage_PassedStar = new HashSet<StarSignature>();
            for (int i = 0; i < save.stage_PassedStar.Count; i += 1) {
                model.stage_PassedStar.Add(new StarSignature(i, save.stage_PassedStar[i]));
                Debug.Log("已经通关的关卡: " + i + " 星级: " + save.stage_PassedStar[i]);
                Debug.Log(save.stage_PassedStar.Count);
            }

            model.stage_UnLock = new HashSet<int>();
            foreach (Int32 u32 in save.stage_UnLock) {
                model.stage_UnLock.Add(u32);
            }

            model.unLockChapterID = save.unLockChapterID;
            model.starAllCount = save.starAllCount;
        }

        // 加载存档
        public static void Slot_LoadGame(GameSaveModel models, out SaveCore saveCore, int slotID) {
            saveCore = new SaveCore();
            // 找到存档
            bool has = saveCore.Slot_TryLoad(slotID, out var saveData);
            // 将存档里面的数值赋值给 GameSaveModel
            Game_Load(models, saveData);
        }

    }
}