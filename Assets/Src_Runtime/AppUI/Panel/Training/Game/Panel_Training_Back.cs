using System;
using UnityEngine;
using UnityEngine.UI;

namespace UF {
    public class Panel_Training_Back : MonoBehaviour {
        [SerializeField] Button btn_Close;
        public Action OnCloseClickHandle;

        [SerializeField] Button btn_Continue;
        public Action OnContinueClickHandle;

        [SerializeField] Button btn_Quit;
        public Action OnQuitClickHandle;

        public void Ctor() {
            btn_Close.onClick.AddListener(() => {
                if (OnCloseClickHandle != null) {
                    OnCloseClickHandle.Invoke();
                }
            });

            btn_Continue.onClick.AddListener(() => {
                if (OnContinueClickHandle != null) {
                    OnContinueClickHandle.Invoke();
                }
            });

            btn_Quit.onClick.AddListener(() => {
                if (OnQuitClickHandle != null) {
                    OnQuitClickHandle.Invoke();
                }
            });
        }

        public void Show() {
            gameObject.SetActive(true);
        }

        public void TearDown() {
            Destroy(gameObject);
        }
    }
}