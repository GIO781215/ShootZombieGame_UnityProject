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
    public GameObject sourcePrefab { get; set; } //�o�ӪZ�����w�s����A�]�N�O���ۦ��}��������H(�Ө�w�ܦ�����H��b�귽��Ƨ���)
    Transform weaponMullze; //�Z�����j�f��m

    [SerializeField] WeaponShootMode ShootMode; //�g���Ҧ�                    shootType
    float delayBetweenShoots = 0.5f; //�g�����j�ɶ�
    float timeSinceLastShoot; //�Z���W���g�������ɶ�
    float AmmoReloadDelay = 2f; //�g�������i�H�ɥR�u�Ķq�����j�ɶ�      

    float currentAmmo; //��e�u�Ķq
    float maxAmmo = 10; //�̤j�u�Ķq               
    float AmmoPerShoot; //�C���g���ɮ��Ӫ��u�Ķq         bulletPerShoot 
    float AmmoReloadRate = 1f; //�C��ɥR���u�Ķq                            

    bool isAim; //�O�_�b�˷Ǫ��A



    private void Start()
    {
        currentAmmo = maxAmmo;
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













}
