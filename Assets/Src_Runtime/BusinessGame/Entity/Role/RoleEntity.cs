using System;
using UnityEngine;

namespace UF {
    public class RoleEntity : MonoBehaviour {
        public IDSignature idSig;
        public RoleType roleType;

        public int typeID;

        public IDSignature belongStage;//在哪一个地图里面

        public bool isOwner => roleType == RoleType.Player;

        // pos 
        [SerializeField] public Transform bodyRoot;
        public Vector2 Centerpos => this.transform.position;
        Vector2 face;

        // user input


        // - Physics
        [SerializeField] Rigidbody2D rb;
        // [SerializeField] Collider2D coll;

        // Movement

        // skillor


        public void Ctor() {

        }


    }
}