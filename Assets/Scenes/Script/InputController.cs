using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public float vertical_value;
    public float horizontal_value;
    private bool canInput;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked; //�N�������
        canInput = true;
        //Cursor.visible = false; //�N��������  (�N�������n���N�|�۰����äF�ACursor.visible = false �n���S�ĪG???
    }

    void Update()
    {
        checkCursorState();

        //������s Vertical �M Horizontal ��ɪ���J���ƭ�
        //vertical_value = Input.GetAxis("Vertical");  //�o�� Unity -> Editor -> Project Settings �� Input Manager �̪� Axis -> Vertical ���ƭ�
        //horizontal_value = Input.GetAxis("Horizontal"); //�o��  Unity -> Editor -> Project Settings �� Input Manager �̪� Axis -> Horizontal ���ƭ�
    }

    private void checkCursorState()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                canInput = false;
            }
            else
            {
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

    public bool GetSpaceInput() //�O�_���U Space ��
    {
        if (canInput)
        {
            return Input.GetKey(KeyCode.Space);
        }
        return false;
    }

    public bool GetKeyZInput() //�O�_���U Z ��
    {
        if (canInput)
        {
            return Input.GetKey(KeyCode.Z);
        }
        return false;
    }

    public bool GetKeyXInput() //�O�_���U X ��
    {
        if (canInput)
            return Input.GetKeyDown(KeyCode.X);
        return false;
    }

    public bool GetKeyCInput() //�O�_���U C ��
    {
        if (canInput)
            return Input.GetKeyDown(KeyCode.C);
        return false;
    }

    public bool GetKeyVInput() //�O�_���U V ��
    {
        if (canInput)
            return Input.GetKeyDown(KeyCode.V);
        return false;
    }

    public bool GetKeyRInput() //�O�_���U R ��
    {
        if (canInput)
            return Input.GetKeyDown(KeyCode.R);
        return false;
    }

    public bool GetShiftInput() //�O�_������ Shift ��
    {
        if(canInput)
        {
            return Input.GetKey(KeyCode.LeftShift);
        }
        return false;
    }

}
