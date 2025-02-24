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
        [SerializeField] Button btn_Attack;
        public Action OnAttackClickHandle;

        [SerializeField] Button btn_Tech;
        public Action OnTechClickHandle;

        public Action OnWeap1ClickHandle;
        public Action OnWeap2ClickHandle;
        public Action OnWeap3ClickHandle;
        public Action OnWeap4ClickHandle;
        Queue<Action> que_Act;

        [SerializeField] GameObject weapPrefab;
        [SerializeField] Transform skillTrans;

        int slotmax;
        public void Set_SlotMax(int value) => slotmax = value;

        Button btn_Weap1;
        Button btn_Weap2;
        Button btn_Weap3;
        Button btn_Weap4;
        Queue<Button> que_Btn;

        public void Ctor(int slotMax) {
            //Pause
            btn_Pause.onClick.AddListener(() => {
                if (OnPauseClickHandle != null) {
                    OnPauseClickHandle.Invoke();
                }
            });

            //Skill
            btn_Attack.onClick.AddListener(() => {
                if (OnAttackClickHandle != null) {
                    OnAttackClickHandle.Invoke();
                }
            });

            btn_Tech.onClick.AddListener(() => {
                if (OnTechClickHandle != null) {
                    OnTechClickHandle.Invoke();
                }
            });

            // Weap
            que_Act = new Queue<Action>();
            que_Act.Enqueue(OnWeap1ClickHandle);
            que_Act.Enqueue(OnWeap2ClickHandle);
            que_Act.Enqueue(OnWeap3ClickHandle);
            que_Act.Enqueue(OnWeap4ClickHandle);

            que_Btn = new Queue<Button>();
            que_Btn.Enqueue(btn_Weap1);
            que_Btn.Enqueue(btn_Weap2);
            que_Btn.Enqueue(btn_Weap3);
            que_Btn.Enqueue(btn_Weap4);

            Set_SlotMax(slotMax);
            Set_WeapButton();
        }

        public void Set_CD(SkillModel skill, float cd, float maxCD) {
            Button btn;
            Image img;

            switch (skill.castKeys) {
                case InputKeyFlag.Attack:
                    btn = btn_Attack;
                    break;
                case InputKeyFlag.Tech:
                    btn = btn_Tech;
                    break;
                case InputKeyFlag.Weap1:
                    if (btn_Weap1 != null) {
                        btn = btn_Weap1;
                    } else {
                        btn = null;
                    }
                    break;
                case InputKeyFlag.Weap2:
                    if (btn_Weap2 != null) {
                        btn = btn_Weap2;
                    } else {
                        btn = null;
                    }
                    break;
                case InputKeyFlag.Weap3:
                    if (btn_Weap3 != null) {
                        btn = btn_Weap3;
                    } else {
                        btn = null;
                    }
                    break;
                case InputKeyFlag.Weap4:
                    if (btn_Weap4 != null) {
                        btn = btn_Weap4;
                    } else {
                        btn = null;
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
                Button btn_Weap = Instantiate(weapPrefab, skillTrans).GetComponent<Button>();
                RectTransform rectTransform = btn_Weap.GetComponent<RectTransform>();

                // -AddListener
                btn_Weap.onClick.AddListener(() => {
                    if (que_Act.Count <= 0) {
                        Debug.LogError("weap按键超出四个");
                        return;
                    }

                    Action action = que_Act.Dequeue();
                    action?.Invoke();
                });

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
                if (que_Btn.Count <= 0) {
                    Debug.LogError("weap按键超出四个");
                    return;
                }

                Button btn = que_Btn.Dequeue();
                btn = btn_Weap;
            }
        }

        public void Show() {
            gameObject.SetActive(true);
        }

        public void TearDown() {
            Destroy(gameObject);
        }
    }
}