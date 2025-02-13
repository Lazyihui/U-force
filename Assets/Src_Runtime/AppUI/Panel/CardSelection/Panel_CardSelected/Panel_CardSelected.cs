using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UF {
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

        public void Add(int slotMax) {
            for (int i = 0; i < slotMax; i += 1) {
                Panel_CardSelectedElement ele = Instantiate(elePrefab, cardGroup);
                ele.Ctor();
                ele.Init(i, null);
                ele.OnCardClickHandle = OnCardSelectedClick;
                eles.Add(ele);
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
