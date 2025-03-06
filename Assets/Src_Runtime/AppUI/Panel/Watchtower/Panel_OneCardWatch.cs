using System;
using UnityEngine;
using UnityEngine.UI;

namespace UF {
    public class Panel_OneCardWatch : MonoBehaviour {
        int id;
        [SerializeField] Image img_bg;
        [SerializeField] Button btn_Card;

        public void Ctor() {

        }

        public void Init(int id, Sprite sprite) {
            this.id = id;
            img_bg.sprite = sprite;
        }
    }
}