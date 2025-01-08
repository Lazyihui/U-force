using System;
using UnityEngine;

namespace UF {
    public class BulletMod : MonoBehaviour {

        [SerializeField] Animator anim;
        [SerializeField] SpriteRenderer sr;

        // [SerializeField] BoxCollider2D coll_box;
        [SerializeField] CircleCollider2D coll_circle;


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