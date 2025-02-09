using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace UF.UIApplication {

    public class Panel_StageSelection : MonoBehaviour {

        [SerializeField] Transform oneStegeGroup;
        [SerializeField] TextMeshProUGUI txt_chapter;
        [SerializeField] Panel_StageSelection_OneStage oneStagePrefab;

        List<Panel_StageSelection_OneStage> stage;

        [SerializeField] Button btn_c1;
        [SerializeField] Button btn_c2;
        [SerializeField] Button btn_c3;
        [SerializeField] Button btn_c4;
        [SerializeField] Button btn_c5;

        public Action<int> OnChapterClickHandle;

        public void Ctor() {
            btn_c1.onClick.AddListener(() => OnChapterClickHandle.Invoke(1));
            // btn_c1.onClick.AddListener(() => {
            //     if (OnChapterClickHandle != null) {
            //         OnChapterClickHandle.Invoke(1);
            //     }
            // });
            btn_c2.onClick.AddListener(() => OnChapterClickHandle.Invoke(2));
            btn_c3.onClick.AddListener(() => OnChapterClickHandle.Invoke(3));
            btn_c4.onClick.AddListener(() => OnChapterClickHandle.Invoke(4));
            btn_c5.onClick.AddListener(() => OnChapterClickHandle.Invoke(5));

            stage = new List<Panel_StageSelection_OneStage>();
        }

        public void TearDown() {
            Destroy(gameObject);
        }

        public void Show() {
            gameObject.SetActive(true);
        }

    }
}