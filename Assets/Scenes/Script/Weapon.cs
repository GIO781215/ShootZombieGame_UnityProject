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
    float currentAmmo; //��e�u�Ķq
    float maxAmmo = 10; //�̤j�u�Ķq               
    float AmmoPerShoot; //�C���g���ɮ��Ӫ��u�Ķq         bulletPerShoot 
    float AmmoReloadRate = 1f; //�C��ɥR���u�Ķq    
    float AmmoReloadDelay = 2f; //�g�������i�H�ɥR�u�Ķq�����j�ɶ�      
    */


    bool isAim; //�O�_�b�˷Ǫ��A



    private void Start()
    {
        //currentAmmo = maxAmmo;
    }

    private void Update()
    {
        UpdateAmmo(); //�����ɥR�u�Ķq
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
            Projectile projectile = Instantiate(projectilePrefab, weaponMullze.position, Quaternion.LookRotation(weaponMullze.forward)); //���ͤl�u�ALookRotation(Vector3(x,y,z)) : ����@�ӦV�q�Ҫ�ܪ���V�]�N�@�ӦV�q�ର�ӦV�q�ҫ�����V�^
            timeLastShoot = Time.time;

        }
    }
}
