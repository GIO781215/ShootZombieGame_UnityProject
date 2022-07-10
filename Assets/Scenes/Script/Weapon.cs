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
    public GameObject sourcePrefab { get; set; } //這個武器的預製物件，也就是掛著此腳本的父對象(而其已變成父對象放在資源資料夾中)
    Transform weaponMullze; //武器的槍口位置

    [SerializeField] WeaponShootMode ShootMode; //射擊模式                    shootType
    float delayBetweenShoots = 0.5f; //射擊間隔時間
    float timeSinceLastShoot; //距離上次射擊完的時間
    float AmmoReloadDelay = 2f; //射完之後到可以補充彈藥量的間隔時間      

    float currentAmmo; //當前彈藥量
    float maxAmmo = 10; //最大彈藥量               
    float AmmoPerShoot; //每次射擊時消耗的彈藥量         bulletPerShoot 
    float AmmoReloadRate = 1f; //每秒補充的彈藥量                            

    bool isAim; //是否在瞄準狀態



    private void Start()
    {
        currentAmmo = maxAmmo;
    }

    private void Update()
    {
        UpdateAmmo(); //不停補充彈藥量
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













}
