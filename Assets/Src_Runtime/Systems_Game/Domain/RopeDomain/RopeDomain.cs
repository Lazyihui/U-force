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

            // 设置line
            rope.Set_OriginalPos(origionPos);
            rope.Set_TargetPos();
            rope.Set_Line();

            // 抓取
            var pos = rope.hook.transform.position;
            detectionStuffByPos(ctx, rope, pos);

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

        static void detectionStuffByPos(GameSystemContext ctx, RopeEntity rope, Vector3 pos) {
            int len = ctx.stuffRepository.TakeAll(out var stuffs);

            for (int i = 0; i < len; i++) {
                var stuff = stuffs[i];

                float dis = Vector3.Distance(stuff.TF_GetPos(), pos);

                if (dis < 1) {
                    rope.EnterShortening();
                    stuff.TF_SetPos(pos);

                    // TODO: 物品掉落(成功获取) 和 根据物品质量设置回收速度
                }
            }
        }
    }
}