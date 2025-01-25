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

        // Panel_Login
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
            if (panel == null) { return; }

            panel.TearDown();
            ctx.panel_GameDefend_Button = null;
        }

        public void Panel_GameDefend_Basic_Open()
        {
            Panel_GameDefend_Basic panel = ctx.panel_GameDefend_Basic;

            if (panel == null)
            {
                GameObject prefab = ctx.assetsCore.Panel_GetGameDefend_Basic();

                GameObject go = GameObject.Instantiate(prefab, ctx.screenCanvas.transform);
                panel = go.GetComponent<Panel_GameDefend_Basic>();

                panel.Ctor();
            }

            ctx.panel_GameDefend_Basic = panel;

            panel.Show();
        }

        public void Panel_GameDefend_Basic_Close()
        {
            Panel_GameDefend_Basic panel = ctx.panel_GameDefend_Basic;
            if (panel == null) { return; }
            
            panel.TearDown();
            ctx.panel_GameDefend_Basic = null;
        }

        // Panel_GamePause
        public void Panel_GamePause_Open()
        {
            Panel_GamePause panel = ctx.panel_GamePause;

            if (panel == null)
            {
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

        public void Panel_GamePause_Close()
        {
            Panel_GamePause panel = ctx.panel_GamePause;
            if (panel == null) { return; }
            
            panel.TearDown();
            ctx.panel_GamePause = null;
        }
    }
}