using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UF.System_Train;

namespace UF {

    public class ClientMain : MonoBehaviour {

        [SerializeField] Canvas screenCanvas;
        [SerializeField] Camera mainCamera;
        bool isInit = false;
        bool isTearDown = false;

        public GameSaveModel playerSaveData;
        public SaveCore saveCore;

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

            playerSaveData = new GameSaveModel();
            saveCore = new SaveCore();

            // save存档
            bool has0 = saveCore.Slot_Has(0);
            if (!has0) {
                playerSaveData.slotID = 0;
                saveCore.Slot_Save(playerSaveData);
            }
            bool has3 = saveCore.Slot_Has(3);
            if (!has3) {
                playerSaveData.slotID = 3;
                saveCore.Slot_Save(playerSaveData);
            }
            // 合并存档
            // saveCore.Slot_Delete(0);
            // saveCore.Slot_Delete(3);
            has0 = saveCore.Slot_TryLoad(0, out var save0);
            has3 = saveCore.Slot_TryLoad(3, out var save3);
            save0 = saveCore.CombineRightToLeft(save0, save3);
            playerSaveData.slotID = 0;
            // 加载存档
            GameMainDomain.Game_Load(playerSaveData, save0);
            // 保存存档
            saveCore.Slot_Save(playerSaveData);

            // Inject
            gameSystem.Inject(screenCanvas, assetsModule, templateModule, appUI, inputModule, roleRepository, bulletRepository, playerSaveData, mainCamera, saveCore);
            loginSystem.Inject(appUI, inputModule, saveCore);
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
                // 进入游戏
                Debug.Log("登入游戏");
                // 存档
                GameMainDomain.Slot_LoadGame(playerSaveData, out saveCore, playerSaveData.slotID);
                appUI.Panel_Login_Close();
                UIDomain.Panel_StageSelectionEnter(appUI);
            };

            loginEvent.OnTrainingHandle += () => {
                appUI.Panel_Login_Close();
                UIDomain.Panel_Training_Select_Open(appUI, assetsModule, templateModule);
            };

            loginEvent.OnSettingHandle += () => {
                Debug.Log("设置");
                // Button TODO
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

                appUI.selectedInFormation.typeIDs = typeIDs;
                int stageID = appUI.playerSaveData.GetStageCurID();
                int roleTypeID = appUI.selectedInFormation.roleSelectedTypeID;
                int sciType = appUI.selectedInFormation.scienceTypeID;
                int slotMax = appUI.playerSaveData.GetSlotMax();

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
                UIDomain.Update_CardSelectionDetails(typeID, templateModule, appUI);
                appUI.Panel_CardDetailsBtn_UnLock();
            };

            loginEvent.OnConfirmClickHandle += (typeID) => {

                appUI.Panel_CardSelected_TakeAll(out List<int> typeIDs);
                int roleTypeID = appUI.selectedInFormation.roleSelectedTypeID;
                int sciType = appUI.selectedInFormation.scienceTypeID;

                UIDomain.Panel_SelectedCardAddToSlot(appUI, templateModule, typeID);
                appUI.Panel_OneCardSelection_Lock(typeID);
                appUI.Panel_CardDetailsBtn_Lock();
            };

            loginEvent.OnCardSelectedClickHandle += (typeID) => {
                appUI.Panel_CardSelection_RemoveCardSelected(typeID);
                appUI.Panel_OneCardSelection_UnLock(typeID);
            };

