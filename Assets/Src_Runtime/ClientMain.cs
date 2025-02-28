using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UF.System_Train;

namespace UF {

    public class ClientMain : MonoBehaviour {

        [SerializeField] Canvas screenCanvas;
        bool isInit = false;
        bool isTearDown = false;

        public PlayerEntity playerSaveData;

        // system
        public static GameSystem gameSystem;
        public static LoginSystem loginSystem;
        public static PauseSystem pauseSystem;
        public static TrainSystem trainSystem;

        // ==== Cores ====
        public static AppUI appUI;

        // ==== Modules ====
        public static AssetsModule assetsModule;
        public static TemplateMoudle templateModule;
        public static InputModule inputModule;

        // repos
        public RoleRepository roleRepository;
        public BulletRepository bulletRepository;
        public PropRepository propRepository;

        // TODO:不能写在这里 没想好好放哪里先放这里


        void Start() {

            // ---  UI
            appUI = GetComponentInChildren<AppUI>();
            appUI.Ctor();

            // ---  GameSystem  ---
            gameSystem = GetComponentInChildren<GameSystem>();
            gameSystem.Ctor();

            loginSystem = GetComponentInChildren<LoginSystem>();
            loginSystem.Ctor();

            pauseSystem = GetComponentInChildren<PauseSystem>();
            pauseSystem.Ctor();

            trainSystem = GetComponentInChildren<TrainSystem>();
            trainSystem.Ctor();

            // ---  Modules  ---
            assetsModule = GetComponentInChildren<AssetsModule>();
            assetsModule.Ctor();

            templateModule = GetComponentInChildren<TemplateMoudle>();
            templateModule.Ctor();

            inputModule = GetComponentInChildren<InputModule>();
            inputModule.Ctor();

            // repos
            roleRepository = new RoleRepository();
            bulletRepository = new BulletRepository();
            propRepository = new PropRepository();

            playerSaveData = new PlayerEntity();


            // Inject
            gameSystem.Inject(screenCanvas, assetsModule, templateModule, appUI, inputModule, roleRepository, bulletRepository, playerSaveData);
            loginSystem.Inject(appUI, inputModule);
            pauseSystem.Inject(appUI, roleRepository, bulletRepository, propRepository);
            trainSystem.Inject(appUI, templateModule, assetsModule, inputModule, roleRepository, bulletRepository, propRepository);

            appUI.Inject(screenCanvas, assetsModule, playerSaveData);

            // ---  Binding  ---
            // Binding_Login();
            Login_Binding();
            Game_Binding();
            Pause_Binding();
            Train_Binding();

            Action action = async () => {

                await assetsModule.LoadAll();
                await templateModule.LoadAll();

                isInit = true;

                // ---  Enter  ---
                loginSystem.EnterPanel_Login();
            };

            action.Invoke();

        }

