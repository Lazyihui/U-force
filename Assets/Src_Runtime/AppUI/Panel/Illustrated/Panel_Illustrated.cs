using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UF {
    public class Panel_Illustrated : MonoBehaviour {
        [SerializeField] Panel_CardOneIllustrated oneCardPrefab;
        List<Panel_CardOneIllustrated> cards;
        [SerializeField] Transform group;

        [SerializeField] Button btn_Close;
        public Action OnCloseClickHandle;

        [SerializeField] Image img_Sprite;
        [SerializeField] TextMeshProUGUI text_Name;
        [SerializeField] TextMeshProUGUI text_Description;

        public Action<int> OnGameIllusCardClickHandle;
        public Action<int> OnGameIllusStuffCardClickHandle;

        public void Ctor() {
            cards = new List<Panel_CardOneIllustrated>();

            btn_Close.onClick.AddListener(() => {
                if (OnCloseClickHandle != null) {
                    OnCloseClickHandle.Invoke();
                }
            });
        }

        public void Show() {
            gameObject.SetActive(true);
        }

        public void TearDown() {
            Destroy(gameObject);
        }

        public void Add(int id, Sprite img, bool isRole) {
            Panel_CardOneIllustrated card = Instantiate(oneCardPrefab, group);
            card.Ctor();
            card.Init(id, img);
            card.isRole = isRole;

            if (isRole) {
                card.OnCardClickHandle += OnCardClick;
            } else {
                card.OnCardClickHandle += OnStuffCardClick;
            }

            cards.Add(card);
        }

        void OnCardClick(int typeID) {
            OnGameIllusCardClickHandle?.Invoke(typeID);
            SetButtonInter();
        }

        void OnStuffCardClick(int typeID) {
            OnGameIllusStuffCardClickHandle?.Invoke(typeID);
            SetButtonInter();
        }

        public void SetContent(Sprite sprite, string name, string description) {
            img_Sprite.sprite = sprite;
            text_Name.text = name;
            text_Description.text = description;
        }

        public void SetButtonInter() {
            int len = cards.Count;

            for (int i = 0; i < len; i++) {
                var card = cards[i];
                card.SetInterTrue();
            }
        }

        public bool TryGet(int typeID) {
            int len = cards.Count;

            for (int i = 0; i < len; i++) {
                var card = cards[i];

                if (card.id == typeID) {
                    return true;
                }
            }

            return false;
        }
    }
}