            loginEvent.OnViewClickHandle += () => {
                UIDomain.Panel_WatchTower_View_Open(appUI, templateModule);
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

                    UIDomain.Panel_Training_Select_AddPlayer(appUI, templateModule);

                    hasPlayer = false;
                }
            };

            loginEvent.OnPlayer2ReadyClickHandle += () => {
                if (hasPlayer) {
                    appUI.Panel_Training_Select_AddPlayer();
                    appUI.Panel_Training_Select_TakeAll(out List<int> typeIDs);
                    appUI.selectedInTrain.typeIDs = typeIDs;

                    appUI.Panel_Training_Select_Close();

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

            loginEvent.OnTrainBackClickHandle += () => {
                appUI.Panel_Training_Select_Close();
                appUI.Panel_Login_Open();
            };

            loginEvent.OnSelectionTrainCardClickHandle += (typeID) => {
                Debug.Log("SelectionTrainCardClick " + typeID);
                // 这样写感觉model没什么意义
                templateModule.Role_TryGet(typeID, out RoleTM roleTM);
                var skill = roleTM.skillorSoArray[0];
                templateModule.Bullet_TryGet(skill.tm.typeID, out BulletTM bulletTM);

                appUI.Panel_Training_Select_SetPlayer(typeID, roleTM.sprite, roleTM.feature, bulletTM.moveSpeed.ToString(), bulletTM.attackDamage.ToString(), skill.tm.cd.ToString());

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

            gameEvent.OnGameIllusCloseClickHandle += () => {
                appUI.Panel_Illustrated_Close();
            };

            gameEvent.OnGameIllusCardClickHandle += (int typeID) => {
                templateModule.Role_TryGet(typeID, out RoleTM tm);

                appUI.Panel_WatchDetail_Open(tm.sprite, tm.typeName, tm.feature, tm.hp, tm.skill, tm.restraint);
            };

            gameEvent.OnGameIllusStuffCardClickHandle += (int typeID) => {
                templateModule.Stuff_TryGet(typeID, out StuffTM tm);

                appUI.Panel_WatchDetail_Open(tm.sprite, tm.typeName, tm.description);
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
                UIDomain.Panel_Illustrated_Open(appUI, templateModule);
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

            pauseEvent.OnResetWarningHandle += () => { // 重新开始

                // 重新开始游戏

                var typeIDs = appUI.selectedInFormation.typeIDs;
                int stageID = appUI.playerSaveData.GetStageCurID();
                int roleTypeID = appUI.selectedInFormation.roleSelectedTypeID;
                int sciType = appUI.selectedInFormation.scienceTypeID;
                int slotMax = appUI.playerSaveData.GetSlotMax();

                appUI.Panel_BackSelection_Close();

                gameSystem.Exit();
                gameSystem.ClearGame();
                pauseSystem.Exit();

                Debug.Log("重新开始游戏" + "role" + roleTypeID + " sci" + sciType);
                gameSystem.Enter(stageID, roleTypeID, typeIDs.ToArray(), sciType);
            };

            pauseEvent.OnQuitWarningHandle += () => {
                gameSystem.ClearGame();
                gameSystem.Exit();
                loginSystem.Enter();
                appUI.Panel_BackSelection_Close();
                UIDomain.Panel_CardSelection_Open(appUI, assetsModule, templateModule);
            };

            pauseEvent.OnTrainBackCloseClickHandle += () => {
                appUI.Panel_Training_Back_Close();

                pauseSystem.Exit();
                trainSystem.ExitPause();
            };

            pauseEvent.OnTrainBackContinueClickHandle += () => {
                appUI.Panel_Training_Back_Close();

                pauseSystem.Exit();
                trainSystem.ExitPause();
            };

            pauseEvent.OnTrainBackQuitClickHandle += () => {
                trainSystem.ClearGame();
                trainSystem.Exit();
                loginSystem.Enter();

                appUI.Panel_Training_Back_Close();
                UIDomain.Panel_Training_Select_Open(appUI, assetsModule, templateModule);
            };

            pauseEvent.OnEliminatePauseClickHandle += () => {
                appUI.Panel_Eliminate_Pause_Open();
                gameSystem.Exit();
                pauseSystem.Enter();
            };

            pauseEvent.OnEliminateContinueClickHandle += () => {
                appUI.Panel_Eliminate_Pause_Close();

                pauseSystem.Exit();
                gameSystem.Continue();
            };

            pauseEvent.OnEliminateIllustratedClickHandle += () => {
                UIDomain.Panel_Eliminate_Illustrated_Open(appUI, templateModule);
            };

            pauseEvent.OnEliminateQuitClickHandle += () => {
                appUI.Panel_BackSelection_Open();
                appUI.Panel_Eliminate_Pause_Close();
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
                UIDomain.Panel_TrainIllustrated_Open(appUI, templateModule);
            };

            trainEvent.OnTrainQuitClickHandle += () => {
                appUI.Panel_Training_Back_Open();
                appUI.Panel_Training_Pause_Close();
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

            trainEvent.OnTrainIllusCloseClickHandle += () => {
                appUI.Panel_TrainIllustrated_Close();
            };

            trainEvent.OnTrainIllusCardClickHandle += (int typeID) => {
                templateModule.Prop_TryGet(typeID, out var tm);

                appUI.Panel_WatchDetail_Open(tm.sprite, tm.typeName, tm.feature);
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
