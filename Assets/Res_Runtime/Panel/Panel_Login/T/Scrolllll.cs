using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VerticalScroll : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    [SerializeField] private List<Item> items; // 4 个元素：[1, 2, 3, 1]
    private Vector3 lastPosition;
    private float offset;
    private float itemHeight = 300f; // 100 (高度) + 100 (间隔)


    void Awake() {
        if (items == null || items.Count < 4) {
            Debug.LogError("需要 4 个 Item：[1, 2, 3, 1]");
            return;
        }
    }

    public void OnBeginDrag(PointerEventData eventData) {
        lastPosition = eventData.position;
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
                firstItem.transform.localPosition = items[3].transform.localPosition - new Vector3(0, itemHeight-50, 0);
                
                // 更新值：1→2, 2→3, 3→1
                firstItem.value = (firstItem.value % 3) + 1;

                // 调整列表顺序
                items.RemoveAt(0);
                items.Add(firstItem);
            }
        } else {
            // 下滑：检测最后一个 Item 是否超出视野
            if (items[3].transform.localPosition.y <= -itemHeight) {
                Item lastItem = items[3];
                lastItem.transform.localPosition = items[0].transform.localPosition + new Vector3(0, itemHeight-50, 0);
                
                // 更新值：1→3, 2→1, 3→2
                lastItem.value = (lastItem.value + 1) % 3 + 1;

                // 调整列表顺序
                items.RemoveAt(3);
                items.Insert(0, lastItem);
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        // 可以添加惯性滑动逻辑
    }
}