using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UF.Systems_Setting;

namespace UF {

    public class SettingSystem : MonoBehaviour {

        SettingContext ctx;

        SettingSystemEvents events;
        public SettingSystemEvents Events => events;

        public void  Ctor() {
            ctx = new SettingContext();
            events = new SettingSystemEvents();
        }

        public void Inject(AppUI appUI) {
            ctx.Inject(appUI);
        }

    }
}