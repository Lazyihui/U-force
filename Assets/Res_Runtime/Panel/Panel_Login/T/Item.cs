using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Item : MonoBehaviour {
    [SerializeField] TextMeshProUGUI textMeshProUGUI;
    public int value;
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        textMeshProUGUI.text = value.ToString();
    }
}
