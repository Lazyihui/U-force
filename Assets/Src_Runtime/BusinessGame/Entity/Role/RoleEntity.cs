using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UF {
    public class RoleEntity : MonoBehaviour {
        public IDSignature idSig;
        public RoleType roleType;
        public bool isOwner => roleType == RoleType.Player;

        public int typeID;
        public IDSignature belongStage;//在哪一个地图里面

        // - FSM
        RoleFSMConponent fsmComponent;
        public RoleFSMConponent FSMComponent => fsmComponent;

        // pos 
        [SerializeField] public Transform bodyRoot;
        public Vector2 Centerpos => this.transform.position;
        Vector2 face;

        // user input
        [SerializeField] RoleInpuComponent inputComponent;//TODO:不知道[SerializeField]是干嘛的
        public RoleInpuComponent InputComponent => inputComponent;


        // - Physics
        [SerializeField] Rigidbody2D rb;
        // [SerializeField] Collider2D coll;

        // Movement

        // skillor
        public RoleSkillSlotComponent skillorSlotComponent;


        public void Ctor() {
            fsmComponent = new RoleFSMConponent();
            skillorSlotComponent = new RoleSkillSlotComponent();
            inputComponent = new RoleInpuComponent();
        }

        public void Reuse() {
            gameObject.SetActive(true);
        }

        public void Release() {
            skillorSlotComponent.Release();
            inputComponent.Release();
        }

        public void Jump() {
            // TODO:暂时这么写
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
            Debug.Log("Jump");
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
            // TODO:inputcom.IsJump这个条件不应该在这里

            if (inputcom.IsJump) {
                Debug.Log("Jump要删掉");
                int len = skillor.TakeSkillsByFlushInput(out SkillModel[] skills, inputKeys);
                for (int i = 0; i < len; i++) {
                    var skill = skills[i];
                    if (skill != null) {
                        result = skill;
                        break;
                    }
                }
            }
            return result;
        }


    }
}