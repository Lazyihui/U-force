using System;
using UnityEngine;

namespace UF {
    public static class MapDomain {

        public static MapEntity Spawn(GameSystemContext ctx, GameObject mapPrefab) {
            MapEntity map = GameFactory.Map_Create(ctx.assetsModule, ctx.idService, mapPrefab);
            ctx.gameEntity.CurrentMapIDsig = map.idSig;
            return map;
        }

    }
}