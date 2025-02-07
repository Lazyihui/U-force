using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 不知道HUD是什么的缩写，所以非button逻辑先写在这了
public class HUD_CD : MonoBehaviour
{
    [SerializeField] Image CD_Attack;
    [SerializeField] Image CD_Tech;
    [SerializeField] Image CD_Weap1;
    [SerializeField] Image CD_Weap2;
    [SerializeField] Image CD_Weap3;
    [SerializeField] Image CD_Weap4;

    public void Ctor()
    {

    }

    public void Set_CD_Attack(float cd, float maxCD)
    {
        CD_Attack.fillAmount = Mathf.Clamp(cd / maxCD, 0f, 1f);

        TextMeshProUGUI text = CD_Attack.GetComponentInChildren<TextMeshProUGUI>();
        text.text = cd.ToString("0.00");

        if (cd <= 0)
        {
            CD_Attack.gameObject.SetActive(false);
        }
    }

    public void Set_CD_Tech(float cd, float maxCD)
    {
        CD_Tech.fillAmount = Mathf.Clamp(cd / maxCD, 0f, 1f);

        TextMeshProUGUI text = CD_Tech.GetComponentInChildren<TextMeshProUGUI>();
        text.text = cd.ToString("0.00");

        if (cd <= 0)
        {
            CD_Tech.gameObject.SetActive(false);
        }
    }

    public void Set_CD_Weap1(float cd, float maxCD)
    {
        CD_Weap1.fillAmount = Mathf.Clamp(cd / maxCD, 0f, 1f);

        TextMeshProUGUI text = CD_Weap1.GetComponentInChildren<TextMeshProUGUI>();
        text.text = cd.ToString("0.00");

        if (cd <= 0)
        {
            CD_Weap1.gameObject.SetActive(false);
        }
    }

    public void Set_CD_Weap2(float cd, float maxCD)
    {
        CD_Weap2.fillAmount = Mathf.Clamp(cd / maxCD, 0f, 1f);

        TextMeshProUGUI text = CD_Weap2.GetComponentInChildren<TextMeshProUGUI>();
        text.text = cd.ToString("0.00");

        if (cd <= 0)
        {
            CD_Weap2.gameObject.SetActive(false);
        }
    }

    public void Set_CD_Weap3(float cd, float maxCD)
    {
        CD_Weap3.fillAmount = Mathf.Clamp(cd / maxCD, 0f, 1f);

        TextMeshProUGUI text = CD_Weap3.GetComponentInChildren<TextMeshProUGUI>();
        text.text = cd.ToString("0.00");

        if (cd <= 0)
        {
            CD_Weap3.gameObject.SetActive(false);
        }
    }

    public void Set_CD_Weap4(float cd, float maxCD)
    {
        CD_Weap4.fillAmount = Mathf.Clamp(cd / maxCD, 0f, 1f);

        TextMeshProUGUI text = CD_Weap4.GetComponentInChildren<TextMeshProUGUI>();
        text.text = cd.ToString("0.00");

        if (cd <= 0)
        {
            CD_Weap4.gameObject.SetActive(false);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void TearDown()
    {
        Destroy(gameObject);
    }
}