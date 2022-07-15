using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum WeaponShootMode //武器的射擊模式
{
    Single,    //單發
    Automatic, //連射
}


public class Weapon : MonoBehaviour
{
    [SerializeField] public GameObject sourcePrefab { get; set; } //這個武器的預製物件，也就是掛著此腳本的父對象(而其已變成父對象放在資源資料夾中)
    [SerializeField] public Transform weaponMullze; //武器的槍口位置
    [SerializeField] public Projectile projectilePrefab; //子彈的預製物件
    [SerializeField] public WeaponShootMode ShootMode; //射擊模式                    shootType
    float delayBetweenShoots = 0.1f; //射擊間隔時間
    float timeLastShoot = -Mathf.Infinity; //上次射擊完的時間

    /* 先不設計成有需要彈藥量的遊戲機制
    float currentAmmo; //當前彈藥量
    float maxAmmo = 10; //最大彈藥量               
    float AmmoPerShoot; //每次射擊時消耗的彈藥量         bulletPerShoot 
    float AmmoReloadRate = 1f; //每秒補充的彈藥量    
    float AmmoReloadDelay = 2f; //射完之後到可以補充彈藥量的間隔時間      
    */


    bool isAim; //是否在瞄準狀態

    bool i = true;

    private void Start()
    {
        //currentAmmo = maxAmmo;
    }

    private void Update()
    {
        UpdateAmmo(); //不停補充彈藥量

        /* 最後覺得不該抓畫面的座標點來控制發射方向，邊走邊射會有 bug
        // Camera.main.ScreenPointToRay() : 返回一條射線從攝像機通過一個屏幕點，產生的射線是在世界空間中，從相機的近裁剪面開始並穿過屏幕上的點 position(x, y)，position.z 會被忽略，屏幕的左下為(0, 0)，右上是 (pixelWidth, pixelHeight)
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2 , Screen.height / 2));
        Debug.DrawLine(weaponMullze.transform.position, ray.GetPoint(10), Color.blue); //ray.GetPoint(distance) : 返回射線上 distance 個單位處的點。
        weaponMullze.transform.rotation = Quaternion.LookRotation(ray.GetPoint(10) - weaponMullze.transform.position);  
        */

        /*//---------------------------------------------------------------------------------------------------
        1.創建一條射線，從自己出發，發射向目標
        Ray ray = new Ray(transform.position, target.position - transform.position);    // 第一個參數是射線的起點 ray.origin，第二個參數是射線的方向 ray.direction

        2.繪製出射線，方便調試（在Scene視角能看見，Game視角看不見）
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);

        例子:
        Ray R1 = new Ray(new Vector3(0, 5, 0), new Vector3(0, 5, 100) - (new Vector3(0, 5, 0)));
        Debug.DrawRay(R1.origin, R1.direction * 100, Color.red);
        *///---------------------------------------------------------------------------------------------------


        //---------------- 得到攝影機俯仰轉動的角度 ----------------
        float angle = 0;
        angle = Camera.main.transform.rotation.eulerAngles.x;
        if (angle >= 180) angle -= 360;
        angle = 15 - angle;
        Debug.Log(angle); //angle 初始值是 0，範圍在 -5 ~ 25 之間
        //----------------------------------------------------------

        //創建一條從攝影機向其前方發出的射線 
        Ray R1 = new Ray(Camera.main.transform.position, Camera.main.transform.forward + /*初始斜度補正*/ new Vector3(0, + 0.2f, 0) + /*俯仰轉動倍率加成*/ new Vector3(0, 0.015f, 0) * angle);   
        Debug.DrawRay(R1.origin, R1.direction * 10, Color.red);
        weaponMullze.transform.rotation = Quaternion.LookRotation(R1.GetPoint(10) - R1.GetPoint(0)); //將 weaponMullze 的朝向設成與 R1 相同

    }

    private void UpdateAmmo()
    {
        //補充彈藥量
    }

    public void ShowWeapon() //顯示武器模型
    {
        foreach (Transform child in transform) //遍歷子對象
        {
            Renderer render = child.gameObject.GetComponent<Renderer>();
            if (render != null)
            {
                render.enabled = true;
            }
        }
    }

    public void HiddenWeapon() //隱藏武器模型
    {
        foreach (Transform child in transform) //遍歷子對象
        {
            Renderer render = child.gameObject.GetComponent<Renderer>();
            if (render != null)
            {
                render.enabled = false;
            }
        }
    }


    public void HandleShootInput(bool inputDown, bool inputHeld, bool inputUp)
    {
        switch (ShootMode) //先判斷自己現在處於哪種射擊模式
        {
            case WeaponShootMode.Single:
                if(inputDown)
                {
                    print("Single射擊");
                    SingleShoot();
                }
                return;

            case WeaponShootMode.Automatic:
                if (inputHeld)
                {
                    print("Auto射擊");
                }
                return;
            default:
                return;
        }

    }

    private void SingleShoot()
    {
        if (timeLastShoot + delayBetweenShoots <  Time.time)
        {
            Projectile projectile = Instantiate(projectilePrefab, weaponMullze.position, Quaternion.LookRotation(weaponMullze.forward)); //產生子彈，LookRotation(Vector3(x,y,z)) : 獲取一個向量所表示的方向（將一個向量轉為該向量所指的方向）
            timeLastShoot = Time.time;

        }
    }
}
