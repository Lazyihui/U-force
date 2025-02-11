using System;
using UnityEngine;

namespace UF {

    public class LoginSystemContext {
        public bool isRunning;
        public float restFixTime;
        public AppUI appUI;
        public InputModule inputModule;

        public LoginSystemContext() {
            isRunning = false;
        }


        public void Inject( AppUI appUI,InputModule input) {
            this.appUI = appUI;
            this.inputModule = input;
        }


    }
}