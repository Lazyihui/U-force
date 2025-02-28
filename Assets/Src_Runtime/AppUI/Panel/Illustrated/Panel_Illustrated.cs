using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UF {
    public class Panel_Illustrated : MonoBehaviour {
        [SerializeField] Panel_CardOneIllustrated oneCardPrefab;
        List<Panel_CardOneIllustrated> cards;
        Transform group;

        [SerializeField] Button btn_Close;
        public Action OnCloseClickHandle;

        [SerializeField] Image img_Sprite;
        [SerializeField] TextMeshProUGUI text_Name;
        [SerializeField] TextMeshProUGUI text_Description;

        public Action<int> OnGameIllusCardClickHandle;

        public void Ctor() {
            btn_Close.onClick.AddListener(() => OnCloseClickHandle?.Invoke());
        }

        public void Show() {
            gameObject.SetActive(true);
        }

        public void TearDown() {
            Destroy(gameObject);
        }

        public void Add(int id, Sprite img)
        {
            Panel_CardOneIllustrated card = Instantiate(oneCardPrefab, group);
            card.Ctor();
            card.Init(id, img);
            card.OnCardClickHandle += OnCardClick;

            cards.Add(card);
        }

        void OnCardClick(int typeID) {
            OnGameIllusCardClickHandle?.Invoke(typeID);
        }

        public void SetContent(Sprite sprite, string name, string description) {
            img_Sprite.sprite = sprite;
            text_Name.text = name;
            text_Description.text = description;
        }
    }
}