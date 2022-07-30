using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageUI : MonoBehaviour
{
    public Text text_1;
    public Text text_2;
    public Text text_3;
    public Text text_4;
    public Text text_5;
    public Text text_6;
    public Text text_7;
    public Text text_8;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            message_1();
        }
    }

    public void message_1()
    {
        text_1.text = "本頁面統合處理各維基計劃全域轉換和中文維基百科本地全局轉";
        text_2.text = "本頁面統合處理各維基計劃全域轉換和中文維基百科本地全局轉";
        text_3.text = "本頁面統合處理各維基計劃全域轉換和中文維基百科本地全局轉";
        text_4.text = "本頁面統合處理各維基計劃全域轉換和中文維基百科本地全局轉";
        text_5.text = "本頁面統合處理各維基計劃全域轉換和中文維基百科本地全局轉";
        text_6.text = "本頁面統合處理各維基計劃全域轉換和中文維基百科本地全局轉";
        text_7.text = "本頁面統合處理各維基計劃全域轉換和中文維基百科本地全局轉";
        text_8.text = "本頁面統合處理各維基計劃全域轉換和中文維基百科本地全局轉";
    }



}
