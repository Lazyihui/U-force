using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UF {
    public class HUD_Progressbar : MonoBehaviour {
        // waves波次

        [SerializeField] Slider slider_Waves;

        public int waves;

        public void Ctor() {
        }

        public void Set_Waves(int waves) {
            this.waves = waves;
        }

        public void Show() {
            gameObject.SetActive(true);
        }

        public void TearDown() {
            Destroy(gameObject);
        }
    }
}