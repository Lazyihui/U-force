using System;
using System.Collections.Generic;
using UnityEngine;


namespace UF {
    public class AppUI {

        public UIContext ctx;
        public UIEventCenter eventCenter;

        public AppUI() {
            ctx = new UIContext();
            eventCenter = new UIEventCenter();
        }

        public void Inject(Canvas screenCanvas, AssetsCore assetsCore) {
            ctx.Inject(screenCanvas, assetsCore);
        }

        #region Login
        public void Panel_Login_Open() {
            Panel_Login panel = ctx.panel_Login;

            if (panel == null) {

                GameObject prefab = ctx.assetsCore.Panel_GetLogin();

                GameObject go = GameObject.Instantiate(prefab, ctx.screenCanvas.transform);
                panel = go.GetComponent<Panel_Login>();

                panel.Ctor();

                panel.OnLoginClickHandle += () => {
                    eventCenter.OnLoginStart();
                };

                panel.OnTrainingClickHandle += () => {
                    eventCenter.OnTraining();
                };

                panel.OnSettingClickHandle += () => {
                    eventCenter.OnSetting();
                };

                panel.OnQuitClickHandle += () => {
                    eventCenter.OnQuit();
                };

            }
            ctx.panel_Login = panel;
            panel.Show();
        }

        public void Panel_Login_Close() {
            Panel_Login panel = ctx.panel_Login;
            if (panel == null) {
                return;
            }

            panel.TearDown();
            ctx.panel_Login = null;
        }

        #endregion

        #region GameDefend

        // Panel_GameDefend
        public void Panel_GameDefend_Button_Open() {
            Panel_GameDefend_Button panel = ctx.panel_GameDefend_Button;

            if (panel == null) {
                GameObject prefab = ctx.assetsCore.Panel_GetGameDefend_Button();

                GameObject go = GameObject.Instantiate(prefab, ctx.screenCanvas.transform);
                panel = go.GetComponent<Panel_GameDefend_Button>();

                panel.Ctor();

                //Handle
                panel.OnPauseClickHandle += () => {
                    eventCenter.OnGamePause();
                };

                panel.OnAttackClickHandle += () => {
                    eventCenter.OnAttack();
                };

                panel.OnTechClickHandle += () => {
                    eventCenter.OnTech();
                };

                panel.OnWeap1ClickHandle += () => {
                    eventCenter.OnWeap1();
                };

                panel.OnWeap2ClickHandle += () => {
                    eventCenter.OnWeap2();
                };

                panel.OnWeap3ClickHandle += () => {
                    eventCenter.OnWeap3();
                };

                panel.OnWeap4ClickHandle += () => {
                    eventCenter.OnWeap4();
                };
            }
            ctx.panel_GameDefend_Button = panel;

            panel.Show();
        }

        public void Panel_GameDefend_Button_Close() {
            Panel_GameDefend_Button panel = ctx.panel_GameDefend_Button;
            if (panel == null) {
                return;
            }

            panel.TearDown();
            ctx.panel_GameDefend_Button = null;
        }

        #endregion

        #region GamePause
        // Panel_GamePause
        public void Panel_GamePause_Open() {
            Panel_GamePause panel = ctx.panel_GamePause;

            if (panel == null) {
                GameObject prefab = ctx.assetsCore.Panel_GetGamePause();

                GameObject go = GameObject.Instantiate(prefab, ctx.screenCanvas.transform);
                panel = go.GetComponent<Panel_GamePause>();

                panel.Ctor();

                panel.OnContinueClickHandle += () => {
                    eventCenter.OnContinue();
                };

                panel.OnIllustratedClickHandle += () => {
                    eventCenter.OnIllustrated();
                };

                panel.OnQuitClickHandle += () => {
                    eventCenter.OnQuit();
                };
            }

            ctx.panel_GamePause = panel;

            panel.Show();
        }

        public void Panel_GamePause_Close() {
            Panel_GamePause panel = ctx.panel_GamePause;
            if (panel == null) {
                return;
            }
            panel.TearDown();
            ctx.panel_GamePause = null;
        }
        #endregion

        #region HUD
        public void HUD_Progressbar_Open(int waves) {
            HUD_Progressbar hud = ctx.hud_Progressbar;

            if (hud == null) {
                GameObject prefab = ctx.assetsCore.HUD_GetProgressbar();

                GameObject go = GameObject.Instantiate(prefab, ctx.screenCanvas.transform);
                hud = go.GetComponent<HUD_Progressbar>();
                hud.Ctor();
            }

            ctx.hud_Progressbar = hud;
            hud.Show();

            HUD_Progressbar_SetWaves(waves);
        }

        void HUD_Progressbar_SetWaves(int waves) {
            HUD_Progressbar hud = ctx.hud_Progressbar;
            if (hud == null) {
                return;
            }
            hud.Set_Waves(waves);
        }

        public void HUD_Progressbar_Update(float progress) {
            HUD_Progressbar hud = ctx.hud_Progressbar;
            if (hud == null) {
                return;
            }
            hud.Set_Progress(progress);
        }

        public void HUD_Progressbar_Close() {
            HUD_Progressbar hud = ctx.hud_Progressbar;
            if (hud == null) {
                return;
            }
            hud.TearDown();
            ctx.hud_Progressbar = null;
        }
        #endregion
    }
}