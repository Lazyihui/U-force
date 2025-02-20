using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UF
{
    public class Panel_Training_Selected : MonoBehaviour
    {
        [SerializeField] Transform cardGroup;

        [SerializeField] Panel_Training_SelectedElement elePrefab;
        List<Panel_Training_SelectedElement> eles;

        public void Ctor()
        {
            eles = new List<Panel_Training_SelectedElement>();
        }

        // public void SetPlayer1(string description, string weaponSpeed, string damage, string attackSpeed, Sprite sprite)
        // {
        //     text_Description_Player1.text = "Description\n" + description;
        //     text_WeaponSpeed_Player1.text = "WeaponSpeed " + weaponSpeed;
        //     text_Damage_Player1.text = "Damage " + damage;
        //     text_AttackSpeed_Player1.text = "AttackSpeed " + attackSpeed;
        //     img_Player1.sprite = sprite;
        // }

        // public void SetPlayer2(string description, string weaponSpeed, string damage, string attackSpeed, Sprite sprite)
        // {
        //     text_Description_Player2.text = "Description\n" + description;
        //     text_WeaponSpeed_Player2.text = "WeaponSpeed " + weaponSpeed;
        //     text_Damage_Player2.text = "Damage " + damage;
        //     text_AttackSpeed_Player2.text = "AttackSpeed " + attackSpeed;
        //     img_Player2.sprite = sprite;
        // }

        // Selected
        public void TakeAll(out List<int> typeIDs)
        {
            typeIDs = new List<int>();
            for (int i = 0; i < eles.Count; i += 1)
            {
                Panel_Training_SelectedElement item = eles[i];
                if (item != null)
                {
                    typeIDs.Add(item.GetID());
                    Debug.Log("TakeAll" + item.GetID());
                }
            }
        }

        public void Init()
        {
            for (int i = 0; i < 2; i += 1)
            {
                Panel_Training_SelectedElement ele = Instantiate(elePrefab, cardGroup);

                if (i == 1)
                {
                    ele.SetScale();
                }
                
                ele.Ctor();
                ele.Init(-1, null, null, null, null, null);
                eles.Add(ele);
            }
        }

        public void SetPlayer(int typeID, Sprite sprite, string description, string weaponSpeed, string damage, string attackSpeed)
        {
            for (int i = 0; i < eles.Count; i += 1)
            {
                var ele = eles[i];
                if (ele.GetID() == -1)
                {
                    ele.Init(typeID, sprite, description, weaponSpeed, damage, attackSpeed);
                    break;
                }
            }
        }

        public void AddPlayer()
        {
            for (int i = 0; i < eles.Count; i += 1)
            {
                var ele = eles[i];
                if (ele.GetID() == -1)
                {
                    ele.SetID();
                    break;
                }
            }
        }
    }
}