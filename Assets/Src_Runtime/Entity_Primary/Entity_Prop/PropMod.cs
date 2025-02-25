using System;
using UnityEngine;

namespace UF {
    public class PropMod : MonoBehaviour {

        [SerializeField] Animator anim;
        [SerializeField] SpriteRenderer sr;
        [SerializeField] BoxCollider2D coll_box;

        public void Ctor() {

        }

        public void Reuse() {
            gameObject.SetActive(true);
        }

        public void Release() {
            gameObject.SetActive(false);
        }

    }
}