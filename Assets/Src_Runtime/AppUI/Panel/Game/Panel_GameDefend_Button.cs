using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UF {
    public class Panel_GameDefend_Button : MonoBehaviour {
        //Pause
        [SerializeField] Button btn_Pause;
        public Action OnPauseClickHandle;

        //Skill
        [SerializeField] Image img_Attack;

        [SerializeField] Image img_Tech;

        [SerializeField] GameObject weapPrefab;
        [SerializeField] Transform skillTrans;
        
        public Action OnCloseHandle;

        int slotmax;
        public void Set_SlotMax(int value) => slotmax = value;

        Image[] weapList;

        public void Ctor(int slotMax) {
            weapList = new Image[slotMax];

            //Pause
            btn_Pause.onClick.AddListener(() => {
                if (OnPauseClickHandle != null) {
                    OnPauseClickHandle.Invoke();
                }
            });

            Set_SlotMax(slotMax);
            Set_WeapButton();
        }

        public void Set_CD(SkillModel skill, float cd, float maxCD) {
            Image btn = null;
            Image img;

            switch (skill.castKeys) {
                case InputKeyFlag.Attack:
                    btn = img_Attack;
                    break;
                case InputKeyFlag.Tech:
                    btn = img_Tech;
                    break;
                case InputKeyFlag.Weap1:
                    if (weapList.Length >= 1 && weapList[0] != null) {
                        btn = weapList[0];
                    }
                    break;
                case InputKeyFlag.Weap2:
                    if (weapList.Length >= 2 && weapList[1] != null) {
                        btn = weapList[1];
                    }
                    break;
                case InputKeyFlag.Weap3:
                    if (weapList.Length >= 3 && weapList[2] != null) {
                        btn = weapList[2];
                    }
                    break;
                case InputKeyFlag.Weap4:
                    if (weapList.Length >= 4 && weapList[3] != null) {
                        btn = weapList[3];
                    }
                    break;
                default:
                    btn = null;
                    break;
            }

            if (btn != null) {
                img = btn.gameObject.GetComponent<Image>();
                img.fillAmount = Mathf.Clamp((maxCD - cd) / maxCD, 0f, 1f);
            }
        }

        void Set_WeapButton() {
            for (int i = 1; i <= slotmax; i++) {
                Image btn_Weap = Instantiate(weapPrefab, skillTrans).GetComponent<Image>();
                RectTransform rectTransform = btn_Weap.GetComponent<RectTransform>();

                // -Transform
                // 设置右下对齐
                rectTransform.anchorMin = new Vector2(1, 0);
                rectTransform.anchorMax = new Vector2(1, 0);

                // 设置位置
                Vector2 pos = new Vector2(0, 0);
                if (i % 2 == 0) {
                    pos.y = 100;
                } else {
                    pos.y = 180;
                }

                int a = (i - 1) / 2;
                pos.x = -300 - 125 * a;

                rectTransform.anchoredPosition = pos;

                // -Name
                string name = "btn_Weap" + i;
                btn_Weap.name = name;

                // -Text
                TextMeshProUGUI text = btn_Weap.GetComponentInChildren<TextMeshProUGUI>();
                string textName = "Weap" + i;
                text.text = textName;

                // -绑定
                weapList[i - 1] = btn_Weap;
            }
        }

        public void Set_BtnTech_Hide() {
            img_Tech.gameObject.SetActive(false);
        }

        public void Show() {
            gameObject.SetActive(true);
        }

        public void TearDown() {
            Destroy(gameObject);
        }
    }
}