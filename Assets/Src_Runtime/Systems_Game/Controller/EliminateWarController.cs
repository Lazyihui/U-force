using System;
using UnityEngine;

namespace UF {
    public static class EliminateWarController {
        public static void Enter(GameSystemContext ctx) {

        }

        public static void PreTick(GameSystemContext ctx, float dt) {

        }

        public static void Tick(GameSystemContext ctx, float dt) {
            // 人物移动柜
            var game = ctx.gameEntity;
            var role = ctx.Get_Owner();

        

        }

        public static void LastTick(float dt) {

        }
    }
}