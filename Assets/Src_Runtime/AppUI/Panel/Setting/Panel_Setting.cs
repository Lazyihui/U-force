using System;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Setting : MonoBehaviour {
    [SerializeField] Button btn_Back;
    public Action OnBackClickHandle;

    [SerializeField] Button btn_Reselustion1920;
    public Action OnReselustion1920ClickHandle;

    [SerializeField] Button btn_Reselustion1600;
    public Action OnReselustion1600ClickHandle;

    [SerializeField] Button btn_Reselustion640;
    public Action OnReselustion640ClickHandle;

    public void Ctor() {
        
        btn_Back.onClick.AddListener(() => {
            if (OnBackClickHandle != null) {
                OnBackClickHandle();
            }
        });

        btn_Reselustion1920.onClick.AddListener(() => OnReselustion1920ClickHandle?.Invoke());
        btn_Reselustion1600.onClick.AddListener(() => OnReselustion1600ClickHandle?.Invoke());
        btn_Reselustion640.onClick.AddListener(() => OnReselustion640ClickHandle?.Invoke());

    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void TearDown() {
        Destroy(gameObject);
    }
}