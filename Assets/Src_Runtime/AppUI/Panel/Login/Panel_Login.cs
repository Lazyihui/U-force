using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

        Button selectedBtn;
        bool isMouseOver;

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

        public void Show() {
            SetSelectedButton(btn_Login);

            gameObject.SetActive(true);
        }

        public void TearDown() {
            GameObject.Destroy(gameObject);
        }

        public void Update_Tick(Canvas canvas, Vector2 moveAxis, bool isConfirm) {
            if (gameObject.activeSelf == false) { return; }

            if (selectedBtn == null) {
                SetSelectedButton(btn_Login);
                return;
            }

            // 确认
            if (isConfirm) {
                ConfirmSelectedButton();
                return;
            }

            // 鼠标悬停
            Button btn = GetMouseOverButton(canvas.gameObject);
            if (isMouseOver) {
                SetSelectedButton(btn);
                return;
            }

            // 选中按钮检测
            if (moveAxis.y == 0) { return; }

            Selectable selectable = selectedBtn.FindSelectable(moveAxis);
            Button newSelectedBtn = selectable as Button;

            if (newSelectedBtn == null) { return; }
            SetSelectedButton(newSelectedBtn);
        }

        void SetSelectedButton(Button newButton) {
            selectedBtn = newButton;

            btn_Login.image.color = (selectedBtn == btn_Login) ? Color.red : Color.white;
            btn_Training.image.color = (selectedBtn == btn_Training) ? Color.red : Color.white;
            btn_Setting.image.color = (selectedBtn == btn_Setting) ? Color.red : Color.white;
            btn_Quit.image.color = (selectedBtn == btn_Quit) ? Color.red : Color.white;
        }

        void ConfirmSelectedButton() {
            if (selectedBtn == null) { return; }

            selectedBtn.onClick.Invoke();
        }

        Button GetMouseOverButton(GameObject canvas) {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;
            GraphicRaycaster gr = canvas.GetComponent<GraphicRaycaster>();
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(pointerEventData, results);

            if (results.Count != 0) {
                var len = results.Count;
                for (int i = 0; i < len; i++) {
                    if (results[i].gameObject.GetComponent<Button>() != null) {
                        isMouseOver = true;

                        Button btn = results[i].gameObject.GetComponent<Button>();
                        return btn;
                    }
                }
            }

            isMouseOver = false;
            return null;
        }
    }
}