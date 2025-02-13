using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UF.UIApplication {

    public class Panel_CardSelection : MonoBehaviour {

        // OnCard
        [SerializeField] Panel_OneCardSelection oneCardPrefab;
        List<Panel_OneCardSelection> cards;
        [SerializeField] Transform cardGroup;
        // 点击的卡片
        public Action<int> OnSelectionCardClickHandle;

        // CardDetails  
        [SerializeField] Panel_CardSelectionDetails cardDetailsPrefab;
        Panel_CardSelectionDetails cardDetails;
        public Action OnConfirmClickHandle; //确认选择

        public Action OnSelectBackClickHandle;
        [SerializeField] Button btn_back;//返回
        public Action OnSelectEnterClickHandle;
        [SerializeField] Button btn_EnterGame;//进入游戏

        public Action OnSelectHeroClickHandle;
        [SerializeField] Button btn_SelectHero;
        public Action OnSelectAgrClickHandle;
        [SerializeField] Button btn_SelectAgr;//选择农业
        public Action OnSelectIndClickHandle;
        [SerializeField] Button btn_SelectInd;//选择工业
        public Action OnSelectSciClickHandle;
        [SerializeField] Button btn_SelectSci;//选择科技


        // text
        [SerializeField] TextMeshProUGUI txt_ChapterName;
        [SerializeField] TextMeshProUGUI txt_StageName;

        CardType cardType;

        public void Ctor() {
            cards = new List<Panel_OneCardSelection>();

            btn_back.onClick.AddListener(() => OnSelectBackClickHandle.Invoke());
            btn_EnterGame.onClick.AddListener(() => OnSelectEnterClickHandle.Invoke());
            btn_SelectHero.onClick.AddListener(() => OnSelectHeroClickHandle.Invoke());
            btn_SelectAgr.onClick.AddListener(() => OnSelectAgrClickHandle.Invoke());
            btn_SelectInd.onClick.AddListener(() => OnSelectIndClickHandle.Invoke());
            btn_SelectSci.onClick.AddListener(() => OnSelectSciClickHandle.Invoke());

        }

        public void Set_Text(int stageID, int chapterID) {

            txt_StageName.text = stageID.ToString() + "战";

            if (chapterID == 1) {
                // TODO:还没定暂时这样写
                txt_ChapterName.text = "防守战";
            } else if (chapterID == 2) {
                txt_ChapterName.text = "Industry";
            } else if (chapterID == 3) {
                txt_ChapterName.text = "Science";
            }
        }

        public void Add(int id, Sprite sprite) {

            // 实例化卡片
            Panel_OneCardSelection oneCard = Instantiate(oneCardPrefab, cardGroup);
            oneCard.Ctor();
            oneCard.Init(id, sprite);
            oneCard.OnCardClickHandle += OnCardClick;//点击卡片 绑定事件

            cards.Add(oneCard);
        }

        public void Clear() {
            foreach (var card in cards) {
                Destroy(card.gameObject);
            }
            cards.Clear();
        }

        public void Set_CardType(CardType type) {
            cardType = type;
        }

        public CardType Get_CardType() {
            return cardType;
        }

        public void UpdateCardDetail(Sprite sprite, string content) {

            if (cardDetails == null) {
                cardDetails = Instantiate(cardDetailsPrefab, transform);
                cardDetails.OnConfirmClickHandle += OnConfirmClick;
                cardDetails.Ctor();
            }
            cardDetails.Set_Detail(sprite, content);
        }


        public void Init() {

        }

        public void Show() {
            gameObject.SetActive(true);
        }

        public void TearDown() {
            Destroy(gameObject);
        }

        void OnCardClick(int stageID) {
            OnSelectionCardClickHandle.Invoke(stageID);
        }

        void OnConfirmClick() {
            OnConfirmClickHandle.Invoke();
        }

    }
}