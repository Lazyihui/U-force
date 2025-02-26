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

        static void Stretch(GameSystemContext ctx, RopeEntity rope, float dt) {
            rope.curLength += rope.stretchSpeed * dt;

            // 超出最大长度
            if (rope.curLength >= rope.stretchLength) {
                rope.curLength = rope.stretchLength;
                rope.EnterShortening();
            }

            // 更新绳子长度
            var scale = rope.transform.localScale;
            rope.transform.localScale = new Vector3(rope.curLength, 1, 1);
            rope.transform.position = rope.originalPos + new Vector2(rope.curLength / 2, 0);

            // 更新绳子位置
            var pos = rope.transform.position;
        }

        static void shortening(GameSystemContext ctx, RopeEntity rope, float dt) {

            rope.curLength -= rope.shorteningSpeed * dt;

            // 超出最小长度
            if (rope.curLength <= 1) {
                rope.curLength = 0;
                rope.UnSpawn(rope);
            }

            // 更新绳子长度
            var scale = rope.transform.localScale;
            rope.transform.localScale = new Vector3(rope.curLength, 1, 1);
            rope.transform.position = rope.originalPos + new Vector2(rope.curLength / 2, 0);

            // 更新绳子位置
            var pos = rope.transform.position;

        }





    }
}