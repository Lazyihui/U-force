using System;
using UnityEngine;

namespace UF
{
    public class TrainSystemContext
    {
        public bool isRunning;
        public float restFixTime;
        public AppUI appUI;

        public TrainSystemContext()
        {
            isRunning = false;
        }

        public void Inject(AppUI appUI)
        {
            this.appUI = appUI;
        }
    }
}