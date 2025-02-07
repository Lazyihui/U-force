using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UF {

    public class ClientMain : MonoBehaviour {

        [SerializeField] Canvas screenCanvas;
        bool isInit = false;
        bool isTearDown = false;

        public GameEntity gameEntity;

        // system
        public static GameSystem gameSystem;
        public static LoginSystem loginSystem;
        public static PauseSystem PauseSystem;

        // ==== Cores ====
        public static AppUI appUI;

        // ==== Modules ====
        public static AssetsModule assetsModule;
        public static TemplateMoudle templateCore;
        public static InputModule inputModule;

        // repos
        public RoleRepository roleRepository;


        void Start() {

            gameEntity = new GameEntity();
            // ---  UI
            appUI = GetComponentInChildren<AppUI>();
            appUI.Ctor();

            // ---  GameSystem  ---
            gameSystem = GetComponentInChildren<GameSystem>();
            gameSystem.Ctor();

            loginSystem = GetComponentInChildren<LoginSystem>();
            loginSystem.Ctor();

            PauseSystem = GetComponentInChildren<PauseSystem>();
            PauseSystem.Ctor();

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
            gameSystem.Inject(gameEntity, screenCanvas, assetsModule, templateCore, appUI, inputModule, roleRepository);
            loginSystem.Inject(gameEntity, appUI);
            PauseSystem.Inject(gameEntity, appUI, roleRepository);

            appUI.Inject(screenCanvas, assetsModule);

            // ---  Binding  ---
            Binding_Login();

            Action action = async () => {

                await assetsModule.LoadAll();
                await templateCore.LoadAll();

                isInit = true;

                // ---  Enter  ---
                loginSystem.Enter();
            };

            action.Invoke();

        }

        void Binding_Login() {

            var UIEvent = appUI.eventCenter;

            var input = inputModule;
            var game = gameEntity;

            UIEvent.OnLoginStartHandle += () => {
                appUI.Panel_Login_Close();
                gameSystem.Enter();
            };

            UIEvent.OnTrainingHandle += () => {
                Debug.Log("Training");
            };

            UIEvent.OnSettingHandle += () => {
                Debug.Log("Setting");
            };

            UIEvent.OnQuitHandle += () => {
                Application.Quit();
                Debug.Log("Game Quit");
            };

            // GamePause
            UIEvent.OnContinueHandle += () => {
                appUI.Panel_GamePause_Close();

                //不知道可不可以这么写
                game.state = GameState.Game;
                Debug.Log("Continue");
            };

            UIEvent.OnIllustratedHandle += () => {
                Debug.Log("Illustrated");
            };

            //GameDefend
            UIEvent.OnGamePauseHandle += () => {
                appUI.Panel_GamePause_Open();
                PauseSystem.Enter();
                //TODO
                Debug.Log("Game Pause");
            };

            UIEvent.OnAttackHandle += () => {
                input.isUpdateProcess = false;
                input.Set_InputCastKey(InputKeyFlag.Attack);

                Debug.Log("Attack");
            };

            UIEvent.OnTechHandle += () => {
                Debug.Log("Tech");
            };

            UIEvent.OnWeap1Handle += () => {
                input.isUpdateProcess = false;
                input.Set_InputCastKey(InputKeyFlag.Weap1);
                Debug.Log("Weap1");
            };

            UIEvent.OnWeap2Handle += () => {
                input.isUpdateProcess = false;
                input.Set_InputCastKey(InputKeyFlag.Weap2);
                Debug.Log("Weap2");
            };

            UIEvent.OnWeap3Handle += () => {
                input.isUpdateProcess = false;
                input.Set_InputCastKey(InputKeyFlag.Weap3);
                Debug.Log("Weap3");
            };

            UIEvent.OnWeap4Handle += () => {
                input.isUpdateProcess = false;
                input.Set_InputCastKey(InputKeyFlag.Weap4);
                Debug.Log("Weap4");
            };

            UIEvent.OnGameOverHandle += () => {
                Debug.Log("GameOver Continue");
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

            // if (game.state == GameState.Login) {
            // } else if (game.state == GameState.Game) {
            // } else if (game.state == GameState.GamePause) {
            //     Pause_Business.Tick(ctx, dt);
            // }


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
