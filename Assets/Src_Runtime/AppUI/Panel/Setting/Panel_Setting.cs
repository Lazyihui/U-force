using System;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Setting : MonoBehaviour {
    [SerializeField] Button btn_Back;
    public Action OnBackClickHandle;

    public void Ctor() {
        btn_Back.onClick.AddListener(() => {
            if (OnBackClickHandle != null) {
                OnBackClickHandle();
            }
        });
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void TearDown() {
        Destroy(gameObject);
    }
}