        #region Binding_Login
        void Login_Binding() {
            loginSystem.Login_Binding();

            var loginEvent = loginSystem.Events;

            loginEvent.OnLoginStartHandle += () => {
                appUI.Panel_Login_Close();
                UIDomain.Panel_StageSelectionEnter(appUI);
            };

            loginEvent.OnTrainingHandle += () => {
                appUI.Panel_Login_Close();
                UIDomain.Panel_Training_Select_Open(appUI, assetsModule, templateModule);
            };

            loginEvent.OnSettingHandle += () => {
                Debug.Log("设置");
            };

            loginEvent.OnQuitHandle += () => {
                Application.Quit();
                Debug.Log("Game Quit");
            };

            // StageSelection
            loginEvent.OnStageClickHandle += (stageID) => {
                appUI.Panel_StageSelection_Close();
                appUI.playerSaveData.SetStageCurID(stageID);
                UIDomain.Panel_CardSelection_Open(appUI, assetsModule, templateModule);

                var card = appUI.Get_CardModel();

                int indCount = card.GetBulletIndTMsCount();
                if (indCount == 0) {
                    Debug.Log("不用选择工业 + 按键设为fasle");
                    appUI.Panel_CardSelected_IndButton(false);
                }

                int agrCount = card.GetBulletAgrTMsCount();
                if (agrCount == 0) {
                    Debug.Log("不用选择农业 + 按键设为fasle");
                    appUI.Panel_CardSelected_AgrButton(false);
                }

                int sciCount = card.GetBulletSciTMsCount();
                if (sciCount == 0) {
                    Debug.Log("不用选择科技 + 按键设为fasle");
                    appUI.Panel_CardSelected_SciButton(false);
                }

            };

            loginEvent.OnChapterClickHandle += (chapter) => {
                var maxChapter = appUI.playerSaveData.GetUnLockChapterID();
                if (chapter > maxChapter) {
                    chapter = 1;
                }
                appUI.playerSaveData.SetChapterCurID(chapter);
                UIDomain.Panel_StageSelection_ChangeChapter(appUI, chapter, maxChapter);
            };

            loginEvent.OnSelectBackHandle += () => {
                appUI.Panel_StageSelection_Close();
                appUI.Panel_Login_Open();
            };

            // CardSelection
            loginEvent.OnSelectHeroClickHandle += () => {
                appUI.Panel_SetCardType(CardType.Hero);
                UIDomain.Panel_OneCardSelection_Update(appUI);
            };

            loginEvent.OnSelectAgrClickHandle += () => {
                appUI.Panel_SetCardType(CardType.Agriculture);
                UIDomain.Panel_OneCardSelection_Update(appUI);

            };

            loginEvent.OnSelectIndClickHandle += () => {
                appUI.Panel_SetCardType(CardType.Industry);
                UIDomain.Panel_OneCardSelection_Update(appUI);
            };

            loginEvent.OnSelectSciClickHandle += () => {
                appUI.Panel_SetCardType(CardType.Science);
                UIDomain.Panel_OneCardSelection_Update(appUI);
            };

            loginEvent.OnSelectEnterGameClickHandle += () => {
                Debug.Log("EnterGame");
                // 放到UIDomain里
                appUI.Panel_CardSelected_TakeAll(out List<int> typeIDs);

                int stageID = appUI.playerSaveData.GetStageCurID();
                int roleTypeID = appUI.selectedInFormation.roleSelectedTypeID;
                int sciType = appUI.selectedInFormation.scienceTypeID;
                int slotMax = appUI.playerSaveData.GetSlotMax();

                appUI.Reset();
                appUI.Panel_CardSelection_Close();
                appUI.Panel_WatchTower_Close();
                loginSystem.Exit();
                gameSystem.Enter(stageID, roleTypeID, typeIDs.ToArray(), sciType);
            };

            loginEvent.OnSelectBackClickHandle += () => {
                Debug.Log("SelectBack");
                appUI.Panel_CardSelection_Close();
                appUI.Panel_WatchTower_Close();
                UIDomain.Panel_StageSelectionEnter(appUI);
            };

            loginEvent.OnSelectionCardClickHandle += (typeID) => {
                Debug.Log("选择的卡片的ID " + typeID);
                UIDomain.Update_CardSelectionDetails(typeID, templateModule, appUI);
                appUI.Panel_CardDetailsBtn_UnLock();
            };

            loginEvent.OnConfirmClickHandle += (typeID) => {
                Debug.Log("确认选择  的卡片" + typeID);
                appUI.Panel_CardSelected_TakeAll(out List<int> typeIDs);
                int roleTypeID = appUI.selectedInFormation.roleSelectedTypeID;
                int sciType = appUI.selectedInFormation.scienceTypeID;
                if (typeIDs.Count >= appUI.playerSaveData.GetSlotMax() && roleTypeID == -1 && sciType == -1) {
                    Debug.Log("已经选择了" + typeIDs.Count + "张卡片");
                    appUI.Panel_Notice_Open("道具已经选完了");
                    return;
                }
                UIDomain.Panel_SelectedCardAddToSlot(appUI, templateModule, typeID);
                appUI.Panel_OneCardSelection_Lock(typeID);
                appUI.Panel_CardDetailsBtn_Lock();
            };

            loginEvent.OnCardSelectedClickHandle += (typeID) => {

                appUI.Panel_CardSelection_RemoveCardSelected(typeID);
                appUI.Panel_OneCardSelection_UnLock(typeID);
            };

            bool isClicked = false;
            loginEvent.OnViewClickHandle += () => {
                Debug.Log("ViewClick");
                appUI.Panel_WatchTower_SetCursor(); // 如果不点击瞭望塔头像, 会一直变化光标, 之后看看怎么改
                isClicked = true;
            };

            loginEvent.OnSelectionWatchCardClickHandle += (typeID) => { // typeID 应该是传进来的
                if (isClicked) {
                    Debug.Log("SelectionWatchCardClick " + typeID);
                    appUI.Panel_WatchTower_ClearCursor();
                    isClicked = false;

                    UIDomain.Panel_WatchTower_GetDetail(typeID, templateModule, appUI);
                }
            };

            loginEvent.OnDetailCloseClickHandle += () => {
                appUI.Panel_WatchDetail_Close();
            };

            // Train
            bool hasPlayer = false;

            loginEvent.OnPlayer1ReadyClickHandle += () => {
                if (hasPlayer) {
                    Debug.Log("Player1Ready");
                    appUI.Panel_Training_Select_SetButton();
                    appUI.Panel_Training_Select_AddPlayer();

                    hasPlayer = false;
                }
            };

            loginEvent.OnPlayer2ReadyClickHandle += () => {
                if (hasPlayer) {
                    appUI.Panel_Training_Select_Close();
                    appUI.Panel_Training_Select_AddPlayer();

                    appUI.Panel_Training_Select_TakeAll(out List<int> typeIDs);

                    loginSystem.Exit();
                    trainSystem.Enter(typeIDs);

                    appUI.HUD_Training_Open();
                    appUI.HUD_Training_CD_Open();
                    appUI.HUD_Training_CD_Open();
                }
            };

            loginEvent.OnTrainTipClickHandle += () => {
                appUI.Panel_Training_Tip_Open();
            };

            loginEvent.OnSelectionTrainCardClickHandle += (typeID) => {
                Debug.Log("SelectionTrainCardClick " + typeID);
                // 这样写感觉model没什么意义
                templateModule.Role_TryGet(typeID, out RoleTM roleTM);
                // TODO: 武器速度 伤害 攻击速度
                appUI.Panel_Training_Select_SetPlayer(typeID, roleTM.sprite, roleTM.feature, "0", "0", "0");

                hasPlayer = true;
            };

            loginEvent.OnTrainTipCloseClickHandle += () => {
                appUI.Panel_Training_Tip_Close();
            };
        }
        #endregion

