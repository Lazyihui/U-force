using System;
using UnityEngine;

namespace UF {

    public class LoginSystemContext {

        public GameEntity gameEntity;

        public AppUI appUI;


        public void Inject(GameEntity gameEntity, AppUI appUI) {
            this.appUI = appUI;
            this.gameEntity = gameEntity;
        }


    }
}