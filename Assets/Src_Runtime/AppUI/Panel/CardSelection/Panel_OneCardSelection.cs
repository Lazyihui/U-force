using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UF {
    public class Panel_OneCardSelection : MonoBehaviour {

        [SerializeField] Image img_bg;
        int id;
        [SerializeField] Button btn_Card;

        public Action<int> OnCardClickHandle;


        public void Ctor() {
            btn_Card.onClick.AddListener(() => {
                if (OnCardClickHandle != null) {
                    OnCardClickHandle.Invoke(id);
                }
            });
        }

        public void Init(int id, Sprite sprite) {
            this.id = id;
            img_bg.sprite = sprite;
        }


    }
}