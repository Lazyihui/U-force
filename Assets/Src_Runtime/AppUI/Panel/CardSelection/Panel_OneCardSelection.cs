using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UF {
    public class Panel_OneCardSelection : MonoBehaviour {

        [SerializeField] Image img_bg;
        [SerializeField] Button btn_Card;

        int typeID;
        public Action<int> OnCardClickHandle;

        public int GetID() {
            return typeID;
        }
        public void Ctor() {
            btn_Card.onClick.AddListener(() => {
                if (OnCardClickHandle != null) {
                    OnCardClickHandle.Invoke(typeID);
                }
            });
        }

        public void Init(int id, Sprite sprite) {
            this.typeID = id;
            img_bg.sprite = sprite;
        }

        public void OneCard_Lock() {
            btn_Card.interactable = false;
            img_bg.color = new Color(255, 0, 0, 255);
        }

        public void OneCard_UnLock() {
            btn_Card.interactable = true;
            img_bg.color = new Color(255f, 255f, 255f, 255);
        }

    }
}