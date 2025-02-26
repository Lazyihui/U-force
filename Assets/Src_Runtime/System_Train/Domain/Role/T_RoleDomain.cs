using System;
using UnityEngine;

namespace UF.System_Train {
    public static partial class RoleDomain {
        // 感觉没必要重复写factory, 所以调了gameFactory, 按理说系统是独立的, 这里相当于using System_Game, 所以不太确定能不能行
        static RoleEntity Spawn(TrainSystemContext ctx, int typeID) {
            RoleEntity role = GameFactory.Role_Create(ctx.assetsModule, ctx.templateMoudle, ctx.idService, typeID);
            ctx.roleRepository.Add(role);
            return role;
        }

        public static RoleEntity SpawnByPos(TrainSystemContext ctx, Vector2 pos, int typeID) {
            RoleEntity entity = Spawn(ctx, typeID);
            entity.TF_SetPos(pos);
            return entity;
        }

        public static void Input_Record(TrainSystemContext ctx, RoleEntity role, float dt) {
            Debug.Assert(role != null, "RoleDomain.Input_Record role == null");
            Debug.Assert(ctx != null, "RoleDomain.Input_Record ctx == null");
            Debug.Assert(role.InputComponent != null, "RoleDomain.Input_Record role.InputComponent == null");
            var inputCom = role.InputComponent;
            var inputModule = ctx.inputModule;

            inputCom.Player1Jump_Set(inputModule.IsPlayer1Jump);
            inputCom.Player1Attack_Set(inputModule.IsPlayer1Attack);
            inputCom.Player1Defense_Set(inputModule.IsPlayer1Defense);

            inputCom.Player2Jump_Set(inputModule.IsPlayer2Jump);
            inputCom.Player2Attack_Set(inputModule.IsPlayer2Attack);
            inputCom.Player2Defense_Set(inputModule.IsPlayer2Defense);
        }

        public static void HpReduce_MinionFall(TrainSystemContext ctx, RoleEntity role, int damage) {
            var attCom = role.AttributeComponent;
            int finalHp = attCom.hp - damage;
            role.Attr_Hp_Set(finalHp);

            if (attCom.hp <= 0) {
                int len = ctx.roleRepository.TakeAll(out var roles);
                for (int i = 0; i < len; i++) {
                    role = roles[i];

                    if (role.AttributeComponent.hp != 0) {
                        string text = "玩家" + (i + 1) + " 胜利";
                        ctx.templateMoudle.Role_TryGet(role.typeID, out var tm);
                        
                        ctx.appUI.Panel_Training_GameOver_Open(text, tm.sprite);
                    }
                }
            }
        }
    }
}