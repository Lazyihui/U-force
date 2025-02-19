using System;
using UnityEngine;

namespace UF
{
    public class TrainSystem : MonoBehaviour
    {
        public SystemType systemType => SystemType.Train;

        TrainSystemContext ctx;

        TrainSystemEvents events;
        public TrainSystemEvents Events => events;

        public void Ctor()
        {
            ctx = new TrainSystemContext();
            events = new TrainSystemEvents();
        }

        public void Inject(AppUI appUI, TemplateMoudle templateMoudle, AssetsModule assetsModule)
        {
            ctx.Inject(appUI, templateMoudle, assetsModule);
        }

        public void Enter()
        {
            ctx.isRunning = true;

            // UI

            // Game, stageID暂存为51
            int stageID = 51;
            bool has = ctx.templateMoudle.Stage_TryGet(stageID, out StageTM tm);
            if (!has)
            {
                Debug.LogError("StageID not found");
                return;
            }

            // Map
            MapEntity map = MapDomain.Spawn(ctx.assetsModule, ctx.idService, tm.mapPrefab);

            // Role
        }

        public void Exit()
        {
            ctx.isRunning = false;
        }

        public void Tick(float dt)
        {
            if (!ctx.isRunning)
            {
                return;
            }

            PreTick(dt);

            ref float restFixTime = ref ctx.restFixTime;

            restFixTime += dt;

            const float FIX_INTERVAL = 0.020f;

            if (restFixTime <= FIX_INTERVAL)
            {

                LogicTick(restFixTime);

                restFixTime = 0;
            }
            else
            {
                while (restFixTime >= FIX_INTERVAL)
                {
                    LogicTick(FIX_INTERVAL);
                    restFixTime -= FIX_INTERVAL;
                }
            }

            LastTick(dt);
        }

        void PreTick(float dt)
        {

        }

        void LogicTick(float dt)
        {

        }

        void LastTick(float dt)
        {

        }


        public void Train_Binding()
        {
            var appUI = ctx.appUI;
            UIEventCenter eventCenter = appUI.eventCenter;

            var events = this.events;

            // 移到 LoginSystem 里了
            // eventCenter.OnPlayer1ReadyClickHandle += () => {
            //     events.OnPlayer1ReadyClick();
            // };

            // eventCenter.OnPlayer2ReadyClickHandle += () => {
            //     events.OnPlayer2ReadyClick();
            // };

            // eventCenter.OnSelectionTrainCardClickHandle += (int typeID) => {
            //     events.OnSelectionTrainCardClick(typeID);
            // };
        }
    }
}