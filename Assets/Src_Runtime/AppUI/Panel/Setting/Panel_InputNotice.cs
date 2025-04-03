using System;
using UnityEngine;
using UnityEngine.UI;

namespace UF {
    public class Panel_InputNotice : MonoBehaviour {

        [SerializeField] Image noticeImage;
        [SerializeField] RectTransform imageRectTransform;
        float initialWidth; // 初始宽度
        public float countTimer;
        public float currentTimer;

        public void Ctor() {
            currentTimer = 3; // 重置计时器
            countTimer = 3; // 重置计时器
            initialWidth = imageRectTransform.sizeDelta.x; // 获取初始宽度
        }

        void Update() {
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