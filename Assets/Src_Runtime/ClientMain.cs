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

        // TODO:不能写在这里 没想好好放哪里先放这里
        bool isNoticeOpen = false;
        float noticeTimer = 1;

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
            playerSaveData = new PlayerEntity();


            // Inject
            gameSystem.Inject(screenCanvas, assetsModule, templateModule, appUI, inputModule, roleRepository, playerSaveData);
            loginSystem.Inject(appUI, inputModule);
            pauseSystem.Inject(appUI, roleRepository);
            trainSystem.Inject(appUI, templateModule, assetsModule, roleRepository);

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
                loginSystem.Enter();
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
                // TODO：放到UIDomain里
                appUI.Panel_StageSelection_Close();
                Debug.Log("StageClick " + stageID);
                appUI.playerSaveData.SetStageCurID(stageID);

                int chapterID = 1;
                appUI.playerSaveData.SetChapterCurID(chapterID);
                UIDomain.Panel_CardSelection_Open(appUI, assetsModule, templateModule, stageID, chapterID);

            };

            loginEvent.OnChapterClickHandle += (chapter) => {
                var maxChapter = appUI.playerSaveData.GetUnLockChapterID();
                if (chapter > maxChapter) {
                    chapter = 1;
                }
                appUI.playerSaveData.SetChapterCurID(chapter);
                UIDomain.Panel_StageSelection_ChangeChapter(appUI, chapter, maxChapter);
            };

            // CardSelection
            loginEvent.OnSelectHeroClickHandle += () => {
                appUI.Panel_SetCardType(CardType.Hero);
                UIDomain.Panel_OneCardSelection_Update(appUI);
            };

            loginEvent.OnSelectAgrClickHandle += () => {
                Debug.Log("选择农业");
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
                Debug.Log("开始游戏");
                // 放到UIDomain里
                appUI.Panel_CardSelected_TakeAll(out List<int> typeIDs);
                int stageID = appUI.playerSaveData.GetStageCurID();
                int roleTypeID = appUI.selectedInFormation.roleSelectedTypeID;
                int sciType = appUI.selectedInFormation.scienceTypeID;
                if (typeIDs.Count < 4 || stageID == -1 || roleTypeID == -1 || sciType == -1) {
                    Debug.Log("请先选择卡片");
                    isNoticeOpen = appUI.Panel_Notice_Open();
                    return;
                }
                // UI
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
            };

            loginEvent.OnConfirmClickHandle += (typeID) => {
                Debug.Log("确认选择的卡片");
                UIDomain.Panel_SelectedCardAddToSlot(appUI, templateModule, typeID);
                appUI.Panel_OneCardSelection_Lock(typeID);
            };

            loginEvent.OnCardSelectedClickHandle += (typeID) => {
                appUI.Panel_CardSelection_RemoveCardSelected(typeID);
                appUI.Panel_OneCardSelection_UnLock(typeID);
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

            // Train
            {
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

            gameEvent.OnGameContinueOverHandle += (isWin) => {
                Debug.Log("游戏结束 继续游戏" + isWin);

                if (!isWin) {
                    // TODO: 传入参数
                    int stageID = appUI.playerSaveData.GetStageCurID();
                    int chapterID = appUI.playerSaveData.GetChapterCurID();
                    UIDomain.Panel_CardSelection_Open(appUI, assetsModule, templateModule, stageID, stageID);
                }

                if (isWin) {
                    UIDomain.Panel_StageSelectionEnter(appUI);
                    // TODO: 要写关卡解锁

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
                appUI.Panel_Training_Pause_Show();
                pauseSystem.Enter();
            };

            trainEvent.OnTrainContinueClickHandle += () => {
                appUI.Panel_Training_Pause_Close();
                pauseSystem.Exit();
            };

            trainEvent.OnTrainIllustratedClickHandle += () => {
                Debug.Log("Train Illustrated");
            };

            trainEvent.OnTrainQuitClickHandle += () => {
                Debug.Log("Train Quit");
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

            if (isNoticeOpen) {
                noticeTimer -= dt;
                if (noticeTimer <= 0) {
                    appUI.Panel_Notice_Close();
                    noticeTimer = 1;
                    isNoticeOpen = false;
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
