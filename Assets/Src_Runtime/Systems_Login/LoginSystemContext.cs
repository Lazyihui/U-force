using System;
using UnityEngine;

namespace UF {

    public class LoginSystemContext {
        public bool isRunning;
        public float restFixTime;
        public AppUI appUI;
        public InputModule inputModule;
        public AudioModule audioModule;

        // model
        public SaveCore saveCore;

        public LoginSystemContext() {
            isRunning = false;

        }

        public void Inject(AppUI appUI, InputModule input, AudioModule audio, SaveCore saveCore) {
            this.appUI = appUI;
            this.inputModule = input;
            this.audioModule = audio;
            this.saveCore = saveCore;
        }

    }
}