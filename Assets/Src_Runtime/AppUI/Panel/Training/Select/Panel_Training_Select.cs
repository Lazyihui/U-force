using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UF {
    public class Panel_Training_Select : MonoBehaviour {
        [SerializeField] Panel_OneCardTrain oneCardPrefab;
        List<Panel_OneCardTrain> cards;
        [SerializeField] Transform cardGroup;

        [SerializeField] Panel_Training_Selected cardSelectedPrefab;
        Panel_Training_Selected cardSelected;

        [SerializeField] Button btn_Ready_Player1;
        public Action OnPlayer1ReadyClickHandle;

        [SerializeField] Button btn_Ready_Player2;
        public Action OnPlayer2ReadyClickHandle;

        [SerializeField] Button btn_Tip;
        public Action OnTipClickHandle;

        [SerializeField] Button btn_Back;
        public Action OnBackClickHandle;

        public Action<int> OnSelectionTrainCardClickHandle;

        public Action OnCloseHandle;

        Button selectedBtn;
        bool isMouseOver;

        // 按钮音效
        [SerializeField] AudioClip btnSelectedClip;
        [SerializeField] AudioClip btnConfirmClip;
        public Action<AudioClip> OnBtnClipPlayHandle;

        public void Ctor() {
            cards = new List<Panel_OneCardTrain>();

            btn_Ready_Player1.onClick.AddListener(() => OnPlayer1ReadyClickHandle.Invoke());
            btn_Ready_Player2.onClick.AddListener(() => OnPlayer2ReadyClickHandle.Invoke());
            btn_Tip.onClick.AddListener(() => OnTipClickHandle.Invoke());
            btn_Back.onClick.AddListener(() => OnBackClickHandle.Invoke());

            btn_Ready_Player2.interactable = false;
        }

        public void Show() {
            gameObject.SetActive(true);
        }

        public void TearDown() {
            Destroy(gameObject);
        }

        public void Add(int id, Sprite sprite, string name) {
            Panel_OneCardTrain oneCard = Instantiate(oneCardPrefab, cardGroup);
            oneCard.Ctor();
            oneCard.Init(id, sprite, name);
            oneCard.OnCardClickHandle += OnCardClick;

            cards.Add(oneCard);
        }

        void OnCardClick(int typeID) {
            OnSelectionTrainCardClickHandle.Invoke(typeID);
        }

        public void SetButton() {
            btn_Ready_Player1.interactable = false;
            btn_Ready_Player2.interactable = true;
        }

        // Selected
        public void Init() {
            Panel_Training_Selected selectedCard = Instantiate(cardSelectedPrefab, transform);
            selectedCard.Ctor();
            selectedCard.Init();
            cardSelected = selectedCard;
        }

        public void SetPlayer(int typeID, Sprite sprite, string description, string weaponSpeed, string damage, string attackSpeed) {
            cardSelected.SetPlayer(typeID, sprite, description, weaponSpeed, damage, attackSpeed);
        }

        public void AddPlayer() {
            cardSelected.AddPlayer();
        }

        public void TakeAll(out List<int> typeIDs) {
            cardSelected.TakeAll(out typeIDs);
        }

        public void Tick(Canvas canvas, Vector2 moveAxis, bool isConfirm) {
            if (gameObject.activeSelf == false) { return; }

            if (selectedBtn == null) {
                var newBtn = cards[0].GetComponent<Button>();
                SetSelectedButton(newBtn);
                return;
            }

            // 确认
            if (isConfirm) {
                ConfirmSelectedButton();
                return;
            }

            // 鼠标悬停
            Button btn = GetMouseOverButton(canvas.gameObject);

            if (btn != null) {
                SetSelectedButton_Mouse(btn);
            }

            if (isMouseOver) {
                return;
            }

            // 选中按钮检测
            if (moveAxis.x == 0) { return; }

            Selectable selectable = selectedBtn.FindSelectable(moveAxis);
            Button newSelectedBtn = selectable as Button;

            var newOneCard = newSelectedBtn.GetComponent<Panel_OneCardTrain>();
            if (newOneCard == null) { return; }

            if (newSelectedBtn == null) { return; }
            SetSelectedButton(newSelectedBtn);
        }

        void SetSelectedButton(Button newButton) {
            selectedBtn = newButton;

            int len = cards.Count;
            for (int i = 0; i < len; i += 1) {
                Panel_OneCardTrain card = cards[i];
                var btn = card.GetComponent<Button>();
                btn.image.color = (selectedBtn == btn) ? Color.red : Color.white;
            }

            newButton.onClick.Invoke();
            OnBtnClipPlayHandle.Invoke(btnSelectedClip);
        }

        void SetSelectedButton_Mouse(Button newButton) {
            selectedBtn = newButton;

            int len = cards.Count;
            for (int i = 0; i < len; i += 1) {
                Panel_OneCardTrain card = cards[i];
                var btn = card.GetComponent<Button>();
                btn.image.color = (selectedBtn == btn) ? Color.red : Color.white;
            }

            OnBtnClipPlayHandle.Invoke(btnSelectedClip);
        }

        void ConfirmSelectedButton() {
            if (selectedBtn == null) { return; }

            if (btn_Ready_Player1.interactable) {
                btn_Ready_Player1.onClick.Invoke();
            } else {
                btn_Ready_Player2.onClick.Invoke();
            }

            OnBtnClipPlayHandle.Invoke(btnConfirmClip);
        }

        Button GetMouseOverButton(GameObject canvas) {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;
            GraphicRaycaster gr = canvas.GetComponent<GraphicRaycaster>();
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(pointerEventData, results);

            if (results.Count != 0) {
                var len = results.Count;
                for (int i = 0; i < len; i++) {
                    Button btn = results[i].gameObject.GetComponent<Button>();

                    int cardLen = cards.Count;
                    for (int j = 0; j < cardLen; j++) {
                        var card = cards[j].GetComponent<Button>();
                        if (card == btn) {
                            isMouseOver = true;
                            return btn;
                        }
                    }
                }
            }

            isMouseOver = false;
            return null;
        }
    }
}