using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UF {

    public class InputBindElement : MonoBehaviour {

        [SerializeField] Button btn_KbBind;
        public Action OnKbBindClickHandle;
        [SerializeField] TextMeshProUGUI txt_KbBindName;

        [SerializeField] Button btn_GamepadBind;
        public Action OnGamepadBindClickHandle;
        [SerializeField] TextMeshProUGUI txt_GamepadBindName;

        [SerializeField] TextMeshProUGUI txt_BindName;

        public void Ctor() {
            btn_KbBind.onClick.AddListener(() => OnKbBindClickHandle?.Invoke());
            btn_GamepadBind.onClick.AddListener(() => OnGamepadBindClickHandle?.Invoke());
        }

        public void Set_TextBindName(string bindName) {
            txt_BindName.text = bindName;
        }

        public void Set_TextBindName_Kb(string bindName) {
            txt_KbBindName.text = bindName;
        }

        public void Set_TextBindName_Gamepad(string bindName) {
            txt_GamepadBindName.text = bindName;
        }

    }
}