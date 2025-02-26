using System;
using UnityEngine;
using UnityEngine.UI;

namespace UF
{
    public class HUD_Training_CD : MonoBehaviour
    {
        [SerializeField] Image cd_Attack;
        [SerializeField] Image cd_Defense;

        [SerializeField] RectTransform rect;

        public void Ctor() {

        }

        public void Show() {
            gameObject.SetActive(true);
        }

        public void TearDown() {
            Destroy(gameObject);
        }

        public void SetAttackCD(float cd, float maxCD) {
            cd_Attack.fillAmount = (maxCD - cd) / maxCD;
        }

        public void SetDefenseCD(float cd, float maxCD) {
            cd_Defense.fillAmount = (maxCD - cd) / maxCD;
        }

        public void SetPos(Vector3 pos) {
            pos = Camera.main.WorldToScreenPoint(pos);

            RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, pos, null, out Vector3 worldPos);
            transform.position = worldPos;
        }
    }
}