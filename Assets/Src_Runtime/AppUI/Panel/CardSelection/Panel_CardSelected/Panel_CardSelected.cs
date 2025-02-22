using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UF {
    // 选择的卡槽
    public class Panel_CardSelected : MonoBehaviour {
        [SerializeField] Transform cardGroup;
        [SerializeField] Image img_hero;
        [SerializeField] Image img_science;

        [SerializeField] Panel_CardSelectedElement elePrefab;
        List<Panel_CardSelectedElement> eles;
        public Action<int> OnCardClickSelectedHandle;

        public void Ctor() {
            eles = new List<Panel_CardSelectedElement>();
        }

        // 遍历物品
        public void TakeAll(out List<int> typeIDs) {
            typeIDs = new List<int>();
            for (int i = 0; i < eles.Count; i += 1) {
                Panel_CardSelectedElement item = eles[i];
                if (item != null) {
                    typeIDs.Add(item.GetID());
                }
            }
        }

        public void Init(int slotMax) {
            for (int i = 0; i < slotMax; i += 1) {
                Panel_CardSelectedElement ele = Instantiate(elePrefab, cardGroup);
                ele.Ctor();
                ele.Init(-1, null);
                ele.OnCardClickHandle = OnCardSelectedClick;
                eles.Add(ele);
            }
        }
        public bool Set_Addprop(int typeID, Sprite sprite) {
            for (int i = 0; i < eles.Count; i += 1) {
                Debug.Log(eles[i].GetID());

                var ele = eles[i];
                if (ele.GetID() == -1) {
                    ele.Init(typeID, sprite);
                    return false;
                }
            }
            return true;
        }

        public void Remove(int typeID) {
            // 移除
            for (int i = 0; i < eles.Count; i += 1) {
                var ele = eles[i];
                if (ele.GetID() == typeID) {
                    ele.Init(-1, null);
                    break;
                }
            }
        }

        // 遍历物品
        public void ForEach(Action<Panel_CardSelectedElement> callback) {
            for (int i = 0; i < eles.Count; i += 1) {
                Panel_CardSelectedElement item = eles[i];
                if (item != null) {
                    callback.Invoke(item);
                }
            }
        }

        public void Set_hero(Sprite sprite) {
            img_hero.sprite = sprite;
        }

        public void Set_science(Sprite sprite) {
            img_science.sprite = sprite;
        }


        void OnCardSelectedClick(int typeID) {
            if (OnCardClickSelectedHandle != null) {
                OnCardClickSelectedHandle.Invoke(typeID);
            }
        }
    }
}
