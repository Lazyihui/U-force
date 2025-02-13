using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UF {

    public class Panel_CardSelectionDetails : MonoBehaviour {

        [SerializeField] Image img_Card;
        [SerializeField] TextMeshProUGUI txt_Content;

        public Action OnConfirmClickHandle;
        [SerializeField] Button btn_confirm;//确认选择

        public void Ctor() {
            btn_confirm.onClick.AddListener(() => OnConfirmClickHandle.Invoke());
        }

        public void Set_Detail(Sprite sprite, string content) {
            img_Card.sprite = sprite;
            txt_Content.text = content;
        }

    }
}