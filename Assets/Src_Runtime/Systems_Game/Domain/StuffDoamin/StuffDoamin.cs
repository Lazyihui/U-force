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

        }


        #endregion
    }
}