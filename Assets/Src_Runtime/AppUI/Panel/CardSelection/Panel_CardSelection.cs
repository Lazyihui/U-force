using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UF.UIApplication {

    public class Panel_CardSelection : MonoBehaviour {
        public Action OnBackClickHandle;
        [SerializeField] Button btn_back;//返回
        public Action OnConfirmClickHandle;
        [SerializeField] Button btn_confirm;//确认选择
        public Action OnEnterGameClickHandle;
        [SerializeField] Button btn_EnterGame;//进入游戏

        public Action OnSelectHeroClickHandle;
        [SerializeField] Button btn_SelectHero;
        public Action OnSelectAgrClickHandle;
        [SerializeField] Button btn_SelectAgr;//选择农业
        public Action OnSelectIndClickHandle;
        [SerializeField] Button btn_SelectInd;//选择工业
        public Action OnSelectSciClickHandle;
        [SerializeField] Button btn_SelectSci;//选择科技


        // text
        [SerializeField] TextMeshProUGUI txt_ChapterName;
        [SerializeField] TextMeshProUGUI txt_StageName;

        public void Ctor() {
            btn_back.onClick.AddListener(() => OnBackClickHandle.Invoke());
            btn_confirm.onClick.AddListener(() => OnConfirmClickHandle.Invoke());
            btn_EnterGame.onClick.AddListener(() => OnEnterGameClickHandle.Invoke());
            btn_SelectHero.onClick.AddListener(() => OnSelectHeroClickHandle.Invoke());
            btn_SelectAgr.onClick.AddListener(() => OnSelectAgrClickHandle.Invoke());
            btn_SelectInd.onClick.AddListener(() => OnSelectIndClickHandle.Invoke());
            btn_SelectSci.onClick.AddListener(() => OnSelectSciClickHandle.Invoke());

        }

        public void Set_Text(int stageID, int chapterID) {

            txt_StageName.text = stageID.ToString() + "Stage ";

            if (chapterID == 1) {
                // TODO:还没定暂时这样写
                txt_ChapterName.text = "防守战";
            } else if (chapterID == 2) {
                txt_ChapterName.text = "Industry";
            } else if (chapterID == 3) {
                txt_ChapterName.text = "Science";
            }

        }

        public void Init() {

        }

        public void Show() {
            gameObject.SetActive(true);
        }

        public void TearDown() {
            Destroy(gameObject);
        }

    }
}