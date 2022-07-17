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
    float timeLastShoot = -Mathf.Infinity; //�W���g�������ɶ�

    /* �����]�p�����ݭn�u�Ķq���C������
    float currentAmmo; //��e�u�Ķq
    float maxAmmo = 10; //�̤j�u�Ķq               
    float AmmoPerShoot; //�C���g���ɮ��Ӫ��u�Ķq         bulletPerShoot 
    float AmmoReloadRate = 1f; //�C��ɥR���u�Ķq    
    float AmmoReloadDelay = 2f; //�g�������i�H�ɥR�u�Ķq�����j�ɶ�      
    */
    bool isAim; //�O�_�b�˷Ǫ��A

    [SerializeField] public GameObject ShootEffectPrefab; //�g���S�Ĺw�s����Amachinegun �N�� FX_BloodSplatter ���L�Aflamethrower �N�� FlameThrower ���L

    Ray shootRay; //�g�X�l�u��V���g�u









    private void Start()
    {
        //currentAmmo = maxAmmo;
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
        if (timeLastShoot + machinegunDelayBetweenShoots <  Time.time)
        {
            MachinegunProjectile projectile = Instantiate(projectilePrefab, weaponMullze.position, Quaternion.LookRotation(weaponMullze.forward)); //���ͤl�u�ALookRotation(Vector3(x,y,z)) : ����@�ӦV�q�Ҫ�ܪ���V�]�N�@�ӦV�q�ର�ӦV�q�ҫ�����V�^
            timeLastShoot = Time.time;

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
        if (timeLastShoot + flamethrowerDelayBetweenShoots < Time.time)
        {
            if (ShootEffectPrefab != null)
            {
                GameObject shootEffect = Instantiate(ShootEffectPrefab, weaponMullze.position, Quaternion.LookRotation(weaponMullze.forward));
                Destroy(shootEffect, 2f);
            }
            timeLastShoot = Time.time;
        }

    }



        
}
