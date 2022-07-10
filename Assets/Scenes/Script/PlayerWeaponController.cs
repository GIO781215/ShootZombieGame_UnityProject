using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//1�O1���j machinegun�A2�O2���j flamethrower


public class PlayerWeaponController : MonoBehaviour
{
    /*//---------------------------------------------------------------------------
    ���ӷd���ӲM�����a��???
    �� Weapon �}������ GameObject �W��A���M�i�H��o�� GameObject ���w�����}���� List<Weapon>�A List �̭��������O���ӬO�n GameObject �~���?
    �����O�b Unity ���i�H�Ȿ���Ӹ}���� GameObject �������w���Ӹ}���� class �ܼơA�ӱo�쪺�̵M�O�o�� class �ե�Ӥ��O�ᵹ�L�� GameObject
    *///---------------------------------------------------------------------------
    [SerializeField] List<Weapon> AllWeaponsList = new List<Weapon>(); //�C�����Ҧ��������Z���A�b Unity ���N��Ҧ��Z���� prefab ������i�ӵ��L (�u�n�� Weapon �}�����b�Z���ҫ����w�s����U�A�N�i�H��������i�h)

    Weapon[] PlayerWeaponSlot = new Weapon[3]; //Player ���Z���ѡAPlayer �̦h�u��o��T�ӪZ��
    int currentWeaponSlotIndex; //�ثe�b�Z���Ѥ���ܪ��Z�������ޭȡA-1 �N��ثe�S����ܥ���j
    [SerializeField] Transform equipPosition; //�˳ƪZ������m (�q Unity �����w���L)
    bool isAim; //�ϧ_�B��˷Ǫ��A

    PlayerController playerController;
    InputController inputController;

 


    void Start()
    {
        currentWeaponSlotIndex = -1; //�@�}�l�]�w���S�����Z��

        inputController = GameManager.Instance.inputController;
        playerController = GetComponent<PlayerController>();
        playerController.onAim += OnAim;

        //    playerController.onAim += OnAim; //�� playerController �}����P���}���� isAim �X�ЦP�B
        //playerController��omAim�O??????????

        foreach (Weapon weapon in AllWeaponsList) //�o��������a�b�@�}�l�N�o��Ҧ��Z�� 
        {
            AddWeapon(weapon);
        }


     }


    void Update()
    {
        hasSwitchWeaponInput(); //�P�_���S�����U�����Z����n�����Z��

        //�B�z�g��
        if (currentWeaponSlotIndex != -1 && isAim) //�p�G�ثe����ܪZ���A�B���˷Ǫ��A
        {
            PlayerWeaponSlot[currentWeaponSlotIndex].HandleShootInput(inputController.GetMouseLeftKeyDown(), inputController.GetMouseLeftKeyHeldDown(), inputController.GetMouseLeftKeyUp()); //�B�z�g��
        }
 
    }







    private void AddWeapon(Weapon weapon) //�N�Z���[�J���a���Z���Ѥ�
    {
        if (weapon == null)
            return;

        if (HasWeapon(weapon)) //�p�G�w�g�� weapon �o��Z��
        {
            print("�w�g���ۦP���Z���A�L�k�A�[�J�@��");
            return;
        }
        for (int i = 0; i < PlayerWeaponSlot.Length; i++)
        {
            if (PlayerWeaponSlot[i] == null)
            {
                Weapon weaponInstance = Instantiate(weapon, equipPosition); //��ҤƤ@�� Weapon �}�� 
                weaponInstance.sourcePrefab = weapon.gameObject; //���o�� Weapon �}���� sourcePrefab ���w��l��         
                PlayerWeaponSlot[i] = weaponInstance; //---------------------------------------------------------------------�ҥH AllWeaponsList ���� Weapon �٤��O����A�� PlayerWeaponSlot[] ���� Weapon �O�����???
                PlayerWeaponSlot[i].HiddenWeapon(); //�������
                //print("���\�[�J�Z�� " + weaponInstance);
                return;
            }
        }
        print("�Z���w�g���F�A�L�k�A�[�J�Z��");
        return;
    }

