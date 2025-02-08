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
        #endregion

        #region FSM

        public static void FSM_Normal_Execute(GameSystemContext ctx, StuffEntity entity) {

        }

        #endregion
    }
}