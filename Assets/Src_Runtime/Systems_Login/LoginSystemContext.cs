using System;
using UnityEngine;

namespace UF {

    public class LoginSystemContext {
        public bool isRunning;
        public float restFixTime;
        public AppUI appUI;
        public InputModule inputModule;

        // model
        public CardModel cardModel;

        public LoginSystemContext() {
            isRunning = false;
            
            cardModel = new CardModel();
        }


        public void Inject(AppUI appUI, InputModule input) {
            this.appUI = appUI;
            this.inputModule = input;
        }


    }
}