using System;
using System.Collections.Generic;
using UnityEngine;
using UF.UIApplication;


namespace UF {
    public class AppUI : MonoBehaviour {

        public UIContext ctx;
        public UIEventCenter eventCenter;

        public void Ctor() {
            ctx = new UIContext();
            eventCenter = new UIEventCenter();
        }

        public void Inject(Canvas screenCanvas, AssetsModule assetsCore) {
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
        public void Panel_GameDefend_Update(SkillModel skill, float cd, float maxCD) {
            Panel_GameDefend_Button panel = ctx.panel_GameDefend_Button;
            if (panel == null) {
                return;
            }
            Panel_GameDefend_SetCD(skill, cd, maxCD);
        }

        void Panel_GameDefend_SetCD(SkillModel skill, float cd, float maxCD) {
            Panel_GameDefend_Button panel = ctx.panel_GameDefend_Button;
            if (panel == null) {
                return;
            }
            panel.Set_CD(skill, cd, maxCD);
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
                    eventCenter.OnPauseExitGame();
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

        #region GameOver
        public void Panel_GameOver_Open(bool isWin, int hurt, int shoot, int hitNumber, float hitRate) {
            Panel_GameOver panel = ctx.panel_GameOver;

            if (panel == null) {
                GameObject prefab = ctx.assetsCore.Panel_GetGameOver();
                GameObject go = GameObject.Instantiate(prefab, ctx.screenCanvas.transform);
                panel = go.GetComponent<Panel_GameOver>();
                panel.Ctor();

                panel.OnGameOverButtonClick += () => {
                    eventCenter.OnGameOver();
                };
            }
            ctx.panel_GameOver = panel;

            panel.Show();
            panel.Init(isWin, hurt, shoot, hitNumber, hitRate);
        }

        public void Panel_GameOver_Close() {
            Panel_GameOver panel = ctx.panel_GameOver;
            if (panel == null) {
                return;
            }
            panel.TearDown();
            ctx.panel_GameOver = null;
        }
        #endregion

        #region HUD_Progressbar
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
            HUD_Progressbar_SetText();
        }

        void HUD_Progressbar_SetWaves(int waves) {
            HUD_Progressbar hud = ctx.hud_Progressbar;
            if (hud == null) {
                return;
            }
            hud.Set_Waves(waves);
        }

        void HUD_Progressbar_SetText() {
            HUD_Progressbar hud = ctx.hud_Progressbar;
            if (hud == null) {
                return;
            }
            hud.Set_Text();
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

        #region StageSelection

        public void Panel_StageSelection_Open(int chapter, int maxChapter) {
            Panel_StageSelection panel = ctx.panel_StageSelection;

            if (panel == null) {
                GameObject prefab = ctx.assetsCore.Panel_GetStageSelection();

                GameObject go = GameObject.Instantiate(prefab, ctx.screenCanvas.transform);
                panel = go.GetComponent<Panel_StageSelection>();
                panel.Ctor();

                panel.OnChapterClickHandle += (int c) => {
                    eventCenter.OnStageSelection(c);
                };
            }

            ctx.panel_StageSelection = panel;
            panel.Show();
            // Panel_StageSelection_SetChapter(chapter, maxChapter);
        }

        public void Panel_StageSelectionOneStage_Add(int stageID, bool isPassed) {
            Panel_StageSelection panel = ctx.panel_StageSelection;
            if (panel == null) {
                return;
            }
            panel.Add(stageID, isPassed);
        }

        public void Panel_StageSelection_Select(InputDeviceType deviceType, int chapter, int maxChapter) {
            Panel_StageSelection panel = ctx.panel_StageSelection;
            if (panel == null) {
                return;
            }
            panel.SelectChapter(deviceType, chapter, maxChapter);
            panel.Clear();
        }

        public void Panel_StageSelection_Close() {
            Panel_StageSelection panel = ctx.panel_StageSelection;
            if (panel == null) {
                return;
            }
            panel.TearDown();
            ctx.panel_StageSelection = null;
        }

        #endregion

    }
}