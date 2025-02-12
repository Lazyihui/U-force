using System;
using UnityEngine;
using UnityEngine.UI;
using UF.UIApplication;


namespace UF {
    public class UIContext {

        public Canvas screenCanvas;
        public AssetsModule assetsCore;

        // panel
        public Panel_Login panel_Login;
        public Panel_GamePause panel_GamePause;
        public Panel_GameDefend_Button panel_GameDefend_Button;
        public Panel_GameOver panel_GameOver;
        public Panel_StageSelection panel_StageSelection;
        public Panel_CardSelection panel_CardSelection;
        public Panel_BackSelection panel_BackSelection;
        public Panel_Watchtower panel_Watchtower;

        // HUD
        public HUD_Progressbar hud_Progressbar;

        // input
        public bool isTabDown;
        public bool isEscDown;


        public UIContext() {
            isTabDown = false;
        }

        public void Inject(Canvas screenCanvas, AssetsModule assetsCore) {
            this.screenCanvas = screenCanvas;
            this.assetsCore = assetsCore;
        }


    }
}