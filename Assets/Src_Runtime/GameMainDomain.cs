using System;
using System.Collections.Generic;
using GameFunctions;
using UnityEngine;

namespace UF {

    public static class GameMainDomain {
        public static void Slot_NewGame(int slotID, GameEntity game, SaveCore saveCore) {
        }

        // 加载存档
        public static void Slot_LoadGame(int slotID, GameEntity game, SaveCore saveCore) {
            // 1.读取存档数据
            saveCore.Slot_Full_TryLoad(slotID, out var dto);
            // 2.赋值存档数据
            game.FromDTO(dto);
            Debug.Log($"加载存档成功, slotID: {slotID}");
        }

    }
}