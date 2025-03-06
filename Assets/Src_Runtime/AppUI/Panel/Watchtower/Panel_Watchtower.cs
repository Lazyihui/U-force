using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UF
{
    public class Panel_WatchTower : MonoBehaviour
    {
        [SerializeField] Panel_OneCardWatch oneCardPrefab;
        List<Panel_OneCardWatch> cards;
        [SerializeField] Transform cardGroup;

        [SerializeField] Button btn_View; // 放大镜按钮, 按了才能点击头像查看详情
        public Action OnViewClickHandle;

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
    }
}