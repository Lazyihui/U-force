using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UF
{
    public class Panel_OneCardTrain : MonoBehaviour
    {
        int id;

        [SerializeField] Image img_bg;
        [SerializeField] TextMeshProUGUI text_name;

        [SerializeField] Button btn_Card;
        public Action<int> OnCardClickHandle;
        
        public void Ctor() {
            btn_Card.onClick.AddListener(() => {
                if (OnCardClickHandle != null) {
                    OnCardClickHandle.Invoke(id);
                }
            });
        }

        public void Init(int id, Sprite sprite, string name) {
            this.id = id;
            img_bg.sprite = sprite;
            text_name.text = name;
        }
    }
}