using System;
using System.Collections;
using UnityEngine;


namespace UF {

    public static class GameDomain {
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
                Debug.Log("通关的关卡: " + (passedCount + 1));
                playerSaveData.AddStageReached(passedCount + 1);
            }
        }

        public static void Slot_SaveCurrent(GameSystemContext ctx) {
            var game = ctx.gameEntity;
            var saveCore = ctx.saveCore;
            saveCore.Slot_Save(game.playerSaveData);

            for (int i = 0; i < game.playerSaveData.stage_Passed.Count; i++) {
                if (game.playerSaveData.stage_Passed.Contains(i)) {
                    Debug.Log("已经通关的关卡: " + i);
                }
            }

        }

    }
}