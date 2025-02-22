using System;
using UnityEngine;
using TriInspector;


namespace UF {

    [Serializable]
    public class GameTM {

        [Title("防守战")]
        public bool isDefendwar;//是否是防御战
        [ShowIf(nameof(isDefendwar))] public int spawnMinionTimer; //一波里的小波次
        [ShowIf(nameof(isDefendwar))] public int spawnMinion; //小波次的预设数量
        [ShowIf(nameof(isDefendwar))] public int maxWaves; //最大波次
        [ShowIf(nameof(isDefendwar))] public int curWaves; //当前波次
        [ShowIf(nameof(isDefendwar))] public float cdMinion; //波次间cd
        [ShowIf(nameof(isDefendwar))] public float cdMinionMax; //预设波次间cd
        [ShowIf(nameof(isDefendwar))] public float spawnMinionInterval;//生成小怪的间隔
        [ShowIf(nameof(isDefendwar))] public float spawnMinionIntervalTimer; //生成小怪计时器
        [ShowIf(nameof(isDefendwar))] public float progress; //每小波次进度条占比
        [ShowIf(nameof(isDefendwar))] public float wavesProgress; //进度条数值

        [Title("进攻战")]
        public bool isOffensiveWar;//是否是进攻战

        [Title("消灭战")]
        public bool isEliminateWar;//是否是消灭战

    }
}