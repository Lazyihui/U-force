using System;
using System.Collections.Generic;
using GameFunctions;
using UnityEngine;

namespace UF {

    public static class GameMainDomain {
        // 赋值存档
        public static void Game_Load(GameSaveModel model, GameSaveDTO save) {
            // 给 GameSaveModel 赋值
            model.FromDTO(save);
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