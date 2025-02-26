using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UF {
    public class RoleEntity : MonoBehaviour {
        public IDSignature idSig;
        public RoleType roleType;
        public bool isOwner => roleType == RoleType.Player;

        public RoleState state;
        public void SetRoleState(RoleState value) => state = value;

        public RoleMoveType moveType;

        public int typeID;
        public IDSignature belongStage;//在哪一个地图里面

        [SerializeField] Transform muzzle;
        public Transform Muzzle => muzzle;

        // - FSM
        RoleFSMConponent fsmComponent;
        public RoleFSMConponent FSMComponent => fsmComponent;

        // pos 
        [SerializeField] public Transform bodyRoot;
        public Vector2 Centerpos => this.transform.position;
        public void TF_SetPos(Vector2 value) => this.transform.position = value;
        public void TF_SetRot(Vector3 value) => this.transform.eulerAngles = value;
        public Vector3 TF_GetPos() => this.transform.position;
        Vector2 face;

        // user input
        [SerializeField] RoleInputComponent inputComponent;//TODO:不知道[SerializeField]是干嘛的
        public RoleInputComponent InputComponent => inputComponent;


        // - Physics
        [SerializeField] Rigidbody2D rb;
        // [SerializeField] Collider2D coll;

        // Movement
        RoleMoveComponent moveComponent;
        public RoleMoveComponent MoveComponent => moveComponent;

        // skillor
        public RoleSkillSlotComponent skillorSlotComponent;

        // mod
        RoleMod mod;

        // pos
        public RoleSpawnerPosComponent spawnerPosComponent;

        // -attribute
        RoleAttributeComponent attributeComponent;
        public RoleAttributeComponent AttributeComponent => attributeComponent;

        // 训练营
        public T_RoleType t_roleType;

        public void Ctor() {
            fsmComponent = new RoleFSMConponent();
            skillorSlotComponent = new RoleSkillSlotComponent();
            inputComponent = new RoleInputComponent();
            moveComponent = new RoleMoveComponent();
            spawnerPosComponent = new RoleSpawnerPosComponent();
            attributeComponent = new RoleAttributeComponent();

            moveComponent.Inject(rb);
        }

        public void TearDown() {
            GameObject.Destroy(gameObject);
        }

        public void Set_RbConstraints() {
            if (roleType == RoleType.Player) {
                moveComponent.Set_PlayerRbConstraints();
            }
            if (roleType == RoleType.Minion) {
                if (state == RoleState.Fall) {
                    moveComponent.Set_FallRbConstraints();
                } else {
                    moveComponent.Set_MinionRbConstraints();
                }
            }
        }

        public void Inject(RoleMod mod) {
            this.mod = mod;
        }

        public void Reuse() {
            gameObject.SetActive(true);
        }

        public void Release() {
            skillorSlotComponent.Release();
            inputComponent.Release();
        }

        #region Move
        public void Move_Player(float moveSpeed) {
            moveComponent.Move_Player(moveSpeed);
        }

        public void Move_Ghost(float moveSpeed) {
            moveComponent.Move_Ghost(moveSpeed);
        }

        public void Move_Offensive(Vector2 dir, float moveSpeed) {
            moveComponent.Move_Attack(dir, moveSpeed);
        }

        public void Move_Stop() {
            moveComponent.Move_Stop();
        }

        public void Move_Reset() {
            moveComponent.Move_Reset();
        }

        public void Fall() {
            moveComponent.Fall();
        }

        public void Jump() {
            moveComponent.Jump();
        }

        public void Jump_Stop() {
            moveComponent.Jump_Stop();
        }

        public bool IsGround() {
            return moveComponent.IsGround;
        }

        public void EnterGround() {
            moveComponent.EnterGround();
        }

        public bool AllowPlayerMove() {
            return moveComponent.AllowPlayerMove();
        }

        #endregion

        public Vector2 GetPos() {
            return transform.position;
        }

        public Vector3 GetMuzzlePos() {
            return mod.muzzle.position;
        }

        public Vector2 GetVelocity() {
            return rb.velocity;
        }

        #region Attribute

        public void Attr_Hp_Set(int value) {
            attributeComponent.SetHp(value);
        }

        public void Attr_HpMax_Set(int value) {
            attributeComponent.SetHpMax(value);
        }

        #endregion

        public SkillFrameModel Skillor_CurrentFrame() {

            SkillModel skill = skillorSlotComponent.CurrentSkill;
            if (skill == null) {
                Debug.Log("skill == null");
                return null;
            }

            SkillFrameModel[] frames = skill.frames;
            if (frames == null || frames.Length == 0) {
                Debug.Log("frames == null || frames.Length == 0");
                return null;
            }

            SkillFrameModel frame = frames[skill.FrameIndex];
            return frame;

        }
        public SkillModel Skill_GetByInput() {
            var inputcom = InputComponent;
            var skillor = skillorSlotComponent;

            InputKeyFlag inputKeys = inputcom.CastKeys;

            SkillModel result = null;
            int len = skillor.TakeSkillsByFlushInput(out SkillModel[] skills, inputKeys);
            for (int i = 0; i < len; i++) {
                var skill = skills[i];
                if (skill != null) {
                    result = skill;
                    break;
                }
            }

            return result;
        }

    }
}