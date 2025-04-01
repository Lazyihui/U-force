using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace UF {

    public class Panel_Setting : MonoBehaviour {
        [SerializeField] Button btn_Back;
        public Action OnBackClickHandle;

        // 分辨率
        [SerializeField] Button btn_Reselustion1920;
        public Action OnReselustion1920ClickHandle;

        [SerializeField] Button btn_Reselustion1600;
        public Action OnReselustion1600ClickHandle;

        [SerializeField] Button btn_Reselustion640;
        public Action OnReselustion640ClickHandle;

        [SerializeField] Toggle toggle_isFullScreen;
        public Action<bool> OnFullScreenToggleHandle;

        // 声音
        [SerializeField] Slider sli_Music;
        public Action<float> OnMusicSliderChangeHandle;

        [SerializeField] Slider sli_SFX;
        public Action<float> OnSFXSliderChangeHandle;

        public Action OnCloseHandle;

        // InputElement 
        [SerializeField] InputBindElement bindElementPrefab;//预制体
        [SerializeField] Transform inputElementRoot;//生成位置
        public List<InputBindElement> inputBindEles;



        public void Ctor() {
            // == 初始化 ==
            inputBindEles = new List<InputBindElement>();

            // 关闭按钮
            btn_Back.onClick.AddListener(() => OnBackClickHandle?.Invoke());
            // 分辨率
            btn_Reselustion1920.onClick.AddListener(() => OnReselustion1920ClickHandle?.Invoke());
            btn_Reselustion1600.onClick.AddListener(() => OnReselustion1600ClickHandle?.Invoke());
            btn_Reselustion640.onClick.AddListener(() => OnReselustion640ClickHandle?.Invoke());
            toggle_isFullScreen.onValueChanged.AddListener((isOn) => OnFullScreenToggleHandle?.Invoke(isOn));
            // 声音
            sli_Music.onValueChanged.AddListener((value) => OnMusicSliderChangeHandle?.Invoke(value));
            sli_SFX.onValueChanged.AddListener((value) => OnSFXSliderChangeHandle?.Invoke(value));
        }

        public void AddInputBindElement(InputKeyFlag key) {
            // InputKeyFlagExtension enumToString = new InputKeyFlagExtension();
            // string keyName = enumToString.GetEnumString(key);
            // if (string.IsNullOrEmpty(keyName)) {
            //     Debug.LogError("AddInputBindElement keyName is null or empty " + key.ToString());
            //     return;
            // }
            //Debug.Log("AddInputBindElement keyName " + keyName + " key " + key.ToString());
            InputBindElement ele = Instantiate(bindElementPrefab, inputElementRoot);
            ele.Ctor();
            ele.Init(key.ToString(), key.ToString(), key.ToString());
            ele.OnKbBindClickHandle = () => {
                Debug.Log("OnKbBindClickHandle " + key.ToString());
            };
            ele.OnGamepadBindClickHandle = () => {
                Debug.Log("OnGamepadBindClickHandle " + key.ToString());
            };
            inputBindEles.Add(ele);
        }

        public void SetSliderValue(float musicValue, float sfxValue) {
            sli_Music.value = musicValue;
            sli_SFX.value = sfxValue;
        }

        public void Show() {
            gameObject.SetActive(true);
        }

        public void TearDown() {
            Destroy(gameObject);
        }
    }
}