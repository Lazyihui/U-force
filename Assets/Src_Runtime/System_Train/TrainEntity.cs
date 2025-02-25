using System;
using UnityEngine;

namespace UF.System_Train
{
    public class TrainEntity
    {
        public TrainState state;

        public float defenseCD1; // Player1防御cd
        public float defenseCD2; // Player2防御cd
        public float defenseMax; // 防御最大cd

        public float attackCD1; // Player1攻击cd
        public float attackCD2; // Player2攻击cd

        public TrainEntity()
        {
            state = TrainState.None;
            defenseCD1 = 0;
            defenseCD2 = 0;
            defenseMax = 1;
            attackCD1 = 0;
            attackCD2 = 0;
        }
    }
}