using System;
using UnityEngine;

namespace UF.UIApplication {

    public class Panel_Notice : MonoBehaviour {

        public void Ctor() {

        }

        public void Show() {
            gameObject.SetActive(true);
        }

        public void TearDown() {
            Destroy(gameObject);
        }
    }
}