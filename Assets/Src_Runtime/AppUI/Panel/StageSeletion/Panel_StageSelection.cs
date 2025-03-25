using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


namespace UF.UIApplication {

    public class Panel_StageSelection : MonoBehaviour {

        [SerializeField] Transform oneStegeGroup;
        [SerializeField] TextMeshProUGUI txt_chapter;
        [SerializeField] TextMeshProUGUI txt_starNum;
        [SerializeField] Panel_StageSelection_OneStage oneStagePrefab;

        List<Panel_StageSelection_OneStage> stage;

        [SerializeField] Button btn_c1;
        [SerializeField] Button btn_c2;
        [SerializeField] Button btn_c3;
        [SerializeField] Button btn_c4;
        [SerializeField] Button btn_c5;

        [SerializeField] Sprite spr_chapterSelected;
        [SerializeField] Sprite spr_chapterNotSelected;

        // 图片
        [SerializeField] Sprite spr_kb;
        [SerializeField] Sprite spr_js;
        [SerializeField] Image img_keyPrompt;//键盘提示图片

        // 放回按钮
        [SerializeField] Button btn_back;
        public Action OnBackHandle;

        // 
        int chapter;
        Dictionary<int, Button> chapterBtns;

        public Action<int> OnChapterClickHandle;//章节
        public Action<int> OnStageClickHandle;
        public Action OnCloseHandle;//关闭

        // 选择关卡
        Panel_StageSelection_OneStage selectedStage;
        bool isMouseOver;

        AudioSource audioSource;
        [SerializeField] AudioClip btnSelectedClip;
        [SerializeField] AudioClip btnConfirmClip;

        public void Ctor() {
            btn_c1.onClick.AddListener(() => OnChapterClickHandle.Invoke(1));
            btn_c2.onClick.AddListener(() => OnChapterClickHandle.Invoke(2));
            btn_c3.onClick.AddListener(() => OnChapterClickHandle.Invoke(3));
            btn_c4.onClick.AddListener(() => OnChapterClickHandle.Invoke(4));
            btn_c5.onClick.AddListener(() => OnChapterClickHandle.Invoke(5));

            btn_back.onClick.AddListener(() => OnBackHandle.Invoke());

            chapterBtns = new Dictionary<int, Button> {
                {1, btn_c1},
                {2, btn_c2},
                {3, btn_c3},
                {4, btn_c4},
                {5, btn_c5},
            };

            stage = new List<Panel_StageSelection_OneStage>();
        }

        public void Init(AudioSource audioSource) {
            this.audioSource = audioSource;
        }

        public void Tick(bool isTabDown, bool isEscDown) {

            if (isTabDown) {
                OnChapterClickHandle.Invoke(chapter + 1);
            }

            if (isEscDown) {
                OnCloseHandle.Invoke();
            }
        }


        public void Add(int stageID, bool isPassed, int stageCount) {
            var oneStage = Instantiate(oneStagePrefab, oneStegeGroup);
            oneStage.Ctor();
            oneStage.OnStageClickHandle = OnStageClick;
            oneStage.Init(stageID, isPassed, stageCount);
            stage.Add(oneStage);

            if (stage.Count == 1) {
                Select(oneStage.gameObject);
            }
        }

        public void Set_GetStar(int stageID, int starCount) {
            foreach (Panel_StageSelection_OneStage oneStage in stage) {
                if (oneStage.stageID == stageID) {
                    oneStage.Set_GetStar(starCount);
                }
            }
        }

        void Select(GameObject go) {
            // TODO:根本不懂
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(go);
        }

        public void SelectChapter(InputDeviceType inputDeviceType, int chapter, int maxChapter) {

            // 没有章节
            if (maxChapter <= 1) {
                img_keyPrompt.gameObject.SetActive(false);
            } else {
                img_keyPrompt.gameObject.SetActive(true);
            }

            // 换章按钮 显示
            foreach (KeyValuePair<int, Button> pair in chapterBtns) {
                pair.Value.gameObject.SetActive(pair.Key <= maxChapter);
            }


            // 选择当前关卡图片
            this.chapter = chapter;
            txt_chapter.text = $"Chapter {chapter}";

            foreach (KeyValuePair<int, Button> pair in chapterBtns) {
                if (pair.Key == chapter) {
                    pair.Value.image.sprite = spr_chapterSelected;
                } else {
                    pair.Value.image.sprite = spr_chapterNotSelected;
                }
            }

            // 手柄还是键盘图片
            if (inputDeviceType == InputDeviceType.Keyboard) {
                img_keyPrompt.sprite = spr_kb;
            } else if (inputDeviceType == InputDeviceType.Gamepad) {
                img_keyPrompt.sprite = spr_js;
            }

            img_keyPrompt.SetNativeSize();//设置图片的原始大小
        }

        public void SetNativeSize() {
            img_keyPrompt.SetNativeSize();
        }

        public void Set_StarNum(int num) {
            txt_starNum.text = "勋章数量: " + num.ToString();
        }

        public void Clear() {
            // 保留第一个 刚刚开始选择关卡的时候
            for (int i = 0; i < oneStegeGroup.childCount; i += 1) {
                Destroy(oneStegeGroup.GetChild(i).gameObject);
            }
            stage.Clear();
        }

        public void TearDown() {
            Destroy(gameObject);
        }

        public void Show() {
            gameObject.SetActive(true);
        }
        void OnStageClick(int stageID) {
            OnStageClickHandle.Invoke(stageID);
        }

        public void Update_Tick(Canvas canvas, Vector2 moveAxis, bool isConfirm) {
            if (gameObject.activeSelf == false) { return; }

            if (selectedStage == null) {
                var zeroStage = stage[0];
                SetSelectedStage(zeroStage);
                return;
            }

            // 确认
            if (isConfirm) {
                ConfirmSelectedStage();
                return;
            }

            // 鼠标悬停
            var newStage = GetMouseOverStage(canvas.gameObject);
            if (isMouseOver) {
                SetSelectedStage(newStage);
                return;
            }

            // 选中按钮检测
            if (moveAxis == Vector2.zero) { return; }

            Button selectedBtn = selectedStage.GetComponent<Button>();
            Selectable selectable = selectedBtn.FindSelectable(moveAxis);

            if (selectable == null) { return; }

            Button newSelectedBtn = selectable as Button;
            var newSelectedStage = newSelectedBtn.GetComponent<Panel_StageSelection_OneStage>();

            if (newSelectedStage == null) { return; }
            SetSelectedStage(newSelectedStage);
        }

        void SetSelectedStage(Panel_StageSelection_OneStage newStage) {
            int len = stage.Count;
            for (int i = 0; i < len; i++) {
                stage[i].OnDeselected();
            }

            selectedStage = newStage;
            newStage.OnSelected();

            audioSource.PlayOneShot(btnSelectedClip);
        }

        void ConfirmSelectedStage() {
            if (selectedStage == null) { return; }

            selectedStage.OnClick();

            audioSource.PlayOneShot(btnConfirmClip);
        }

        Panel_StageSelection_OneStage GetMouseOverStage(GameObject canvas) {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;
            GraphicRaycaster gr = canvas.GetComponent<GraphicRaycaster>();
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(pointerEventData, results);

            if (results.Count != 0) {
                var len = results.Count;
                for (int i = 0; i < len; i++) {
                    var newStage = results[i].gameObject.GetComponent<Panel_StageSelection_OneStage>();
                    if (newStage != null) {
                        isMouseOver = true;
                        return newStage;
                    }
                }
            }

            isMouseOver = false;
            return null;
        }
    }
}