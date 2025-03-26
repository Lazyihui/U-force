using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UF.System_Train;
using System.Linq;


namespace UF {

    public class ClientMain : MonoBehaviour {

        [SerializeField] Canvas screenCanvas;
        [SerializeField] Transform canvasRoot;
        [SerializeField] Camera mainCamera;
        bool isInit = false;
        bool isTearDown = false;
        bool isFullScreen = true;

        public GameSaveModel playerSaveData;
        public GameSettingModel settingSaveData;
        public SaveCore saveCore;

        // system
        public static GameSystem gameSystem;
        public static LoginSystem loginSystem;
        public static PauseSystem pauseSystem;
        public static TrainSystem trainSystem;
        public static SettingSystem settingSystem;
        public static InputBindingSystem inputBindingSystem;

        // ==== Cores ====
        public static AppUI appUI;

        // ==== Modules ====
        public static AssetsModule assetsModule;
        public static TemplateMoudle templateModule;
        public static InputModule inputModule;
        public static AudioModule audioModule;

        // repos
        public RoleRepository roleRepository;
        public BulletRepository bulletRepository;
        public PropRepository propRepository;

        // Entity
        public SaveSlotFullDTO saveSlotFullDTO;
        public GameEntity gameEntity;
        static ProgramEntity programEntity;

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

            settingSystem = GetComponentInChildren<SettingSystem>();
            settingSystem.Ctor();

            inputBindingSystem = GetComponentInChildren<InputBindingSystem>();
            inputBindingSystem.Ctor();

            // ---  Modules  ---
            assetsModule = GetComponentInChildren<AssetsModule>();
            assetsModule.Ctor();

            templateModule = GetComponentInChildren<TemplateMoudle>();
            templateModule.Ctor();

            inputModule = GetComponentInChildren<InputModule>();
            inputModule.Ctor();

            audioModule = GetComponentInChildren<AudioModule>();
            audioModule.Ctor();

            // repos
            roleRepository = new RoleRepository();
            bulletRepository = new BulletRepository();
            propRepository = new PropRepository();

            playerSaveData = new GameSaveModel();
            settingSaveData = new GameSettingModel();
            saveSlotFullDTO = new SaveSlotFullDTO();
            saveCore = new SaveCore();
            gameEntity = new GameEntity();
            programEntity = new ProgramEntity();

            #region 存档
            // 1.加载存档
            // saveCore.DeleteSlot(0);
            saveCore.Slot_Full_TryLoad(0, out saveSlotFullDTO);
            gameEntity.Init(0);
            // 2.将存档数据加载到游戏实体
            #endregion
            // Inject
            gameSystem.Inject(screenCanvas, assetsModule, templateModule, appUI, inputModule, audioModule, roleRepository, bulletRepository, playerSaveData, mainCamera, saveCore, saveSlotFullDTO, gameEntity);
            loginSystem.Inject(appUI, inputModule, audioModule, saveCore);
            pauseSystem.Inject(appUI, roleRepository, bulletRepository, propRepository, audioModule);
            trainSystem.Inject(appUI, templateModule, assetsModule, inputModule, audioModule, roleRepository, bulletRepository, propRepository);
            settingSystem.Inject(appUI);
            inputBindingSystem.Inject(appUI, inputModule);


            appUI.Inject(screenCanvas, canvasRoot, assetsModule, templateModule, inputModule, audioModule, playerSaveData, settingSaveData);
            // ---  Binding  ---
            // Binding_Login();
            Login_Binding();
            Game_Binding();
            Pause_Binding();
            Train_Binding();
            Setting_Binding();

            Action action = async () => {

                await assetsModule.LoadAll();
                await templateModule.LoadAll();

                isInit = true;

                // ---  Enter  ---
                loginSystem.EnterPanel_Login();

                programEntity.isAssetBundleLoaded = true;
            };

            action.Invoke();

        }