        #region Binding_Game
        void Game_Binding() {
            gameSystem.Game_Binding();

            var gameEvent = gameSystem.events;

            gameEvent.OnGamePauseHandle += () => {
                appUI.Panel_GamePause_Open();
                gameSystem.Exit();
                pauseSystem.Enter();
            };

            gameEvent.OnAttackHandle += () => {
                inputModule.isUpdateProcess = false;
                inputModule.Set_InputCastKey(InputKeyFlag.Attack);
            };

            gameEvent.OnTechHandle += () => {
                inputModule.isUpdateProcess = false;
                inputModule.Set_InputCastKey(InputKeyFlag.Tech);
            };

            gameEvent.OnWeap1Handle += () => {
                inputModule.isUpdateProcess = false;
                inputModule.Set_InputCastKey(InputKeyFlag.Weap1);
            };

            gameEvent.OnWeap2Handle += () => {
                inputModule.isUpdateProcess = false;
                inputModule.Set_InputCastKey(InputKeyFlag.Weap2);
            };

            gameEvent.OnWeap3Handle += () => {
                inputModule.isUpdateProcess = false;
                inputModule.Set_InputCastKey(InputKeyFlag.Weap3);
            };

            gameEvent.OnWeap4Handle += () => {
                inputModule.isUpdateProcess = false;
                inputModule.Set_InputCastKey(InputKeyFlag.Weap4);
            };

            gameEvent.OnGameContinueOverHandle += (bool isWin) => {

                if (!isWin) {
                    Debug.Log("重新开始此关");
                    gameSystem.ClearGame();
                    gameSystem.Exit();
                    loginSystem.Enter();
                    UIDomain.Panel_CardSelection_Open(appUI, assetsModule, templateModule);
                }

                if (isWin) {
                    Debug.Log("进入下一关 进入备战(选卡片)页面");
                    gameSystem.ClearGame();
                    gameSystem.Exit();
                    loginSystem.Enter();
                    UIDomain.Panel_StageSelectionEnter(appUI);
                }
            };
        }
        #endregion

