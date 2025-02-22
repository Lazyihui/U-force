using System;
using UnityEngine;

namespace UF.System_Train
{
    public class TrainEntity
    {
        public TrainState state;

        public TrainEntity()
        {
            state = TrainState.None;
        }
    }
}