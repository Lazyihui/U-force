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

        public RoleMoveType MoveType;

        public int typeID;
        public IDSignature belongStage;//在哪一个地图里面

        // - FSM
        RoleFSMConponent fsmComponent;
        public RoleFSMConponent FSMComponent => fsmComponent;

        // pos 
        [SerializeField] public Transform bodyRoot;
        public Vector2 Centerpos => this.transform.position;
        public void TF_SetPos(Vector2 value) => this.transform.position = value;
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

        public void Ctor() {
            fsmComponent = new RoleFSMConponent();
            skillorSlotComponent = new RoleSkillSlotComponent();
            inputComponent = new RoleInputComponent();
            moveComponent = new RoleMoveComponent();
            spawnerPosComponent = new RoleSpawnerPosComponent();

            moveComponent.Inject(rb);
        }

        public void Set_RbConstraints() {
            // TODO:先写在这里
            if (roleType == RoleType.Player) {
            }
            if (roleType == RoleType.Minion) {
                rb.constraints = RigidbodyConstraints2D.FreezePositionY;
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

        public void Move_Ghost(float xAxis) {
            moveComponent.Move_Ghost(xAxis);
        }

        public void Move_Stop() {
            moveComponent.Move_Stop();
        }

        public void Move_Reset() {
            moveComponent.Move_Reset();
        }

        public void Jump() {
            moveComponent.Jump();
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