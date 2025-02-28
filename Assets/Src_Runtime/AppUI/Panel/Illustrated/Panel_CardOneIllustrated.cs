using System;
using UnityEngine;
using UnityEngine.UI;

namespace UF {
    public class Panel_CardOneIllustrated : MonoBehaviour {
        int id;
        [SerializeField] Image img_Sprite;

        [SerializeField] Button btn_Card;
        public Action<int> OnCardClickHandle;

        public void Ctor() {
            btn_Card.onClick.AddListener(() => OnCardClickHandle?.Invoke(id));
        }

        public void Init(int id, Sprite sprite) {
            this.id = id;
            img_Sprite.sprite = sprite;
        }
    }
}