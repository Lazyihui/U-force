using System;
using UnityEngine;
using UnityEngine.UI;

namespace UF
{
    public class Panel_Training_Button : MonoBehaviour
    {
        [SerializeField] Button btn_Pause;
        public Action OnPauseClickHandle;

        public void Ctor()
        {
            btn_Pause.onClick.AddListener(() => {
                if (OnPauseClickHandle != null) {
                    OnPauseClickHandle.Invoke();
                }
            });
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