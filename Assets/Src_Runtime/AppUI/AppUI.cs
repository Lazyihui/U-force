using System;
using System.Collections.Generic;
using UnityEngine;
using UF.UIApplication;


namespace UF {
    public class AppUI : MonoBehaviour {

        public UIContext ctx;
        public UIEventCenter eventCenter;
        public SelectedInFormation selectedInFormation;
        public SelectedInFormation selectedInTrain;
        public GameSaveModel playerSaveData;

        public void Ctor() {
            ctx = new UIContext();
            eventCenter = new UIEventCenter();
            selectedInFormation = new SelectedInFormation();
            selectedInTrain = new SelectedInFormation();
        }

        public void Reset() {
            selectedInFormation.Reset();
        }

        public void Inject(Canvas screenCanvas, AssetsModule assetsCore, TemplateMoudle templateMoudle, InputModule inputModule, GameSaveModel playerSaveData) {
            ctx.Inject(screenCanvas, assetsCore, templateMoudle, inputModule);
            this.playerSaveData = playerSaveData;
        }
        public CardModel Get_CardModel() {
            return ctx.cardModel;
        }

        public void Set_CardModel(CardModel cardModel) {
            ctx.cardModel = cardModel;
        }

        public void Set_InputDown(bool isTabDown, bool isEscDown) {
            ctx.isTabDown = isTabDown;
            ctx.isEscDown = isEscDown;
        }

