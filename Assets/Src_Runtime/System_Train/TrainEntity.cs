using System;
using UnityEngine;

namespace UF.System_Train
{
    public class TrainEntity
    {
        public TrainState state;

        public float defenseCD1; // Player1防御计时
        public float defenseCD2; // Player2防御计时
        public float defenseMax; // 防御最大cd

        public float attackCD1; // Player1攻击计时
        public float attackCD2; // Player2攻击计时

        public float propSpawnCD; // 道具生成计时
        public float propSpawnMax; // 道具生成最大cd

        public float gameTime;

        public TrainEntity()
        {
            state = TrainState.None;
            defenseCD1 = 0;
            defenseCD2 = 0;
            defenseMax = 1;
            attackCD1 = 0;
            attackCD2 = 0;
            propSpawnCD = 0;
            propSpawnMax = 15;
            gameTime = 120;
        }
    }
}