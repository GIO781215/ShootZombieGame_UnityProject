using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] public Health health; //��q����}����� (�i�H�����᱾����q����}������������)
    //[SerializeField] private GameObject Canvas; //��ܦ�����e��
    [SerializeField] private Image blood; //��ܦ���e���U������Ϥ�

    float healthChangeSpeedRatio = 0.05f; //������ܮɪ��ʵe�t��



    public void resetHealth()
    {
        health = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Health>();
    }

    void Update()
    {
        /*
        if (Mathf.Approximately(health.GetHealthRatio(), 0) || Mathf.Approximately(health.GetHealthRatio(), 1)) //�p�G��q�k�s�κ��ȡA�N���æ��
        {
            Canvas.SetActive(false);
            return;
        }
        */

        //Canvas.SetActive(true);
        //Canvas.transform.LookAt(Camera.main.transform.position); //��������e���@�����ۥD��v��
        blood.fillAmount = Mathf.Lerp(blood.fillAmount, health.GetHealthRatio(), healthChangeSpeedRatio); //��������ܦ���ʤ��񪺰Ѽ� fillAmount �@���h�l health.GetHealthRatio() ����
    }
}
