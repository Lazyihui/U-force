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

        [SerializeField] Image img_Player1;
        [SerializeField] Image img_Player2;

        [SerializeField] TextMeshProUGUI text_Description_Player1; // 介绍
        [SerializeField] TextMeshProUGUI text_WeaponSpeed_Player1; // 武器速度
        [SerializeField] TextMeshProUGUI text_Damage_Player1; // 伤害
        [SerializeField] TextMeshProUGUI text_AttackSpeed_Player1; // 攻击速度

        [SerializeField] TextMeshProUGUI text_Description_Player2;
        [SerializeField] TextMeshProUGUI text_WeaponSpeed_Player2;
        [SerializeField] TextMeshProUGUI text_Damage_Player2;
        [SerializeField] TextMeshProUGUI text_AttackSpeed_Player2;

        [SerializeField] Button btn_Ready_Player1;
        public Action OnPlayer1ReadyClickHandle;

        [SerializeField] Button btn_Ready_Player2;
        public Action OnPlayer2ReadyClickHandle;

        public Action<int> OnSelectionTrainCardClickHandle;

        public void Ctor()
        {
            cards = new List<Panel_OneCardTrain>();

            btn_Ready_Player1.onClick.AddListener(() => OnPlayer1ReadyClickHandle.Invoke());
            btn_Ready_Player2.onClick.AddListener(() => OnPlayer2ReadyClickHandle.Invoke());
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void TearDown()
        {
            Destroy(gameObject);
        }

        public void SetPlayer1(string description, string weaponSpeed, string damage, string attackSpeed, Sprite sprite)
        {
            text_Description_Player1.text = "Description\n" + description;
            text_WeaponSpeed_Player1.text = "WeaponSpeed " + weaponSpeed;
            text_Damage_Player1.text = "Damage " + damage;
            text_AttackSpeed_Player1.text = "AttackSpeed " + attackSpeed;
            img_Player1.sprite = sprite;
        }

        public void SetPlayer2(string description, string weaponSpeed, string damage, string attackSpeed, Sprite sprite)
        {
            text_Description_Player2.text = "Description\n" + description;
            text_WeaponSpeed_Player2.text = "WeaponSpeed " + weaponSpeed;
            text_Damage_Player2.text = "Damage " + damage;
            text_AttackSpeed_Player2.text = "AttackSpeed " + attackSpeed;
            img_Player2.sprite = sprite;
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
    }
}