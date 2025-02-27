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

            var moveDir = rope.MoveDir();
            Debug.Log(moveDir);

            // 设置绳子的位置 一直在role的手上
            rope.Set_Pos(origionPos);


            // rope.Set_MoveTarget(origionPos);


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
            entity.hook.transform.localScale = new Vector3(1 / entity.curLength, scaleHook.y, scaleHook.z);

            // 旋转角度 改变rope的Rotation.z
            entity.hook.transform.localScale = new Vector3(1 / entity.curLength, scaleHook.y, scaleHook.z);
            var rotatez = Vector2.SignedAngle(Vector2.left, entity.MoveDir());
            entity.transform.rotation = Quaternion.Euler(0, 0, rotatez);
        }

        static void shortening(GameSystemContext ctx, RopeEntity entity, float dt) {

            entity.curLength -= entity.shorteningSpeed * dt;

            // 超出最小长度
            if (entity.curLength <= 1) {
                entity.curLength = 0;
                UnSpawn(ctx);
            }

            if (ctx.isRopeUnExist()) {
                return;
            }

            var scale = entity.rope.transform.localScale;
            var scaleHook = entity.hook.transform.localScale;
            // // 更新绳子长度
            entity.rope.transform.localScale = new Vector3(entity.curLength, scale.y, scale.z);
            // 是钩子一直在一个大小
            entity.hook.transform.localScale = new Vector3(1 / entity.curLength, scaleHook.y, scaleHook.z);
            var rotatez = Vector2.SignedAngle(Vector2.left, entity.MoveDir());
            entity.transform.rotation = Quaternion.Euler(0, 0, rotatez);
        }

    }
}