using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//1�O1���j machinegun�A2�O2���j flamethrower


public class PlayerWeaponController : MonoBehaviour
{
    /*//---------------------------------------------------------------------------
    ���ӷd���ӲM�����a��???
    �� Weapon �}������ GameObject �W��A���M�i�H��o�� GameObject ���w�����}���� List<Weapon>�A List �̭��������O���ӬO�n GameObject �~���?
    �����O�b Unity ���i�H�Ȿ���Ӹ}���� GameObject �������w���Ӹ}���� class �ܼơA�ӱo�쪺�̵M�O�o�� class �ե�Ӥ��O�ᵹ�L�� GameObject
    *///---------------------------------------------------------------------------
    [SerializeField] List<Weapon> AllWeaponsList = new List<Weapon>(); //�C�����Ҧ��������Z���A�b Unity ���N��Ҧ��Z���� prefab ������i�ӵ��L (�u�n�� Weapon �}�����b�Z���ҫ����w�s����U�A�N�i�H��������i�h)
    public Image[] machinegunUI = new Image[3];
    public Image[] flamethrowerUI = new Image[3];
    //public Sprite image; //�i������Ϥ��귽


    Weapon[] PlayerWeaponSlot = new Weapon[3]; //Player ���Z���ѡAPlayer �̦h�u��o��T�ӪZ���A���o�C���{�b�̦h�N�u�����Z���Ӥw�A�ҥH�Z�� weaponUI �N�u�����] 2 �Ӥw

    int currentWeaponSlotIndex; //�ثe�b�Z���Ѥ���ܪ��Z�������ޭȡA-1 �N��ثe�S����ܥ���j
    [SerializeField] Transform equipPosition; //�˳ƪZ������m (�q Unity �����w���L)
    bool isAim; //�ϧ_�B��˷Ǫ��A
    bool toAim = false; //true �N���讳�_�Z�������A�Afalse �N��S���Z���Τw�g���Z�������A
    PlayerController playerController;
    InputController inputController;





    void Start()
    {

        inputController = GameManager.Instance.inputController;
        playerController = GetComponent<PlayerController>();
        playerController.onAim += OnAim;



        currentWeaponSlotIndex = -1; //�@�}�l�]�w���S�����Z��
        AddWeapon(AllWeaponsList[0]); //�[�J�Z�� machinegun (�@�}�l�N�o�� machinegun)
        /* //�����a�o��Ҧ��Z�� 
        foreach (Weapon weapon in AllWeaponsList) 
        {
            AddWeapon(weapon);
        }
          */




        //��l�ƪZ�� UI�A�ܤp�ܦǩҦ��Z���� UI
        machinegunUI[0].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        flamethrowerUI[0].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        machinegunUI[0].color = new Color(170f / 255f, 170f / 255f, 170f / 255f);
        machinegunUI[1].color = new Color(170f / 255f, 170f / 255f, 170f / 255f);
        machinegunUI[2].color = new Color(170f / 255f, 170f / 255f, 170f / 255f);
        flamethrowerUI[0].color = new Color(170f / 255f, 170f / 255f, 170f / 255f);
        flamethrowerUI[1].color = new Color(170f / 255f, 170f / 255f, 170f / 255f);
        flamethrowerUI[2].color = new Color(170f / 255f, 170f / 255f, 170f / 255f);

    }


