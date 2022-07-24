using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum WeaponType //�Z�����g���Ҧ� (���o���ӭn�g�����~�Ӥ~�� : �Z���q���ݩ� <- machinegun or flamethrower)
{
    machinegun,    
    flamethrower,  
}


public class Weapon : MonoBehaviour
{
    [SerializeField] public GameObject sourcePrefab { get; set; } //�o�ӪZ�����w�s����A�]�N�O���ۦ��}��������H(�Ө�w�ܦ�����H��b�귽��Ƨ���)
    [SerializeField] public Transform weaponMullze; //�Z�����j�f��m
    [SerializeField] public MachinegunProjectile projectilePrefab; //�l�u���w�s����Amachinegun �N�� MachinegunProjectile ���L�Aflamethrower ���Υᵹ�L�A��u�|�Ψ�g���S�Ĺw�s����Ӥw

    [Header("�j������")]
    [SerializeField] public WeaponType weaponType; //�j������
    float machinegunDelayBetweenShoots = 0.1f; //�g�����j�ɶ�
    float flamethrowerDelayBetweenShoots = 0.05f; //�g�����j�ɶ�

    float timeLastShoot_machinegun = -Mathf.Infinity; //�W���g�������ɶ�
    float timeLastShoot_flamethrower = -Mathf.Infinity; //�W���g�������ɶ�

    //�Z���u�Ķq�Ѽ�
    float maxAmmo_machinegun = 100; //�̤j�u�Ķq               
    float currentAmmo_machinegun; //��e�u�Ķq
    float AmmoPerShoot_machinegun = 5f; //�C���g���ɮ��Ӫ��u�Ķq         
    float AmmoReloadRate_machinegun = 10f; //�C��ɥR���u�ĳt�� 
    float AmmoReloadDelay_machinegun = 0.3f; //�g�������i�H�ɥR�u�Ķq�����j�ɶ�      
    bool canAmmoReload_machinegun = true; //�i�ɥR�u�Ķq�X��

    float maxAmmo_flamethrower = 100; //�̤j�u�Ķq               
    float currentAmmo_flamethrower; //��e�u�Ķq
    float AmmoPerShoot_flamethrower = 2f; //�C���g���ɮ��Ӫ��u�Ķq         
    float AmmoReloadRate_flamethrower = 20f; //�C��ɥR���u�ĳt��    
    float AmmoReloadDelay_flamethrower = 2f; //�g�������i�H�ɥR�u�Ķq�����j�ɶ�     
    bool canAmmoReload_flamethrower = true; //�i�ɥR�u�Ķq�X��



    bool isAim; //�O�_�b�˷Ǫ��A

    [SerializeField] public GameObject ShootEffectPrefab; //�g���S�Ĺw�s����Amachinegun �N�� FX_BloodSplatter ���L�Aflamethrower �N�� FlameThrower ���L

    Ray shootRay; //�g�X�l�u��V���g�u

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
        UpdateAmmo(); //�����ɥR�u�Ķq

        /* �̫�ı�o���ӧ�e�����y���I�ӱ���o�g��V�A�䨫��g�|�� bug
        // Camera.main.ScreenPointToRay() : ��^�@���g�u�q�ṳ���q�L�@�ӫ̹��I�A���ͪ��g�u�O�b�@�ɪŶ����A�q�۾�������ŭ��}�l�ì�L�̹��W���I position(x, y)�Aposition.z �|�Q�����A�̹������U��(0, 0)�A�k�W�O (pixelWidth, pixelHeight)
        Ray shootRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2 , Screen.height / 2));
        Debug.DrawLine(weaponMullze.transform.position, shootRay.GetPoint(10), Color.blue); //shootRay.GetPoint(distance) : ��^�g�u�W distance �ӳ��B���I�C
        weaponMullze.transform.rotation = Quaternion.LookRotation(shootRay.GetPoint(10) - weaponMullze.transform.position);  
        */

        /*//---------------------------------------------------------------------------------------------------
        1.�Ыؤ@���g�u�A�q�ۤv�X�o�A�o�g�V�ؼ�
        Ray shootRay = new Ray(transform.position, target.position - transform.position);    // �Ĥ@�ӰѼƬO�g�u���_�I shootRay.origin�A�ĤG�ӰѼƬO�g�u����V shootRay.direction

        2.ø�s�X�g�u�A��K�ոա]�bScene������ݨ��AGame�����ݤ����^
        Debug.DrawRay(shootRay.origin, shootRay.direction * 100, Color.red);

        �Ҥl:
        Ray shootRay = new Ray(new Vector3(0, 5, 0), new Vector3(0, 5, 100) - (new Vector3(0, 5, 0)));
        Debug.DrawRay(shootRay.origin, shootRay.direction * 100, Color.red);
        *///---------------------------------------------------------------------------------------------------


