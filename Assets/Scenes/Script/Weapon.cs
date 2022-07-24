using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum WeaponType //武器的射擊模式 (其實這應該要寫成類繼承才對 : 武器通用屬性 <- machinegun or flamethrower)
{
    machinegun,    
    flamethrower,  
}


public class Weapon : MonoBehaviour
{
    [SerializeField] public GameObject sourcePrefab { get; set; } //這個武器的預製物件，也就是掛著此腳本的父對象(而其已變成父對象放在資源資料夾中)
    [SerializeField] public Transform weaponMullze; //武器的槍口位置
    [SerializeField] public MachinegunProjectile projectilePrefab; //子彈的預製物件，machinegun 就丟 MachinegunProjectile 給他，flamethrower 不用丟給他，其只會用到射擊特效預製物件而已

    [Header("槍的種類")]
    [SerializeField] public WeaponType weaponType; //槍的種類
    float machinegunDelayBetweenShoots = 0.1f; //射擊間隔時間
    float flamethrowerDelayBetweenShoots = 0.05f; //射擊間隔時間

    float timeLastShoot_machinegun = -Mathf.Infinity; //上次射擊完的時間
    float timeLastShoot_flamethrower = -Mathf.Infinity; //上次射擊完的時間

    //武器彈藥量參數
    float maxAmmo_machinegun = 100; //最大彈藥量               
    float currentAmmo_machinegun; //當前彈藥量
    float AmmoPerShoot_machinegun = 5f; //每次射擊時消耗的彈藥量         
    float AmmoReloadRate_machinegun = 10f; //每秒補充的彈藥速度 
    float AmmoReloadDelay_machinegun = 0.3f; //射完之後到可以補充彈藥量的間隔時間      
    bool canAmmoReload_machinegun = true; //可補充彈藥量旗標

    float maxAmmo_flamethrower = 100; //最大彈藥量               
    float currentAmmo_flamethrower; //當前彈藥量
    float AmmoPerShoot_flamethrower = 2f; //每次射擊時消耗的彈藥量         
    float AmmoReloadRate_flamethrower = 20f; //每秒補充的彈藥速度    
    float AmmoReloadDelay_flamethrower = 2f; //射完之後到可以補充彈藥量的間隔時間     
    bool canAmmoReload_flamethrower = true; //可補充彈藥量旗標



    bool isAim; //是否在瞄準狀態

    [SerializeField] public GameObject ShootEffectPrefab; //射擊特效預製物件，machinegun 就丟 FX_BloodSplatter 給他，flamethrower 就丟 FlameThrower 給他

    Ray shootRay; //射出子彈方向的射線

    [SerializeField] AudioClip sound_MachinegunShoot;
    [SerializeField] AudioClip sound_FlamethrowerShoot;
    [SerializeField] AudioClip sound_SwitchWeapon;

    AudioSource audioSource;





    private void Start()
    {
        currentAmmo_machinegun = maxAmmo_machinegun;
        currentAmmo_flamethrower = maxAmmo_flamethrower;
        audioSource = GetComponent<AudioSource>();
    }


    private void Update()
    {
        UpdateAmmo(); //不停補充彈藥量

        /* 最後覺得不該抓畫面的座標點來控制發射方向，邊走邊射會有 bug
        // Camera.main.ScreenPointToRay() : 返回一條射線從攝像機通過一個屏幕點，產生的射線是在世界空間中，從相機的近裁剪面開始並穿過屏幕上的點 position(x, y)，position.z 會被忽略，屏幕的左下為(0, 0)，右上是 (pixelWidth, pixelHeight)
        Ray shootRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2 , Screen.height / 2));
        Debug.DrawLine(weaponMullze.transform.position, shootRay.GetPoint(10), Color.blue); //shootRay.GetPoint(distance) : 返回射線上 distance 個單位處的點。
        weaponMullze.transform.rotation = Quaternion.LookRotation(shootRay.GetPoint(10) - weaponMullze.transform.position);  
        */

        /*//---------------------------------------------------------------------------------------------------
        1.創建一條射線，從自己出發，發射向目標
        Ray shootRay = new Ray(transform.position, target.position - transform.position);    // 第一個參數是射線的起點 shootRay.origin，第二個參數是射線的方向 shootRay.direction

        2.繪製出射線，方便調試（在Scene視角能看見，Game視角看不見）
        Debug.DrawRay(shootRay.origin, shootRay.direction * 100, Color.red);

        例子:
        Ray shootRay = new Ray(new Vector3(0, 5, 0), new Vector3(0, 5, 100) - (new Vector3(0, 5, 0)));
        Debug.DrawRay(shootRay.origin, shootRay.direction * 100, Color.red);
        *///---------------------------------------------------------------------------------------------------


        //---------------- 得到攝影機俯仰轉動的角度 ----------------
        float angle = 0;
        angle = Camera.main.transform.rotation.eulerAngles.x;
        if (angle >= 180) angle -= 360;
        angle = 15 - angle;
        //Debug.Log(angle); //angle 初始值是 0，範圍在 -5 ~ 25 之間
        //----------------------------------------------------------

        //創建一條從攝影機向其前方發出的射線 
        shootRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward + /*初始斜度補正*/ new Vector3(0, + 0.2f, 0) + /*俯仰轉動倍率加成*/ new Vector3(0, 0.015f, 0) * angle);   
        Debug.DrawRay(shootRay.origin, shootRay.direction * 10, Color.red);
        weaponMullze.transform.rotation = Quaternion.LookRotation(shootRay.GetPoint(10) - shootRay.GetPoint(0)); //將 weaponMullze 的朝向設成與 shootRay 相同