        #region Binding_Login
        void Login_Binding() {
            loginSystem.Login_Binding();

            var loginEvent = loginSystem.Events;

            loginEvent.OnLoginStartHandle += () => {
                // 进入游戏
                GameDomain.Slot_LoadGame(0, gameEntity, saveCore);
                UIDomain.Panel_StageSelectionEnter(appUI);
            };

            loginEvent.OnTrainingHandle += () => {
                UIDomain.Panel_Training_Select_Open(appUI, assetsModule, templateModule);
            };

            loginEvent.OnSettingHandle += () => {
                appUI.Panel_Login_Close();
                settingSystem.Enter();
            };

            loginEvent.OnQuitHandle += () => {
                Application.Quit();
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
                appUI.playerSaveData.chapterCurID = chapter;
                Debug.Log("chapter:" + chapter);
                UIDomain.Panel_StageSelection_ChangeChapter(appUI, chapter, maxChapter);
            };

            loginEvent.OnSelectCloseHandle += () => {
                appUI.Panel_StageSelection_Close();
                appUI.Panel_Login_Open();
            };

            loginEvent.OnSelectBackHandle += () => {
                appUI.Panel_StageSelection_Close();
                appUI.Panel_Login_Open();
            };

            // CardSelection
            loginEvent.OnSelectHeroClickHandle += () => {
                UIDomain.Panel_OneCardSelection_Update(appUI, CardType.Hero);
            };

            loginEvent.OnSelectAgrClickHandle += () => {
                UIDomain.Panel_OneCardSelection_Update(appUI, CardType.Agriculture);
            };

            loginEvent.OnSelectIndClickHandle += () => {
                UIDomain.Panel_OneCardSelection_Update(appUI, CardType.Industry);
            };

            loginEvent.OnSelectSciClickHandle += () => {
                UIDomain.Panel_OneCardSelection_Update(appUI, CardType.Science);
            };

            loginEvent.OnSelectEnterGameClickHandle += () => {
                // 放到UIDomain里
                appUI.Panel_CardSelected_TakeAll(out List<int> typeIDs);
                appUI.selectedInFormation.typeIDs = typeIDs;

                loginSystem.Exit();
                gameSystem.Enter();
            };

            loginEvent.OnSelectBackClickHandle += () => {
                appUI.Panel_CardSelection_Close();
                UIDomain.Panel_StageSelectionEnter(appUI);
            };

            loginEvent.OnSelectionCardClickHandle += (typeID) => {
                UIDomain.Update_CardSelectionDetails(typeID, templateModule, appUI);
                // appUI.Panel_CardDetailsBtn_UnLock();
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

            loginEvent.OnSelectCloseClickHandle += () => {
                appUI.Panel_CardSelection_Close();
                UIDomain.Panel_StageSelectionEnter(appUI);
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

                appUI.Panel_Training_Select_SetPlayer(typeID);

                hasPlayer = true;
            };

            loginEvent.OnTrainTipCloseClickHandle += () => {
                appUI.Panel_Training_Tip_Close();
            };

            // 设置
            loginEvent.OnSettingBackClickHandle += () => {
                appUI.Panel_Setting_Close();
                appUI.Panel_Login_Open();
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

            gameEvent.OnGameContinueOverHandle += (bool isWin) => {

                if (!isWin) {
                    Debug.Log("重新开始此关");

                    gameSystem.ClearGame();
                    gameSystem.Exit();
                    loginSystem.Enter();
                    UIDomain.Panel_CardSelection_Open(appUI, assetsModule, templateModule);
                }

                if (isWin) {
                    Debug.Log("继续游戏");

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
                appUI.Panel_WatchDetail_Open(typeID);
            };

            gameEvent.OnGameIllusStuffCardClickHandle += (int typeID) => {
                appUI.Panel_WatchDetail_Open_Stuff(typeID);
            };

            gameEvent.OnEliminateOverResetClickHandle += (bool isWin) => {
                if (!isWin) {
                    Debug.Log("重新开始此关");
                    gameSystem.ClearGame();
                    gameSystem.Exit();
                    loginSystem.Enter();
                    UIDomain.Panel_CardSelection_Open(appUI, assetsModule, templateModule);
                }

                if (isWin) {
                    Debug.Log("1");
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
                Debug.Log("继续游戏");
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
                Debug.Log("继续游戏");
                appUI.Panel_BackSelection_Close();

                pauseSystem.Exit();
                gameSystem.Continue();
            };

            pauseEvent.OnResetWarningHandle += () => { // 重新开始

                // 改成回到选卡片界面
                gameSystem.ClearGame();
                gameSystem.Exit();
                loginSystem.Enter();
                appUI.Panel_BackSelection_Close();
                UIDomain.Panel_CardSelection_Open(appUI, assetsModule, templateModule);
            };

            pauseEvent.OnQuitWarningHandle += () => {
                // 返回选关界面
                gameSystem.ClearGame();
                gameSystem.Exit();
                loginSystem.Enter();
                appUI.Panel_BackSelection_Close();

                UIDomain.Panel_StageSelectionEnter(appUI);
            };

            pauseEvent.OnTrainBackCloseClickHandle += () => {
                appUI.Panel_Training_Back_Close();

                pauseSystem.Exit();
                trainSystem.Continue();
            };

            pauseEvent.OnTrainBackContinueClickHandle += () => {
                Debug.Log("继续游戏");
                appUI.Panel_Training_Back_Close();

                pauseSystem.Exit();
                trainSystem.Continue();
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
                Debug.Log("继续游戏");

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
                trainSystem.Exit();
                pauseSystem.Enter();

                appUI.Panel_Training_Pause_Show();
            };

            trainEvent.OnTrainContinueClickHandle += () => {
                trainSystem.Continue();
                pauseSystem.Exit();

                appUI.Panel_Training_Pause_Close();
            };

            trainEvent.OnTrainIllustratedClickHandle += () => {
                UIDomain.Panel_TrainIllustrated_Open(appUI, templateModule);
            };

            trainEvent.OnTrainQuitClickHandle += () => {
                appUI.Panel_Training_Back_Open();
                appUI.Panel_Training_Pause_Close();
            };

            trainEvent.OnTrainOverResetClickHandle += () => {
                trainSystem.ClearGame();
                trainSystem.Exit();
                loginSystem.Enter();
                UIDomain.Panel_Training_Select_Open(appUI, assetsModule, templateModule);
            };

            trainEvent.OnTrainOverBackClickHandle += () => {
                trainSystem.ClearGame();
                trainSystem.Exit();
                loginSystem.Enter();
                loginSystem.EnterPanel_Login();
            };

            trainEvent.OnTrainIllusCloseClickHandle += () => {
                appUI.Panel_TrainIllustrated_Close();
            };

            trainEvent.OnTrainIllusCardClickHandle += (int typeID) => {
                appUI.Panel_WatchDetail_Open_Prop(typeID);
            };
        }
        #endregion

        #region Binding_Setting

        void Setting_Binding() {
            settingSystem.Setting_Binding();

            var settingEvent = settingSystem.Events;

            settingEvent.OnSettingBackClickHandle += () => {
                appUI.Panel_Setting_Close();
                loginSystem.EnterPanel_Login();
            };

            settingEvent.OnSettingReselustion1920ClickHandle += () => {
                Screen.SetResolution(1920, 1080, isFullScreen);
                Debug.Log("1920*1080");
            };

            settingEvent.OnSettingReselustion1600ClickHandle += () => {
                Screen.SetResolution(1600, 900, isFullScreen);
                Debug.Log("1600*900");
            };

            settingEvent.OnSettingReselustion640ClickHandle += () => {
                Screen.SetResolution(640, 360, isFullScreen);
                Debug.Log("640*360");
            };

            settingEvent.OnSettingFullScreenToggleHandle += (isOn) => {
                isFullScreen = isOn;
                Screen.fullScreen = isOn;
                Debug.Log("全屏" + isOn);
            };

            settingEvent.OnSettingMusicSliderChangeHandle += (value) => {
                audioModule.SetMusicVolume(value);
                settingSaveData.SetMusicVolume(value);
            };

            settingEvent.OnSettingSFXSliderChangeHandle += (value) => {
                audioModule.SetSFXVolume(value);
                settingSaveData.SetSFXVolume(value);
            };
        }
        #endregion

        void Update() {
            if (!isInit) {
                return;
            }

            float dt = Time.deltaTime;

            var input = inputModule;
            input.Process(dt);

            gameSystem.Tick(dt);
            loginSystem.Tick(dt);
            pauseSystem.Tick(dt);
            trainSystem.Tick(dt);
            settingSystem.Tick(dt);
            inputBindingSystem.Tick(dt);

            if (appUI.ctx.isNoticeOpen) {
                appUI.ctx.noticeTimer -= dt;
                if (appUI.ctx.noticeTimer <= 0) {
                    appUI.Panel_Notice_Close();
                    appUI.ctx.noticeTimer = 1;
                    appUI.ctx.isNoticeOpen = false;
                }
            }

            if (Input.GetKey(KeyCode.R)) {
                // 清除存档
                saveCore.DeleteSlot(0);
                Debug.Log("删除存档");
            }
            if (Input.GetKey(KeyCode.T)) {
                // 遍历stageID 
                playerSaveData.ForEachStage((s) => {
                    Debug.Log("关卡ID:" + s.stageID + "星级:" + s.starCount + "状态:" + s.passStatus);
                });
            }
            Screen_Coordinate();
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

        #region Screen Coordinate

        void Screen_Coordinate() {

            // 计算当前屏幕的宽高比 比例 比如16:9
            float ratioDisplay = (float)Screen.width / Screen.height;

            if (ratioDisplay == programEntity.ratioDisplay) {
                return;
            }

            float targetRatio = 16f / 9f;// 目标比例
            float orthographicSize = 11.25f;// 目标正交相机大小
            float matchHeightPercent = 1;// 目标高度占比
            var mode = AspectRatioFitter.AspectMode.EnvelopeParent;
            if ((targetRatio - ratioDisplay).IsAlmostZero(0.0001f)) {
                // 16:9
                ratioDisplay = targetRatio;
            } else if (ratioDisplay < targetRatio) {
                // 宽/高 = 20/11.25
                // 宽 = ratio * 高
                // 高 = 宽 / ratio
                // 以宽为准, 上下留黑边
                orthographicSize = 20f / ratioDisplay;
                matchHeightPercent = 0;
            } else if (ratioDisplay > targetRatio) {
                // 更宽(带鱼屏)
                // 以高为准, 左右留黑边
                mode = AspectRatioFitter.AspectMode.FitInParent;
            }

            Canvas_SetMatchWidthOrHeightAll(matchHeightPercent);
            Canvas_SetFitRatioAll(mode, targetRatio);

            // Camera
            Camera_Orthographic_SetBaseSize(orthographicSize, ratioDisplay);

            programEntity.ratioDisplay = ratioDisplay;

        }
        void Canvas_SetMatchWidthOrHeightAll(float matchHeightPercent) {
            // 获取所有 UI Canvas
            Canvas[] allCanvases = GameObject.FindObjectsOfType<Canvas>();

            // 遍历所有 Canvas
            foreach (Canvas canvas in allCanvases) {
                // 获取 Canvas 的 CanvasScaler 组件
                CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
                if (scaler != null) {
                    // 设置 CanvasScaler 的匹配模式
                    scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                    scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                    scaler.matchWidthOrHeight = matchHeightPercent;
                }
            }
        }
        void Canvas_SetFitRatioAll(AspectRatioFitter.AspectMode mode, float targetRatio) {
            // 获取所有 UI Canvas
            Canvas[] allCanvases = GameObject.FindObjectsOfType<Canvas>();

            // 遍历所有 Canvas
            foreach (Canvas canvas in allCanvases) {
                // 获取或添加 AspectRatioFitter 组件
                AspectRatioFitter fitter = canvas.GetComponent<AspectRatioFitter>();
                if (fitter == null) {
                    fitter = canvas.gameObject.AddComponent<AspectRatioFitter>();
                }

                // 设置 AspectRatioFitter 的适配模式和目标宽高比
                fitter.aspectMode = mode;
                fitter.aspectRatio = targetRatio;
            }
        }
        void Camera_Orthographic_SetBaseSize(float orthographicSize, float ratioDisplay) {
            // 获取所有正交相机
            Camera[] orthographicCameras = GameObject.FindObjectsOfType<Camera>()
                .Where(camera => camera.orthographic) // 过滤出正交相机
                .ToArray();

            // 遍历所有正交相机
            foreach (Camera camera in orthographicCameras) {
                // 设置正交相机的 orthographicSize
                camera.orthographicSize = orthographicSize;

                // 根据宽高比调整相机的视野范围
                float aspectRatio = ratioDisplay;
                camera.aspect = aspectRatio;

                // 如果需要，可以进一步调整相机的其他参数
                // 例如：设置相机的近裁剪面和远裁剪面
                camera.nearClipPlane = 0.1f; // 近裁剪面
                camera.farClipPlane = 100f;  // 远裁剪面
            }
        }
        #endregion
    }
}
