using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputController : MonoBehaviour
{

    bool IsFirstTime = true; //�O�_���Ĥ@������

    public float vertical_value;
    [SerializeField] CameraController cameraController;
    [HideInInspector] public float horizontal_value;
    [HideInInspector] private bool canInput;
    public GameObject pauseUI;

    [SerializeField] AudioClip sound_Stop; //�Ȱ�����
    AudioSource audioSource;


    [SerializeField] GameObject pauseUI_ClolseButton_Prefab; //���F�ѨM�ηƹ����F��������A�U���A����L Enter ��������A�|���������s�J�I�w�g�s�b�ҥH�Q����Ĳ�o�⦸�����D�A�����P�����ؤ@�ӨӸѨM
    [SerializeField] GameObject closeButtonPosition;
    GameObject pauseUI_ClolseButton;

    [SerializeField] GameObject LittleMapUI; //�p�a�ϡA�}���������\��[�b���}��
    [SerializeField] GameObject PhoneUI;
    [SerializeField] GameObject ComputerWeaponUI;
    bool LittleMapUI_Flag = true;


    //���������Ѽ�
    public bool Phone_Forward = false;  
    public bool Phone_Back = false;  
    public bool Phone_Left = false;  
    public bool Phone_Right = false;  
    public bool Phone_Forward_Left = false;  
    public bool Phone_Forward_Right = false;  
    public bool Phone_Back_Left = false;  
    public bool Phone_Back_Right = false;  


    public bool Phone_Rush = false; //�O�_������Ĩ�
    public bool Phone_Shoot = false; //�O�_������g��
    public bool Phone_Camera = false; //�O�_��������v��

    public bool Phone_Aim = false; //�O�_���U�F�˷�
    public bool Phone_Jump = false; //�O�_���U�F���D
    public bool Phone_Machinegun = false; //�O�_���U�F�}���p�a��
    public bool Phone_OnOffLittleMap = false; //�O�_���U�F��������j
    public bool Phone_Flamethrower = false; //�O�_���U�F�������K�j



    float player_Forward_speed = 0;
    float player_Back_speed = 0;
    float player_Left_speed = 0;
    float player_Right_speed = 0;


    private void Start()
    {


        Cursor.lockState = CursorLockMode.Locked; //�N�������
        canInput = true;

        //Cursor.visible = false; //�N��������  (�N�������n���N�|�۰����äF�ACursor.visible = false �n���S�ĪG???
    }

    void Update()
    {
        if (GameManager.Instance.IsPhoneMode) //��������ʳB�z
        {
            PhoneUI_Move_process();
        }

        if (IsFirstTime)
        {
            IsFirstTime = false;
            Invoke("StartMessage", 1f);
        }

        audioSource = GetComponent<AudioSource>();

        checkCursorState();

        On_Off_LittleMap(); //�}���p�a��

        //������s Vertical �M Horizontal ��ɪ���J���ƭ�
        //vertical_value = Input.GetAxis("Vertical");  //�o�� Unity -> Editor -> Project Settings �� Input Manager �̪� Axis -> Vertical ���ƭ�
        //horizontal_value = Input.GetAxis("Horizontal"); //�o��  Unity -> Editor -> Project Settings �� Input Manager �̪� Axis -> Horizontal ���ƭ�
    }

    void StartMessage()
    {
        audioSource.PlayOneShot(sound_Stop);
        Time.timeScale = 0; //�Ȱ��C��
        if (pauseUI != null)
        {
            pauseUI.SetActive(true);
        }
        Cursor.lockState = CursorLockMode.None;
        canInput = false;
        pauseUI_ClolseButton = Instantiate(pauseUI_ClolseButton_Prefab, closeButtonPosition.transform.position, closeButtonPosition.transform.rotation, GameObject.FindGameObjectsWithTag("closeButtonPosition")[0].transform);

    }

    private void checkCursorState()
    {
        if ((Input.GetKeyDown(KeyCode.Return)) && !cameraController.IsPlayerDeath && !cameraController.IsMutantDeath)
        {
            if (Cursor.lockState == CursorLockMode.Locked) //if (pauseUI.activeInHierarchy == false)   
            {
                audioSource.PlayOneShot(sound_Stop);

                Time.timeScale = 0; //�Ȱ��C��
                if (pauseUI != null)
                {
                    pauseUI.SetActive(true);
                }
                Cursor.lockState = CursorLockMode.None;
                canInput = false;
                pauseUI_ClolseButton = Instantiate(pauseUI_ClolseButton_Prefab, closeButtonPosition.transform.position, closeButtonPosition.transform.rotation, GameObject.FindGameObjectsWithTag("closeButtonPosition")[0].transform);
            }
            else
            {
                audioSource.PlayOneShot(sound_Stop);

                Time.timeScale = 1; //�~��C�� (��_�C���B��t��)
                if (pauseUI != null)
                {
                    pauseUI.SetActive(false);
                }
                Cursor.lockState = CursorLockMode.Locked;
                canInput = true;
                if (pauseUI_ClolseButton)
                    Destroy(pauseUI_ClolseButton);
            }
        }
    }

    public void ContinueGame()
    {
        audioSource.PlayOneShot(sound_Stop);

        Time.timeScale = 1; //�~��C�� (��_�C���B��t��)
        if (pauseUI != null)
        {
            pauseUI.SetActive(false);
        }
        if (GameManager.Instance.IsPhoneMode)
        {
            Cursor.lockState = CursorLockMode.None;

        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        canInput = true;
        if (pauseUI_ClolseButton)
            Destroy(pauseUI_ClolseButton);
    }


    public float GetMouseX() //�o��ƹ������k���ʿ�J
    {
        if (canInput)
            return Input.GetAxis("Mouse X");
        return 0;
    }

    public float GetMouseY() //�o��ƹ����e�Ჾ�ʿ�J
    {
        if (canInput)
            return Input.GetAxis("Mouse Y");
        return 0;
    }

    public float GetMouseScrollWheel() //�o��ƹ����u����J
    {
        if (canInput)
            return Input.GetAxis("Mouse ScrollWheel");
        return 0;
    }

    public bool GetMouseLeftKeyDown() //�O�_���U�ƹ�������
    {
        if (canInput)
            return Input.GetMouseButtonDown(0);
        return false;
    }

    public bool GetMouseLeftKeyHeldDown() //�O�_������۷ƹ�������
    {
        if (canInput)
            return Input.GetMouseButton(0);
        return false;
    }

    public bool GetMouseLeftKeyUp() //�O�_�P�}�ƹ�������
    {
        if (canInput)
            return Input.GetMouseButtonUp(0);
        return false;
    }

    public bool GetMouseRightKeyDown() //�O�_���U�ƹ����k��
    {
        if (canInput)
            return Input.GetMouseButtonDown(1);
        return false;
    }




    public Vector3 GetMoveInput() //�o����L�e�ᥪ�k����J��
    {

        if (!GameManager.Instance.IsPhoneMode) //�q�������ʳB�z
        {
            Vector3 move;
            move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            move = Vector3.ClampMagnitude(move, 1); //�N�V�q����b 1 ���
            return move;
        }
        else  //��������ʳB�z
        {
            Vector3 move;
            move = new Vector3(player_Right_speed - player_Left_speed, 0f, player_Forward_speed - player_Back_speed);
            move = Vector3.ClampMagnitude(move, 1); //�N�V�q����b 1 ���
            return move;
        }
    }



    public bool GetSpaceInputDown() //�O�_���U Space ��
    {
        if (canInput)
        {
            return Input.GetKeyDown(KeyCode.Space);
        }
        return false;
    }

    public bool GetKeyXInputDown() //�O�_���U X ��
    {
        if (canInput)
            return Input.GetKeyDown(KeyCode.X);
        return false;
    }

    public bool GetKeyCInputDown() //�O�_���U C ��
    {
        if (canInput)
            return Input.GetKeyDown(KeyCode.C);
        return false;
    }

    public bool GetKeyVInputDown() //�O�_���U V ��
    {
        if (canInput)
            return Input.GetKeyDown(KeyCode.V);
        return false;
    }

    public bool GetKeyBInputDown() //�O�_���U B ��
    {
        if (canInput)
            return Input.GetKeyDown(KeyCode.B);
        return false;
    }

    public bool GetKeyRInputDown() //�O�_���U R ��
    {
        if (canInput)
            return Input.GetKeyDown(KeyCode.R);
        return false;
    }

    public bool GetKeyFInputDown() //�O�_���U F ��
    {
        if (canInput)
            return Input.GetKeyDown(KeyCode.F);
        return false;
    }

    public bool GetKeyEInputDown() //�O�_���U E ��
    {
        if (canInput)
        {
            return Input.GetKeyDown(KeyCode.E);
        }
        return false;
    }

    public bool GetKeyGInputDown() //�O�_���U G ��
    {
        if (canInput)
            return Input.GetKeyDown(KeyCode.G);
        return false;
    }

    public bool GetKeyQInputDown() //�O�_�����U Q ��
    {
        if (canInput)
        {
            return Input.GetKeyDown(KeyCode.Q);
        }
        return false;
    }

    public bool GetTabInputDown() //�O�_�����U Tab ��
    {
        if (canInput)
        {
            return Input.GetKeyDown(KeyCode.Tab);
        }
        return false;
    }

    public bool GetShiftInputDown() //�O�_�����U Shift ��
    {
        if (canInput)
        {
            return Input.GetKeyDown(KeyCode.LeftShift);
        }
        return false;
    }







    public bool GetShiftInputHold() //�O�_������ Shift ��
    {
        if (canInput)
        {
            return Input.GetKey(KeyCode.LeftShift);
        }
        return false;
    }

    public bool GetSpaceInputHold() //�O�_���� Space ��
    {
        if (canInput)
        {
            return Input.GetKey(KeyCode.Space);
        }
        return false;
    }

    public bool GetKeyZInputHold() //�O�_���� Z ��
    {
        if (canInput)
        {
            return Input.GetKey(KeyCode.Z);
        }
        return false;
    }

    public bool GetWInputHold() //�O�_������ W ��
    {
        if (canInput)
        {
            return Input.GetKey(KeyCode.W);
        }
        return false;
    }
    public bool GetSInputHold() //�O�_������ S ��
    {
        if (canInput)
        {
            return Input.GetKey(KeyCode.S);
        }
        return false;
    }
    public bool GetAInputHold() //�O�_������ A ��
    {
        if (canInput)
        {
            return Input.GetKey(KeyCode.A);
        }
        return false;
    }
    public bool GetDInputHold() //�O�_������ D ��
    {
        if (canInput)
        {
            return Input.GetKey(KeyCode.D);
        }
        return false;
    }
    public bool GetQInputHold() //�O�_������ Q ��
    {
        if (canInput)
        {
            return Input.GetKey(KeyCode.Q);
        }
        return false;
    }
    public bool GetEInputHold() //�O�_������ E ��
    {
        if (canInput)
        {
            return Input.GetKey(KeyCode.E);
        }
        return false;
    }





    //--------------------------------------��� UI (�ƹ�)����ާ@--------------------------------------
    void PhoneUI_Move_process()
    {
        if (Phone_Forward || Phone_Forward_Left || Phone_Forward_Right)
        {
            player_Forward_speed = Mathf.Lerp(player_Forward_speed, 1, 0.1f);
        }
        else
        {
            player_Forward_speed = 0;
        }

        if (Phone_Back || Phone_Back_Left || Phone_Back_Right)
        {
            player_Back_speed = Mathf.Lerp(player_Back_speed, 1, 0.1f);
        }
        else
        {
            player_Back_speed = 0;
        }

        if (Phone_Left || Phone_Forward_Left || Phone_Back_Left)
        {
            player_Left_speed = Mathf.Lerp(player_Left_speed, 1, 0.1f);
        }
        else
        {
            player_Left_speed = 0;
        }
        if (Phone_Right || Phone_Forward_Right || Phone_Back_Right)
        {
            player_Right_speed = Mathf.Lerp(player_Right_speed, 1, 0.1f);
        }
        else
        {
            player_Right_speed = 0;
        }
    }


    public void PhoneUI_Forward_Down()
    {
        if (canInput && GameManager.Instance.playerController != null && Phone_Forward == false)
        {
            Phone_Forward = true;
        }
    }
    public void PhoneUI_Forward_Up()
    {
        if (canInput && GameManager.Instance.playerController != null)
        {
            //Invoke("_PhoneUI_Forward_Up", 0.01f); //����m�s�X�� �O���F�ѨM����ʯ�Ӯt�|�d��y���G��Ĳ�o PhoneUI_Forward_Down() �����D
            Phone_Forward = false;
            StartCoroutine(_PhoneUI_Forward_Up());
        }
    }
    IEnumerator _PhoneUI_Forward_Up()
    {
        for (int i = 0; i < 2; i++)
        {
            Phone_Forward = false;
            yield return new WaitForSeconds(0.005f);
        }
    }



    public void PhoneUI_Back_Down()
    {
        if (canInput && GameManager.Instance.playerController != null && Phone_Back == false)
        {
            Phone_Back = true;
        }
    }
    public void PhoneUI_Back_Up()
    {
        if (canInput && GameManager.Instance.playerController != null)
        {
            Phone_Back = false;
            StartCoroutine(_PhoneUI_Back_Up());
        }
    }
    IEnumerator _PhoneUI_Back_Up()
    {
        for (int i = 0; i < 2; i++)
        {
            Phone_Back = false;
            yield return new WaitForSeconds(0.005f);
        }
    }

 

    public void PhoneUI_Left_Down()
    {
        if (canInput && GameManager.Instance.playerController != null && Phone_Left == false)
        {
            Phone_Left = true;
        }
    }
    public void PhoneUI_Left_Up()
    {
        if (canInput && GameManager.Instance.playerController != null)
        {
            Phone_Left = false;
            StartCoroutine(_PhoneUI_Left_Up());
        }
    }
    IEnumerator _PhoneUI_Left_Up()
    {
        for (int i = 0; i < 2; i++)
        {
            Phone_Left = false;
            yield return new WaitForSeconds(0.005f);
        }
    }

   

    public void PhoneUI_Right_Down()
    {
        if (canInput && GameManager.Instance.playerController != null && Phone_Right == false)
        {
            Phone_Right = true;
        }
    }
    public void PhoneUI_Right_Up()
    {
        if (canInput && GameManager.Instance.playerController != null)
        {
            Phone_Right = false;
            StartCoroutine(_PhoneUI_Right_Up());
        }
    }
    IEnumerator _PhoneUI_Right_Up()
    {
        for (int i = 0; i < 2; i++)
        {
            Phone_Right = false;
            yield return new WaitForSeconds(0.005f);
        }
    }


    public void PhoneUI_Forward_Left_Down()
    {
        if (canInput && GameManager.Instance.playerController != null && Phone_Forward_Left == false)
        {
            Phone_Forward_Left = true;
        }
    }
    public void PhoneUI_Forward_Left_Up()
    {
        if (canInput && GameManager.Instance.playerController != null)
        {
            Phone_Forward_Left = false;
            StartCoroutine(_PhoneUI_Forward_Left_Up());
        }
    }
    IEnumerator _PhoneUI_Forward_Left_Up()
    {
        for (int i = 0; i < 2; i++)
        {
            Phone_Forward_Left = false;
            yield return new WaitForSeconds(0.005f);
        }
    }
 

    public void PhoneUI_Forward_Right_Down()
    {
        if (canInput && GameManager.Instance.playerController != null && Phone_Forward_Right == false)
        {
            Phone_Forward_Right = true;
        }
    }
    public void PhoneUI_Forward_Right_Up()
    {
        if (canInput && GameManager.Instance.playerController != null)
        {
            Phone_Forward_Right = false;
            StartCoroutine(_PhoneUI_Forward_Right_Up());
        }
    }
    IEnumerator _PhoneUI_Forward_Right_Up()
    {
        for (int i = 0; i < 2; i++)
        {
            Phone_Forward_Right = false;
            yield return new WaitForSeconds(0.005f);
        }
    }
  

    public void PhoneUI_Back_Left_Down()
    {
        if (canInput && GameManager.Instance.playerController != null && Phone_Back_Left == false)
        {
            Phone_Back_Left = true;
        }
    }
    public void PhoneUI_Back_Left_Up()
    {
        if (canInput && GameManager.Instance.playerController != null)
        {
            Phone_Back_Left = false;
            StartCoroutine(_PhoneUI_Back_Left_Up());
        }
    }
    IEnumerator _PhoneUI_Back_Left_Up()
    {
        for (int i = 0; i < 2; i++)
        {
            Phone_Back_Left = false;
            yield return new WaitForSeconds(0.005f);
        }
    }

    public void PhoneUI_Back_Right_Down()
    {
        if (canInput && GameManager.Instance.playerController != null && Phone_Back_Right == false)
        {
            Phone_Back_Right = true;
        }
    }
    public void PhoneUI_Back_Right_Up()
    {
        if (canInput && GameManager.Instance.playerController != null)
        {
            Phone_Back_Right = false;
            StartCoroutine(_PhoneUI_Back_Right_Up());
        }
    }
    IEnumerator _PhoneUI_Back_Right_Up()
    {
        for (int i = 0; i < 2; i++)
        {
            Phone_Back_Right = false;
            yield return new WaitForSeconds(0.005f);
        }
    }



    public void PhoneUI_Camera_Drag()
    {
        if (canInput && GameManager.Instance.playerController != null)
        {
            cameraController.PhoneUI_ControllCamera();
            //Invoke("Phone_Camera_True", 0.01f); //����@�U�A�]�� true�A�׶}Ĳ�ŲĤ@�U�Z���|���ܤj���P�w���D
        }
    }
    public void PhoneUI_Camera_Enter()
    {
        if (canInput && GameManager.Instance.playerController != null)
        {
            Phone_Camera = true;
            //Invoke("Phone_Camera_True", 0.01f); //����@�U�A�]�� true�A�׶}Ĳ�ŲĤ@�U�Z���|���ܤj���P�w���D
        }
    }
    public void PhoneUI_Camera_Up()
    {
        if (canInput && GameManager.Instance.playerController != null)
        {
            Phone_Camera = false;
        }
    }




    public void PhoneUI_Shoot_Down()
    {
        if (canInput && GameManager.Instance.playerController != null && Phone_Shoot == false) //�קK�����ɭ���Ĳ�o
        {
            Phone_Shoot = true;
            GameManager.Instance.playerController.ShootAimBehaviour_PhoneMode(); //�ܦ��g���ʧ@�P�ϯ�g���X��
            GameManager.Instance.playerWeaponController.PhoneUI_Machinegun_Shoot();
        }
    }
    public void PhoneUI_Shoot_Up()
    {
        if (canInput && GameManager.Instance.playerController != null)
        {
            Invoke("_PhoneUI_Shoot_Up", 0.01f);
        }
    }
    void _PhoneUI_Shoot_Up()
    {
        Phone_Shoot = false;
    }



    public void PhoneUI_Rush_Down()
    {
        if (canInput && GameManager.Instance.playerController != null && Phone_Rush == false)
        {
            Phone_Rush = true;
        }
    }
    public void PhoneUI_Rush_Up()
    {
        if (canInput && GameManager.Instance.playerController != null)
        {
            Invoke("_PhoneUI_Rush_Up", 0.01f);
        }
    }
    void _PhoneUI_Rush_Up()
    {
        Phone_Rush = false;
    }
    

    public void PhoneUI_Jump_Down()
    {
        if (canInput && GameManager.Instance.playerController != null && Phone_Jump == false)
        {
            Phone_Jump = true;
            GameManager.Instance.playerController.jumpBehaviour_PhoneMode();
            Invoke("PhoneUI_Jump_Up", 0.8f); //�a����۰ʭ��m�X��
        }
    }
    public void PhoneUI_Jump_Up()
    {
        Phone_Jump = false;
    }



    public void PhoneUI_Aim_Down()
    {
        if (canInput && GameManager.Instance.playerController != null && Phone_Aim == false)
        {
            Phone_Aim = true;
            GameManager.Instance.playerController.AimBehaviour_PhoneMode();
        }
    }
    public void PhoneUI_Aim_Up()
    {
        if (canInput && GameManager.Instance.playerController != null)
        {
            Phone_Aim = false;
            StartCoroutine(_PhoneUI_Aim_Up());
        }
    }
 
    IEnumerator _PhoneUI_Aim_Up()
    {
        for (int i = 0; i < 2; i++)
        {
            Phone_Aim = false;
            yield return new WaitForSeconds(0.005f);
        }
    }

    public void PhoneUI_Machinegun_Down()
    {
        if (GameManager.Instance.playerWeaponController != null && Phone_Machinegun == false)
        {
            Phone_Machinegun = true;
            GameManager.Instance.playerWeaponController.SwitchWeaponToMachinegun();
        }
    }
    public void PhoneUI_Machinegun_Up()
    {
        Invoke("_PhoneUI_Machinegun_Up", 0.01f);
    }
    void _PhoneUI_Machinegun_Up()
    {
        Phone_Machinegun = false;
    }


    public void PhoneUI_Flamethrower_Down()
    {
        if (GameManager.Instance.playerWeaponController != null && Phone_Flamethrower == false)
        {
            Phone_Flamethrower = true;
            GameManager.Instance.playerWeaponController.SwitchWeaponToFlamethrower();
        }
    }
    public void PhoneUI_Flamethrower_Up()
    {
        Invoke("_PhoneUI_Flamethrower_Up", 0.01f);
    }
    void _PhoneUI_Flamethrower_Up()
    {
        Phone_Flamethrower = false;
    }


    public void PhoneUI_OnOffLittleMap_Dowm()
    {
        if (canInput && GameManager.Instance.playerController != null && Phone_OnOffLittleMap == false)
        {
            Phone_OnOffLittleMap = true;
            audioSource.PlayOneShot(sound_Stop);
            LittleMapUI_Flag = !LittleMapUI_Flag;
            LittleMapUI.SetActive(LittleMapUI_Flag);
        }
    }
    public void PhoneUI_OnOffLittleMap_Up()
    {
        Invoke("_PhoneUI_OnOffLittleMap_Up", 0.01f);
    }
    void _PhoneUI_OnOffLittleMap_Up()
    {
        Phone_OnOffLittleMap = false;

    }


    public void PhoneUI_Setting()
    {
        if (!cameraController.IsPlayerDeath && !cameraController.IsMutantDeath)
        {
            if (pauseUI.activeInHierarchy == false)   
            {
                audioSource.PlayOneShot(sound_Stop);

                Time.timeScale = 0; //�Ȱ��C��
                if (pauseUI != null)
                {
                    pauseUI.SetActive(true);
                }
                Cursor.lockState = CursorLockMode.None;
                canInput = false;
                pauseUI_ClolseButton = Instantiate(pauseUI_ClolseButton_Prefab, closeButtonPosition.transform.position, closeButtonPosition.transform.rotation, GameObject.FindGameObjectsWithTag("closeButtonPosition")[0].transform);
            }
            else
            {
                audioSource.PlayOneShot(sound_Stop);

                Time.timeScale = 1; //�~��C�� (��_�C���B��t��)
                if (pauseUI != null)
                {
                    pauseUI.SetActive(false);
                }
                Cursor.lockState = CursorLockMode.Locked;
                canInput = true;
                if (pauseUI_ClolseButton)
                    Destroy(pauseUI_ClolseButton);
            }
        }
    }

    //----------------------------------------------------------------------------------------------------------------------------------








    //�}�� / �����p�a��
    void On_Off_LittleMap()
    {
        if (GetKeyQInputDown())
        {
            audioSource.PlayOneShot(sound_Stop);
            LittleMapUI_Flag = !LittleMapUI_Flag;
            LittleMapUI.SetActive(LittleMapUI_Flag);
        }
    }

    public void SetComputerUI()
    {
        audioSource.PlayOneShot(sound_Stop);
        ComputerWeaponUI.SetActive(true);
        PhoneUI.SetActive(false);
    }

    public void SetPhoneUI()
    {
        audioSource.PlayOneShot(sound_Stop);
        ComputerWeaponUI.SetActive(false);
        PhoneUI.SetActive(true);
    }

    public void SetComputerUI_NoSound() //�� PlayerWeaponController ��l�ƮɥΪ�
    {
        ComputerWeaponUI.SetActive(true);
        PhoneUI.SetActive(false);
    }

    public void SetPhoneUI_NoSound() //�� PlayerWeaponController ��l�ƮɥΪ�
    {
        ComputerWeaponUI.SetActive(false);
        PhoneUI.SetActive(true);
    }

    public void SetAllUI(bool value) //�� PlayerWeaponController ��l�ƮɥΪ�
    {
        ComputerWeaponUI.SetActive(value);
        PhoneUI.SetActive(value);
    }
}
