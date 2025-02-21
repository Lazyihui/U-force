using System;
using UnityEngine;
using TMPro;

namespace UF.UIApplication {

    public class Panel_Notice : MonoBehaviour {

        [SerializeField] TextMeshProUGUI textContent;

        public void Ctor() {

        }

        public void Inti(string content) {
            textContent.text = "";
            textContent.text = content;
        }

        public void Show() {
            gameObject.SetActive(true);
        }

        public void TearDown() {
            Destroy(gameObject);
        }
    }
}