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
                playerSaveData.AddStageReached(passedCount + 1);
            }
        }

    }
}