        //補充武器彈藥
        reloadAmmo_machinegun();
        reloadAmmo_flamethrower();
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
        switch (weaponType) //先判斷是使用哪種武器
        {
            case WeaponType.machinegun:
                if(inputDown)
                {
                    //print("machinegun 射擊");
                    machinegunShoot();
                }
                return;

            case WeaponType.flamethrower:
                if (inputHeld)
                {
                    //print("flamethrower 射擊");
                    flamethrowerShoot();
                }
                return;
            default:
                return;
        }

    }

    private void machinegunShoot()
    {
        if (timeLastShoot_machinegun + machinegunDelayBetweenShoots <  Time.time && currentAmmo_machinegun >= AmmoPerShoot_machinegun)
        {
            //減少彈藥量
            currentAmmo_machinegun -= AmmoPerShoot_machinegun;
            currentAmmo_machinegun = Mathf.Max(currentAmmo_machinegun, 0);
            if (currentAmmo_machinegun < AmmoPerShoot_machinegun)
                currentAmmo_machinegun = 0;
            canAmmoReload_machinegun = false;


            if (projectilePrefab != null)
            {
                MachinegunProjectile projectile = Instantiate(projectilePrefab, weaponMullze.position, Quaternion.LookRotation(weaponMullze.forward)); //產生子彈，LookRotation(Vector3(x,y,z)) : 獲取一個向量所表示的方向（將一個向量轉為該向量所指的方向）
                audioSource.PlayOneShot(sound_MachinegunShoot);
            }
            timeLastShoot_machinegun = Time.time;



            /* //射擊噴發效果，感覺挺干擾射擊，還是不要用好了...
            if(ShootEffectPrefab != null)
            {
                GameObject shootEffect = Instantiate(ShootEffectPrefab, weaponMullze.position, Quaternion.LookRotation(weaponMullze.forward));
                Destroy(shootEffect, 1.5f);
            }
            */
        }
    }


    private void flamethrowerShoot()
    {
        if (timeLastShoot_flamethrower + flamethrowerDelayBetweenShoots < Time.time && currentAmmo_flamethrower >= AmmoPerShoot_flamethrower)
        {
            //減少彈藥量
            currentAmmo_flamethrower -= AmmoPerShoot_flamethrower;
            currentAmmo_flamethrower = Mathf.Max(currentAmmo_flamethrower, 0);
            if (currentAmmo_flamethrower < AmmoPerShoot_flamethrower)
                currentAmmo_flamethrower = 0;
            canAmmoReload_flamethrower = false;


            if (ShootEffectPrefab != null)
            {
                GameObject shootEffect = Instantiate(ShootEffectPrefab, weaponMullze.position, Quaternion.LookRotation(weaponMullze.forward));
                audioSource.PlayOneShot(sound_FlamethrowerShoot);
                Destroy(shootEffect, 2f);
            }
            timeLastShoot_flamethrower = Time.time;
        }

    }



   void reloadAmmo_machinegun()  
    {
        if (timeLastShoot_machinegun + AmmoReloadDelay_machinegun < Time.time) //射擊過程中不會補充彈藥量，射完經過一段時間才能繼續補充彈藥量
        {
            canAmmoReload_machinegun = true;
        }

            if (currentAmmo_machinegun < maxAmmo_machinegun && canAmmoReload_machinegun == true)
        {
            currentAmmo_machinegun += AmmoReloadRate_machinegun * Time.deltaTime;
        }
    }

    void reloadAmmo_flamethrower()  
    {
        if (timeLastShoot_flamethrower + AmmoReloadDelay_flamethrower < Time.time) //射擊過程中不會補充彈藥量，射完經過一段時間才能繼續補充彈藥量
        {
            canAmmoReload_flamethrower = true;
        }

        if (currentAmmo_flamethrower < maxAmmo_flamethrower && canAmmoReload_flamethrower == true)
        {
            currentAmmo_flamethrower += AmmoReloadRate_flamethrower * Time.deltaTime;
        }
    }

    public float CurrentAmmoRatio_machinegun() //獲得武器彈藥量的百分比
    {
        return currentAmmo_machinegun / maxAmmo_machinegun;
    }

    public float CurrentAmmoRatio_flamethrower() //獲得武器彈藥量的百分比
    {
        return currentAmmo_flamethrower / maxAmmo_flamethrower;
    }
}
