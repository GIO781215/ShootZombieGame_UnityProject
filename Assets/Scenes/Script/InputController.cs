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
        Cursor.lockState = CursorLockMode.Locked; //將鼠標鎖住
        canInput = true;
        //Cursor.visible = false; //將鼠標隱藏  (將鼠標鎖住好像就會自動隱藏了，Cursor.visible = false 好像沒效果???
    }

    void Update()
    {
        checkCursorState();

        //不停更新 Vertical 和 Horizontal 實時的輸入的數值
        //vertical_value = Input.GetAxis("Vertical");  //得到 Unity -> Editor -> Project Settings 中 Input Manager 裡的 Axis -> Vertical 的數值
        //horizontal_value = Input.GetAxis("Horizontal"); //得到  Unity -> Editor -> Project Settings 中 Input Manager 裡的 Axis -> Horizontal 的數值
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



    public float GetMouseX() //得到滑鼠的左右移動輸入
    {
        if (canInput)
            return Input.GetAxis("Mouse X");
        return 0;
    }

    public float GetMouseY() //得到滑鼠的前後移動輸入
    {
        if (canInput)
            return Input.GetAxis("Mouse Y");
        return 0;
    }

    public float GetMouseScrollWheel() //得到滑鼠的滾輪輸入
    {
        if (canInput) 
            return Input.GetAxis("Mouse ScrollWheel");
        return 0;
    }

    public bool GetMouseLeftKeyDown() //是否按下滑鼠的左鍵
    {
        if (canInput)
            return Input.GetMouseButtonDown(0);
        return false;
    }

    public bool GetMouseLeftKeyHeldDown() //是否持續按著滑鼠的左鍵
    {
        if (canInput)
            return Input.GetMouseButton(0);
        return false;
    }

    public bool GetMouseLeftKeyUp() //是否鬆開滑鼠的左鍵
    {
        if (canInput)
            return Input.GetMouseButtonUp(0);
        return false;
    }

    public bool GetMouseRightKeyDown() //是否按下滑鼠的右鍵
    {
        if (canInput)
            return Input.GetMouseButtonDown(1);
        return false;
    }





    public Vector3 GetMoveInput() //得到鍵盤前後左右的輸入值
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        move = Vector3.ClampMagnitude(move, 1); //將向量限制在 1 單位
        return move;
    }

    public bool GetSpaceInput() //是否按下 Space 鍵
    {
        if (canInput)
        {
            return Input.GetKey(KeyCode.Space);
        }
        return false;
    }

    public bool GetKeyZInput() //是否按下 Z 鍵
    {
        if (canInput)
        {
            return Input.GetKey(KeyCode.Z);
        }
        return false;
    }

    public bool GetKeyXInput() //是否按下 X 鍵
    {
        if (canInput)
            return Input.GetKeyDown(KeyCode.X);
        return false;
    }

    public bool GetKeyCInput() //是否按下 C 鍵
    {
        if (canInput)
            return Input.GetKeyDown(KeyCode.C);
        return false;
    }

    public bool GetKeyVInput() //是否按下 V 鍵
    {
        if (canInput)
            return Input.GetKeyDown(KeyCode.V);
        return false;
    }

    public bool GetKeyRInput() //是否按下 R 鍵
    {
        if (canInput)
            return Input.GetKeyDown(KeyCode.R);
        return false;
    }

    public bool GetShiftInput() //是否有按住 Shift 鍵
    {
        if(canInput)
        {
            return Input.GetKey(KeyCode.LeftShift);
        }
        return false;
    }

}
