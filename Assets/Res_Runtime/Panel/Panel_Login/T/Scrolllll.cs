using System.Collections;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.EventSystems;

public class Vetical : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    [SerializeField] public RectTransform content;
    [SerializeField] public List<Item> items;
    Vector3 lastPosition;
    float offset;
    float index0y;

    void Awake() {
        index0y = items[1].transform.localPosition.y;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        lastPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData) {
        offset = eventData.position.y - lastPosition.y;
        lastPosition = eventData.position;

        for (int i = 0; i < items.Count; i++) {
            var item = items[i];
            item.transform.localPosition += new Vector3(0, offset, 0);
        }


        if (offset > 0) {
            // 上滑
        //     if (items[0].transform.localPosition.y >= index0y) {
        //         // 把最后一个出来
        //         // 添加一个到末尾
        //         items[items.Count - 1].transform.localPosition = items[items.Count - 2].transform.localPosition + new Vector3(0, sizey, 0);
        //     }
        //     if (items[0].transform.localPosition.y >= index0y + sizey) {
        //         // 讲第一个放到末尾
        //         //重置位置
        //         Item temp = items[0];
        //         temp.transform.localPosition = items[items.Count - 1].transform.localPosition + new Vector3(0, sizey, 0);

        //         for (int i = 1; i < items.Count; i++) {
        //             items[i - 1] = items[i];
        //         }

        //         items[items.Count - 1] = temp;
        //     }
        } else {
        //     // 下滑 
        //     if (items[0].transform.localPosition.y <= index0y) {
        //         // 把最后一个出来
        //         // 添加一个到开始
        //         items[items.Count - 1].transform.localPosition = items[0].transform.localPosition - new Vector3(0, sizey, 0);
        //     }
        //     if (items[0].transform.localPosition.y <= index0y + sizey) {
        //         // 讲第一个放到开始
        //         //重置位置
        //         Item temp = items[items.Count - 1];

        //         temp.transform.localPosition = items[0].transform.localPosition - new Vector3(0, sizey, 0);
        //         for (int i = 0; i < items.Count - 1; i++) {
        //             items[i] = items[i - 1];
        //         }
        //         items[0] = temp;
        //     }


        }
    }

    public void OnEndDrag(PointerEventData eventData) {
    }
}

