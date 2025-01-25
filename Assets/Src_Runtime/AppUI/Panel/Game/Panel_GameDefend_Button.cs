using System;
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

        [SerializeField] Button btn_Weap1;
        public Action OnWeap1ClickHandle;

        [SerializeField] Button btn_Weap2;
        public Action OnWeap2ClickHandle;

        [SerializeField] Button btn_Weap3;
        public Action OnWeap3ClickHandle;

        [SerializeField] Button btn_Weap4;
        public Action OnWeap4ClickHandle;

        public void Ctor() {
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

            btn_Weap1.onClick.AddListener(() => {
                if (OnWeap1ClickHandle != null) {
                    OnWeap1ClickHandle.Invoke();
                }
            });

            btn_Weap2.onClick.AddListener(() => {
                if (OnWeap2ClickHandle != null) {
                    OnWeap2ClickHandle.Invoke();
                }
            });

            btn_Weap3.onClick.AddListener(() => {
                if (OnWeap3ClickHandle != null) {
                    OnWeap3ClickHandle.Invoke();
                }
            });

            btn_Weap4.onClick.AddListener(() => {
                if (OnWeap4ClickHandle != null) {
                    OnWeap4ClickHandle.Invoke();
                }
            });
        }

        public void Show() {
            gameObject.SetActive(true);
        }

        public void TearDown() {
            Destroy(gameObject);
        }
    }
}