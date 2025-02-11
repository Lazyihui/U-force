using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UF.UIApplication {
    public class Panel_StageSelection_OneStage : MonoBehaviour {
        [SerializeField] Image img_bg;
        [SerializeField] Image img_pass;

        [SerializeField] TextMeshProUGUI txt_stage;
        [SerializeField] Button btn_stage;

        int id;
        public Action<int> OnStageClickHandle;

        public void Ctor() {
            btn_stage.onClick.AddListener(() => {
                if (OnStageClickHandle != null) {
                    OnStageClickHandle.Invoke(id);
                }
            });
        }

        public void Init(int stageID, bool isPassed) {

            id = stageID;
            string str = stageID.ToString();
            txt_stage.text = str;//关卡名

            if (isPassed) {
                img_pass.gameObject.SetActive(false);
            } else {
                img_pass.gameObject.SetActive(true);
            }
        }
    }
}