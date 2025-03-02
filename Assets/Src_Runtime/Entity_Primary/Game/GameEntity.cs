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
        public int spawnEnemyTimer; //一波里的小波次 （一波里生成几个）
        public int spawnEnemyCount; //小波次的预设数量（一波里生成几个）
        public int maxWaves; //最大波次
        public int curWaves; //当前波次

        public float cdEnemy; //波次间cd
        public float cdEnemyMax; //预设波次间cd

        public float spawnEnemyInterval;//生成小怪的间隔
        public float spawnEnemyIntervalTimer; //生成小怪计时器

        public float progress; //每小波次进度条占比
        public float wavesProgress; //进度条数值

        #endregion

        #region 资源战数据
        public float gameTime;
        public float gameMax;
        public int energyNumber; // 能量棒数量

        public float stuffCD; // 道具生成cd
        public float stuffMax;
        #endregion

        public bool isDefendWar;
        public bool isOffensiveWar;
        public bool isEliminateWar;

        public GameEntity() {
            wavesProgress = 0;

            gameTime = 0;
            energyNumber = 0;
        }

        public void InitFromTM(GameTM tm) {
            isDefendWar = tm.isDefendWar;
            isOffensiveWar = tm.isOffensiveWar;
            isEliminateWar = tm.isEliminateWar;

            if (isDefendWar) {
                Debug.Log("进入防守战");
                spawnEnemyTimer = tm.spawnEnemyCount;
                spawnEnemyCount = tm.spawnEnemyCount;
                maxWaves = tm.maxWaves;
                curWaves = tm.curWaves;
                cdEnemy = tm.cdEnemy;
                cdEnemyMax = tm.cdEnemyMax;
                spawnEnemyInterval = tm.spawnEnemyInterval;
                spawnEnemyIntervalTimer = tm.spawnEnemyIntervalTimer;
                progress = tm.progress;
            }

            if (isOffensiveWar) {
                Debug.Log("进入进攻战");
                spawnEnemyTimer = tm.spanwBossTimer;
                spawnEnemyCount = tm.spanwBossCount;
                maxWaves = tm.bossMaxWaves;
                curWaves = tm.bossCurWaves;
                cdEnemy = tm.bossCdMinion;
                cdEnemyMax = tm.bossCd;
                spawnEnemyInterval = tm.bossSpawnMinionInterval;
                spawnEnemyIntervalTimer = tm.bossSpawnMinionIntervalTimer;
                progress = tm.bossProgress;
            }

            if (isEliminateWar) {
                Debug.Log("进入资源战");
                gameMax = tm.gameMax;
                stuffMax = tm.stuffMax;
            }

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