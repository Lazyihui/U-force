using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UF {

    public class Panel_GameOver : MonoBehaviour {
        [SerializeField] TextMeshProUGUI Text_Title;
        [SerializeField] TextMeshProUGUI Text_Hurt;
        [SerializeField] TextMeshProUGUI Text_Shoot;
        [SerializeField] TextMeshProUGUI Text_HitNumber;
        [SerializeField] TextMeshProUGUI Text_HitRate;

        // 文字写的是Continue, 但和Pause的会有冲突，所以命名GameOver
        [SerializeField] Button Btn_GameOver;
        public Action<bool> OnGameOverButtonClick;

        [SerializeField] GameObject star1;
        [SerializeField] GameObject star2;
        [SerializeField] GameObject star3;
        Dictionary<int, GameObject> starDict;

        bool isWin;

        public void Ctor() {
            Btn_GameOver.onClick.AddListener(() => {
                if (OnGameOverButtonClick != null) {
                    OnGameOverButtonClick.Invoke(isWin);
                }
            });

            starDict = new Dictionary<int, GameObject>()
            {
                {1, star1},
                {2, star2},
                {3, star3}
            };
        }

        public void Init(bool isWin, int hurt, int shoot, int hitNumber, float hitRate, int starCount) {
            this.isWin = isWin;
            Text_Hurt.text = "<b>受伤害</b>    " + hurt.ToString();
            Text_Shoot.text = "<b>射击次数</b>    " + shoot.ToString();
            Text_HitNumber.text = "<b>命中次数</b>    " + hitNumber.ToString();
            Text_HitRate.text = "<b>命中率</b>    " + ( hitRate * 100 ).ToString("F2") + "%";

            if (isWin) {
                Text_Title.text = "胜利";

                if (starCount > 3) {
                    Debug.LogWarning("starCount大于3");
                    return;
                }

                for (int i = 1; i <= starCount; i++) {
                    GameObject star = starDict[i];
                    star.SetActive(true);
                }

                return;
            }
            Text_Title.text = "失败";
        }

        public void Show() {
            gameObject.SetActive(true);
        }

        public void TearDown() {
            Destroy(gameObject);
        }
    }
}