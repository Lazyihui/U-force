using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UF
{
    public class Panel_GameDefend_Basic : MonoBehaviour
    {
        //暂时先用public存
        //Waves波次
        public Slider slider_Waves;

        //Achievement成就
        public TextMeshProUGUI Text_Achievement;

        public void Ctor()
        {

        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void TearDown()
        {
            Destroy(gameObject);
        }
    }
}