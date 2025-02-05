using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UF {
    public class HUD_Progressbar : MonoBehaviour {
        // waves波次

        [SerializeField] Slider slider_Waves;

        public int waves;
        public float progress;//有几个波次 用分数表示

        public void Ctor() {
        }

        // 设置波次
        public void Set_Waves(int waves) {
            this.waves = waves;
        }

        // 进度条
        public void Set_Progress(float progress) {
            slider_Waves.value = progress;
        }

        public void Show() {
            gameObject.SetActive(true);
        }

        public void TearDown() {
            Destroy(gameObject);
        }
    }
}