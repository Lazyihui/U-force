using System;
using System.Collections;
using System.Collections.Generic;
using TriInspector;
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

        [SerializeField] Vector2 muzzlePos;
        public Vector2 MuzzlePos => muzzlePos;

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

        // -Feature//特性
        RoleFeatureComponent featureComponent;
        public RoleFeatureComponent FeatureComponent => featureComponent;

        // 训练营
        public T_RoleType t_roleType;
        public float AIDeviation;
        public float minY;

        // 特殊移动行为
        public bool isChangeLine; // 是否换行
        [ShowIf(nameof(isChangeLine))] public Vector2 changeLineDir; // 换行方向
        [ShowIf(nameof(isChangeLine))] public float changeLineLength; // 换行距离(行数 * 每行长度)

        public bool isRepulsed; // 是否被击退
        [ShowIf(nameof(isRepulsed))] public float repulsedLength; // 被击退距离

        [HideInInspector] public Vector2 changeLinePos;
        [HideInInspector] public Vector2 repulsedPos;
        [HideInInspector] public bool hasChangeLinePos;
        [HideInInspector] public bool hasRepulsedPos;

        // 资源战
        public bool isEliminateRole; // 是否为资源战中除玩家的role

        // 判断是否被子弹击中
        public bool isHit;

        public void Ctor() {
            fsmComponent = new RoleFSMConponent();
            skillorSlotComponent = new RoleSkillSlotComponent();
            inputComponent = new RoleInputComponent();
            moveComponent = new RoleMoveComponent();
            spawnerPosComponent = new RoleSpawnerPosComponent();
            attributeComponent = new RoleAttributeComponent();
            featureComponent = new RoleFeatureComponent();
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
            if (isRepulsed) {
                moveComponent.Move_Repulsed();

                if (hasRepulsedPos) { return; }
                repulsedPos = TF_GetPos();
                hasRepulsedPos = true;
            } else {
                moveComponent.Move_Ghost(moveSpeed);
            }

            if (isChangeLine) {
                moveComponent.Move_ChangeLine(changeLineDir);

                if (hasChangeLinePos) { return; }
                changeLinePos = TF_GetPos();
                hasChangeLinePos = true;

            }
        }

        public void Move_Offensive(Vector2 dir, float moveSpeed) {
            moveComponent.Move_Attack(dir, moveSpeed);
        }

        public void Move_Regular(float moveSpeed) { // 资源战动态障碍-固定
            var pos = GetPos();
            moveComponent.Move_Regular(pos, moveSpeed);
        }

        public void Move_Random(float moveSpeed) { // 资源战动态障碍-随机
            var pos = GetPos();
            moveComponent.Move_Random(pos, moveSpeed);
        }

        public void Move_Stop() {
            moveComponent.Move_Stop();
        }

        public void Move_Stretched_Stop() {
            moveComponent.Move_Stretched_Stop();
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

        public RigidbodyType2D GetRbType() {
            return rb.bodyType;
        }

        public void SetRbType(RigidbodyType2D type) {
            moveComponent.SetRbType(type);
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

        #region Feature

        public void Set_SpriteAppear() {
            mod.Set_SpriteDisAppear();
        }

        public void Set_SpriteDisappear() {
            mod.Set_SpriteAppear();
        }

        #endregion

        public float GetMinY() {
            var pos = GetPos();
            if (pos.y < minY) {
                minY = pos.y;
            }
            return minY;
        }
    }
}