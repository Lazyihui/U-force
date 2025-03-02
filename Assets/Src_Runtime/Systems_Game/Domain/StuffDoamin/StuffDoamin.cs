using System;
using System.Collections.Generic;
using UnityEngine;


namespace UF {
    public static class StuffDoamin {
        #region life
        public static StuffEntity Spawn(GameSystemContext ctx, int typeID) {
            StuffEntity entity = GameFactory.Stuff_Create(ctx.assetsModule, ctx.templateCore, ctx.idService, typeID);
            ctx.stuffRepository.Add(entity);
            return entity;
        }

        public static StuffEntity SpawnBySpawner(GameSystemContext ctx, StuffSpawnerTM spawnerTM) {
            StuffEntity entity = GameFactory.Stuff_CreateBySpawner(ctx.assetsModule, ctx.templateCore, ctx.idService, spawnerTM);
            ctx.stuffRepository.Add(entity);
            return entity;
        }

        public static void UnSpawn(GameSystemContext ctx, StuffEntity entity) {
            ctx.stuffRepository.Remove(entity);
            entity.TearDown();
        }

        public static void Clear(GameSystemContext ctx) {
            int len = ctx.stuffRepository.TakeAll(out StuffEntity[] entities);
            for (int i = 0; i < len; i += 1) {
                StuffEntity entity = entities[i];
                entity.TearDown();
            }
            ctx.stuffRepository.Clear();
        }

        #endregion

        #region FSM

        public static void FSM_Normal_Execute(GameSystemContext ctx, StuffEntity entity) {
            bool isLeave = ComparePos(entity);
            if (isLeave) {
                Loco_Fall(entity);
            }
        }

        public static bool ComparePos(StuffEntity stuff) { // true表示离开引力场
            var pos = stuff.TF_GetPos();
            Vector2 pointPos = new Vector2(9, 0);

            float dis = Vector2.Distance(pos, pointPos);

            if (dis < 10) { // 半径10
                return false;
            }
            stuff.isStretched = false;
            return true;
        }

        public static void Loco_Fall(StuffEntity stuff) { // 离开引力场的掉落
            stuff.Fall();
        }
        #endregion

        #region 检测
        public static void CheckUnSpawn(GameSystemContext ctx, StuffEntity stuff) {
            Vector3 pos = stuff.TF_GetPos();
            if (pos.x < -21 || pos.x > 21) {
                UnSpawn(ctx, stuff);
            } else if (pos.y < -12 || pos.y > 12) {
                UnSpawn(ctx, stuff);
            }

        }
        #endregion
    }
}