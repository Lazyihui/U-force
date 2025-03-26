using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UF {

    public class Panel_GamePause : MonoBehaviour {
        [SerializeField] Button btn_Continue;
        public Action OnContinueClickHandle;

        // 图鉴
        [SerializeField] Button btn_Illustrated;
        public Action OnIllustratedClickHandle;

        [SerializeField] Button btn_Quit;
        public Action OnQuitClickHandle;

        public Action OnCloseHandle;

        public void Ctor() {
            btn_Continue.onClick.AddListener(() => {
                if (OnContinueClickHandle != null) {
                    OnContinueClickHandle.Invoke();
                }
            });

            btn_Illustrated.onClick.AddListener(() => {
                if (OnIllustratedClickHandle != null) {
                    OnIllustratedClickHandle.Invoke();
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
