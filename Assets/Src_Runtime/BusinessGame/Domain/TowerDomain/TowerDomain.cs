using System;
using System.Collections.Generic;
using UnityEngine;


namespace UF {
    public static class TowerDomain {

        public static TowerEntity Spawn(GameContext ctx, int typeID) {
            TowerEntity entity = GameFactory.Tower_Create(ctx.assetsCore, ctx.templateCore, ctx.idService, typeID);

            ctx.towerRepository.Add(entity);
            return entity;
        }
    }
}
