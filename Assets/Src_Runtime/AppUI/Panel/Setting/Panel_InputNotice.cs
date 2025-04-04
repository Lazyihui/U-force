using System;
using UnityEngine;
using UnityEngine.UI;

namespace UF {
    public class Panel_InputNotice : MonoBehaviour {

        [SerializeField] Image noticeImage;
        [SerializeField] RectTransform imageRectTransform;
        [SerializeField] GameObject img_confirm;
        [SerializeField] Button btn_confirm;
        public Action OnInputNoticeClickHandle;

        public bool isImg_confirmOpen;
        float initialWidth; // 初始宽度
        public float countTimer;
        public float currentTimer;

        public void Ctor() {
            btn_confirm.onClick.AddListener(() => {
                OnInputNoticeClickHandle?.Invoke();
            });

            isImg_confirmOpen = false;
            img_confirm.SetActive(false);
            currentTimer = 3; // 重置计时器
            countTimer = 3; // 重置计时器
            initialWidth = imageRectTransform.sizeDelta.x; // 获取初始宽度
        }

        public void img_confirmOpen() {
            isImg_confirmOpen = true;
            img_confirm.SetActive(true);
        }

        void Update() {
            if (isImg_confirmOpen) {
                return;
            }

            // TODO:这里要改到外面去
            // Debug.Log("Update Timer: " + currentTimer);
            if (currentTimer > 0) {
                currentTimer -= Time.deltaTime;

                float widthPercentage = currentTimer / countTimer;
                float newWidth = initialWidth * widthPercentage;

                imageRectTransform.sizeDelta = new Vector2(
                    newWidth,
                    imageRectTransform.sizeDelta.y
                );
            } else {
                TearDown();
            }
        }

        public void Show() {
            gameObject.SetActive(true);
        }

        public void TearDown() {
            Destroy(gameObject);
        }

    }
}