using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UF {
    public class HUD_Eliminate : MonoBehaviour {
        // waves波次

        [SerializeField] Slider slider_Time;
        
        [SerializeField] TextMeshProUGUI Text_Number;

        public float time;
        public GameObject text;

        public void Ctor() {
            
        }

        public void Set_Time(float time) {
            this.time = time;
        }

        public void Set_Progress(float progress) {
            slider_Time.value = progress;
        }

        public void Set_Text() {
            RectTransform rectTrans = slider_Time.GetComponent<RectTransform>();
            float width = rectTrans.rect.width;
            float x = width / 2;
            float y = rectTrans.rect.height / 2;
            width = -width / 2;

            GameObject time0 = Instantiate(text, slider_Time.transform);
            GameObject timeMiddle = Instantiate(text, slider_Time.transform);
            GameObject timeMax = Instantiate(text, slider_Time.transform);

            Vector2 pos0 = new Vector2(width, y);
            Vector2 posMiddle = new Vector2(width + x, y);
            Vector2 posMax = new Vector2(width + 2 * x, y);

            time0.transform.localPosition = pos0;
            timeMiddle.transform.localPosition = posMiddle;
            timeMax.transform.localPosition = posMax;

            TextMeshProUGUI timeText0 = time0.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI timeTextMiddle = timeMiddle.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI timeTextMax = timeMax.GetComponent<TextMeshProUGUI>();

            timeText0.text = "0";
            timeTextMiddle.text = (time / 2).ToString();
            timeTextMax.text = time.ToString() + "s";
        }

        public void Set_NumberText(int number)
        {
            Text_Number.text = "能量棒数量: " + number  + " / 4";
        }

        public void Show() {
            gameObject.SetActive(true);
        }

        public void TearDown() {
            Destroy(gameObject);
        }
    }
}