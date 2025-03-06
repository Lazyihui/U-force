using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UF
{
    public class Panel_WatchDetail : MonoBehaviour
    {
        [SerializeField] Image img_Sprite;
        [SerializeField] TextMeshProUGUI text_Name;
        [SerializeField] TextMeshProUGUI text_Feature;
        [SerializeField] TextMeshProUGUI text_HP;
        [SerializeField] TextMeshProUGUI text_Skill;
        [SerializeField] TextMeshProUGUI text_Restraint;

        [SerializeField] Button btn_Close;
        public Action OnCloseClickHandle;

        public void Ctor()
        {
            btn_Close.onClick.AddListener(() => OnCloseClickHandle.Invoke());
        }

        public void Init(Sprite sprite, string name, string feature, int hp, string skill, string restraint)
        {
            Set_Sprite(sprite);
            Set_Name(name);
            Set_Feature(feature);
            Set_HP(hp);
            Set_Skill(skill);
            Set_Restraint(restraint);
        }

        public void Init(Sprite sprite, string name, string feature)
        {
            Set_Sprite(sprite);
            Set_Name(name);
            Set_Feature(feature);
            text_HP.text = "";
            text_Skill.text = "";
            text_Restraint.text = "";
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void TearDown()
        {
            Destroy(gameObject);
        }

        void Set_Sprite(Sprite sprite)
        {
            img_Sprite.sprite = sprite;
        }

        void Set_Name(string name)
        {
            text_Name.text = name;
        }

        void Set_Feature(string feature)
        {
            text_Feature.text = "<b>Feature:</b> " + feature;
        }

        void Set_HP(int hp)
        {
            text_HP.text = "<b>HP:</b> " + hp.ToString();
        }

        void Set_Skill(string skill)
        {
            text_Skill.text = "<b>Skill:</b> " + skill;
        }

        void Set_Restraint(string restraint)
        {
            text_Restraint.text = "<b>Restraint:</b> " + restraint;
        }
    }
}