using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UF {
    public class HUD_Training : MonoBehaviour {
        [SerializeField] Slider slider_Player1;
        [SerializeField] Slider slider_Player2;
        [SerializeField] TextMeshProUGUI text_Time;

        public void Ctor() {

        }

        public void Show() {
            gameObject.SetActive(true);
        }

        public void TearDown() {
            Destroy(gameObject);
        }

        public void SetPlayer1Hp(int hp, int maxHp) {
            slider_Player1.value = (float)hp / maxHp;
        }

        public void SetPlayer2Hp(int hp, int maxHp) {
            slider_Player2.value = (float)hp / maxHp;
        }

        public void SetTime(float time) {
            time = (int)time;
            text_Time.text = time.ToString() + "s";
        }
    }
}