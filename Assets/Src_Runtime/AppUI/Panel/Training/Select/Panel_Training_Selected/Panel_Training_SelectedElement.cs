using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UF
{
    public class Panel_Training_SelectedElement : MonoBehaviour
    {
        int typeID;
        int id;

        [SerializeField] Image img_Player;
        [SerializeField] TextMeshProUGUI text_Description_Player; // 介绍
        [SerializeField] TextMeshProUGUI text_WeaponSpeed_Player; // 武器速度
        [SerializeField] TextMeshProUGUI text_Damage_Player; // 伤害
        [SerializeField] TextMeshProUGUI text_AttackSpeed_Player; // 攻击速度

        [SerializeField] GameObject texts;


        public void Ctor()
        {
            typeID = -1;
        }

        public void Init(int id, Sprite sprite, string description, string weaponSpeed, string damage, string attackSpeed)
        {
            this.id = id;
            this.img_Player.sprite = sprite;
            this.text_Description_Player.text = "Description\n" + description;
            this.text_WeaponSpeed_Player.text = "WeaponSpeed " + weaponSpeed;
            this.text_Damage_Player.text = "Damage " + damage;
            this.text_AttackSpeed_Player.text = "AttackSpeed " + attackSpeed;
        }

        public void SetID()
        {
            typeID = id;
        }

        public int GetID()
        {
            return typeID;
        }

        public void SetScale()
        {
            transform.localScale = new Vector3(-1, 1, 1);

            texts.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}