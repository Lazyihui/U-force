using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UF {
    public class HUD_Progressbar : MonoBehaviour {
        // waves波次

        [SerializeField] Slider slider_Waves;
        
        [SerializeField] TextMeshProUGUI Text_Achievement;

        public int waves;
        public GameObject text; // 先直接在Inspector里拖过来, 单一个text感觉没必要寻址

        public void Ctor() {
        }

        // 设置波次
        public void Set_Waves(int waves) {
            this.waves = waves;
        }

        // 进度条
        public void Set_Progress(float progress) {
            // 这块是控制进度条变化的
            slider_Waves.value = progress;
        }

        // 设置进度条文字
        public void Set_Text() {
            RectTransform rectTrans = slider_Waves.GetComponent<RectTransform>();
            float width = rectTrans.rect.width;
            width = width / -2;
            float x = rectTrans.rect.width / waves;
            float y = rectTrans.rect.height / 2;

            for (int i = 1; i <= waves; i++) {
                GameObject prefab = Instantiate(text, slider_Waves.transform);

                int a = i - 1;
                Vector2 pos = new Vector2((width + a * x), y);
                prefab.transform.localPosition = pos;

                TextMeshProUGUI waveText = prefab.GetComponent<TextMeshProUGUI>();
                waveText.text = i.ToString();
            }
        }

        // 设置防守成就(基地血量)
        public void Set_AchiText(int hp, int maxHp)
        {
            Text_Achievement.text = "Achievement: " + hp + " / " + maxHp;
        }

        public void Show() {
            gameObject.SetActive(true);
        }

        public void TearDown() {
            Destroy(gameObject);
        }
    }
}