using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UF {
    public class Panel_Training_GameOver : MonoBehaviour {
        [SerializeField] TextMeshProUGUI text_Over;

        [SerializeField] Button btn_Reset;
        public Action OnResetClickHandle;

        [SerializeField] Button btn_Back;
        public Action OnBackClickHandle;

        [SerializeField] Image img_Winner;

        public void Ctor() {
            btn_Reset.onClick.AddListener(() => {
                if (OnResetClickHandle != null) {
                    OnResetClickHandle.Invoke();
                }
            });

            btn_Back.onClick.AddListener(() => {
                if (OnBackClickHandle != null) {
                    OnBackClickHandle.Invoke();
                }
            });
        }

        public void Show() {
            gameObject.SetActive(true);
        }

        public void TearDown() {
            Destroy(gameObject);
        }

        public void SetText(string text) {
            text_Over.text = text;
        }

        public void SetWinner(Sprite winner) {
            img_Winner.sprite = winner;
        }
    }
}