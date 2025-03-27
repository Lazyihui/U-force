using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UF
{
    public class Panel_Training_Select : MonoBehaviour
    {
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

        public void Ctor()
        {
            cards = new List<Panel_OneCardTrain>();

            btn_Ready_Player1.onClick.AddListener(() => OnPlayer1ReadyClickHandle.Invoke());
            btn_Ready_Player2.onClick.AddListener(() => OnPlayer2ReadyClickHandle.Invoke());
            btn_Tip.onClick.AddListener(() => OnTipClickHandle.Invoke());
            btn_Back.onClick.AddListener(() => OnBackClickHandle.Invoke());

            btn_Ready_Player2.interactable = false;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void TearDown()
        {
            Destroy(gameObject);
        }

        public void Add(int id, Sprite sprite, string name)
        {
            Panel_OneCardTrain oneCard = Instantiate(oneCardPrefab, cardGroup);
            oneCard.Ctor();
            oneCard.Init(id, sprite, name);
            oneCard.OnCardClickHandle += OnCardClick;

            cards.Add(oneCard);
        }

        void OnCardClick(int typeID)
        {
            OnSelectionTrainCardClickHandle.Invoke(typeID);
        }

        public void SetButton()
        {
            btn_Ready_Player1.interactable = false;
            btn_Ready_Player2.interactable = true;
        }

        // Selected
        public void Init()
        {
            Panel_Training_Selected selectedCard = Instantiate(cardSelectedPrefab, transform);
            selectedCard.Ctor();
            selectedCard.Init();
            cardSelected = selectedCard;
        }

        public void SetPlayer(int typeID, Sprite sprite, string description, string weaponSpeed, string damage, string attackSpeed)
        {
            cardSelected.SetPlayer(typeID, sprite, description, weaponSpeed, damage, attackSpeed);
        }

        public void AddPlayer()
        {
            cardSelected.AddPlayer();
        }

        public void TakeAll(out List<int> typeIDs)
        {
            cardSelected.TakeAll(out typeIDs);
        }
    }
}