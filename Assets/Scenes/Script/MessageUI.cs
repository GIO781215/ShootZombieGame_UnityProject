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
        text_1.text = "�������ΦX�B�z�U����p�������ഫ�M�������ʬ쥻�a������";
        text_2.text = "�������ΦX�B�z�U����p�������ഫ�M�������ʬ쥻�a������";
        text_3.text = "�������ΦX�B�z�U����p�������ഫ�M�������ʬ쥻�a������";
        text_4.text = "�������ΦX�B�z�U����p�������ഫ�M�������ʬ쥻�a������";
        text_5.text = "�������ΦX�B�z�U����p�������ഫ�M�������ʬ쥻�a������";
        text_6.text = "�������ΦX�B�z�U����p�������ഫ�M�������ʬ쥻�a������";
        text_7.text = "�������ΦX�B�z�U����p�������ഫ�M�������ʬ쥻�a������";
        text_8.text = "�������ΦX�B�z�U����p�������ഫ�M�������ʬ쥻�a������";
    }



}
