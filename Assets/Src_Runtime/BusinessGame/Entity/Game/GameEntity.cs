using System;
using System.Collections;
using UnityEngine;

namespace UF {

    public class GameEntity {

        public float restFixTime;
        public GameState state;
        public int stageCurID;

        public IDSignature OwnerIDsig;

        public int spawnMinionTimer;
        public int spawnMinion;

        public float cdMinion;
        public float cdMinionMax;

        public float spawnMinionInterval;
        public float spawnMinionIntervalTimer;

        public GameEntity() {

            stageCurID = 0;

            spawnMinionTimer = 4;
            spawnMinion = 4;

            cdMinion = 0;
            cdMinionMax = 4;

            spawnMinionInterval = 2;
            spawnMinionIntervalTimer = 2;

        }

    }
}