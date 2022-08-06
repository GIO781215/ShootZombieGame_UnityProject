using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Victory_UI_Controller : MonoBehaviour
{
    bool IsHied = true;

    public Image black_background;
    public Image victory_pictrue;
    public Image victory_background;
    public Image restart;


    float startDelayTime = 2;  //過幾秒後開始淡入UI畫面




    public void Hied()
    {
        black_background.color = new Color(0 / 255f, 0 / 255f, 0 / 255f, 0 / 255f);
        victory_pictrue.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 0 / 255f);
        victory_background.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 0 / 255f);
        restart.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 0 / 255f);

        IsHied = true;
    }


     public void Show()
    {
        black_background.color = new Color(0 / 255f, 0 / 255f, 0 / 255f, 0 / 255f);
        victory_pictrue.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 0 / 255f);
        victory_background.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 0 / 255f);
        restart.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 0 / 255f);

        Invoke("_show", startDelayTime);
        IsHied = false;
    }

    void _show()
    {
        StartCoroutine(__show());
    }

    private IEnumerator __show()
    {
        float a = 0;
        float b = 0;
        float d = 0;
        float c = 0;

        while (true)
        {
         
            a = Mathf.Lerp(a, 100f, 0.01f);
            b = Mathf.Lerp(b, 255f, 0.01f);
            c = Mathf.Lerp(c, 80f, 0.01f);
            d = Mathf.Lerp(d, 255f, 0.01f);


            black_background.color = new Color(0 / 255f, 0 / 255f, 0 / 255f, a / 255f);
            victory_pictrue.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, b / 255f);
            victory_background.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, c / 255f);
            restart.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, d / 255f);

            yield return new WaitForSeconds(0.01f);

            if (a > 95f || IsHied)
                break;
            
        }
    }



}
