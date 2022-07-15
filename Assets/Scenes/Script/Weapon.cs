using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum WeaponShootMode //�Z�����g���Ҧ�
{
    Single,    //��o
    Automatic, //�s�g
}


public class Weapon : MonoBehaviour
{
    [SerializeField] public GameObject sourcePrefab { get; set; } //�o�ӪZ�����w�s����A�]�N�O���ۦ��}��������H(�Ө�w�ܦ�����H��b�귽��Ƨ���)
    [SerializeField] public Transform weaponMullze; //�Z�����j�f��m
    [SerializeField] public Projectile projectilePrefab; //�l�u���w�s����
    [SerializeField] public WeaponShootMode ShootMode; //�g���Ҧ�                    shootType
    float delayBetweenShoots = 0.1f; //�g�����j�ɶ�
    float timeLastShoot = -Mathf.Infinity; //�W���g�������ɶ�

    /* �����]�p�����ݭn�u�Ķq���C������
    float currentAmmo; //���e�u�Ķq
    float maxAmmo = 10; //�̤j�u�Ķq               
    float AmmoPerShoot; //�C���g���ɮ��Ӫ��u�Ķq         bulletPerShoot 
    float AmmoReloadRate = 1f; //�C���ɥR���u�Ķq    
    float AmmoReloadDelay = 2f; //�g�������i�H�ɥR�u�Ķq�����j�ɶ�      
    */


    bool isAim; //�O�_�b�˷Ǫ��A

    bool i = true;

    private void Start()
    {
        //currentAmmo = maxAmmo;
    }

    private void Update()
    {
        UpdateAmmo(); //�����ɥR�u�Ķq

        /* �̫�ı�o���ӧ�e�����y���I�ӱ���o�g��V�A�䨫��g�|�� bug
        // Camera.main.ScreenPointToRay() : ��^�@���g�u�q�ṳ���q�L�@�ӫ̹��I�A���ͪ��g�u�O�b�@�ɪŶ����A�q�۾�������ŭ��}�l�ì�L�̹��W���I position(x, y)�Aposition.z �|�Q�����A�̹������U��(0, 0)�A�k�W�O (pixelWidth, pixelHeight)
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2 , Screen.height / 2));
        Debug.DrawLine(weaponMullze.transform.position, ray.GetPoint(10), Color.blue); //ray.GetPoint(distance) : ��^�g�u�W distance �ӳ��B���I�C
        weaponMullze.transform.rotation = Quaternion.LookRotation(ray.GetPoint(10) - weaponMullze.transform.position);  
        */

        /*//---------------------------------------------------------------------------------------------------
        1.�Ыؤ@���g�u�A�q�ۤv�X�o�A�o�g�V�ؼ�
        Ray ray = new Ray(transform.position, target.position - transform.position);    // �Ĥ@�ӰѼƬO�g�u���_�I ray.origin�A�ĤG�ӰѼƬO�g�u����V ray.direction

        2.ø�s�X�g�u�A��K�ոա]�bScene������ݨ��AGame�����ݤ����^
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);

        �Ҥl:
        Ray R1 = new Ray(new Vector3(0, 5, 0), new Vector3(0, 5, 100) - (new Vector3(0, 5, 0)));
        Debug.DrawRay(R1.origin, R1.direction * 100, Color.red);
        *///---------------------------------------------------------------------------------------------------


        //---------------- �o����v��������ʪ����� ----------------
        float angle = 0;
        angle = Camera.main.transform.rotation.eulerAngles.x;
        if (angle >= 180) angle -= 360;
        angle = 15 - angle;
        Debug.Log(angle); //angle ��l�ȬO 0�A�d��b -5 ~ 25 ����
        //----------------------------------------------------------

        //�Ыؤ@���q��v���V��e��o�X���g�u 
        Ray R1 = new Ray(Camera.main.transform.position, Camera.main.transform.forward + /*��l�׫׸ɥ�*/ new Vector3(0, + 0.2f, 0) + /*������ʭ��v�[��*/ new Vector3(0, 0.015f, 0) * angle);   
        Debug.DrawRay(R1.origin, R1.direction * 10, Color.red);
        weaponMullze.transform.rotation = Quaternion.LookRotation(R1.GetPoint(10) - R1.GetPoint(0)); //�N weaponMullze ���¦V�]���P R1 �ۦP

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
        switch (ShootMode) //���P�_�ۤv�{�b�B����خg���Ҧ�
        {
            case WeaponShootMode.Single:
                if(inputDown)
                {
                    print("Single�g��");
                    SingleShoot();
                }
                return;

            case WeaponShootMode.Automatic:
                if (inputHeld)
                {
                    print("Auto�g��");
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
            Projectile projectile = Instantiate(projectilePrefab, weaponMullze.position, Quaternion.LookRotation(weaponMullze.forward)); //���ͤl�u�ALookRotation(Vector3(x,y,z)) : ����@�ӦV�q�Ҫ��ܪ���V�]�N�@�ӦV�q�ର�ӦV�q�ҫ�����V�^
            timeLastShoot = Time.time;

        }
    }
}