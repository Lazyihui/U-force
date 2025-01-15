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
        public RoleSkillSlotComponent skillSlotComponent;


        public void Ctor() {
            fsmComponent = new RoleFSMConponent();
            skillSlotComponent = new RoleSkillSlotComponent();
            inputComponent = new RoleInpuComponent();
        }

        public void Reuse() {
            gameObject.SetActive(true);
        }

        public void Release() {
            skillSlotComponent.Release();
            inputComponent.Release();
        }
        public SkillFrameModel Skillor_CurrentFrame() {

            if (fsmComponent.Status != RoleFSMStatus.Casting) {
                return null;
            }

            SkillModel skill = skillSlotComponent.CurrentSkill;
            if (skill == null) {
                return null;
            }

            SkillFrameModel[] frames = skill.frames;
            if (frames == null || frames.Length == 0) {
                return null;
            }

            SkillFrameModel frame = frames[skill.FrameIndex];
            return frame;

        }
        public SkillModel Skill_GetByInput() {
            var inputcom = InputComponent;
            var skillor = skillSlotComponent;

            InputKeyFlag inputKeys = inputcom.CastKeys;

            SkillModel skill = null;
            if (inputcom.IsJump) {
                int len = skillor.TakeSkillsByFlushInput(out SkillModel[] result, inputKeys);
                Debug.Log("len:" + len + "result:" + result + inputKeys);
                // for (int i = 0; i < len; i++) {
                //     if (result[i].typeID == 1) {
                //         skill = result[i];
                //         break;
                //     }
                // }
            }
            if (skill != null) {
                Debug.Log("skill:" + skill.typeID);
            } else {
            }
            return skill;
        }


    }
}