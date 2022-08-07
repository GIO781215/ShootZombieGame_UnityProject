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
    Image[] machinegunUI = new Image[3];
    Image[] flamethrowerUI = new Image[3];
    Image[] phone_machinegunUI = new Image[3];
    Image[] phone_flamethrowerUI = new Image[3];
    //public Sprite image; //�]�i������Ϥ��귽


    Weapon[] PlayerWeaponSlot = new Weapon[3]; //Player ���Z���ѡAPlayer �̦h�u��o��T�ӪZ���A���o�C���{�b�̦h�N�u�����Z���Ӥw�A�ҥH�Z�� weaponUI �N�u�����] 2 �Ӥw

    int currentWeaponSlotIndex; //�ثe�b�Z���Ѥ���ܪ��Z�������ޭȡA-1 �N��ثe�S����ܥ���j
    [SerializeField] Transform equipPosition; //�˳ƪZ������m (�q Unity �����w���L)
    bool isAim; //�ϧ_�B��˷Ǫ��A
    bool toAim = false; //true �N���讳�_�Z�������A�Afalse �N��S���Z���Τw�g���Z�������A
    PlayerController playerController;
    InputController inputController;

    [SerializeField] AudioClip sound_SwtichWeapon; //�����Z��������
    AudioSource audioSource;




    void Start()
    {

        WeaponUI_Init(); //�n����Ҧ��� UI �l�ॴ�}�~���l��

        audioSource = GetComponent<AudioSource>();
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

        //�̫�A�ӧP�_�O���ؼҦ���UI�n���
        if (GameManager.Instance.IsPhoneMode)
        {
            GameManager.Instance.inputController.SetPhoneUI_NoSound();
        }
        else
        {
            GameManager.Instance.inputController.SetComputerUI_NoSound();
        }
    }


    void Update()
    {

        if (playerController.health.currentHealth == 0)
            return;
         

        hasSwitchWeaponInput(); //�P�_���S�����U�����Z����n�����Z��

        //�B�z�g��
        if (currentWeaponSlotIndex != -1 && isAim && !toAim && !GameManager.Instance.IsPhoneMode) //�p�G�ثe����ܪZ���A�B���˷Ǫ��A�A�B���O������� UI ��
        {
            //�ηƹ�����g��
            PlayerWeaponSlot[currentWeaponSlotIndex].HandleShootInput(inputController.GetMouseLeftKeyDown(), inputController.GetMouseLeftKeyHeldDown()); //�B�z�g��

            //����L����g��
            PlayerWeaponSlot[currentWeaponSlotIndex].HandleShootInput(inputController.GetSpaceInputDown(), inputController.GetSpaceInputHold()); //�B�z�g��
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

        //������s�Ҧ��Z���� UI ���u�Ķq�� (�����)
        foreach (Weapon weapon in PlayerWeaponSlot)
        {
            if (weapon != null && weapon.weaponType == WeaponType.machinegun)
            {
                phone_machinegunUI[1].fillAmount = Mathf.Lerp(phone_machinegunUI[1].fillAmount, weapon.CurrentAmmoRatio_machinegun(), 0.05f);
            }
            else if (weapon != null && weapon.weaponType == WeaponType.flamethrower)
            {
                phone_flamethrowerUI[1].fillAmount = Mathf.Lerp(phone_flamethrowerUI[1].fillAmount, weapon.CurrentAmmoRatio_flamethrower(), 0.3f);
            }
        }




    }


    public void WeaponUI_Init() //��l�ƪZ����UI
    {

        GameManager.Instance.inputController.SetAllUI(true);


        //�q���� UI ��l��---------------------------------------------

        //��l�ƪZ�� UI�A��o�Z���� UI �� GameObject
        machinegunUI[0] = GameObject.FindGameObjectsWithTag("machinegunUI_1")[0].GetComponent<Image>();
        machinegunUI[1] = GameObject.FindGameObjectsWithTag("machinegunUI_2")[0].GetComponent<Image>();
        machinegunUI[2] = GameObject.FindGameObjectsWithTag("machinegunUI_3")[0].GetComponent<Image>();
        flamethrowerUI[0] = GameObject.FindGameObjectsWithTag("flamethrowerUI_1")[0].GetComponent<Image>();
        flamethrowerUI[1] = GameObject.FindGameObjectsWithTag("flamethrowerUI_2")[0].GetComponent<Image>();
        flamethrowerUI[2] = GameObject.FindGameObjectsWithTag("flamethrowerUI_3")[0].GetComponent<Image>();

        //�ܤp�ܦǩҦ��Z���� UI
        machinegunUI[0].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        flamethrowerUI[0].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

        machinegunUI[0].color = new Color(170f / 255f, 170f / 255f, 170f / 255f, 255f / 255f);
        machinegunUI[1].color = new Color(170f / 255f, 170f / 255f, 170f / 255f, 255f / 255f);
        machinegunUI[2].color = new Color(170f / 255f, 170f / 255f, 170f / 255f, 255f / 255f);
        flamethrowerUI[0].color = new Color(170f / 255f, 170f / 255f, 170f / 255f, 255f / 255f);
        flamethrowerUI[1].color = new Color(170f / 255f, 170f / 255f, 170f / 255f, 255f / 255f);
        flamethrowerUI[2].color = new Color(170f / 255f, 170f / 255f, 170f / 255f, 255f / 255f);

        //����� UI ��l��---------------------------------------------
        phone_machinegunUI[0] = GameObject.FindGameObjectsWithTag("phone_machinegunUI_1")[0].GetComponent<Image>();
        phone_machinegunUI[1] = GameObject.FindGameObjectsWithTag("phone_machinegunUI_2")[0].GetComponent<Image>();
        phone_machinegunUI[2] = GameObject.FindGameObjectsWithTag("phone_machinegunUI_3")[0].GetComponent<Image>();
        phone_flamethrowerUI[0] = GameObject.FindGameObjectsWithTag("phone_flamethrowerUI_1")[0].GetComponent<Image>();
        phone_flamethrowerUI[1] = GameObject.FindGameObjectsWithTag("phone_flamethrowerUI_2")[0].GetComponent<Image>();
        phone_flamethrowerUI[2] = GameObject.FindGameObjectsWithTag("phone_flamethrowerUI_3")[0].GetComponent<Image>();

        //�ܤp�ܦǩҦ��Z���� UI
        phone_machinegunUI[0].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        phone_flamethrowerUI[0].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

        phone_machinegunUI[0].color = new Color(170f / 255f, 170f / 255f, 170f / 255f, 0f / 255f);
        phone_machinegunUI[1].color = new Color(170f / 255f, 170f / 255f, 170f / 255f, 0f / 255f);
        phone_machinegunUI[2].color = new Color(170f / 255f, 170f / 255f, 170f / 255f, 160f / 255f);
        phone_flamethrowerUI[0].color = new Color(170f / 255f, 170f / 255f, 170f / 255f, 0f / 255f);
        phone_flamethrowerUI[1].color = new Color(170f / 255f, 170f / 255f, 170f / 255f, 0f / 255f);
        phone_flamethrowerUI[2].color = new Color(170f / 255f, 170f / 255f, 170f / 255f, 160f / 255f);



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
                PlayerWeaponSlot[i].HiddenWeapon(); //������ܪZ���ҫ�

                //��ܥ[�J���Z����UI
                if (weapon == AllWeaponsList[0]) //�Z���O machinegun
                {
                    machinegunUI[2].color = new Color(170f / 255f, 170f / 255f, 170f / 255f, 0 / 255f);        //�q���O UI
                    phone_machinegunUI[2].color = new Color(170f / 255f, 170f / 255f, 170f / 255f, 0 / 255f);  //����O UI
                }
                else if (weapon == AllWeaponsList[1])  //�Z���O flamethrower
                {
                    flamethrowerUI[2].color = new Color(170f / 255f, 170f / 255f, 170f / 255f, 0 / 255f);        //�q���O UI
                    phone_flamethrowerUI[2].color = new Color(170f / 255f, 170f / 255f, 170f / 255f, 0 / 255f);  //����O UI
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
        if (inputController.GetKeyVInputDown())
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
            audioSource.PlayOneShot(sound_SwtichWeapon);


            //------------------ �Q�̫��{�ɧ�@�U�A�ܦ��@�w�|���j�A�B�u�� V ��ഫ�j(�ϥ��u�����) --------------------
            if(currentWeaponSlotIndex == -1) currentWeaponSlotIndex++;
            //--------------------------------------------------------------------------------------------------------- 


            switchWeapon(currentWeaponSlotIndex); //����������Z��
        }
        /*  //�� B �䥢�h�\��
        else if (inputController.GetKeyBInputDown())
        {
            //�������� index
            currentWeaponSlotIndex -= 1;
            if (currentWeaponSlotIndex < -1)
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
            audioSource.PlayOneShot(sound_SwtichWeapon);

            switchWeapon(currentWeaponSlotIndex); //����������Z��
        }
        */
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
            flamethrowerUI[0].color = new Color(170f / 255f, 170f / 255f, 170f / 255f);
            flamethrowerUI[1].color = new Color(170f / 255f, 170f / 255f, 170f / 255f);

            //����� UI
            phone_machinegunUI[0].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            phone_flamethrowerUI[0].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

            //�w�g���o�ǪZ���~�h�ʥL�z����
            if (HasWeapon(AllWeaponsList[0])) // �p�G�w�g�������j�F
            {
                phone_machinegunUI[0].color = new Color(170f / 255f, 170f / 255f, 170f / 255f, 200f / 255f);
                phone_machinegunUI[1].color = new Color(170f / 255f, 170f / 255f, 170f / 255f, 100f / 255f);
            }
            if (HasWeapon(AllWeaponsList[1])) // �p�G�w�g�����K�j�F
            {
                phone_flamethrowerUI[0].color = new Color(170f / 255f, 170f / 255f, 170f / 255f, 200f / 255f);
                phone_flamethrowerUI[1].color = new Color(170f / 255f, 170f / 255f, 170f / 255f, 100f / 255f);
            }



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

                    //����� UI
                    phone_machinegunUI[0].transform.localScale = new Vector3(1f, 1f, 1f);
                    phone_machinegunUI[0].color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 200f / 255f);
                    phone_machinegunUI[1].color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 100f / 255f);
                }
                else if (PlayerWeaponSlot[index].weaponType == WeaponType.flamethrower)
                {
                    flamethrowerUI[0].transform.localScale = new Vector3(1f, 1f, 1f);
                    flamethrowerUI[1].color = Color.white;

                    //����� UI
                    phone_flamethrowerUI[0].transform.localScale = new Vector3(1f, 1f, 1f);
                    phone_flamethrowerUI[0].color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 200f / 255f);
                    phone_flamethrowerUI[1].color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 100f / 255f);
                }
            }
            else
            {
                //print("�����Z�� " );
            }

        }
    }



    private void OnAim(bool value)
    {
        if (isAim == false && value == true) //�p�G��褣�O�˷ǰʧ@�A��M�ܦ��˷ǰʧ@
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
        if (gameObject != null)
        {
            PickUpItem pickUpItem = gameObject.GetComponent<PickUpItem>();
            if (pickUpItem != null)
            {
                if (pickUpItem.weaponType == WeaponType.flamethrower)
                {
                    AddWeapon(AllWeaponsList[1]); //�ߨ���K�j -> �[�J�Z�� flamethrower
                }
            }
        }
    }


    public void SwitchWeaponToFlamethrower() //�������������K�j
    {
        int index = 0;

        foreach (Weapon PWS_weapon in PlayerWeaponSlot)
        {
            if (PWS_weapon != null && PWS_weapon.weaponType == WeaponType.flamethrower)
            {
                break;
            }
            index++;
        }
        if (index >= PlayerWeaponSlot.Length) return;

        currentWeaponSlotIndex = index;
        audioSource.PlayOneShot(sound_SwtichWeapon);
        switchWeapon(currentWeaponSlotIndex);

    }


}