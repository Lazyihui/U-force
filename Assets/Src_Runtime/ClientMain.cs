using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UF {

    public class ClientMain : MonoBehaviour {

        [SerializeField] Canvas screenCanvas;
        bool isInit = false;
        bool isTearDown = false;

        public CardModel cardModel;

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


            // Inject
            gameSystem.Inject(screenCanvas, assetsModule, templateModule, appUI, inputModule, roleRepository);
            loginSystem.Inject(appUI, inputModule);
            pauseSystem.Inject(appUI, roleRepository);
            trainSystem.Inject(appUI);

            appUI.Inject(screenCanvas, assetsModule);

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
                loginSystem.Enter();
            };

            action.Invoke();

        }

        void Login_Binding() {
            loginSystem.Login_Binding();

            var loginEvent = loginSystem.Events;

            loginEvent.OnLoginStartHandle += () => {
                appUI.Panel_Login_Close();

                appUI.Panel_StageSelection_Select(InputDeviceType.Keyboard, 1, 5);

                appUI.Panel_StageSelection_Open(1, 1);

                appUI.Panel_StageSelectionOneStage_Add(1, true);

                // loginSystem.Exit();
                // gameSystem.Enter(1);

            };

            loginEvent.OnTrainingHandle += () => {
                appUI.Panel_Login_Close();

                appUI.Panel_Training_Select_Open();
                appUI.Panel_Training_Select_Add(1, null, "1");

                loginSystem.Exit();
                trainSystem.Enter();
            };

            loginEvent.OnSettingHandle += () => {
                Debug.Log("Setting");
            };

            loginEvent.OnQuitHandle += () => {
                Application.Quit();
                Debug.Log("Game Quit");
            };

            // StageSelection
            loginEvent.OnStageClickHandle += (stageID) => {
                // TODO：放到UIDomain里
                appUI.Panel_StageSelection_Close();

                int chapterID = 1;
                UIDomain.Panel_CardSelection_Open(appUI, assetsModule, templateModule, stageID, chapterID);

                Debug.Log("StageClick " + stageID);
            };

            loginEvent.OnChapterClickHandle += (chapter) => {
                var maxChapter = 5;
                if (chapter > maxChapter) {
                    chapter = 1;
                }
                appUI.Panel_StageSelection_Select(InputDeviceType.Keyboard, chapter, maxChapter);
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

            loginEvent.OnSelectEnterClickHandle += () => {
                Debug.Log("SelectEnter");
                appUI.Panel_CardSelection_Close();
                appUI.Panel_WatchTower_Close();
                loginSystem.Exit();
                gameSystem.Enter(1);
            };

            loginEvent.OnSelectBackClickHandle += () => {
                Debug.Log("SelectBack");
                appUI.Panel_CardSelection_Close();
                appUI.Panel_WatchTower_Close();
                appUI.Panel_StageSelection_Open(1, 1);

                appUI.Panel_StageSelectionOneStage_Add(1, true);
                appUI.Panel_StageSelection_Select(InputDeviceType.Keyboard, 1, 5);
            };

            loginEvent.OnSelectionCardClickHandle += (typeID) => {
                Debug.Log("SelectionCardClick " + typeID);

                UIDomain.Update_CardSelectionDetails(typeID, templateModule, appUI);

            };

            loginEvent.OnConfirmClickHandle += (typeID) => {
                Debug.Log("Confirm");
                UIDomain.Panel_SelectedCardAddToSlot(appUI, templateModule, typeID);

            };

            {
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
            }

            loginEvent.OnDetailCloseClickHandle += () => {
                appUI.Panel_WatchDetail_Close();
            };
        }

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
                Debug.Log("Tech");
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

            gameEvent.OnGameOverHandle += () => {
                Debug.Log("GameOver Continue");
            };
        }

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

        void Train_Binding() {
            trainSystem.Train_Binding();

            var trainEvent = trainSystem.Events;

            trainEvent.OnPlayer1ReadyClickHandle += () => {
                Debug.Log("Player1Ready");
            };

            trainEvent.OnPlayer2ReadyClickHandle += () => {
                Debug.Log("Player2Ready");
            };

            trainEvent.OnSelectionTrainCardClickHandle += (typeID) => {
                Debug.Log("SelectionTrainCardClick " + typeID);
            };
        }


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
