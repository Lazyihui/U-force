using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UF {

    public class Panel_CardSelectionDetails : MonoBehaviour {

        [SerializeField] Image img_Card;
        [SerializeField] TextMeshProUGUI txt_Content;//内容

        public Action<int> OnConfirmClickHandle;
        [SerializeField] Button btn_confirm;//确认选择

        public int typeID;

        public void Ctor() {
            btn_confirm.onClick.AddListener(() => OnConfirmClickHandle.Invoke(typeID));
            btn_confirm.interactable = false;
        }

        public void Set_Detail(Sprite sprite, string content, int typeID) {
            img_Card.sprite = sprite;
            txt_Content.text = content;
            this.typeID = typeID;
        }

        public void CardDetailsBtn_Lock() {
            btn_confirm.interactable = false;
        }

        public void CardDetailsBtn_UnLock() {
            btn_confirm.interactable = true;
        }

        public void OnConfirmClick() {
            btn_confirm.onClick.Invoke();
        }
    }
}