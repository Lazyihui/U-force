using System;
using UnityEngine;
using UnityEngine.UI;

namespace UF {
    public class Panel_BackSelection : MonoBehaviour {
        // 因为不要重复绑定, 所以命名加了Warning, 方便区分
        [SerializeField] Button btn_CloseWarning;
        public Action OnCloseWarningClickHandle;

        [SerializeField] Button btn_ContinueWarning;
        public Action OnContinueWarningClickHandle;

        [SerializeField] Button btn_ResetWarning;
        public Action OnResetWarningClickHandle;

        [SerializeField] Button btn_QuitWarning;
        public Action OnQuitWarningClickHandle;

        public void Ctor() {
            btn_CloseWarning.onClick.AddListener(() => {
                if (OnCloseWarningClickHandle != null) {
                    OnCloseWarningClickHandle.Invoke();
                }
            });

            btn_ContinueWarning.onClick.AddListener(() => {
                if (OnContinueWarningClickHandle != null) {
                    OnContinueWarningClickHandle.Invoke();
                }
            });

            btn_ResetWarning.onClick.AddListener(() => {
                if (OnResetWarningClickHandle != null) {
                    OnResetWarningClickHandle.Invoke();
                }
            });

            btn_QuitWarning.onClick.AddListener(() => {
                if (OnQuitWarningClickHandle != null) {
                    OnQuitWarningClickHandle.Invoke();
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