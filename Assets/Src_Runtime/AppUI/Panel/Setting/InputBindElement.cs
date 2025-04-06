using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UF {

    public class InputBindElement : MonoBehaviour {

        // TODO:这里加一个ID比较好；

        [SerializeField] Button btn_KbBind;
        public Action OnKbBindClickHandle;
        [SerializeField] TextMeshProUGUI txt_KbBindName;

        [SerializeField] Button btn_GamepadBind;
        public Action OnGamepadBindClickHandle;
        [SerializeField] TextMeshProUGUI txt_GamepadBindName;

        [SerializeField] TextMeshProUGUI txt_BindName;
        public string Get_BindName => txt_BindName.text;


        public void Ctor() {
            txt_KbBindName.text = "按键绑定";
            btn_KbBind.onClick.AddListener(() => OnKbBindClickHandle?.Invoke());
            btn_GamepadBind.onClick.AddListener(() => OnGamepadBindClickHandle?.Invoke());
        }

        public void Init(string bindName, string kbBindName, string gamepadBindName) {
            Set_TextBindName(bindName);
            Set_TextBindName_KbAndGamepad(kbBindName, gamepadBindName);
        }

        public void Set_TextBindName(string bindName) {
            txt_BindName.text = bindName;
        }

        public void Set_TextBindName_KbAndGamepad(string KbbindName, string GamepadBindName) {
            txt_KbBindName.text = KbbindName;
            txt_GamepadBindName.text = GamepadBindName;
        }

    }
}