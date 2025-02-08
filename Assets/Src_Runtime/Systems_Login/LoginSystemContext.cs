using System;
using UnityEngine;

namespace UF {

    public class LoginSystemContext {
        public bool isRunning;
        public float restFixTime;
        public AppUI appUI;

        public LoginSystemContext() {
            isRunning = false;
        }


        public void Inject( AppUI appUI) {
            this.appUI = appUI;
        }


    }
}