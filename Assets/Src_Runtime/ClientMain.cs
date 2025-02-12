using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UF {

    public class ClientMain : MonoBehaviour {

        [SerializeField] Canvas screenCanvas;
        bool isInit = false;
        bool isTearDown = false;


        // system
        public static GameSystem gameSystem;
        public static LoginSystem loginSystem;
        public static PauseSystem pauseSystem;

        // ==== Cores ====
        public static AppUI appUI;

        // ==== Modules ====
        public static AssetsModule assetsModule;
        public static TemplateMoudle templateCore;
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

            // ---  Modules  ---
            assetsModule = GetComponentInChildren<AssetsModule>();
            assetsModule.Ctor();

            templateCore = GetComponentInChildren<TemplateMoudle>();
            templateCore.Ctor();

            inputModule = GetComponentInChildren<InputModule>();
            inputModule.Ctor();

            // repos
            roleRepository = new RoleRepository();


            // Inject
            gameSystem.Inject(screenCanvas, assetsModule, templateCore, appUI, inputModule, roleRepository);
            loginSystem.Inject(appUI, inputModule);
            pauseSystem.Inject(appUI, roleRepository);

            appUI.Inject(screenCanvas, assetsModule);

            // ---  Binding  ---
            // Binding_Login();
            Login_Binding();
            Game_Binding();
            Pause_Binding();

            Action action = async () => {

                await assetsModule.LoadAll();
                await templateCore.LoadAll();

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
                appUI.Panel_StageSelectionOneStage_Add(2, true);
                appUI.Panel_StageSelectionOneStage_Add(3, true);
                appUI.Panel_StageSelectionOneStage_Add(4, false);
                appUI.Panel_StageSelectionOneStage_Add(5, false);
                appUI.Panel_StageSelectionOneStage_Add(6, false);
                appUI.Panel_StageSelectionOneStage_Add(7, false);
                appUI.Panel_StageSelectionOneStage_Add(8, false);
                appUI.Panel_StageSelectionOneStage_Add(9, false);
                appUI.Panel_StageSelectionOneStage_Add(10, false);

                // loginSystem.Exit();
                // gameSystem.Enter(1);

            };

            loginEvent.OnTrainingHandle += () => {
                Debug.Log("Training");
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
                appUI.Panel_StageSelection_Close();

                appUI.Panel_CardSelection_Open(stageID, 1);

                appUI.Panel_CardSelection_Add(1, null);
                appUI.Panel_CardSelection_Add(2, null);
                appUI.Panel_CardSelection_Add(3, null);
                appUI.Panel_CardSelection_Add(4, null);

                appUI.Panel_WatchTower_Open();
                appUI.Panel_WatchTower_Add(1, null);
                appUI.Panel_WatchTower_Add(2, null);
                appUI.Panel_WatchTower_Add(3, null);
                // loginSystem.Exit();
                // gameSystem.Enter(stageID);
                Debug.Log("StageClick " + stageID);
                // 进入游戏
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
                Debug.Log("SelectHero");
            };

            loginEvent.OnSelectAgrClickHandle += () => {
                Debug.Log("SelectAgr");
            };

            loginEvent.OnSelectIndClickHandle += () => {
                Debug.Log("SelectInd");
            };

            loginEvent.OnSelectSciClickHandle += () => {
                Debug.Log("SelectSci");
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

            loginEvent.OnSelectionCardClickHandle += (id) => {
                Debug.Log("SelectionCardClick " + id);
            };

            {
                bool isClicked = false;

                loginEvent.OnViewClickHandle += () => {
                    Debug.Log("ViewClick");
                    appUI.Panel_WatchTower_SetCursor(); // 如果不点击瞭望塔头像, 会一直变化光标, 之后看看怎么改
                    isClicked = true;
                };

                loginEvent.OnSelectionWatchCardClickHandle += (typeID) => { // typeID 应该是传进来的
                    if (isClicked)
                    {
                        Debug.Log("SelectionWatchCardClick " + typeID);
                        appUI.Panel_WatchTower_ClearCursor();

                        // TODO: 弹出详情页面
                    }
                };
            }
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
            templateCore.UnLoadAll();
        }



    }
}
