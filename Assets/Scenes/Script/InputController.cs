using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{

    bool IsFirstTime = true; //是否為第一次執行

    public float vertical_value;
    [HideInInspector] public float horizontal_value;
    [HideInInspector] private bool canInput;
    public GameObject pauseUI;

    [SerializeField] AudioClip sound_Stop; //暫停音效
    AudioSource audioSource;


    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked; //將鼠標鎖住
        canInput = true;

        //Cursor.visible = false; //將鼠標隱藏  (將鼠標鎖住好像就會自動隱藏了，Cursor.visible = false 好像沒效果???
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

        //不停更新 Vertical 和 Horizontal 實時的輸入的數值
        //vertical_value = Input.GetAxis("Vertical");  //得到 Unity -> Editor -> Project Settings 中 Input Manager 裡的 Axis -> Vertical 的數值
        //horizontal_value = Input.GetAxis("Horizontal"); //得到  Unity -> Editor -> Project Settings 中 Input Manager 裡的 Axis -> Horizontal 的數值
    }

    void StartMessage()
    {
        audioSource.PlayOneShot(sound_Stop);
        Time.timeScale = 0; //暫停遊戲
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

                Time.timeScale = 0; //暫停遊戲
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

                Time.timeScale = 1; //繼續遊戲 (恢復遊戲運行數度)
                if(pauseUI != null)
                {
                    pauseUI.SetActive(false);
                }
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



    public bool GetSpaceInputDown() //是否按下 Space 鍵
    {
        if (canInput)
        {
            return Input.GetKeyDown(KeyCode.Space);
        }
        return false;
    }

    public bool GetSpaceInputHold() //是否按住 Space 鍵
    {
        if (canInput)
        {
            return Input.GetKey(KeyCode.Space);
        }
        return false;
    }

    public bool GetKeyZInputHold() //是否按住 Z 鍵
    {
        if (canInput)
        {
            return Input.GetKey(KeyCode.Z);
        }
        return false;
    }





    public bool GetKeyXInputDown() //是否按下 X 鍵
    {
        if (canInput)
            return Input.GetKeyDown(KeyCode.X);
        return false;
    }

    public bool GetKeyCInputDown() //是否按下 C 鍵
    {
        if (canInput)
            return Input.GetKeyDown(KeyCode.C);
        return false;
    }

    public bool GetKeyVInputDown() //是否按下 V 鍵
    {
        if (canInput)
            return Input.GetKeyDown(KeyCode.V);
        return false;
    }

    public bool GetKeyBInputDown() //是否按下 B 鍵
    {
        if (canInput)
            return Input.GetKeyDown(KeyCode.B);
        return false;
    }

    public bool GetKeyRInputDown() //是否按下 R 鍵
    {
        if (canInput)
            return Input.GetKeyDown(KeyCode.R);
        return false;
    }

    public bool GetKeyFInputDown() //是否按下 F 鍵
    {
        if (canInput)
            return Input.GetKeyDown(KeyCode.F);
        return false;
    }

    public bool GetShiftInputHold() //是否有按住 Shift 鍵
    {
        if(canInput)
        {
            return Input.GetKey(KeyCode.LeftShift);
        }
        return false;
    }
    









    public bool GetWInputHold() //是否有按住 W 鍵
    {
        if (canInput)
        {
            return Input.GetKey(KeyCode.W);
        }
        return false;
    }
    public bool GetSInputHold() //是否有按住 S 鍵
    {
        if (canInput)
        {
            return Input.GetKey(KeyCode.S);
        }
        return false;
    }
    public bool GetAInputHold() //是否有按住 A 鍵
    {
        if (canInput)
        {
            return Input.GetKey(KeyCode.A);
        }
        return false;
    }
    public bool GetDInputHold() //是否有按住 D 鍵
    {
        if (canInput)
        {
            return Input.GetKey(KeyCode.D);
        }
        return false;
    }
    public bool GetQInputHold() //是否有按住 Q 鍵
    {
        if (canInput)
        {
            return Input.GetKey(KeyCode.Q);
        }
        return false;
    }
    public bool GetEInputHold() //是否有按住 R 鍵
    {
        if (canInput)
        {
            return Input.GetKey(KeyCode.E);
        }
        return false;
    }

 
}