        public void Tick(float dt) {
            // input
            var inputModule = ctx.inputModule;

            Set_InputDown(inputModule.IsTabDown, inputModule.IsEscDown);
            Panel_StageSelection_Tick(ctx.isTabDown, ctx.isEscDown);
            Panel_Login_Update_Tick(ctx.screenCanvas, inputModule.uiAxis, inputModule.isConfirm);
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

        public void Panel_Login_Update_Tick(Canvas canvas, Vector2 axis, bool isConfirm) {
            Panel_Login panel = ctx.panel_Login;
            if (panel == null) {
                return;
            }

            panel.Update_Tick(canvas, axis, isConfirm);
        }
        #endregion

        #region GameDefend

        // Panel_GameDefend
        public void Panel_GameDefend_Button_Open(int slotMax) {
            Panel_GameDefend_Button panel = ctx.panel_GameDefend_Button;

            if (panel == null) {
                GameObject prefab = ctx.assetsCore.Panel_GetGameDefend_Button();

                GameObject go = GameObject.Instantiate(prefab, ctx.screenCanvas.transform);
                panel = go.GetComponent<Panel_GameDefend_Button>();

                panel.Ctor(slotMax);

                //Handle
                panel.OnPauseClickHandle += () => {
                    eventCenter.OnGamePause();
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

        public void Panel_GameDefend_SetBtnTech_Hide() {
            Panel_GameDefend_Button panel = ctx.panel_GameDefend_Button;
            if (panel == null) {
                return;
            }
            panel.Set_BtnTech_Hide();
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
        public void Panel_GameOver_Open(bool isWin, int starCount) {
            Panel_GameOver panel = ctx.panel_GameOver;

            if (panel == null) {
                GameObject prefab = ctx.assetsCore.Panel_GetGameOver();
                GameObject go = GameObject.Instantiate(prefab, ctx.screenCanvas.transform);
                panel = go.GetComponent<Panel_GameOver>();
                panel.Ctor();

                panel.OnGameOverButtonClick += (isWin) => {
                    eventCenter.OnGameOver(isWin);
                };
            }
            ctx.panel_GameOver = panel;

            panel.Show();
            panel.Init(isWin, playerSaveData.GetRoleGetHurt(), playerSaveData.shootTimes, playerSaveData.HitTimes, playerSaveData.GetRoleHitRate(), starCount);
        }

        public void Panel_GameOver_Close() {
            Panel_GameOver panel = ctx.panel_GameOver;
            if (panel == null) {
                return;
            }
            panel.TearDown();
            ctx.panel_GameOver = null;
        }

        // TODO: 资源战结算UI
        public void Panel_EliminateGameOver_Open(bool isWin, int starCount) {
            Panel_EliminateGameOver panel = ctx.panel_EliminateGameOver;

            if (panel == null) {
                GameObject prefab = ctx.assetsCore.Panel_GetEliminateGameOver();
                GameObject go = GameObject.Instantiate(prefab, ctx.screenCanvas.transform);
                panel = go.GetComponent<Panel_EliminateGameOver>();
                panel.Ctor();

                panel.OnEliminateGameOverButtonClick += (iswin) => {
                    eventCenter.OnEliminateOverResetClick(iswin);
                };
            }
            ctx.panel_EliminateGameOver = panel;
            panel.Show();
            panel.Init(isWin, playerSaveData.GetEnergyCount(), playerSaveData.GetEliShootTimes(), playerSaveData.GetEliTakeTimes(), starCount);
        }

        public void Panel_Eliminate_GameOver_Close() {
            Panel_EliminateGameOver panel = ctx.panel_EliminateGameOver;
            if (panel == null) { return; }
            panel.TearDown();
            ctx.panel_EliminateGameOver = null;
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

        public void HUD_Progressbar_Update(float progress, int hp, int maxHp) {
            HUD_Progressbar hud = ctx.hud_Progressbar;
            if (hud == null) {
                return;
            }
            hud.Set_Progress(progress);
            hud.Set_AchiText(hp, maxHp);
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

        #region HUD_Training
        public void HUD_Training_Open() {
            HUD_Training hud = ctx.hud_Training;

            if (hud == null) {
                GameObject prefab = ctx.assetsCore.HUD_GetTraining();

                GameObject go = GameObject.Instantiate(prefab, ctx.screenCanvas.transform);
                hud = go.GetComponent<HUD_Training>();
                hud.Ctor();
            }

            ctx.hud_Training = hud;
            hud.Show();
        }

        public void HUD_Training_Close() {
            HUD_Training hud = ctx.hud_Training;

            if (hud == null) { return; }

            hud.TearDown();
            ctx.hud_Training = null;
        }

        public void HUD_Training_Update(int player1Hp, int player1Max, int player2Hp, int player2Max, float time) {
            HUD_Training hud = ctx.hud_Training;

            if (hud == null) { return; }

            HUD_Training_SetPlayer1Hp(player1Hp, player1Max);
            HUD_Training_SetPlayer2Hp(player2Hp, player2Max);
            HUD_Training_SetTime(time);
        }

        void HUD_Training_SetPlayer1Hp(int hp, int maxHp) {
            HUD_Training hud = ctx.hud_Training;

            if (hud == null) { return; }

            hud.SetPlayer1Hp(hp, maxHp);
        }

        void HUD_Training_SetPlayer2Hp(int hp, int maxHp) {
            HUD_Training hud = ctx.hud_Training;

            if (hud == null) { return; }

            hud.SetPlayer2Hp(hp, maxHp);
        }

        void HUD_Training_SetTime(float time) {
            HUD_Training hud = ctx.hud_Training;

            if (hud == null) { return; }

            hud.SetTime(time);
        }

        public void HUD_Training_CD_Open() {
            if (ctx.hud_Training_CD2 != null) { return; }

            GameObject prefab = ctx.assetsCore.HUD_GetTraining_CD();

            GameObject go = GameObject.Instantiate(prefab, ctx.screenCanvas.transform);
            HUD_Training_CD hud = go.GetComponent<HUD_Training_CD>();
            hud.Ctor();
            hud.Show();

            if (ctx.hud_Training_CD1 != null) {
                ctx.hud_Training_CD2 = hud;
            } else {
                ctx.hud_Training_CD1 = hud;
            }
        }

        public void HUD_Training_CD_Close() {
            if (ctx.hud_Training_CD1 != null) {
                ctx.hud_Training_CD1.TearDown();
                ctx.hud_Training_CD1 = null;
            }
            if (ctx.hud_Training_CD2 != null) {
                ctx.hud_Training_CD2.TearDown();
                ctx.hud_Training_CD2 = null;
            }
        }

        public void HUD_Training_CD_Update(RoleEntity role, float attackCD, float attackMax, float defenseCD, float defenseMax) {
            HUD_Training_CD hud;
            Vector3 pos = role.TF_GetPos();

            if (role.t_roleType == T_RoleType.Player1) {
                hud = ctx.hud_Training_CD1;
                pos.x -= 1;
            } else if (role.t_roleType == T_RoleType.Player2) {
                hud = ctx.hud_Training_CD2;
                pos.x += 1;
            } else {
                return;
            }

            if (hud == null) { return; }

            hud.SetAttackCD(attackCD, attackMax);
            hud.SetDefenseCD(defenseCD, defenseMax);

            hud.SetPos(pos);
        }
        #endregion

        #region HUD_Eliminate
        public void HUD_Eliminate_Open(float time) {
            HUD_Eliminate hud = ctx.hud_Eliminate;

            if (hud == null) {
                GameObject prefab = ctx.assetsCore.HUD_GetEliminate();

                GameObject go = GameObject.Instantiate(prefab, ctx.screenCanvas.transform);
                hud = go.GetComponent<HUD_Eliminate>();
                hud.Ctor();
            }

            ctx.hud_Eliminate = hud;
            hud.Show();

            hud.Set_Time(time);
            hud.Set_Text();
        }

        public void HUD_Eliminate_Close() {
            HUD_Eliminate hud = ctx.hud_Eliminate;

            if (hud == null) { return; }

            hud.TearDown();
            ctx.hud_Eliminate = null;
        }

        public void HUD_Eliminate_Update(float progress, int number) {
            HUD_Eliminate hud = ctx.hud_Eliminate;

            if (hud == null) { return; }

            hud.Set_Progress(progress);
            hud.Set_NumberText(number);
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

                panel.OnStageClickHandle = (stageID) => {
                    eventCenter.OnStageClick(stageID);
                };

                panel.OnChapterClickHandle = (chapterID) => {
                    eventCenter.OnChapterClick(chapterID);
                };

                panel.OnCloseHandle = () => {
                    Panel_StageSelection_Close();
                };

                panel.OnBackHandle = () => {
                    eventCenter.OnBackClick();
                };

            }

            ctx.panel_StageSelection = panel;
            panel.Show();
            panel.SetNativeSize();
        }

        public void Panel_StageSelection_Tick(bool isTabDown, bool isEscDown) {
            Panel_StageSelection panel = ctx.panel_StageSelection;
            if (panel == null) {
                return;
            }
            panel.Tick(isTabDown, isEscDown);
        }

        public void Panel_StageSelectionOneStage_Add(int stageID, bool isPassed, int getStarCount) {
            Panel_StageSelection panel = ctx.panel_StageSelection;
            if (panel == null) {
                return;
            }
            panel.Add(stageID, isPassed, getStarCount);
        }

        public void Panel_StageSelection_SetStarNum(int starNum) {
            Panel_StageSelection panel = ctx.panel_StageSelection;
            if (panel == null) {
                return;
            }
            panel.Set_StarNum(starNum);
        }

        // 当前关卡的星星
        public void Panel_StageSelection_SetGetStar(int stageID, int starCount) {
            Panel_StageSelection panel = ctx.panel_StageSelection;
            if (panel == null) {
                return;
            }
            panel.Set_GetStar(stageID, starCount);
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

        #region  CardSelection

        public void Panel_CardSelection_Open(int stageID, int ChapterID) {
            Panel_CardSelection panel = ctx.panel_CardSelection;

            if (panel == null) {
                GameObject prefab = ctx.assetsCore.Panel_GetCardSelection();

                GameObject go = GameObject.Instantiate(prefab, ctx.screenCanvas.transform);
                panel = go.GetComponent<Panel_CardSelection>();
                panel.Ctor();

                panel.OnSelectHeroClickHandle = () => {
                    eventCenter.OnSelectHeroClick();
                };

                panel.OnSelectAgrClickHandle = () => {
                    eventCenter.OnSelectAgrClick();
                };

                panel.OnSelectIndClickHandle = () => {
                    eventCenter.OnSelectIndClick();
                };

                panel.OnSelectSciClickHandle = () => {
                    eventCenter.OnSelectSciClick();
                };

                panel.OnSelectEnterClickHandle = () => {
                    eventCenter.OnSelectEnterClick();
                };

                panel.OnSelectBackClickHandle = () => {
                    eventCenter.OnSelectBackClick();
                };

                // Card Click
                panel.OnSelectionCardClickHandle = (id) => {
                    eventCenter.OnSelectionCardClick(id);
                };

                // Card Confirm
                panel.OnConfirmClickHandle = (typeID) => {
                    eventCenter.OnConfirmClick(typeID);
                };

                panel.OnCardSelectedClickHandle = (typeID) => {
                    eventCenter.OnCardSelectedClick(typeID);
                };

            }

            panel.Set_Text(stageID, ChapterID);
            panel.Show();
            ctx.panel_CardSelection = panel;
        }

        public void Panel_CardSelection_AddCard(int id, Sprite sprite) {
            Panel_CardSelection panel = ctx.panel_CardSelection;
            if (panel == null) {
                return;
            }
            panel.AddCard(id, sprite);
        }

        // CardSelected
        public void Panel_CardSelection_AddCardSelected(int slotMax) {
            Panel_CardSelection panel = ctx.panel_CardSelection;
            if (panel == null) {
                return;
            }
            panel.InitCardSelected(slotMax);
        }

        public void Panel_CardSelection_RemoveCardSelected(int typeID) {
            Panel_CardSelection panel = ctx.panel_CardSelection;
            if (panel == null) {
                return;
            }
            panel.RemoveCardSelected(typeID);

            Panel_CardDetailsBtn_UnLock();

            CardType type = Panel_GetCardType();
            if (type == CardType.Agriculture || type == CardType.Industry) {
                int id = Panel_CardDetails_GetTypeID();

                if (id == -1) { return; }

                Panel_CardSelected_TakeAll(out List<int> typeIDs);
                int len = typeIDs.Count;

                for (int i = 0; i < len; i++) {
                    if (id == typeIDs[i]) {
                        Panel_CardDetailsBtn_Lock();
                        return;
                    }
                }
            }
        }

        public void Panel_CardSelected_TakeAll(out List<int> typeIDs) {
            Panel_CardSelection panel = ctx.panel_CardSelection;
            if (panel == null) {
                typeIDs = new List<int>();
                return;
            }
            panel.TakeAll(out typeIDs);
        }


        public void Panel_CardSelection_SetHero(Sprite sprite) {
            Panel_CardSelection panel = ctx.panel_CardSelection;
            if (panel == null) {
                return;
            }
            panel.CardSelected_SetHero(sprite);
        }

        public void Panel_CardSelection_SetScience(Sprite sprite) {
            Panel_CardSelection panel = ctx.panel_CardSelection;
            if (panel == null) {
                return;
            }
            panel.CardSelected_SetScience(sprite);
        }

        // 这种typeID本就是从tm里传进来的, 没必要为了精简重复获取感觉
        public bool Panel_CardSelection_SetProp(int typeID, Sprite sprite) {
            Panel_CardSelection panel = ctx.panel_CardSelection;
            if (panel == null) {
                return false;
            }
            return panel.CardSelected_SetProp(typeID, sprite);
        }

        public void Panel_OneCardSelection_Clear() {
            Panel_CardSelection panel = ctx.panel_CardSelection;
            if (panel == null) {
                return;
            }
            panel.Clear();
        }

        public void Panel_OneCardSelection_Lock(int id) {
            Panel_CardSelection panel = ctx.panel_CardSelection;
            if (panel == null) {
                return;
            }
            panel.LockCard(id);
        }

        public void Panel_OneCardSelection_UnLock(int id) {
            Panel_CardSelection panel = ctx.panel_CardSelection;
            if (panel == null) {
                return;
            }
            panel.UnLockCard(id);
        }

        public void Panel_SetCardType(CardType type) {
            Panel_CardSelection panel = ctx.panel_CardSelection;
            if (panel == null) {
                return;
            }
            panel.Set_CardType(type);
        }

        public CardType Panel_GetCardType() {
            Panel_CardSelection panel = ctx.panel_CardSelection;
            if (panel == null) {
                return CardType.None;
            }
            return panel.Get_CardType();
        }

        public void Panel_CardDetails_Update(Sprite sprite, string content, int typeID) {
            Panel_CardSelection panel = ctx.panel_CardSelection;
            if (panel == null) {
                return;
            }
            panel.UpdateCardDetail(sprite, content, typeID);

            Panel_CardDetailsBtn_UnLock();

            CardType type = Panel_GetCardType();
            if (type == CardType.Hero) {
                if (selectedInFormation.roleSelectedTypeID == typeID) {
                    Panel_CardDetailsBtn_Lock();
                }

            } else if (type == CardType.Agriculture || type == CardType.Industry) {
                CardModel card = Get_CardModel();
                Panel_CardSelected_TakeAll(out List<int> typeIDs);
                int len = typeIDs.Count;

                if (len == card.slotMax) {
                    Panel_CardDetailsBtn_Lock();
                    return;
                }

                for (int i = 0; i < len; i++) {
                    if (typeID == typeIDs[i]) {
                        Panel_CardDetailsBtn_Lock();
                        return;
                    }
                }

            } else if (type == CardType.Science) {
                if (selectedInFormation.scienceTypeID == typeID) {
                    Panel_CardDetailsBtn_Lock();
                }
            }
        }

        public void Panel_CardDetailsBtn_Lock() {
            Panel_CardSelection panel = ctx.panel_CardSelection;
            if (panel == null) {
                return;
            }
            panel.CardDetailsBtn_Lock();
        }

        public void Panel_CardDetailsBtn_UnLock() {
            Panel_CardSelection panel = ctx.panel_CardSelection;
            if (panel == null) {
                return;
            }
            panel.CardDetailsBtn_UnLock();
        }

        public int Panel_CardDetails_GetTypeID() {
            Panel_CardSelection panel = ctx.panel_CardSelection;
            if (panel == null) {
                return -1;
            }
            return panel.CardDetails_GetTypeID();
        }

        public void Panel_CardSelection_Close() {
            Panel_CardSelection panel = ctx.panel_CardSelection;
            if (panel == null) {
                return;
            }
            panel.TearDown();
            ctx.panel_CardSelection = null;
        }

        public void Panel_CardSelected_SciButton(bool isShow) {
            Panel_CardSelection panel = ctx.panel_CardSelection;
            if (panel == null) {
                return;
            }
            panel.SetSicButton(isShow);
        }

        public void Panel_CardSelected_IndButton(bool isShow) {
            Panel_CardSelection panel = ctx.panel_CardSelection;
            if (panel == null) {
                return;
            }
            panel.SetIndButton(isShow);
        }

        public void Panel_CardSelected_AgrButton(bool isShow) {
            Panel_CardSelection panel = ctx.panel_CardSelection;
            if (panel == null) {
                return;
            }
            panel.SetAgrButton(isShow);
        }

        public void Panel_CardSelected_EnterGame(bool isShow) {
            Panel_CardSelection panel = ctx.panel_CardSelection;
            if (panel == null) {
                return;
            }
            panel.SetEnterGameButton(isShow);
        }

        public void Panel_CardSelected_SciTxtActive(bool isActive) {
            Panel_CardSelection panel = ctx.panel_CardSelection;
            if (panel == null) {
                return;
            }
            panel.Set_sicTxtActive(isActive);
        }

        public void Panel_CardSelected_PropTxtActive(bool isActive) {
            Panel_CardSelection panel = ctx.panel_CardSelection;
            if (panel == null) {
                return;
            }
            panel.Set_PropTxtActice(isActive);
        }

        #endregion

        #region Notice
        public bool Panel_Notice_Open(string content) {
            Panel_Notice panel = ctx.panel_Notice;

            if (panel == null) {
                GameObject prefab = ctx.assetsCore.Panel_GetNotice();
                GameObject go = GameObject.Instantiate(prefab, ctx.screenCanvas.transform);
                panel = go.GetComponent<Panel_Notice>();
                panel.Ctor();
            }
            panel.Inti(content);
            ctx.panel_Notice = panel;
            panel.Show();
            return true;
        }

        public void Panel_Notice_Close() {
            Panel_Notice panel = ctx.panel_Notice;
            if (panel == null) {
                return;
            }
            panel.TearDown();
            ctx.panel_Notice = null;
        }

        #endregion

        #region BackSelection
        public void Panel_BackSelection_Open() {
            Panel_BackSelection panel = ctx.panel_BackSelection;

            if (panel == null) {
                GameObject prefab = ctx.assetsCore.Panel_GetBackSelection();
                GameObject go = GameObject.Instantiate(prefab, ctx.screenCanvas.transform);
                panel = go.GetComponent<Panel_BackSelection>();
                panel.Ctor();

                panel.OnCloseWarningClickHandle = () => {
                    eventCenter.OnCloseWarningClick();
                };

                panel.OnContinueWarningClickHandle = () => {
                    eventCenter.OnContinueWarningClick();
                };

                panel.OnResetWarningClickHandle = () => {
                    eventCenter.OnResetWarningClick();
                };

                panel.OnQuitWarningClickHandle = () => {
                    eventCenter.OnQuitWarningClick();
                };
            }

            ctx.panel_BackSelection = panel;
            panel.Show();
        }

        public void Panel_BackSelection_Close() {
            Panel_BackSelection panel = ctx.panel_BackSelection;
            if (panel == null) {
                return;
            }
            panel.TearDown();
            ctx.panel_BackSelection = null;
        }
        #endregion

        #region WatchTower
        public void Panel_WatchTower_Open() {
            Panel_WatchTower panel = ctx.panel_Watchtower;

            if (panel == null) {
                GameObject prefab = ctx.assetsCore.Panel_GetWatchTower();
                GameObject go = GameObject.Instantiate(prefab, ctx.screenCanvas.transform);
                panel = go.GetComponent<Panel_WatchTower>();
                panel.Ctor();

                panel.OnViewClickHandle = () => {
                    eventCenter.OnViewClick();
                };
            }
            ctx.panel_Watchtower = panel;
            panel.Show();
        }

        public void Panel_WatchTower_Add(int id, Sprite sprite) {
            Panel_WatchTower panel = ctx.panel_Watchtower;

            if (panel == null) { return; }

            panel.Add(id, sprite);
        }

        public void Panel_WatchTower_Close() {
            Panel_WatchTower panel = ctx.panel_Watchtower;

            if (panel == null) { return; }

            panel.TearDown();
            ctx.panel_Watchtower = null;
        }
        #endregion

        #region WatchDetail
        public void Panel_WatchDetail_Open(int typeID) {
            Panel_WatchDetail panel = ctx.panel_WatchDetail;

            if (panel == null) {
                GameObject prefab = ctx.assetsCore.Panel_GetWatchDetail();
                GameObject go = GameObject.Instantiate(prefab, ctx.screenCanvas.transform);
                panel = go.GetComponent<Panel_WatchDetail>();
                panel.Ctor();

                panel.OnCloseClickHandle = () => {
                    eventCenter.OnDetailClose();
                };
            }
            ctx.panel_WatchDetail = panel;
            panel.Show();

            ctx.templateMoudle.Role_TryGet(typeID, out RoleTM tm);
            var sprite = tm.sprite;
            var name = tm.typeName;
            var feature = tm.feature;
            var hp = tm.hp;
            var skill = tm.skill;
            var restraint = tm.restraint;
            panel.Init(sprite, name, feature, hp, skill, restraint);
        }

        public void Panel_WatchDetail_Open_Stuff(int typeID) { // 给stuff用
            Panel_WatchDetail panel = ctx.panel_WatchDetail;

            if (panel == null) {
                GameObject prefab = ctx.assetsCore.Panel_GetWatchDetail();
                GameObject go = GameObject.Instantiate(prefab, ctx.screenCanvas.transform);
                panel = go.GetComponent<Panel_WatchDetail>();
                panel.Ctor();

                panel.OnCloseClickHandle = () => {
                    eventCenter.OnDetailClose();
                };
            }
            ctx.panel_WatchDetail = panel;
            panel.Show();

            ctx.templateMoudle.Stuff_TryGet(typeID, out StuffTM tm);
            var sprite = tm.sprite;
            var name = tm.typeName;
            var feature = tm.description;
            panel.Init(sprite, name, feature);
        }

        public void Panel_WatchDetail_Close() {
            Panel_WatchDetail panel = ctx.panel_WatchDetail;

            if (panel == null) { return; }

            panel.TearDown();
            ctx.panel_WatchDetail = null;
        }
        #endregion

        #region Training
        public void Panel_Training_Select_Open() {
            Panel_Training_Select panel = ctx.panel_Training_Select;

            if (panel == null) {
                GameObject prefab = ctx.assetsCore.Panel_GetTraining_Select();
                GameObject go = GameObject.Instantiate(prefab, ctx.screenCanvas.transform);
                panel = go.GetComponent<Panel_Training_Select>();
                panel.Ctor();

                panel.OnPlayer1ReadyClickHandle = () => {
                    eventCenter.OnPlayer1ReadyClick();
                };

                panel.OnPlayer2ReadyClickHandle = () => {
                    eventCenter.OnPlayer2ReadyClick();
                };

                panel.OnTipClickHandle = () => {
                    eventCenter.OnTrainTipClick();
                };

                panel.OnBackClickHandle = () => {
                    eventCenter.OnTrainBackClick();
                };

                panel.OnSelectionTrainCardClickHandle = (typeID) => {
                    eventCenter.OnSelectionTrainCardClick(typeID);
                };
            }
            ctx.panel_Training_Select = panel;
            panel.Show();
            panel.Init();
        }

        public void Panel_Training_Select_Add(int id, Sprite sprite, string name) {
            Panel_Training_Select panel = ctx.panel_Training_Select;

            if (panel == null) { return; }

            panel.Add(id, sprite, name);
        }

        public void Panel_Training_Select_Close() {
            Panel_Training_Select panel = ctx.panel_Training_Select;

            if (panel == null) { return; }

            panel.TearDown();
            ctx.panel_Training_Select = null;
        }

        public void Panel_Training_Select_SetPlayer(int typeID) {
            Panel_Training_Select panel = ctx.panel_Training_Select;

            if (panel == null) { return; }

            ctx.templateMoudle.Role_TryGet(typeID, out RoleTM roleTM);
            var skill = roleTM.skillorSoArray[0];
            ctx.templateMoudle.Bullet_TryGet(skill.tm.typeID, out BulletTM bulletTM);

            var sprite = roleTM.sprite;
            var feature = roleTM.feature;
            var weaponSpeed = bulletTM.moveSpeed.ToString();
            var damage = bulletTM.attackDamage.ToString();
            var attackSpeed = skill.tm.cd.ToString();
            
            panel.SetPlayer(typeID, sprite, feature, weaponSpeed, damage, attackSpeed);
        }

        public void Panel_Training_Select_AddPlayer() {
            Panel_Training_Select panel = ctx.panel_Training_Select;

            if (panel == null) { return; }

            panel.AddPlayer();
        }

        public void Panel_Training_Select_SetButton() {
            Panel_Training_Select panel = ctx.panel_Training_Select;

            if (panel == null) { return; }

            panel.SetButton();
        }

        public void Panel_Training_Select_TakeAll(out List<int> typeIDs) {
            Panel_Training_Select panel = ctx.panel_Training_Select;
            if (panel == null) {
                typeIDs = new List<int>();
                return;
            }
            panel.TakeAll(out typeIDs);
        }

        public void Panel_Training_Tip_Open() {
            Panel_Training_Tip panel = ctx.panel_Training_Tip;

            if (panel == null) {
                GameObject prefab = ctx.assetsCore.Panel_GetTraining_Tip();
                GameObject go = GameObject.Instantiate(prefab, ctx.screenCanvas.transform);
                panel = go.GetComponent<Panel_Training_Tip>();
                panel.Ctor();

                panel.OnCloseClickHandle = () => {
                    eventCenter.OnTrainTipCloseClick();
                };
            }
            ctx.panel_Training_Tip = panel;
            panel.Show();
        }

        public void Panel_Training_Tip_Close() {
            Panel_Training_Tip panel = ctx.panel_Training_Tip;

            if (panel == null) { return; }

            panel.TearDown();
            ctx.panel_Training_Tip = null;
        }

        public void Panel_Training_Button_Show() {
            Panel_Training_Button panel = ctx.panel_Training_Button;

            if (panel == null) {
                GameObject prefab = ctx.assetsCore.Panel_GetTraining_Button();

                GameObject go = GameObject.Instantiate(prefab, ctx.screenCanvas.transform);
                panel = go.GetComponent<Panel_Training_Button>();

                panel.Ctor();

                panel.OnPauseClickHandle += () => {
                    eventCenter.OnTrainPauseClick();
                };
            }

            ctx.panel_Training_Button = panel;

            panel.Show();
        }

        public void Panel_Training_Button_Close() {
            Panel_Training_Button panel = ctx.panel_Training_Button;

            if (panel == null) { return; }

            panel.TearDown();
            ctx.panel_Training_Button = null;
        }

        public void Panel_Training_Pause_Show() {
            Panel_Training_Pause panel = ctx.panel_Training_Pause;

            if (panel == null) {
                GameObject prefab = ctx.assetsCore.Panel_GetTraining_Pause();

                GameObject go = GameObject.Instantiate(prefab, ctx.screenCanvas.transform);
                panel = go.GetComponent<Panel_Training_Pause>();

                panel.Ctor();

                panel.OnContinueClickHandle += () => {
                    eventCenter.OnTrainContinueClick();
                };

                panel.OnIllustratedClickHandle += () => {
                    eventCenter.OnTrainIllustratedClick();
                };

                panel.OnQuitClickHandle += () => {
                    eventCenter.OnTrainQuitClick();
                };
            }

            ctx.panel_Training_Pause = panel;

            panel.Show();
        }

        public void Panel_Training_Pause_Close() {
            Panel_Training_Pause panel = ctx.panel_Training_Pause;

            if (panel == null) { return; }

            panel.TearDown();
            ctx.panel_Training_Pause = null;
        }
        #endregion

        #region TrainingOver
        public void Panel_Training_GameOver_Open(string text, Sprite winner) {
            Panel_Training_GameOver panel = ctx.panel_Training_GameOver;

            if (panel != null) { return; }

            if (panel == null) {
                GameObject prefab = ctx.assetsCore.Panel_GetTraining_GameOver();
                GameObject go = GameObject.Instantiate(prefab, ctx.screenCanvas.transform);
                panel = go.GetComponent<Panel_Training_GameOver>();
                panel.Ctor();

                panel.OnResetClickHandle = () => {
                    eventCenter.OnTrainOverResetClick();
                };

                panel.OnBackClickHandle = () => {
                    eventCenter.OnTrainOverBackClick();
                };
            }
            ctx.panel_Training_GameOver = panel;
            panel.Show();

            Panel_Training_GameOver_SetText(text);
            Panel_Training_GameOver_SetWinner(winner);
        }

        public void Panel_Training_GameOver_Close() {
            Panel_Training_GameOver panel = ctx.panel_Training_GameOver;

            if (panel == null) { return; }

            panel.TearDown();
            ctx.panel_Training_GameOver = null;
        }

        void Panel_Training_GameOver_SetText(string text) {
            Panel_Training_GameOver panel = ctx.panel_Training_GameOver;

            if (panel == null) { return; }

            panel.SetText(text);
        }

        public void Panel_Training_GameOver_SetWinner(Sprite sprite) {
            Panel_Training_GameOver panel = ctx.panel_Training_GameOver;

            if (panel == null) { return; }

            panel.SetWinner(sprite);
        }
        #endregion

        #region Training Back
        public void Panel_Training_Back_Open() {
            Panel_Training_Back panel = ctx.panel_Training_Back;

            if (panel == null) {
                GameObject prefab = ctx.assetsCore.Panel_GetTraining_Back();
                GameObject go = GameObject.Instantiate(prefab, ctx.screenCanvas.transform);
                panel = go.GetComponent<Panel_Training_Back>();
                panel.Ctor();

                panel.OnCloseClickHandle = () => {
                    eventCenter.OnTrainBackCloseClick();
                };

                panel.OnContinueClickHandle = () => {
                    eventCenter.OnTrainBackContinueClick();
                };

                panel.OnQuitClickHandle = () => {
                    eventCenter.OnTrainBackQuitClick();
                };
            }

            ctx.panel_Training_Back = panel;
            panel.Show();
        }

        public void Panel_Training_Back_Close() {
            Panel_Training_Back panel = ctx.panel_Training_Back;

            if (panel == null) { return; }

            panel.TearDown();
            ctx.panel_Training_Back = null;
        }
        #endregion

        #region Illustrated
        public void Panel_Illustrated_Open() {
            Panel_Illustrated panel = ctx.panel_Illustrated;

            if (panel == null) {
                GameObject prefab = ctx.assetsCore.Panel_GetIllustrated();
                GameObject go = GameObject.Instantiate(prefab, ctx.screenCanvas.transform);
                panel = go.GetComponent<Panel_Illustrated>();
                panel.Ctor();

                panel.OnCloseClickHandle = () => {
                    eventCenter.OnGameIllusCardClick();
                };

                panel.OnGameIllusCardClickHandle = (typeID) => {
                    eventCenter.OnGameIllusCardClick(typeID);
                };

                panel.OnGameIllusStuffCardClickHandle = (typeID) => {
                    eventCenter.OnGameIllusStuffCardClick(typeID);
                };
            }
            ctx.panel_Illustrated = panel;
            panel.Show();
        }

        public void Panel_Illustrated_Add(int typeID, Sprite sprite, bool isRole) {
            Panel_Illustrated panel = ctx.panel_Illustrated;

            if (panel == null) { return; }

            panel.Add(typeID, sprite, isRole);
        }

        public void Panel_Illustrated_Close() {
            Panel_Illustrated panel = ctx.panel_Illustrated;

            if (panel == null) { return; }

            panel.TearDown();
            ctx.panel_Illustrated = null;
        }

        public bool Panel_Illustrated_TryGet(int typeID) {
            Panel_Illustrated panel = ctx.panel_Illustrated;

            if (panel == null) { return false; }

            return panel.TryGet(typeID);
        }
        #endregion

        #region TrainIllustrated
        public void Panel_TrainIllustrated_Open() {
            Panel_TrainIllustrated panel = ctx.panel_TrainIllustrated;

            if (panel == null) {
                GameObject prefab = ctx.assetsCore.Panel_GetTrainIllustrated();
                GameObject go = GameObject.Instantiate(prefab, ctx.screenCanvas.transform);
                panel = go.GetComponent<Panel_TrainIllustrated>();
                panel.Ctor();

                panel.OnCloseClickHandle = () => {
                    eventCenter.OnTrainIllusCardClick();
                };

                panel.OnGameIllusCardClickHandle = (typeID) => {
                    eventCenter.OnTrainIllusCardClick(typeID);
                };
            }
            ctx.panel_TrainIllustrated = panel;
            panel.Show();
        }

        public void Panel_TrainIllustrated_Add(int typeID, Sprite sprite) {
            Panel_TrainIllustrated panel = ctx.panel_TrainIllustrated;

            if (panel == null) { return; }

            panel.Add(typeID, sprite);
        }

        public void Panel_TrainIllustrated_Close() {
            Panel_TrainIllustrated panel = ctx.panel_TrainIllustrated;

            if (panel == null) { return; }

            panel.TearDown();
            ctx.panel_TrainIllustrated = null;
        }
        #endregion

        #region Eliminate资源战
        public void Panel_Eliminate_Button_Open() {
            Panel_Eliminate_Button panel = ctx.panel_Eliminate_Button;

            if (panel == null) {
                GameObject prefab = ctx.assetsCore.Panel_GetEliminate_Button();
                GameObject go = GameObject.Instantiate(prefab, ctx.screenCanvas.transform);
                panel = go.GetComponent<Panel_Eliminate_Button>();
                panel.Ctor();

                panel.OnPauseClickHandle = () => {
                    eventCenter.OnEliminateCloseClick();
                };
            }
            ctx.panel_Eliminate_Button = panel;
            panel.Show();
        }

        public void Panel_Eliminate_Button_Close() {
            Panel_Eliminate_Button panel = ctx.panel_Eliminate_Button;

            if (panel == null) { return; }

            panel.TearDown();
            ctx.panel_Eliminate_Button = null;
        }

        public void Panel_Eliminate_Pause_Open() {
            Panel_Eliminate_Pause panel = ctx.panel_Eliminate_Pause;

            if (panel == null) {
                GameObject prefab = ctx.assetsCore.Panel_GetEliminate_Pause();
                GameObject go = GameObject.Instantiate(prefab, ctx.screenCanvas.transform);
                panel = go.GetComponent<Panel_Eliminate_Pause>();
                panel.Ctor();

                panel.OnContinueClickHandle = () => {
                    eventCenter.OnEliminateContinueClick();
                };

                panel.OnIllustratedClickHandle = () => {
                    eventCenter.OnEliminateIllustratedClick();
                };

                panel.OnQuitClickHandle = () => {
                    eventCenter.OnEliminateQuitClick();
                };
            }
            ctx.panel_Eliminate_Pause = panel;
            panel.Show();
        }

        public void Panel_Eliminate_Pause_Close() {
            Panel_Eliminate_Pause panel = ctx.panel_Eliminate_Pause;

            if (panel == null) { return; }

            panel.TearDown();
            ctx.panel_Eliminate_Pause = null;
        }



        #endregion

        #region Setting
        public void Panel_Setting_Open() {
            Panel_Setting panel = ctx.panel_Setting;

            if (panel == null) {
                GameObject prefab = ctx.assetsCore.Panel_GetSetting();
                GameObject go = GameObject.Instantiate(prefab, ctx.screenCanvas.transform);
                panel = go.GetComponent<Panel_Setting>();
                panel.Ctor();

                panel.OnBackClickHandle = () => {
                    eventCenter.OnSettingBackClick();
                };
            }
            ctx.panel_Setting = panel;
            panel.Show();
        }

        public void Panel_Setting_Close() {
            Panel_Setting panel = ctx.panel_Setting;

            if (panel == null) { return; }

            panel.TearDown();
            ctx.panel_Setting = null;
        }
        #endregion
    }
}