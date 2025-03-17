using System;
using UnityEngine;

public class Panel_Setting : MonoBehaviour {
    // TODO: 设置
    public void Ctor() {
        
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void TearDown() {
        Destroy(gameObject);
    }
}