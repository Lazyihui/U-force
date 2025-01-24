using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UF {

    public class ClientMain : MonoBehaviour {

        GameContext ctx;
        [SerializeField] Canvas screenCanvas;

        bool isInit = false;

        bool isTearDown = false;
        void Awake() {

            // ---  GameContext  ---
            ctx = new GameContext();
            ctx.Inject(screenCanvas);

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
            UIEvent.OnLoginStartHandle += () => {
                ctx.appUI.Panel_Login_Close();
                Game_Business.Enter(ctx);
                ctx.appUI.Panel_Game_Defend_Open();
            };

            UIEvent.OnGamePauseHandle += () => {
                Debug.Log("Game Pause");
            };

            UIEvent.OnAttackHandle += () => {
                Debug.Log("Attack");
            };
        }

        void Update() {
            if (!isInit) {
                return;
            }

            float dt = Time.deltaTime;

            var input = ctx.inputModule;
            input.Process(dt);

            var game = ctx.gameEntity;
            if (game.state == GameState.Login) {
                Login_Business.Tick(ctx, dt);
            } else if (game.state == GameState.Game) {
                Game_Business.Tick(ctx, dt);
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
