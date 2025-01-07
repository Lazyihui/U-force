using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace UF {
    public class Panel_Login : MonoBehaviour {

        [SerializeField] Button btn_Login;
        public Action OnLoginClickHandle;

        public void Ctor() {

            btn_Login.onClick.AddListener(() => {
                if (OnLoginClickHandle != null) {
                    OnLoginClickHandle.Invoke();
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