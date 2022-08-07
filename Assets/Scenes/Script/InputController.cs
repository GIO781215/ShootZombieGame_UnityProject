using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputController : MonoBehaviour
{

    bool IsFirstTime = true; //是否為第一次執行

    public float vertical_value;
    [SerializeField] CameraController cameraController;
    [HideInInspector] public float horizontal_value;
    [HideInInspector] private bool canInput;
    public GameObject pauseUI;

    [SerializeField] AudioClip sound_Stop; //暫停音效
    AudioSource audioSource;


    [SerializeField] GameObject pauseUI_ClolseButton_Prefab; //為了解決用滑鼠按了關閉之後，下次再用鍵盤 Enter 鍵按關閉，會有關閉按鈕焦點已經存在所以被重複觸發兩次的問題，直接銷毀重建一個來解決
    [SerializeField] GameObject closeButtonPosition;
    GameObject pauseUI_ClolseButton;

    [SerializeField] GameObject LittleMapUI; //小地圖，開啟關閉的功能加在此腳本
    [SerializeField] GameObject PhoneUI;
    [SerializeField] GameObject ConputgerWeaponUI;
    bool LittleMapUI_Flag = true;


    //手機版按鍵參數
    public bool Phone_Forward = false; //是否正按住前進
    public bool Phone_Back = false; //是否正按住後退
    public bool Phone_Left = false; //是否正按住往左
    public bool Phone_Right = false; //是否正按住往右
    public bool Phone_Rush = false; //是否正按住衝刺
    public bool Phone_Shoot = false; //是否正按住射擊
    public bool Phone_Camera = false; //是否正按住攝影機
    float player_Forward_speed = 0;
    float player_Back_speed = 0;
    float player_Left_speed = 0;
    float player_Right_speed = 0;




    private void Start()
    {


        Cursor.lockState = CursorLockMode.Locked; //將鼠標鎖住
        canInput = true;

        //Cursor.visible = false; //將鼠標隱藏  (將鼠標鎖住好像就會自動隱藏了，Cursor.visible = false 好像沒效果???
    }

    void Update()
    {
        if (GameManager.Instance.IsPhoneMode) //手機版移動處理
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

        On_Off_LittleMap(); //開關小地圖

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
        pauseUI_ClolseButton = Instantiate(pauseUI_ClolseButton_Prefab, closeButtonPosition.transform.position, closeButtonPosition.transform.rotation, GameObject.FindGameObjectsWithTag("closeButtonPosition")[0].transform);

    }

    private void checkCursorState()
    {
        if ((Input.GetKeyDown(KeyCode.Return)) && !cameraController.IsPlayerDeath && !cameraController.IsMutantDeath)
        {
            if (Cursor.lockState == CursorLockMode.Locked) //if (pauseUI.activeInHierarchy == false)   
            {
                audioSource.PlayOneShot(sound_Stop);

                Time.timeScale = 0; //暫停遊戲
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

                Time.timeScale = 1; //繼續遊戲 (恢復遊戲運行速度)
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

        Time.timeScale = 1; //繼續遊戲 (恢復遊戲運行速度)
        if (pauseUI != null)
        {
            pauseUI.SetActive(false);
        }
        if(GameManager.Instance.IsPhoneMode)
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

        if (!GameManager.Instance.IsPhoneMode) //電腦版移動處理
        {
            Vector3 move;
            move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            move = Vector3.ClampMagnitude(move, 1); //將向量限制在 1 單位
            print(move);
            return move;
        }
        else  //手機版移動處理
        {
            Vector3 move;
            move = new Vector3(player_Right_speed - player_Left_speed, 0f, player_Forward_speed - player_Back_speed);
            move = Vector3.ClampMagnitude(move, 1); //將向量限制在 1 單位
            print(move);

            return move;
        }

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
        if (canInput)
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





    //--------------------------------------手機 UI (滑鼠)控制操作--------------------------------------
     void PhoneUI_Move_process()
    {
        if(Phone_Forward)
        {
            player_Forward_speed = Mathf.Lerp(player_Forward_speed, 1, 0.1f);
        }
        else
        {
            player_Forward_speed = 0;
        }

        if (Phone_Back)
        {
            player_Back_speed = Mathf.Lerp(player_Back_speed, 1, 0.1f);
        }
        else
        {
            player_Back_speed = 0;
        }

        if (Phone_Left)
        {
            player_Left_speed = Mathf.Lerp(player_Left_speed, 1, 0.1f);
        }
        else
        {
            player_Left_speed = 0;
        }
        if (Phone_Right)
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
        if (canInput && GameManager.Instance.playerController != null)
        {
            Phone_Forward = true;
        }
    }
    public void PhoneUI_Forward_Up()
    {
        if (canInput && GameManager.Instance.playerController != null)
        {
            Phone_Forward = false;
        }
    }

    public void PhoneUI_Back_Down()
    {
        if (canInput && GameManager.Instance.playerController != null)
        {
            Phone_Back = true;
        }
    }
    public void PhoneUI_Back_Up()
    {
        if (canInput && GameManager.Instance.playerController != null)
        {
            Phone_Back = false;
        }
    }

    public void PhoneUI_Left_Down()
    {
        if (canInput && GameManager.Instance.playerController != null)
        {
            Phone_Left = true;
        }
    }
    public void PhoneUI_Left_Up()
    {
        if (canInput && GameManager.Instance.playerController != null)
        {
            Phone_Left = false;
        }
    }

    public void PhoneUI_Right_Down()
    {
        if (canInput && GameManager.Instance.playerController != null)
        {
            Phone_Right = true;
        }
    }
    public void PhoneUI_Right_Up()
    {
        if (canInput && GameManager.Instance.playerController != null)
        {
            Phone_Right = false;
        }
    }

    public void PhoneUI_Camera_Down()
    {
        if (canInput && GameManager.Instance.playerController != null)
        {
            Phone_Camera = true;
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
        if (canInput && GameManager.Instance.playerController != null)
        {
            Phone_Shoot = true;
            GameManager.Instance.playerController.ShootAimBehaviour_PhoneMode(); //變成射擊動作與使能射擊旗標
        }
    }
    public void PhoneUI_Shoot_Up()
    {
        if (canInput && GameManager.Instance.playerController != null)
        {
            Phone_Shoot = false;
        }
    }

    public void PhoneUI_Rush_Down()
    {
        if (canInput && GameManager.Instance.playerController != null)
        {
            Phone_Rush = true;
        }
    }
    public void PhoneUI_Rush_Up()
    {
        if (canInput && GameManager.Instance.playerController != null)
        {
            Phone_Rush = false;
        }
    }
  
    public void PhoneUI_Jump()
    {
        if (canInput && GameManager.Instance.playerController != null)
        {
            GameManager.Instance.playerController.jumpBehaviour_PhoneMode();
        }
    }
    public void PhoneUI_Aim()
    {
        if (canInput && GameManager.Instance.playerController != null)
        {
            GameManager.Instance.playerController.AimBehaviour_PhoneMode();
        }
    }
    public void PhoneUI_Machinegun()
    {
      


    }
    public void PhoneUI_Flamethrower()
    {



    }
    public void PhoneUI_Setting()
    {
        if (!cameraController.IsPlayerDeath && !cameraController.IsMutantDeath)
        {
            if (pauseUI.activeInHierarchy == false)   
            {
                audioSource.PlayOneShot(sound_Stop);

                Time.timeScale = 0; //暫停遊戲
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

                Time.timeScale = 1; //繼續遊戲 (恢復遊戲運行速度)
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








    //開啟 / 關閉小地圖
    void On_Off_LittleMap()
    {
        if (GetKeyFInputDown())
        {
            LittleMapUI_Flag = !LittleMapUI_Flag;
            LittleMapUI.SetActive(LittleMapUI_Flag);
        }
    }

    public void SetComputerUI()
    {
        audioSource.PlayOneShot(sound_Stop);
        ConputgerWeaponUI.SetActive(true);
        PhoneUI.SetActive(false);
    }

    public void SetPhoneUI()
    {
        audioSource.PlayOneShot(sound_Stop);
        ConputgerWeaponUI.SetActive(false);
        PhoneUI.SetActive(true);
    }

    public void SetComputerUI_NoSound() //給 PlayerWeaponController 初始化時用的
    {
        ConputgerWeaponUI.SetActive(true);
        PhoneUI.SetActive(false);
    }

    public void SetPhoneUI_NoSound() //給 PlayerWeaponController 初始化時用的
    {
        ConputgerWeaponUI.SetActive(false);
        PhoneUI.SetActive(true);
    }

    public void SetAllUI(bool value) //給 PlayerWeaponController 初始化時用的
    {
        ConputgerWeaponUI.SetActive(value);
        PhoneUI.SetActive(value);
    }
}
