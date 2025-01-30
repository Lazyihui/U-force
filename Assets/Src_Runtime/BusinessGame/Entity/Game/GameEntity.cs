using System;
using System.Collections;
using UnityEngine;

namespace UF {

    public class GameEntity {

        public float restFixTime;
        public GameState state;
        public int stageCurID;

        public IDSignature OwnerIDsig;

        public float spawnMinionInterval;
        public float spawnMinionIntervalTimer;

        public GameEntity() {

            spawnMinionInterval = 2;
            spawnMinionIntervalTimer = 0;

            stageCurID = 0;

        }

    }
}