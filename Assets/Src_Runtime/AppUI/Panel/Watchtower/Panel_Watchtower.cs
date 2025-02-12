using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UF
{
    public class Panel_Watchtower : MonoBehaviour
    {
        [SerializeField] Panel_OneCardWatch oneCardPrefab;
        List<Panel_OneCardWatch> cards;
        [SerializeField] Transform cardGroup;

        [SerializeField] Button btn_View; // 放大镜按钮, 按了才能点击头像查看详情
        public Action OnViewClickHandle;

        public Action<int> OnSelectionWatchCardClickHandle;

        [SerializeField] Texture2D icon;

        public void Ctor()
        {
            cards = new List<Panel_OneCardWatch>();
            
            btn_View.onClick.AddListener(() => OnViewClickHandle.Invoke());
        }

        public void Add(int id, Sprite sprite) {
            Panel_OneCardWatch oneCard = Instantiate(oneCardPrefab, cardGroup);
            oneCard.Ctor();
            oneCard.Init(id, sprite);
            oneCard.OnCardClickHandle += OnCardClick;//点击卡片 绑定事件

            cards.Add(oneCard);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void TearDown()
        {
            Destroy(gameObject);
        }

        void OnCardClick(int typeID)
        {
            OnSelectionWatchCardClickHandle.Invoke(typeID);
        }

        public void Set_Cursor()
        {
            Cursor.SetCursor(icon, Vector2.zero, CursorMode.Auto);
        }

        public void Clear_Cursor()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
}