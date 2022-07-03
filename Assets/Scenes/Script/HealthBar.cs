using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health health; //血量控制腳本實例 (可以直接丟掛有血量控制腳本的父項角色)
    [SerializeField] private GameObject Canvas; //顯示血條的畫布
    [SerializeField] private Image foreground; //顯示血條畫布下的血條圖片

    float healthChangeSpeedRatio = 0.05f; //血條改變時的動畫速度


    void Update()
    {
        if(Mathf.Approximately(health.GetHealthRatio(), 0) || Mathf.Approximately(health.GetHealthRatio(), 1)) //如果血量歸零或滿值，就隱藏血條
        {
            Canvas.SetActive(false);
            return;
        }

        Canvas.SetActive(true);
        Canvas.transform.LookAt(Camera.main.transform.position); //讓血條的畫布一直面相主攝影機
        foreground.fillAmount = Mathf.Lerp(foreground.fillAmount, health.GetHealthRatio(), healthChangeSpeedRatio); //讓控制顯示血條百分比的參數 fillAmount 一直去追 health.GetHealthRatio() 的值


    }
}
