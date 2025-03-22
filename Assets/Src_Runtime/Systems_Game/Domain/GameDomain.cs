using System;
using System.Collections;
using UnityEngine;


namespace UF {

    public static class GameDomain {
        // 加载存档
        public static void Slot_LoadGame(int slotID, GameEntity game, SaveCore saveCore) {
            // 1.读取存档数据
            saveCore.Slot_Full_TryLoad(slotID, out var dto);
            // 2.赋值存档数据
            game.FromDTO(dto);
            Debug.Log($"加载存档成功, slotID: {slotID}");
        }
        public static bool isGameWin(GameSystemContext ctx) {
            var game = ctx.gameEntity;
            int len = ctx.roleRepository.TakeAll(out var roles);

            if (game.curWaves >= game.maxWaves && len == 1) {
                return true;
            }
            return false;
        }

        public static void Add_ReachStage(GameSystemContext ctx) {
            var game = ctx.gameEntity;
            var playerSaveData = game.playerSaveData;

            int passedCount = playerSaveData.GetStagePassedCount();
            int stageAllCount = playerSaveData.GetstarAllCount();
            if (stageAllCount >= passedCount * 1.5f) {
                playerSaveData.AddStageReached(passedCount + 1, StagePassStatus.Reached);
            }
            // 保存数据
            GameDomain.Slot_SaveCurrent(ctx);
        }

        public static void Slot_SaveCurrent(GameSystemContext ctx) {
            var game = ctx.gameEntity;
            var saveCore = ctx.saveCore;
            saveCore.Slot_Full_Save(game);
            Debug.Log($"存档成功, slotID:");
        }

    }
}