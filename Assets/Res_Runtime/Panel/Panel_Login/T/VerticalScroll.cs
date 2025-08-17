using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VerticalScroll : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    [SerializeField] List<Item> items; // 4 个元素：[1, 2, 3, 1]
    [SerializeField] RectTransform targetArea; // 目标区域
    [SerializeField] float itemHeight = 200f; // 100 (高度) + 100 (间隔)

    Vector3 lastPosition;
    float offset;
    List<Vector3> originalPositions;
    Item currentTargetItem;
    Item lastSnappedItem;
    int lastSnappedIndex;
    Vector3 lastSnappedPosition;

    void Start() {
        if (items == null || items.Count < 4) {
            Debug.LogError("需要 4 个 Item：[1, 2, 3, 1]");
            return;
        }

        // 初始化默认吸附第二个item(值为2)
        lastSnappedItem = items[1];
        lastSnappedIndex = 1;
        lastSnappedPosition = items[1].transform.localPosition;
        SnapToTarget(lastSnappedItem);
        DebugItemsOrder("初始状态");
    }

    public void OnBeginDrag(PointerEventData eventData) {
        lastPosition = eventData.position;
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

        HandleEdgeItems();
        CheckTargetArea();
    }

    void HandleEdgeItems() {
        // 上滑处理
        if (offset > 0) {
            if (items[0].transform.localPosition.y >= itemHeight) {
                MoveFirstToLast();
                DebugItemsOrder("上滑调整后");
            }
        } 
        // 下滑处理
        else {
            if (items[3].transform.localPosition.y <= -itemHeight) {
                MoveLastToFirst();
                DebugItemsOrder("下滑调整后");
            }
        }
    }

    void MoveFirstToLast() {
        Item firstItem = items[0];
        firstItem.transform.localPosition = items[3].transform.localPosition - new Vector3(0, itemHeight-100, 0);
        
        // 更新值：1→2, 2→3, 3→1
        firstItem.value = (firstItem.value % 3) + 1;
        firstItem.UpdateColor();

        items.RemoveAt(0);
        items.Add(firstItem);
        
        // 更新上次吸附的索引
        if (lastSnappedIndex > 0) lastSnappedIndex--;
        else lastSnappedIndex = 3;
    }

    void MoveLastToFirst() {
        Item lastItem = items[3];
        lastItem.transform.localPosition = items[0].transform.localPosition + new Vector3(0, itemHeight-100, 0);
        
        // 更新值：1→3, 2→1, 3→2
        lastItem.value = (lastItem.value + 1) % 3 + 1;
        lastItem.UpdateColor();

        items.RemoveAt(3);
        items.Insert(0, lastItem);
        
        // 更新上次吸附的索引
        if (lastSnappedIndex < 3) lastSnappedIndex++;
        else lastSnappedIndex = 0;
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (currentTargetItem != null) {
            SnapToTarget(currentTargetItem);
            lastSnappedItem = currentTargetItem;
            lastSnappedIndex = items.IndexOf(currentTargetItem);
            DebugItemsOrder("吸附到新item后");
        } else {
            RestoreLastSnappedPosition();
            DebugItemsOrder("恢复到上一次吸附状态");
        }
    }

    void RestoreLastSnappedPosition() {
        // 计算需要移动的偏移量
        float offsetY = lastSnappedPosition.y - items[lastSnappedIndex].transform.localPosition.y;
        
        // 移动所有item
        foreach (var item in items) {
            item.transform.localPosition += new Vector3(0, offsetY, 0);
        }
        
        // 确保所有item重置颜色
        foreach (var item in items) {
            item.ResetColor();
        }
        
        // 高亮上次吸附的item
        items[lastSnappedIndex].SetColor(Color.yellow);
    }

    void CheckTargetArea() {
        foreach (var item in items) {
            item.ResetColor();
        }

        currentTargetItem = null;

        foreach (var item in items) {
            if (IsInTargetArea(item.transform.position)) {
                currentTargetItem = item;
                item.SetColor(Color.yellow);
                break;
            }
        }
    }

    bool IsInTargetArea(Vector3 position) {
        if (targetArea == null) return false;

        Vector3[] corners = new Vector3[4];
        targetArea.GetWorldCorners(corners);

        return position.y >= corners[0].y && position.y <= corners[1].y;
    }

    void SnapToTarget(Item targetItem) {
        if (targetArea == null) return;

        float targetY = targetArea.position.y;
        float currentY = targetItem.transform.position.y;
        float offsetY = targetY - currentY;

        foreach (var item in items) {
            item.transform.position += new Vector3(0, offsetY, 0);
        }
        
        // 更新上次吸附的位置
        lastSnappedPosition = targetItem.transform.localPosition;
        
        foreach (var item in items) {
            item.ResetColor();
        }
        targetItem.SetColor(Color.yellow);
    }

    void DebugItemsOrder(string context) {
        string log = context + ": ";
        for (int i = 0; i < items.Count; i++) {
            log += $"[{i}]:{items[i].value} ";
            if (i == lastSnappedIndex) {
                log += "(上次吸附) ";
            }
            log += $"PosY:{items[i].transform.localPosition.y.ToString("F0")} ";
        }
    }
}