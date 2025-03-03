using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UF {

    public class Panel_EliminateGameOver : MonoBehaviour {

        [SerializeField] TextMeshProUGUI Text_Title;
        [SerializeField] TextMeshProUGUI Text_Energy;
        [SerializeField] TextMeshProUGUI Text_Shoot;
        [SerializeField] TextMeshProUGUI Text_TakeTime;

        [SerializeField] Button Btn_GameOver;
        public Action<bool> OnEliminateGameOverButtonClick;

        [SerializeField] GameObject star1;
        [SerializeField] GameObject star2;
        [SerializeField] GameObject star3;
        Dictionary<int, GameObject> starDict;

        bool isWin;

        public void Ctor() {
            Btn_GameOver.onClick.AddListener(() => {
                if (OnEliminateGameOverButtonClick != null) {
                    OnEliminateGameOverButtonClick.Invoke(isWin);
                }
            });

            starDict = new Dictionary<int, GameObject>()
            {
                {1, star1},
                {2, star2},
                {3, star3}
            };
        }

        public void Init(bool isWin, int takeEnergy, int shoot, int takeTime, int starCount) {
            this.isWin = isWin;
            Text_Energy.text = "<b>获得能量</b>    " + takeEnergy.ToString();
            Text_Shoot.text = "<b>射击次数</b>    " + shoot.ToString();
            Text_TakeTime.text = "<b>花费时间</b>    " + takeTime.ToString();

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
