using System;
using System.Collections;
using UnityEngine;

namespace UF {

    public class GameEntity {

        public float restFixTime;
        public GameState state;
        public int stageCurID;

        public IDSignature OwnerIDsig;


        #region 生成小怪的波次数据

        // TODO: 之后可能要改用行的。cs文件里面的数据，不要放在这里
        public int spawnMinionTimer;
        public int spawnMinion;//
        public int maxWaves;
        public int curWaves;

        public float cdMinion;
        public float cdMinionMax;

        public float spawnMinionInterval;//生成小怪的间隔
        public float spawnMinionIntervalTimer;

        #endregion

        public GameEntity() {

            stageCurID = 0;

            spawnMinionTimer = 4;
            spawnMinion = 4;
            maxWaves = 4;
            curWaves = 0;

            cdMinion = 0;
            cdMinionMax = 4;

            spawnMinionInterval = 2;
            spawnMinionIntervalTimer = 2;

        }

    }
}