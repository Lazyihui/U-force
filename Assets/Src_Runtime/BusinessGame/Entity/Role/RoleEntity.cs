using System;
using UnityEngine;

namespace UF {
    public class RoleEntity : MonoBehaviour {
        public IDSignature idSig;
        public RoleType roleType;

        public int typeID;

        public IDSignature belongStage;//在哪一个地图里面

        public bool isOwner => roleType == RoleType.Player;

        // - FSM
        RoleFSMConponent fsmConponent;
        public RoleFSMConponent Fsm => fsmConponent;

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
            fsmConponent = new RoleFSMConponent();
            skillSlotComponent = new RoleSkillSlotComponent();
            inputComponent = new RoleInpuComponent();
        }

        public void Reuse(){
            gameObject.SetActive(true);
        }

        public void Release() {
            skillSlotComponent.Release();
            inputComponent.Release();
        }

        public SkillModel Skill_GetByInput() {

            return null;
        }


    }
}