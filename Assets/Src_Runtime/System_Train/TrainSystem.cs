using System;
using System.Collections.Generic;
using UnityEngine;

namespace UF.System_Train
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

        public void Inject(AppUI appUI, TemplateMoudle templateMoudle, AssetsModule assetsModule, RoleRepository roleRepository)
        {
            ctx.Inject(appUI, templateMoudle, assetsModule, roleRepository);
        }

        public void Enter(List<int> typeIDs)
        {
            ctx.isRunning = true;

            // UI
            ctx.appUI.Panel_Training_Button_Show();

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
            if (typeIDs.Count < 2)
            {
                Debug.LogError("没有获取到两个role");
                return;
            }

            Vector2 pos1 = new Vector2(-15, 0);
            Vector2 pos2 = new Vector2(15, 0);
            RoleDomain.SpawnByPos(ctx, pos1, typeIDs[0]);
            RoleDomain.SpawnByPos(ctx, pos2, typeIDs[1]);
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
            // 所有角色逻辑
            ctx.roleRepository.TakeAll(out RoleEntity[] roles);
            //不知道为什么, 用length一直是3, 会有bug
            for (int i = 0; i < 2; i++)
            {
                RoleEntity role = roles[i];
                RoleDomain.Loco_Execute(role, dt);
            }
        }

        void LastTick(float dt)
        {

        }


        public void Train_Binding()
        {
            var appUI = ctx.appUI;
            UIEventCenter eventCenter = appUI.eventCenter;

            var events = this.events;

            eventCenter.OnTrainPauseClickHandle += () => {
                events.OnTrainPauseClick();
            };

            eventCenter.OnTrainContinueClickHandle += () => {
                events.OnTrainContinueClick();
            };

            eventCenter.OnTrainIllustratedClickHandle += () => {
                events.OnTrainIllustratedClick();
            };

            eventCenter.OnTrainQuitClickHandle += () => {
                events.OnTrainQuitClick();
            };
        }
    }
}