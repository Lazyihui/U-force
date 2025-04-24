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

        // 存档数据
        public GameSaveModel playerSaveData;

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
        public bool isBossWar;
        public bool isEliminateWar;

        public GameEntity() {
            // 存档
            playerSaveData = new GameSaveModel();

            wavesProgress = 0;

            gameTime = 0;
            energyNumber = 0;
        }

        #region 存档
        public void Init(int slotID) {
            playerSaveData.slotID = slotID;
        }

        public void FromDTO(SaveSlotFullDTO dto) {
            // 将存档数据赋值给当前对象
            playerSaveData.FromDTO(dto.gameSaveDTO);
        }

        public SaveSlotFullDTO ToDTO() {
            // 将当前对象的数据转换为存档数据
            SaveSlotFullDTO dto = new SaveSlotFullDTO();
            dto.gameSaveDTO = playerSaveData.ToDTO();
            return dto;
        }

        #endregion
        public void Reset() {
            // 存档
            playerSaveData.Reset();

            // TOOO:没认真看这个要不要reset
            wavesProgress = 0;
            gameTime = 0;
            energyNumber = 0;
        }

        public void Ctor() {

        }

        public void InitFromTM(GameTM tm) {
            isDefendWar = tm.isDefendWar;
            isBossWar = tm.isOffensiveWar;
            isEliminateWar = tm.isEliminateWar;

            // 防守战   
            if (isDefendWar) {
                spawnEnemyTimer = tm.spawnEnemyCount;
                spawnEnemyCount = tm.spawnEnemyCount;
                maxWaves = tm.maxWaves;
                curWaves = 0;
                cdEnemy = 0;
                cdEnemyMax = tm.cdEnemyMax;
                spawnEnemyInterval = tm.spawnEnemyInterval;
                spawnEnemyIntervalTimer = tm.spawnEnemyIntervalTimer;
                progress = 1 / (float)spawnEnemyCount / maxWaves;
            }

            if (isBossWar) {
                Debug.Log("进入进攻战");
                spawnEnemyTimer = tm.spanwBossTimer;
                spawnEnemyCount = tm.spanwBossCount;
                maxWaves = tm.bossMaxWaves;
                curWaves = 0;
                cdEnemy = tm.bossCdMinion;
                cdEnemyMax = 0;
                spawnEnemyInterval = tm.bossSpawnMinionInterval;
                spawnEnemyIntervalTimer = tm.bossSpawnMinionIntervalTimer;
                progress = 1 / (float)spawnEnemyCount / maxWaves;
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
            playerSaveData.Clear();

            wavesProgress = 0;

            gameTime = 0;
            energyNumber = 0;
        }

        public void Inject(GameSaveModel playerSaveData) {
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