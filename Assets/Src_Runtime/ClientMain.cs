using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UF {

    public class ClientMain : MonoBehaviour {

        GameContext ctx;
        [SerializeField] Canvas screenCanvas;

        public int stageCurID;

        bool isInit = false;

        bool isTearDown = false;
        void Awake() {

            // ---  GameContext  ---
            ctx = new GameContext();
            ctx.Inject(screenCanvas);

            var game = ctx.gameEntity;
            game.stageCurID = stageCurID;
            // ---  Binding  ---
            Binding();

            Action action = async () => {

                await ctx.assetsCore.LoadAll();
                await ctx.templateCore.LoadAll();

                isInit = true;

                // ---  Enter  ---
                Login_Business.Enter(ctx);
            };

            action.Invoke();

        }

        void Binding() {
            var UIEvent = ctx.appUI.eventCenter;
            var input = ctx.inputModule;
            var game = ctx.gameEntity;


            UIEvent.OnLoginStartHandle += () => {
                ctx.appUI.Panel_Login_Close();
                Game_Business.Enter(ctx);

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

            //GamePause
            UIEvent.OnContinueHandle += () => {
                ctx.appUI.Panel_GamePause_Close();

                //不知道可不可以这么写
                game.state = GameState.Game;
                Debug.Log("Continue");
            };

            UIEvent.OnIllustratedHandle += () => {
                Debug.Log("Illustrated");
            };

            //GameDefend
            UIEvent.OnGamePauseHandle += () => {
                ctx.appUI.Panel_GamePause_Open();
                Pause_Business.Enter(ctx);
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
                input.isPressStuff1 = true;
                Debug.Log("Weap1");
            };

            UIEvent.OnWeap2Handle += () => {
                Debug.Log("Weap2");
            };

            UIEvent.OnWeap3Handle += () => {
                Debug.Log("Weap3");
            };

            UIEvent.OnWeap4Handle += () => {
                Debug.Log("Weap4");
            };
        }

        void Update() {
            if (!isInit) {
                return;
            }

            float dt = Time.deltaTime;

            var input = ctx.inputModule;
            if (input.isUpdateProcess) {
                input.Process(dt);
            }

            var game = ctx.gameEntity;
            if (game.state == GameState.Login) {
                Login_Business.Tick(ctx, dt);
            } else if (game.state == GameState.Game) {
                Game_Business.Tick(ctx, dt);
            } else if (game.state == GameState.GamePause) {
                Pause_Business.Tick(ctx, dt);
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

            ctx.assetsCore.UnLoadAll();
            ctx.templateCore.UnLoadAll();
        }



    }
}