    void Update()
    {
        hasSwitchWeaponInput(); //�P�_���S�����U�����Z����n�����Z��

        //�B�z�g��
        if (currentWeaponSlotIndex != -1 && isAim && !toAim) //�p�G�ثe����ܪZ���A�B���˷Ǫ��A
        {
            PlayerWeaponSlot[currentWeaponSlotIndex].HandleShootInput(inputController.GetMouseLeftKeyDown(), inputController.GetMouseLeftKeyHeldDown(), inputController.GetMouseLeftKeyUp()); //�B�z�g��
        }
 

       
        //������s�Ҧ��Z���� UI ���u�Ķq��
        foreach (Weapon weapon in PlayerWeaponSlot)
        {
            if (weapon != null && weapon.weaponType == WeaponType.machinegun)
            {
                machinegunUI[1].fillAmount = Mathf.Lerp(machinegunUI[1].fillAmount, weapon.CurrentAmmoRatio_machinegun(), 0.05f);
            }
            else if (weapon != null && weapon.weaponType == WeaponType.flamethrower)
            {
                flamethrowerUI[1].fillAmount = Mathf.Lerp(flamethrowerUI[1].fillAmount, weapon.CurrentAmmoRatio_flamethrower(), 0.3f);
            }
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

                //��ܥ[�J���Z����UI
                if (weapon == AllWeaponsList[0]) //�Z���O machinegun
                {
                    machinegunUI[2].enabled = false;

                }
                else if (weapon == AllWeaponsList[1])  //�Z���O flamethrower
                {
                    flamethrowerUI[2].enabled = false;
                }

                //print("���\�[�J�Z�� " + weaponInstance);
                return;
            }
        }
        print("�Z���w�g���F�A�L�k�A�[�J�Z��");
        return;
    }

    private bool HasWeapon(Weapon weapon) //�ˬd�O�_�w�g�� weapon �o��Z��
    {
        foreach (Weapon PWS_weapon in PlayerWeaponSlot)
        {
            if (PWS_weapon != null && PWS_weapon.weaponType == weapon.weaponType)
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
            //�ܤp�ܦǩҦ��Z���� UI
            machinegunUI[0].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            flamethrowerUI[0].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            machinegunUI[0].color = new Color(170f / 255f, 170f / 255f, 170f / 255f);
            machinegunUI[1].color = new Color(170f / 255f, 170f / 255f, 170f / 255f);
            machinegunUI[2].color = new Color(170f / 255f, 170f / 255f, 170f / 255f);
            flamethrowerUI[0].color = new Color(170f / 255f, 170f / 255f, 170f / 255f);
            flamethrowerUI[1].color = new Color(170f / 255f, 170f / 255f, 170f / 255f);
            flamethrowerUI[2].color = new Color(170f / 255f, 170f / 255f, 170f / 255f);


            //�u��ܨ���Z��
            if (index != -1)
            {
                //print("�������Z�� " + PlayerWeaponSlot[index]);
                PlayerWeaponSlot[index].ShowWeapon();

                //��ܪZ���� UI
                if (PlayerWeaponSlot[index].weaponType == WeaponType.machinegun)
                {
                    machinegunUI[0].transform.localScale = new Vector3(1f, 1f, 1f);
                    machinegunUI[1].color = Color.white;
                }
                else if(PlayerWeaponSlot[index].weaponType == WeaponType.flamethrower)
                {
                    flamethrowerUI[0].transform.localScale = new Vector3(1f, 1f, 1f);
                    flamethrowerUI[1].color = Color.white;
                }
            }
            else
            {
                //print("�����Z�� " );
            }
        }
    }



    private void  OnAim(bool value)
    {
        if(isAim == false && value == true) //�p�G��褣�O�˷ǰʧ@�A��M�ܦ��˷ǰʧ@
        {
            toAim = true;
        }
        else
        {
            toAim = false;
        }

        //�p�G���S���j�A��M�ܦ��˷ǰʧ@ -> �Τ@�ܦ����Ĥ@��j
        if (currentWeaponSlotIndex == -1)
        {
            currentWeaponSlotIndex = 0;
            switchWeapon(currentWeaponSlotIndex);
        }

        isAim = value;
    }


    public void PickUpWeapon(GameObject gameObject)
    {     
        if (gameObject != null  )
        {
            PickUpItem pickUpItem = gameObject.GetComponent<PickUpItem>();
            if(pickUpItem != null )
            {
                if(pickUpItem.weaponType == WeaponType.flamethrower)
                {
                    AddWeapon(AllWeaponsList[1]); //�ߨ���K�j -> �[�J�Z�� flamethrower
                }
            }
        }
    }


}
