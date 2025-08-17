using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Item : MonoBehaviour {
    [SerializeField] TextMeshProUGUI textMeshProUGUI;
    Image image;
    public int value;

    void Start() {
        // 获取Image组件
        image = GetComponent<Image>();
        // 初始化时根据value设置颜色
        UpdateColor();
    }

    void Update() {
        textMeshProUGUI.text = value.ToString();
    }

    // 根据value值更新颜色
    public void UpdateColor() {
        if (image == null) return;
        image.color = Color.white;
    }

    // 重置为默认颜色（根据value）
    public void ResetColor() {
        UpdateColor();
    }

    // 设置特定颜色
    public void SetColor(Color color) {
        if (image != null) {
            image.color = color;
        }
    }
}