using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Item : MonoBehaviour {
    [SerializeField] TextMeshProUGUI text; // 显示数字的文本 
    public int value;
    private Image image;
    private Color originalColor;

    void Awake() {
        image = GetComponent<Image>();
        originalColor = image.color;
    }

    void Update() {
        if (text != null) {
            text.text = value.ToString();
        }
    }

    public void SetColor(Color color) {
        if (image != null)
            image.color = color;
    }

    public void ResetColor() {
        if (image != null)
            image.color = originalColor;
    }

    public void UpdateColor() {
        // 可以根据value设置不同颜色
        // 这里只是示例，你可以根据需要修改
        ResetColor();
    }
}