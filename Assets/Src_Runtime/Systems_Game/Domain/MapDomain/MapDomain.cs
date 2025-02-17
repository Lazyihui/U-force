using System;
using UnityEngine;

namespace UF {
    public static class MapDomain {

        public static MapEntity Spawn(AssetsModule assetsModule, IDService iDService, GameObject mapPrefab) {
            MapEntity map = GameFactory.Map_Create(assetsModule, iDService, mapPrefab);
            return map;
        }

    }
}