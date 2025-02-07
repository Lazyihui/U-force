using System;
using UnityEngine;
using UnityEngine.UI;

namespace UF {
    public class UIContext {

        public Canvas screenCanvas;
        public UIEventCenter eventCenter;
        public AssetsCore assetsCore;

        // panel
        public Panel_Login panel_Login;
        public Panel_GamePause panel_GamePause;
        public Panel_GameDefend_Button panel_GameDefend_Button;
        public Panel_GameOver panel_GameOver;

        // HUD
        public HUD_Progressbar hud_Progressbar;

        public UIContext() {

        }

        public void Inject(Canvas screenCanvas, AssetsCore assetsCore) {
            this.screenCanvas = screenCanvas;
            this.assetsCore = assetsCore;
        }


    }
}