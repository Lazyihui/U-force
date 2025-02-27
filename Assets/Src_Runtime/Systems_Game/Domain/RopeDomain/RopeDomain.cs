using System;
using UnityEngine;

namespace UF {

    public static class RopeDomain {

        public static RopeEntity Spawn(GameSystemContext ctx, Transform muzzle) {
            var rope = GameFactory.Rope_Create(ctx.assetsModule, ctx.idService, muzzle);
            ctx.ropeEntity = rope;
            return rope;
        }

        public static void FSM_All_Tick(GameSystemContext ctx, RopeEntity rope, Transform origionPos, float dt) {
            if (rope == null) {
                return;
            }
            rope.originalPos = new Vector2(origionPos.position.x, origionPos.position.y);

            var state = rope.state;
            if (state == RopeStage.Stretch) {
                Stretch(ctx, rope, dt);
            } else if (state == RopeStage.shortening) {
                shortening(ctx, rope, dt);
            }

        }

        static void Stretch(GameSystemContext ctx, RopeEntity entity, float dt) {
            entity.curLength += entity.stretchSpeed * dt;

            // 超出最大长度
            if (entity.curLength >= entity.stretchLength) {
                entity.curLength = entity.stretchLength;
                entity.EnterShortening();
            }
            var scale = entity.rope.transform.localScale;
            var scaleHook = entity.hook.transform.localScale;

            // 更新绳子长度
            entity.rope.transform.localScale = new Vector3(entity.curLength, scale.y, scale.z);
            entity.hook.transform.localScale = new Vector3(1/entity.curLength, scaleHook.y, scaleHook.z);

        }

        static void shortening(GameSystemContext ctx, RopeEntity entity, float dt) {

            entity.curLength -= entity.shorteningSpeed * dt;

            // 超出最小长度
            if (entity.curLength <= 1) {
                entity.curLength = 0;
                entity.UnSpawn(entity);
            }
            var scale = entity.rope.transform.localScale;
            var scaleHook = entity.hook.transform.localScale;
            // // 更新绳子长度
            entity.rope.transform.localScale = new Vector3(entity.curLength, scale.y, scale.z);
            // 是钩子一直在一个大小
            entity.hook.transform.localScale = new Vector3(1/entity.curLength, scaleHook.y, scaleHook.z);
        }

    }
}