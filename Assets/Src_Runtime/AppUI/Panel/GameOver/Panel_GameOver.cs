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
        public Action OnGameOverButtonClick;

        [SerializeField] GameObject star1;
        [SerializeField] GameObject star2;
        [SerializeField] GameObject star3;
        Dictionary<int, GameObject> starDict = new Dictionary<int, GameObject>();

        public void Ctor() {
            Btn_GameOver.onClick.AddListener(() => {
                if (OnGameOverButtonClick != null) {
                    OnGameOverButtonClick.Invoke();
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
            Text_Hurt.text = "<b>Hurt</b>    " + hurt.ToString();
            Text_Shoot.text = "<b>Shoot</b>    " + shoot.ToString();
            Text_HitNumber.text = "<b>HitNumber</b>    " + hitNumber.ToString();
            Text_HitRate.text = "<b>HitRate</b>    " + hitRate.ToString("F2") + "%";

            if (isWin) {
                Text_Title.text = "Win";

                if (starCount > 3)
                {
                    Debug.Log("starCount大于3");
                    return;
                }

                for (int i = 1; i <= starCount; i++)
                {
                    GameObject star = starDict[i];
                    star.SetActive(true);
                }

                return;
            }
            Text_Title.text = "Fail";
        }

        public void Show() {
            gameObject.SetActive(true);
        }

        public void TearDown() {
            Destroy(gameObject);
        }
    }
}