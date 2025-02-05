using System;
using System.Collections.Generic;
using UnityEngine;


namespace UF {
    public static class StuffDoamin {
        #region life
        public static StuffEntity Spawn(GameContext ctx, int typeID) {
            StuffEntity entity = GameFactory.Stuff_Create(ctx.assetsCore, ctx.templateCore, ctx.idService, typeID);
            ctx.stuffRepository.Add(entity);
            return entity;
        }
        #endregion

        #region FSM

        public static void FSM_Normal_Execute(GameContext ctx, StuffEntity entity) {

        }

        #endregion
    }
}