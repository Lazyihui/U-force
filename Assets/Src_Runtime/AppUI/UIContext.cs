using System;
using UnityEngine;
using UnityEngine.UI;
using UF.UIApplication;


namespace UF {
    public class UIContext {

        public CardModel cardModel;

        public Canvas screenCanvas;
        public Transform canvasRoot;
        public AssetsModule assetsCore;
        public TemplateMoudle templateMoudle;
        public InputModule inputModule;
        public AudioModule audioModule;

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
        public Panel_Training_Back panel_Training_Back;
        public Panel_Eliminate_Button panel_Eliminate_Button;
        public Panel_Eliminate_Pause panel_Eliminate_Pause;
        public Panel_EliminateGameOver panel_EliminateGameOver;
        public Panel_Setting panel_Setting;

        // HUD
        public HUD_Progressbar hud_Progressbar;
        public HUD_Training hud_Training;
        public HUD_Training_CD hud_Training_CD1;
        public HUD_Training_CD hud_Training_CD2;
        public HUD_Eliminate hud_Eliminate;

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

        public void Inject(Canvas screenCanvas, Transform canvasRoot,AssetsModule assetsCore, TemplateMoudle templateMoudle, InputModule inputModule, AudioModule audioModule) {
            this.screenCanvas = screenCanvas;
            this.assetsCore = assetsCore;
            this.templateMoudle = templateMoudle;
            this.inputModule = inputModule;
            this.audioModule = audioModule;
            this.canvasRoot = canvasRoot;
        }


    }
}