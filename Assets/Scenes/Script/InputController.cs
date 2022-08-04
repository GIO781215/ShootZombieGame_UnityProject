using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{

    bool IsFirstTime = true; //�O�_���Ĥ@������

    public float vertical_value;
    [HideInInspector] public float horizontal_value;
    [HideInInspector] private bool canInput;
    public GameObject pauseUI;

    [SerializeField] AudioClip sound_Stop; //�Ȱ�����
    AudioSource audioSource;


    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked; //�N�������
        canInput = true;

        //Cursor.visible = false; //�N��������  (�N�������n���N�|�۰����äF�ACursor.visible = false �n���S�ĪG???
    }

    void Update()
    {
        if(IsFirstTime)
        {
            IsFirstTime=false;
            Invoke("StartMessage", 1f);
        }

        audioSource = GetComponent<AudioSource>();
        checkCursorState();

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
    }


    private void checkCursorState()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                audioSource.PlayOneShot(sound_Stop);

                Time.timeScale = 0; //�Ȱ��C��
                if (pauseUI != null)
                {
                    pauseUI.SetActive(true);
                }
                Cursor.lockState = CursorLockMode.None;
                canInput = false;
            }
            else
            {
                audioSource.PlayOneShot(sound_Stop);

                Time.timeScale = 1; //�~��C�� (��_�C���B��ƫ�)
                if(pauseUI != null)
                {
                    pauseUI.SetActive(false);
                }
                Cursor.lockState = CursorLockMode.Locked;
                canInput = true;
            }
        }
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
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        move = Vector3.ClampMagnitude(move, 1); //�N�V�q����b 1 ���
        return move;
    }



    public bool GetSpaceInputDown() //�O�_���U Space ��
    {
        if (canInput)
        {
            return Input.GetKeyDown(KeyCode.Space);
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

    public bool GetShiftInputHold() //�O�_������ Shift ��
    {
        if(canInput)
        {
            return Input.GetKey(KeyCode.LeftShift);
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
    public bool GetEInputHold() //�O�_������ R ��
    {
        if (canInput)
        {
            return Input.GetKey(KeyCode.E);
        }
        return false;
    }

 
}
