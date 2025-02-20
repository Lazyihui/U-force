using System;
using UnityEngine;

namespace UF.System_Train
{
    public static partial class RoleDomain
    {
        // 感觉没必要重复写factory, 所以调了gameFactory, 按理说系统是独立的, 这里相当于using System_Game, 所以不太确定能不能行
        static RoleEntity Spawn(TrainSystemContext ctx, int typeID)
        {
            RoleEntity role = GameFactory.Role_Create(ctx.assetsModule, ctx.templateMoudle, ctx.idService, typeID);
            ctx.roleRepository.Add(role);
            return role;
        }

        public static RoleEntity SpawnByPos(TrainSystemContext ctx, Vector2 pos, int typeID)
        {
            RoleEntity entity = Spawn(ctx, typeID);
            entity.TF_SetPos(pos);
            return entity;
        }
    }
}