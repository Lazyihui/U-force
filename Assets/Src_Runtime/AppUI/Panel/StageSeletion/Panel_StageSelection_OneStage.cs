using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UF.UIApplication {
    public class Panel_StageSelection_OneStage : MonoBehaviour {
        [SerializeField] GameObject stageLock;

        [SerializeField] Transform starGroup;
        [SerializeField] GameObject stageGoup;
        [SerializeField] GameObject stageLightStar;
        [SerializeField] GameObject stageDarkStar;

        [SerializeField] TextMeshProUGUI txt_stage;
        [SerializeField] Button btn_stage;

        [SerializeField] Image img_Bg;
        [SerializeField] Sprite bg_Unlock;
        [SerializeField] Sprite bg_Lock;

        public int stageID;
        public Action<int> OnStageClickHandle;

        public void Ctor() {
            btn_stage.onClick.AddListener(() => {
                if (OnStageClickHandle != null) {
                    OnStageClickHandle.Invoke(stageID);
                }
            });
        }

        public void Set_GetStar(int starCount) {
            for (int i = 0; i < starCount; i++) {
                // 添加亮星星
                GameObject star = Instantiate(stageLightStar, starGroup);
            }
            int darkStarCount = 3 - starCount;
            for (int i = 0; i < darkStarCount; i++) {
                // 添加暗星星
                GameObject star = Instantiate(stageDarkStar, starGroup);
            }
        }

        // 关卡是否锁定
        public void Set_IsLock(bool isUnLock) {

            if (isUnLock) {
                img_Bg.sprite = bg_Unlock;
                // btn_stage.enabled = true;
            } else {
                img_Bg.sprite = bg_Lock;
                // btn_stage.enabled = false;
            }
        }


        public void Init(int stageID, bool isPassed, int getStarCount) {

            this.stageID = stageID;
            string str = stageID.ToString();
            txt_stage.text = str;//关卡名

            if (isPassed) {
                stageLock.gameObject.SetActive(false);
                stageGoup.gameObject.SetActive(true);
                Set_GetStar(getStarCount);
                Set_IsLock(isPassed);

            } else {
                stageLock.gameObject.SetActive(true);
                stageGoup.gameObject.SetActive(false);
                Set_IsLock(isPassed);
            }
        }

    }
}