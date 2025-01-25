using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace UF {
    public class Panel_Login : MonoBehaviour {

        [SerializeField] Button btn_Login;
        public Action OnLoginClickHandle;

        [SerializeField] Button btn_Training;
        public Action OnTrainingClickHandle;

        [SerializeField] Button btn_Setting;
        public Action OnSettingClickHandle;

        [SerializeField] Button btn_Quit;
        public Action OnQuitClickHandle;

        public void Ctor() {

            btn_Login.onClick.AddListener(() => {
                if (OnLoginClickHandle != null) {
                    OnLoginClickHandle.Invoke();
                }
            });

            btn_Training.onClick.AddListener(() => {
                if (OnTrainingClickHandle != null) {
                    OnTrainingClickHandle.Invoke();
                }
            });

            btn_Setting.onClick.AddListener(() => {
                if (OnSettingClickHandle != null) {
                    OnSettingClickHandle.Invoke();
                }
            });

            btn_Quit.onClick.AddListener(() => {
                if (OnQuitClickHandle != null) {
                    OnQuitClickHandle.Invoke();
                }
            });

        }

        public void Show(){
            gameObject.SetActive(true);
        }

        public void TearDown(){
            GameObject.Destroy(gameObject);
        }

    }
}