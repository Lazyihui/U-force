using System;
using UnityEngine;

namespace UF {
    public class StuffMod : MonoBehaviour {

        [SerializeField] Animator anim;
        [SerializeField] SpriteRenderer sr;

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