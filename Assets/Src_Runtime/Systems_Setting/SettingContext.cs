using System;
using System.Collections.Generic;
using UnityEngine;

namespace UF.Systems_Setting{

    public class SettingContext{

        public bool isRunning;

        public AppUI appUI;

        public void Inject(AppUI appUI){
            this.appUI = appUI;
        }

    }


}