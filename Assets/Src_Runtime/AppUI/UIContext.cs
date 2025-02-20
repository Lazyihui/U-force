using System;
using UnityEngine;
using UnityEngine.UI;
using UF.UIApplication;


namespace UF {
    public class UIContext {

        public CardModel cardModel;


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
        public Panel_WatchTower panel_Watchtower;
        public Panel_WatchDetail panel_WatchDetail;
        public Panel_Training_Select panel_Training_Select;
        public Panel_Training_Tip panel_Training_Tip;
        public Panel_Training_Button panel_Training_Button;
        public Panel_Training_Pause panel_Training_Pause;

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