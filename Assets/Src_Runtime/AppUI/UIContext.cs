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
        public Panel_Notice panel_Notice;
        public Panel_Training_GameOver panel_Training_GameOver;
        public Panel_Illustrated panel_Illustrated;
        public Panel_TrainIllustrated panel_TrainIllustrated;

        // HUD
        public HUD_Progressbar hud_Progressbar;
        public HUD_Training hud_Training;
        public HUD_Training_CD hud_Training_CD1;
        public HUD_Training_CD hud_Training_CD2;

        // input
        public bool isTabDown;
        public bool isEscDown;

        // temp
        public bool isNoticeOpen;
        public float noticeTimer;


        public UIContext() {
            isTabDown = false;
            isNoticeOpen = false;
            noticeTimer = 1;
        }

        public void Inject(Canvas screenCanvas, AssetsModule assetsCore) {
            this.screenCanvas = screenCanvas;
            this.assetsCore = assetsCore;
        }


    }
}