        #region Binding_Pause
        void Pause_Binding() {

            pauseSystem.Pause_Binding();

            var pauseEvent = pauseSystem.events;

            pauseEvent.OnPauseContinueHandle += () => {
                appUI.Panel_GamePause_Close();

                pauseSystem.Exit();
                gameSystem.Continue();
            };

            pauseEvent.OnPauseIllustratedHandle += () => {
                Debug.Log("Illustrated");
            };

            pauseEvent.OnPauseExitGameHandle += () => {
                appUI.Panel_BackSelection_Open();
                appUI.Panel_GamePause_Close();
            };

            pauseEvent.OnCloseWarningHandle += () => {
                appUI.Panel_BackSelection_Close();

                pauseSystem.Exit();
                gameSystem.Continue();
            };

            pauseEvent.OnContinueWarningHandle += () => {
                appUI.Panel_BackSelection_Close();

                pauseSystem.Exit();
                gameSystem.Continue();
            };

            pauseEvent.OnResetWarningHandle += () => {
                Debug.Log("重新开始此关");
            };

            pauseEvent.OnQuitWarningHandle += () => {
                Debug.Log("进入备战(选卡片)页面");
            };

        }
        #endregion

        #region Binding_Train
        void Train_Binding() {
            trainSystem.Train_Binding();

            var trainEvent = trainSystem.Events;

            trainEvent.OnTrainPauseClickHandle += () => {
                trainSystem.EnterPause();
                pauseSystem.Enter();
            };

            trainEvent.OnTrainContinueClickHandle += () => {
                trainSystem.ExitPause();
                pauseSystem.Exit();
            };

            trainEvent.OnTrainIllustratedClickHandle += () => {
                Debug.Log("Train Illustrated");
            };

            trainEvent.OnTrainQuitClickHandle += () => {
                Debug.Log("Train Quit");
            };

            trainEvent.OnTrainOverResetClickHandle += () => {
                trainSystem.Exit();
                loginSystem.Enter();
                UIDomain.Panel_Training_Select_Open(appUI, assetsModule, templateModule);
            };

            trainEvent.OnTrainOverBackClickHandle += () => {
                trainSystem.Exit();
                loginSystem.Enter();
                loginSystem.EnterPanel_Login();
            };
        }
        #endregion


        void Update() {
            if (!isInit) {
                return;
            }

            float dt = Time.deltaTime;

            var input = inputModule;
            if (input.isUpdateProcess) {
                input.Process(dt);
            }

            gameSystem.Tick(dt);
            loginSystem.Tick(dt);
            pauseSystem.Tick(dt);
            trainSystem.Tick(dt);

            if (appUI.ctx.isNoticeOpen) {
                appUI.ctx.noticeTimer -= dt;
                if (appUI.ctx.noticeTimer <= 0) {
                    appUI.Panel_Notice_Close();
                    appUI.ctx.noticeTimer = 1;
                    appUI.ctx.isNoticeOpen = false;
                }
            }
        }

        void OnDestroy() {
            TearDown();
        }

        void OnApplicationQuit() {
            TearDown();
        }

        void TearDown() {
            if (isTearDown) {
                return;
            }
            isTearDown = true;

            assetsModule.UnLoadAll();
            templateModule.UnLoadAll();
        }



    }
}
