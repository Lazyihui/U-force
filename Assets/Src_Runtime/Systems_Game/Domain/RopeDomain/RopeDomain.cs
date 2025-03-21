using System;
using UnityEngine;

namespace UF {

    public static class RopeDomain {

        public static RopeEntity Spawn(GameSystemContext ctx, Vector2 muzzle) {
            var rope = GameFactory.Rope_Create(ctx.assetsModule, ctx.idService, muzzle);
            ctx.ropeEntity = rope;

            var playerSaveData = ctx.gameEntity.playerSaveData;
            playerSaveData.shootTimes += 1;
            
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
            detectionRoleByPos(ctx, rope, pos);
            detectionOtherMove(ctx, rope, pos);

            var state = rope.state;
            if (state == RopeStage.Stretch) {
                Stretch(ctx, rope, dt);
            } else if (state == RopeStage.Shortening) {
                shortening(ctx, rope, dt);
            } else if (state == RopeStage.ShorteningWithStuff) {
                shorteningWithStuff(ctx, rope, dt);
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

        static void shorteningWithStuff(GameSystemContext ctx, RopeEntity entity, float dt) {
            var hookPos = entity.hook.transform.position;

            // 移动
            hookPos = Vector2.MoveTowards(hookPos, entity.originalPos, entity.backWithStuffMoveSpeed * dt);
            entity.Set_HookPos(hookPos);

            // 销毁
            if (ComparePos(entity)) {
                entity.EnterShortening();
            }

            float dis = Vector3.Distance(hookPos, entity.originalPos);
            if (dis <= 1) {
                UnSpawn(ctx);
            }
        }

        static bool ComparePos(RopeEntity rope) { // true表示离开引力场
            var pos = rope.hook.transform.position;
            Vector2 pointPos = new Vector2(9, 0);

            float dis = Vector2.Distance(pos, pointPos);

            if (dis < 10.5) { // 半径10.5(略大于stuff判断半径, 防止停在引力场内)
                return false;
            }
            return true;
        }

        static void detectionStuffByPos(GameSystemContext ctx, RopeEntity rope, Vector3 pos) {
            if (rope.state != RopeStage.Stretch) { return; }

            int len = ctx.stuffRepository.TakeAll(out var stuffs);

            for (int i = 0; i < len; i++) {
                var stuff = stuffs[i];

                float dis = Vector3.Distance(stuff.TF_GetPos(), pos);

                if (dis < 1) {
                    if (stuff.isTimeStuff) { // 时间道具
                        rope.EnterShortening();
                        StuffDomain.UnSpawn(ctx, stuff);

                        var addTime = stuff.addTime;
                        ctx.gameEntity.gameTime -= addTime;
                        if (ctx.gameEntity.gameTime < 0) {
                            ctx.gameEntity.gameTime = 0;
                        }

                        return;
                    }

                    // 其他道具
                    int stuffWeight = stuff.weight;
                    rope.backWithStuffMoveSpeed = 10f / stuffWeight;

                    if (!ComparePos(rope)) {
                        rope.EnterShorteningWithStuff();
                    }

                    if (rope.state == RopeStage.ShorteningWithStuff) {
                        stuff.isStretched = true;
                    }
                }
            }
        }

        static void detectionRoleByPos(GameSystemContext ctx, RopeEntity rope, Vector3 pos) { //障碍物和核心
            if (rope.state != RopeStage.Stretch) { return; }

            int len = ctx.roleRepository.TakeAll(out var roles);

            for (int i = 0; i < len; i++) {
                var role = roles[i];

                if (role.roleType != RoleType.Barrier) { continue; }

                var dis = Vector3.Distance(role.TF_GetPos(), pos);

                if (dis < 1) {
                    var attr = role.AttributeComponent;
                    rope.backWithStuffMoveSpeed = 10f / attr.GetHp();

                    if (!ComparePos(rope)) {
                        rope.EnterShorteningWithStuff();
                    }

                    if (rope.state == RopeStage.ShorteningWithStuff) {
                        role.state = RoleState.Stretched;
                    }
                }
            }
        }

        static void detectionOtherMove(GameSystemContext ctx, RopeEntity rope, Vector3 pos) { // 被抓取物品的移动
            int roleLen = ctx.roleRepository.TakeAll(out var roles);
            int stuffLen = ctx.stuffRepository.TakeAll(out var stuffs);

            for (int i = 0; i < roleLen; i++) {
                var role = roles[i];
                if (role.state == RoleState.Stretched) {
                    role.TF_SetPos(pos);
                    role.Move_Stretched_Stop();
                }
            }

            for (int i = 0; i < stuffLen; i++) {
                var stuff = stuffs[i];
                if (stuff.isStretched) {
                    stuff.TF_SetPos(pos);
                }
            }
        }
    }
}