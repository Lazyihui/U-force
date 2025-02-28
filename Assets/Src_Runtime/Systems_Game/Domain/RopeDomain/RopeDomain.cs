using System;
using UnityEngine;

namespace UF {

    public static class RopeDomain {

        public static RopeEntity Spawn(GameSystemContext ctx, Vector2 muzzle) {
            var rope = GameFactory.Rope_Create(ctx.assetsModule, ctx.idService, muzzle);
            ctx.ropeEntity = rope;
            return rope;
        }

        public static void UnSpawn(GameSystemContext ctx) {
            var rope = ctx.ropeEntity;
            rope.TearDown(rope);
            ctx.ropeEntity = null;
        }

        public static void FSM_All_Tick(GameSystemContext ctx, Vector2 origionPos, float dt) {
            var rope = ctx.ropeEntity;
            if (rope == null) {
                return;
            }

            rope.Set_OriginalPos(origionPos);
            rope.Set_TargetPos();
            rope.Set_Line();

            var state = rope.state;
            if (state == RopeStage.Stretch) {
                Stretch(ctx, rope, dt);
            } else if (state == RopeStage.shortening) {
                shortening(ctx, rope, dt);
            }
        }


        static void Stretch(GameSystemContext ctx, RopeEntity entity, float dt) {
            var hookPos = entity.hook.transform.position;

            // hook移动
            if (hookPos.x < entity.spawnPos.x + entity.maxLength) {
                hookPos.x += entity.outMoveSpeed * dt;
                entity.Set_HookPos(hookPos);
            } else {
                entity.EnterShortening();
            }
        }

        static void shortening(GameSystemContext ctx, RopeEntity entity, float dt) {
            var hookPos = entity.hook.transform.position;

            // 移动
            hookPos = Vector2.MoveTowards(hookPos, entity.originalPos, entity.backMoveSpeed * dt);
            entity.Set_HookPos(hookPos);

            // 销毁
            float dis = Vector3.Distance(hookPos, entity.originalPos);
            if (dis <= 1) {
                UnSpawn(ctx);
            }
        }
    }
}