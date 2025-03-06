using System;
using UnityEngine;
using UnityEngine.UI;

namespace UF {
    public class Panel_CardOneIllustrated : MonoBehaviour {
        public int id;
        [SerializeField] Image img_Sprite;

        [SerializeField] Button btn_Card;
        public Action<int> OnCardClickHandle;

        public bool isRole;

        public void Ctor() {
            btn_Card.onClick.AddListener(() => {
                if (OnCardClickHandle != null) {
                    OnCardClickHandle.Invoke(id);
                }
            });
        }

        public void Init(int id, Sprite sprite) {
            this.id = id;
            img_Sprite.sprite = sprite;
        }

        public void OnClick() {
            btn_Card.onClick.Invoke();
        }
    }
}