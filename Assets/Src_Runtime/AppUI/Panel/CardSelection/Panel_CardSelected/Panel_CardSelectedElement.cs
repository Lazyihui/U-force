using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UF {

    public class Panel_CardSelectedElement : MonoBehaviour {

        [SerializeField] Image img_Sprite;

        [SerializeField] Button btn_Card;
        public Action<int> OnCardClickHandle;
        int typeID;

        public void Ctor() {
            btn_Card.onClick.AddListener(() => {
                if (OnCardClickHandle != null) {
                    OnCardClickHandle.Invoke(typeID);
                }
            });
        }

        public int GetID() {
            return typeID;
        }

        public Sprite GetSprite() {
            return img_Sprite.sprite;
        }

        public void Init(int typeID, Sprite sprite) {
            this.typeID = typeID;
            img_Sprite.sprite = sprite;
        }

    }
}