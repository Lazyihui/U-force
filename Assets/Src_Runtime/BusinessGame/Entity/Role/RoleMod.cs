using System;
using UnityEngine;

namespace UF {
    public class RoleMod : MonoBehaviour {
        [SerializeField] SpriteRenderer sr;

        [SerializeField] public Transform muzzle;

        [SerializeField] Animator anim;

        public void Ctor() { }


        public void Reuse() {
            this.gameObject.SetActive(true);
        }

        public void Release() {
            this.gameObject.SetActive(false);
        }


    }
}