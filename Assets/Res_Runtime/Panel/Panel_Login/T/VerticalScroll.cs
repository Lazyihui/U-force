using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VerticalScroll : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    [SerializeField] List<Item> items; // 4 个元素：[1, 2, 3, 1]
    [SerializeField] RectTransform targetArea; // 目标区域
    [SerializeField] float itemHeight = 300f; // 100 (高度) + 100 (间隔)

    Vector3 lastPosition;
    float offset;
    List<Vector3> originalPositions; // 用于存储拖动前的位置
    Item currentTargetItem; // 当前在目标区域的item

    void Start() {
        if (items == null || items.Count < 4) {
            Debug.LogError("需要 4 个 Item：[1, 2, 3, 1]");
            return;
        }

        // 初始化时检查是否有item在目标区域
        CheckTargetArea();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        lastPosition = eventData.position;
        // 存储拖动前的位置
        originalPositions = new List<Vector3>();
        foreach (var item in items) {
            originalPositions.Add(item.transform.localPosition);
        }
    }

    public void OnDrag(PointerEventData eventData) {
        offset = eventData.position.y - lastPosition.y;
        lastPosition = eventData.position;

        // 移动所有 Item
        for (int i = 0; i < items.Count; i++) {
            items[i].transform.localPosition += new Vector3(0, offset, 0);
        }

        if (offset > 0) {
            // 上滑：检测第一个 Item 是否超出视野
            if (items[0].transform.localPosition.y >= itemHeight) {
                Item firstItem = items[0];
                firstItem.transform.localPosition = items[3].transform.localPosition - new Vector3(0, itemHeight - 50, 0);

                // 更新值：1→2, 2→3, 3→1
                firstItem.value = (firstItem.value % 3) + 1;
                firstItem.UpdateColor(); // 更新颜色

                // 调整列表顺序
                items.RemoveAt(0);
                items.Add(firstItem);
            }
        } else {
            // 下滑：检测最后一个 Item 是否超出视野
            if (items[3].transform.localPosition.y <= -itemHeight) {
                Item lastItem = items[3];
                lastItem.transform.localPosition = items[0].transform.localPosition + new Vector3(0, itemHeight - 50, 0);

                // 更新值：1→3, 2→1, 3→2
                lastItem.value = (lastItem.value + 1) % 3 + 1;
                lastItem.UpdateColor(); // 更新颜色

                // 调整列表顺序
                items.RemoveAt(3);
                items.Insert(0, lastItem);
            }
        }

        // 拖动时实时检查目标区域
        CheckTargetArea();
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (currentTargetItem != null) {
            // 如果有item在目标区域，将其固定在中心
            SnapToTarget(currentTargetItem);
        } else {
            // 如果没有item在目标区域，恢复到拖动前状态
            ResetToOriginalPositions();
        }
    }

    // 检查是否有item在目标区域内
    void CheckTargetArea() {
        // 先重置所有item颜色
        foreach (var item in items) {
            item.ResetColor();
        }

        currentTargetItem = null;

        // 检查每个item是否在目标区域内
        foreach (var item in items) {
            if (IsInTargetArea(item.transform.position)) {
                currentTargetItem = item;
                item.SetColor(Color.yellow); // 设置为黄色
                break; // 只处理第一个找到的item
            }
        }
    }

    // 判断位置是否在目标区域内
    bool IsInTargetArea(Vector3 position) {
        if (targetArea == null) return false;

        Vector3[] corners = new Vector3[4];
        targetArea.GetWorldCorners(corners);

        // 简单的矩形区域检测
        return position.y >= corners[0].y && position.y <= corners[1].y;
    }

    // 将item对齐到目标区域中心
    void SnapToTarget(Item targetItem) {
        if (targetArea == null) return;

        // 计算需要移动的距离
        float targetY = targetArea.position.y;
        float currentY = targetItem.transform.position.y;
        float offsetY = targetY - currentY;

        // 移动所有item
        foreach (var item in items) {
            item.transform.position += new Vector3(0, offsetY, 0);
        }
    }

    // 恢复到拖动前的位置
    void ResetToOriginalPositions() {
        if (originalPositions == null || originalPositions.Count != items.Count) return;

        for (int i = 0; i < items.Count; i++) {
            items[i].transform.localPosition = originalPositions[i];
        }
    }
}