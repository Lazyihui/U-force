using System;
using UnityEngine;
using UnityEngine.UI;

namespace UF
{
    public class Panel_Training_Tip : MonoBehaviour
    {
        [SerializeField] Button btn_Close;
        public Action OnCloseClickHandle;

        public void Ctor()
        {
            btn_Close.onClick.AddListener(() => OnCloseClickHandle.Invoke());
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