    private bool HasWeapon(Weapon weaponPrefab) //�ˬd�O�_�w�g�� weaponPrefab �o��Z��
    {
        foreach (Weapon weapon in PlayerWeaponSlot)
        {
            if (weapon != null && weapon.sourcePrefab == weaponPrefab)
            {
                return true;
            }
        }
        return false;
    }

    private bool HasWeaponAtIndex(Weapon weaponPrefab) //�ˬd�b�o�Ӱ}�C���ޭȤW�O�_���Z��
    {
        foreach (Weapon weapon in PlayerWeaponSlot)
        {
            if (weapon != null && weapon.sourcePrefab == weaponPrefab)
            {
                return true;
            }
        }
        return false;
    }


    private void hasSwitchWeaponInput() //�P�_���S�����U�����Z����n�����Z��
    {
        if (inputController.GetKeyCInput())
        {
            //���k���� index
            currentWeaponSlotIndex += 1;
            if (currentWeaponSlotIndex > PlayerWeaponSlot.Length - 1) 
                currentWeaponSlotIndex = -1;
            if (currentWeaponSlotIndex != -1 && PlayerWeaponSlot[currentWeaponSlotIndex] == null)
                currentWeaponSlotIndex = -1;

            //�p�G�w�g�b�˷ǰʧ@ �L�k���������j�����A <--- �ڳo�˼g�O�w�]�F�û����|���@��Z���A�i�ण�Ӧn�A�H��n��A��
            if (currentWeaponSlotIndex == -1 && isAim)
                currentWeaponSlotIndex += 1;

            switchWeapon(currentWeaponSlotIndex); //����������Z��
        }
        else if (inputController.GetKeyVInput())
        {
            //�������� index
            currentWeaponSlotIndex -= 1;
            if (currentWeaponSlotIndex <  -1)
                currentWeaponSlotIndex = PlayerWeaponSlot.Length - 1;
            while (currentWeaponSlotIndex != -1 && PlayerWeaponSlot[currentWeaponSlotIndex] == null) //���L�ż�
            {
                currentWeaponSlotIndex -= 1;
            }

            //�p�G�w�g�b�˷ǰʧ@ �L�k���������j�����A <--- �ڳo�˼g�O�w�]�F�û����|���@��Z���A�i�ण�Ӧn�A�H��n��A��
            if (currentWeaponSlotIndex == -1 && isAim)
                currentWeaponSlotIndex = PlayerWeaponSlot.Length - 1;
            while (currentWeaponSlotIndex != -1 && PlayerWeaponSlot[currentWeaponSlotIndex] == null) //���L�ż�
            {
                currentWeaponSlotIndex -= 1;
            }

            switchWeapon(currentWeaponSlotIndex); //����������Z��
        }
    }

    private void switchWeapon(int index) //�����Z���A���u�O�b���Z��������/��ܦӤw
    {
        if (index >= -1 && index < PlayerWeaponSlot.Length)
        {
            //�����éҦ��Z��
            foreach (Weapon weapon in PlayerWeaponSlot)
            {
                if (weapon != null)
                {
                    weapon.HiddenWeapon();
                }
            }
            //�u��ܨ���Z��
            if (index != -1)
            {
                //print("�������Z�� " + PlayerWeaponSlot[index]);
                PlayerWeaponSlot[index].ShowWeapon();
            }
            else
            {
                //print("�����Z�� " );
            }
        }
    }



    private void  OnAim(bool value)
    {
        //�p�G���S���j�A��M�ܦ��˷ǰʧ@ -> �Τ@�ܦ����Ĥ@��j
        if(currentWeaponSlotIndex == -1)
        {
            currentWeaponSlotIndex = 0;
            switchWeapon(currentWeaponSlotIndex);
        }

        isAim = value;
    }





}
