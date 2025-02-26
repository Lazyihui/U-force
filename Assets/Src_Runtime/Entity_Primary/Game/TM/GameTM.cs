using System;
using UnityEngine;
using TriInspector;


namespace UF {

    [Serializable]
    public class GameTM {

        [Title("防守战")]
        public bool isDefendWar;//是否是防御战
        [ShowIf(nameof(isDefendWar))] public int spawnEnemyCount; //小波次的预设数量
        [ShowIf(nameof(isDefendWar))] public int maxWaves; //最大波次
        [ShowIf(nameof(isDefendWar))] public int curWaves; //当前波次
        [ShowIf(nameof(isDefendWar))] public float cdEnemy; //波次间cd
        [ShowIf(nameof(isDefendWar))] public float cdEnemyMax; //预设波次间cd
        [ShowIf(nameof(isDefendWar))] public float spawnEnemyInterval;//生成小怪的间隔
        [ShowIf(nameof(isDefendWar))] public float spawnEnemyIntervalTimer; //生成小怪计时器
        [ShowIf(nameof(isDefendWar))] public float progress; //每小波次进度条占比

        [Title("进攻战")]
        public bool isOffensiveWar;//是否是进攻战
        [ShowIf(nameof(isOffensiveWar))] public int spanwBossTimer;
        [ShowIf(nameof(isOffensiveWar))] public int spanwBossCount;

        [ShowIf(nameof(isOffensiveWar))] public int bossMaxWaves;
        [ShowIf(nameof(isOffensiveWar))] public int bossCurWaves;
        [ShowIf(nameof(isOffensiveWar))] public float bossCdMinion;
        [ShowIf(nameof(isOffensiveWar))] public float bossCd;
        [ShowIf(nameof(isOffensiveWar))] public float bossSpawnMinionInterval;
        [ShowIf(nameof(isOffensiveWar))] public float bossSpawnMinionIntervalTimer;
        [ShowIf(nameof(isOffensiveWar))] public float bossProgress;

        [Title("消除战")]
        public bool isEliminateWar;//是否是消除战

        [Title("训练营")]
        public bool isTrainingWar;//是否是训练营
        [ShowIf(nameof(isTrainingWar))] public PropSo[] sceneProps; // 场景道具
    }
}