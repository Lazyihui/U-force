using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UF.UIApplication {

    public class Panel_CardSelection : MonoBehaviour {

        // OnCard
        [SerializeField] Transform cardGroup;
        [SerializeField] Panel_OneCardSelection oneCardPrefab;
        List<Panel_OneCardSelection> cards;
        public Action<int> OnSelectionCardClickHandle;// 点击的卡片

        // CardDetails  
        [SerializeField] Panel_CardSelectionDetails cardDetailsPrefab;

        Panel_CardSelectionDetails cardDetails;

        public Action<int> OnConfirmClickHandle; //确认选择

        // SelectedCard 选择的卡片
        [SerializeField] Panel_CardSelected cardSelectedPrefab;
        Panel_CardSelected cardSelected;
        public Action<int> OnCardSelectedClickHandle;

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
                txt_ChapterName.text = "进攻战";
            } else if (chapterID == 3) {
                txt_ChapterName.text = "消除战";
            }
        }

        #region Card
        public void AddCard(int id, Sprite sprite) {
            // 实例化卡片
            Panel_OneCardSelection oneCard = Instantiate(oneCardPrefab, cardGroup);
            oneCard.Ctor();
            oneCard.Init(id, sprite);
            oneCard.OnCardClickHandle += OnCardClick;//点击卡片 绑定事件    
            cards.Add(oneCard);
        }
        public void RemoveCard(int id) {
            for (int i = 0; i < cards.Count; i++) {
                if (cards[i].GetID() == id) {
                    Destroy(cards[i].gameObject);
                    cards.RemoveAt(i);
                    Debug.Log("RemoveCard");
                    break;
                }
            }
        }

        public void Clear() {
            foreach (var card in cards) {
                Destroy(card.gameObject);
            }
            cards.Clear();
        }

        // 锁定卡片
        public void LockCard(int id) {
            foreach (var card in cards) {
                if (card.GetID() == id) {
                    card.OneCard_Lock();
                }
            }
        }

        // 解锁卡片
        public void UnLockCard(int id) {
            foreach (var card in cards) {
                if (card.GetID() == id) {
                    card.OneCard_UnLock();
                }
            }
        }

        #endregion

        #region CardSelected
        public void InitCardSelected(int slotMax) {
            // 选择的卡片初始化 都是空白格
            Panel_CardSelected selectedCard = Instantiate(cardSelectedPrefab, transform);
            selectedCard.Ctor();
            selectedCard.Init(slotMax);
            selectedCard.OnCardClickSelectedHandle += OnCardSelectedClick;//点击卡片 绑定事件
            cardSelected = selectedCard;//存下来

        }

        public void RemoveCardSelected(int typeID) {
            // 移除
            cardSelected.Remove(typeID);
            // 重新排序
            cardSelected.ForEach((ele) => {
                if (ele.GetID() != -1) {
                    ele.Init(ele.GetID(), ele.GetSprite());
                }
            });
        }

        public void TakeAll(out List<int> typeIDs) {
            cardSelected.TakeAll(out typeIDs);
        }

        public void CardSelected_SetHero(Sprite sprite) {
            cardSelected.Set_hero(sprite);
        }

        public void CardSelected_SetScience(Sprite sprite) {
            cardSelected.Set_science(sprite);
        }

        public bool CardSelected_SetProp(int typeID, Sprite sprite) {
            bool isFull = cardSelected.Set_Addprop(typeID, sprite);
            return isFull;
        }

        #endregion
        public void Set_CardType(CardType type) {
            cardType = type;
        }

        public CardType Get_CardType() {
            return cardType;
        }

        public void UpdateCardDetail(Sprite sprite, string content, int typeID) {

            if (cardDetails == null) {
                cardDetails = Instantiate(cardDetailsPrefab, transform);
                cardDetails.OnConfirmClickHandle += OnConfirmClick;
                cardDetails.Ctor();
            }
            cardDetails.Set_Detail(sprite, content, typeID);
        }

        // 锁定卡片确认键
        public void CardDetailsBtn_Lock() {
            cardDetails.CardDetailsBtn_Lock();
        }

        // 解锁卡片确认键
        public void CardDetailsBtn_UnLock() {
            cardDetails.CardDetailsBtn_UnLock();
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

        void OnConfirmClick(int typeID) {
            OnConfirmClickHandle.Invoke(typeID);
        }

        void OnCardSelectedClick(int typeID) {
            OnCardSelectedClickHandle.Invoke(typeID);
        }

    }
}