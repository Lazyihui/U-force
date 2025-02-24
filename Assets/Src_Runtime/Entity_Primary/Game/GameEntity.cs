using System;
using System.Collections;
using UnityEngine;

namespace UF {

    public class GameEntity {

        public bool isOwnerDying;//是Player否死亡
        public bool isRunning;//游戏game是否在运行
        public float restFixTime;
        public GameState state;
        public IDSignature OwnerIDsig;
        public PlayerEntity playerSaveData;


        #region 生成小怪的波次数据
        // TODO: 之后可能要改用行的。cs文件里面的数据，不要放在这里
        public int spawnMinionTimer; //一波里的小波次
        public int spawnMinion; //小波次的预设数量
        public int maxWaves; //最大波次
        public int curWaves; //当前波次

        public float cdMinion; //波次间cd
        public float cdMinionMax; //预设波次间cd

        public float spawnMinionInterval;//生成小怪的间隔
        public float spawnMinionIntervalTimer; //生成小怪计时器

        public float progress; //每小波次进度条占比
        public float wavesProgress; //进度条数值

        #endregion

        public bool isDefendWar;
        public bool isOffensiveWar;
        public bool isEliminateWar;

        public GameEntity() {
        }

        public void InitFromTM(GameTM tm) {
            spawnMinionTimer = tm.spawnMinionTimer;
            spawnMinion = tm.spawnMinion;
            maxWaves = tm.maxWaves;
            curWaves = tm.curWaves;
            cdMinion = tm.cdMinion;
            cdMinionMax = tm.cdMinionMax;
            spawnMinionInterval = tm.spawnMinionInterval;
            spawnMinionIntervalTimer = tm.spawnMinionIntervalTimer;
            progress = tm.progress;
            wavesProgress = tm.wavesProgress;

            isDefendWar = tm.isDefendWar;
            isOffensiveWar = tm.isOffensiveWar;
            isEliminateWar = tm.isEliminateWar;
        }

        public void Clear() {
            isOwnerDying = false;
            isRunning = false;
            restFixTime = 0;
            state = GameState.None;
        }


        public void Inject(PlayerEntity playerSaveData) {
            this.playerSaveData = playerSaveData;
        }

        public void Run() {
            isRunning = true;
        }

        public void Stop() {
            isRunning = false;
        }

    }
}