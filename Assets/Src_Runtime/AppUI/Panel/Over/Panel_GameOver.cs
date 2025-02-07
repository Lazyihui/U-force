using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel_GameOver : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Text_Title;
    [SerializeField] TextMeshProUGUI Text_Hurt;
    [SerializeField] TextMeshProUGUI Text_Shoot;
    [SerializeField] TextMeshProUGUI Text_HitNumber;
    [SerializeField] TextMeshProUGUI Text_HitRate;

    // 文字写的是Continue, 但和Pause的会有冲突，所以命名GameOver
    [SerializeField] Button Btn_GameOver;
    public Action OnGameOverButtonClick;

    public void Ctor()
    {
        Btn_GameOver.onClick.AddListener(() => {
            if (OnGameOverButtonClick != null)
            {
                OnGameOverButtonClick.Invoke();
            }
        });
    }

    public void SetTitle(bool isWin)
    {
        if (isWin)
        {
            Text_Title.text = "Win";
            return;
        }
        Text_Title.text = "Fail";
    }

    public void SetHurt(int hurt)
    {
        Text_Hurt.text = "<b>Hurt</b>    " + hurt.ToString();
    }

    public void SetShoot(int shoot)
    {
        Text_Shoot.text = "<b>Shoot</b>    " + shoot.ToString();
    }

    public void SetHitNumber(int hitNumber)
    {
        Text_HitNumber.text = "<b>HitNumber</b>    " + hitNumber.ToString();
    }

    public void SetHitRate(float hitRate)
    {
        Text_HitRate.text = "<b>HitRate</b>    " + hitRate.ToString("F2") + "%";
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