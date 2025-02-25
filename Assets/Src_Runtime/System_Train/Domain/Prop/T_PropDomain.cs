using System;
using UnityEngine;

namespace UF.System_Train
{
    public static class PropDomain
    {
        public static PropEntity Spawn(TrainSystemContext ctx, int typeID, Vector3 pos)
        {
            PropEntity entity = GameFactory.Prop_Create(ctx.assetsModule, ctx.templateMoudle, ctx.idService, typeID, pos);
            ctx.propRepository.Add(entity);
            return entity;
        }

        public static PropEntity SpawnByRole(TrainSystemContext ctx, RoleEntity role)
        {
            var pos = role.GetMuzzlePos();
            if (role.t_roleType == T_RoleType.Player1) {
                pos.x += 1;
            } else if (role.t_roleType == T_RoleType.Player2) {
                pos.x -= 1;
            }

            var prop = Spawn(ctx, 100, pos); // typeID暂用防护罩的100, 虽然没什么必要，但是后面要改
            return prop;
        }

        public static void UnSpawn(TrainSystemContext ctx, PropEntity prop)
        {
            ctx.propRepository.Remove(prop);
            prop.TearDown();
        }

        public static void Clear(TrainSystemContext ctx) {
            int len = ctx.propRepository.TakeAll(out PropEntity[] props);
            for (int i = 0; i < len; i += 1) {
                PropEntity prop = props[i];
                prop.TearDown();
            }
            ctx.propRepository.Clear();
        }
    }
}