        //---------------- �o����v��������ʪ����� ----------------
        float angle = 0;
        angle = Camera.main.transform.rotation.eulerAngles.x;
        if (angle >= 180) angle -= 360;
        angle = 15 - angle;
        //Debug.Log(angle); //angle ��l�ȬO 0�A�d��b -5 ~ 25 ����
        //----------------------------------------------------------

        //�Ыؤ@���q��v���V��e��o�X���g�u 
        shootRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward + /*��l�׫׸ɥ�*/ new Vector3(0, + 0.2f, 0) + /*������ʭ��v�[��*/ new Vector3(0, 0.015f, 0) * angle);   
        Debug.DrawRay(shootRay.origin, shootRay.direction * 10, Color.red);
        weaponMullze.transform.rotation = Quaternion.LookRotation(shootRay.GetPoint(10) - shootRay.GetPoint(0)); //�N weaponMullze ���¦V�]���P shootRay �ۦP





        //�ɥR�Z���u��
        reloadAmmo_machinegun();
        reloadAmmo_flamethrower();
    }

    private void UpdateAmmo()
    {
        //�ɥR�u�Ķq
    }

    public void ShowWeapon() //��ܪZ���ҫ�
    {
        foreach (Transform child in transform) //�M���l��H
        {
            Renderer render = child.gameObject.GetComponent<Renderer>();
            if (render != null)
            {
                render.enabled = true;
            }
        }
    }

    public void HiddenWeapon() //���êZ���ҫ�
    {
        foreach (Transform child in transform) //�M���l��H
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
        switch (weaponType) //���P�_�O�ϥέ��تZ��
        {
            case WeaponType.machinegun:
                if(inputDown)
                {
                    //print("machinegun �g��");
                    machinegunShoot();
                }
                return;

            case WeaponType.flamethrower:
                if (inputHeld)
                {
                    //print("flamethrower �g��");
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
            //��ּu�Ķq
            currentAmmo_machinegun -= AmmoPerShoot_machinegun;
            currentAmmo_machinegun = Mathf.Max(currentAmmo_machinegun, 0);
            if (currentAmmo_machinegun < AmmoPerShoot_machinegun)
                currentAmmo_machinegun = 0;
            canAmmoReload_machinegun = false;


            if (projectilePrefab != null)
            {
                MachinegunProjectile projectile = Instantiate(projectilePrefab, weaponMullze.position, Quaternion.LookRotation(weaponMullze.forward)); //���ͤl�u�ALookRotation(Vector3(x,y,z)) : ����@�ӦV�q�Ҫ�ܪ���V�]�N�@�ӦV�q�ର�ӦV�q�ҫ�����V�^
                audioSource.PlayOneShot(sound_MachinegunShoot);
            }
            timeLastShoot_machinegun = Time.time;



            /* //�g���Q�o�ĪG�A�Pı���z�Z�g���A�٬O���n�Φn�F...
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
            //��ּu�Ķq
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
        if (timeLastShoot_machinegun + AmmoReloadDelay_machinegun < Time.time) //�g���L�{�����|�ɥR�u�Ķq�A�g���g�L�@�q�ɶ��~���~��ɥR�u�Ķq
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
        if (timeLastShoot_flamethrower + AmmoReloadDelay_flamethrower < Time.time) //�g���L�{�����|�ɥR�u�Ķq�A�g���g�L�@�q�ɶ��~���~��ɥR�u�Ķq
        {
            canAmmoReload_flamethrower = true;
        }

        if (currentAmmo_flamethrower < maxAmmo_flamethrower && canAmmoReload_flamethrower == true)
        {
            currentAmmo_flamethrower += AmmoReloadRate_flamethrower * Time.deltaTime;
        }
    }

    public float CurrentAmmoRatio_machinegun() //��o�Z���u�Ķq���ʤ���
    {
        return currentAmmo_machinegun / maxAmmo_machinegun;
    }

    public float CurrentAmmoRatio_flamethrower() //��o�Z���u�Ķq���ʤ���
    {
        return currentAmmo_flamethrower / maxAmmo_flamethrower;
    }
}
