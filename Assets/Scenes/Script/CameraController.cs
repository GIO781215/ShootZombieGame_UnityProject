using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameManager gameManager;
    InputController inputController;
    [SerializeField] GameObject Victory_UI;
    [SerializeField] GameObject Lose_UI;

    public bool IsPlayerDeath = false; //玩家是否死亡旗標
    public bool IsMutantDeath = false; //魔王是否死亡旗標

    [SerializeField] AudioClip sound_BGM; //BGM
    [SerializeField] AudioClip sound_Restart; //重新開始音效
    [SerializeField] AudioClip sound_Victory; //勝利音效
    [SerializeField] GameObject restartButton; //重新開始的按鈕

    [SerializeField] AudioClip sound_Lose; //失敗音效
    AudioSource audioSource;


    [Header("相機跟隨的目標")]
    public Transform target; //private 可省略，預設就是 private
    //[SerializeField] private Transform target_temp; //刪除玩家時攝影機要變更的追隨目標

    [SerializeField] GameObject player; //做受傷特效用，要得到玩家的血量系統
    [SerializeField] GameObject mutant; //判別打倒魔王後跳出勝利UI用的

    [SerializeField] ParticleSystem behitEffect; //受傷時的特效
    [SerializeField] ParticleSystem rushEffect; //衝刺時的特效



    [Header("攝影機的高度")]
    [SerializeField] float HeightOffset = 3; //攝影機的高度
    float CameraAngle_X = 0; //攝影機左右起始角度
    float CameraAngle_Y = 15; //攝影機上下起始角度
    float sensitivity_X = 2; //滑鼠控制攝影機左右移動的靈敏度
    float sensitivity_Y = 2; //滑鼠控制攝影機上下移動的靈敏度
    float sensitivity_ScrollWheel = 5; //滑鼠控制攝影機前後移動的靈敏度
    float minVerticalAngle = -10; //攝影機上下移動的最小角度 (其實是往上仰視的最大角度，因為在 Unity 裡的 Edir -> Project Setting -> Input Manager -> Mouse Y 有勾選 Invert，所以上下有相反)
    float maxVerticalAngle = 20; //攝影機上下移動的最大角度 (其實是往下俯視的最大角度，因為在 Unity 裡的 Edir -> Project Setting -> Input Manager -> Mouse Y 有勾選 Invert，所以上下有相反)
    float cameraToTargetDistance = 10; //攝影機與目標的起始距離
    float cameraToTargetMinDistance = 5; //攝影機與目標的最小距離
    float cameraToTargetMaxDistance = 15; //攝影機與目標的最大距離
    [SerializeField] float CameraAngle_Offset; //攝影機 Y 軸的起始旋轉角度
    bool IsFirsrRun = true; //第一次執行 LateUpdate() 的旗標

    //-----------------讓攝影機平滑追蹤物體需要的變數//-----------------
    [Header("攝影機平滑追蹤物體的時間")]
    [SerializeField] float smoothMoveTime = 0.2f; //攝影機平滑追蹤物體的時間
    Vector3 referenceObjectPosition = Vector3.zero; //虛擬參考物的座標位置
    Vector3 currentVelocity = Vector3.zero; //Vector3.SmoothDamp() 要使用到的全局變量變數 (此值為虛擬參考物的當前速度)
    Vector3 referenceObjectToCameraOffset = Vector3.zero; //虛擬參考物距離攝影機的距離

    //用來暫存 W S A D 輸入的變數
    float WSAD_speed = 0.1f; // W S A D 輸入讓視角移動的快慢
    float WSAD_Max_speed = 0.8f; // W S A D 輸入讓視角移動的最大速度
    float W_value = 0;
    float S_value = 0;
    float A_value = 0;
    float D_value = 0;





    void Start()
    {
        inputController = GameManager.Instance.inputController;
        player.GetComponent<Health>().onDamage += OnDamage;
        player.GetComponent<Health>().onDie += OnDie_Player;
        player.GetComponent<PlayerController>().onSprint += OnSprint;

        mutant.GetComponent<Health>().onDie += OnDie_Mutant;
        audioSource = GetComponent<AudioSource>();

        //設置勝利與失敗的UI
        Victory_UI.SetActive(true);
        Victory_UI.GetComponent<Victory_UI_Controller>().Hied();
        Lose_UI.SetActive(true);
        Lose_UI.GetComponent<Lose_UI_Controller>().Hied();

        //一開始先播放 BGM
        audioSource.volume = 0.5f;
        audioSource.loop = true;
        audioSource.clip = sound_BGM;
        audioSource.Play();

    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && (IsPlayerDeath || IsMutantDeath))
        {
            GameManager.Instance.RestartGame();
        }
    }

    private void LateUpdate() //在 Update 後執行
    {
        if (Cursor.lockState == CursorLockMode.Locked || GameManager.Instance.IsPhoneMode)
        {


            //設置攝影機角度----------------------------------------------------------------------------------------------------------------------------------------------------------------------------

             //---------------------------------電腦版的控制---------------------------------
            if (!GameManager.Instance.IsPhoneMode) 
            {
                //滑鼠輸入
                CameraAngle_X += inputController.GetMouseX() * sensitivity_X;
                //鍵盤輸入
                if (inputController.GetDInputHold())
                {
                    D_value = Mathf.Lerp(D_value, WSAD_Max_speed, WSAD_speed);
                }
                else
                {
                    D_value = 0;
                }
                CameraAngle_X += D_value;

                if (inputController.GetAInputHold())
                {
                    A_value = Mathf.Lerp(A_value, WSAD_Max_speed, WSAD_speed);
                }
                else
                {
                    A_value = 0;
                }
                CameraAngle_X -= A_value;

                //------------------- 

                //滑鼠輸入
                CameraAngle_Y += inputController.GetMouseY() * sensitivity_Y;
                //鍵盤輸入
                if (inputController.GetWInputHold())
                {
                    W_value = Mathf.Lerp(W_value, WSAD_Max_speed, WSAD_speed);
                }
                else
                {
                    W_value = 0;
                }
                CameraAngle_Y -= W_value;

                if (inputController.GetSInputHold())
                {
                    S_value = Mathf.Lerp(S_value, WSAD_Max_speed, WSAD_speed);
                }
                else
                {
                    S_value = 0;
                }
                CameraAngle_Y += S_value;
            }
             
            //---------------------------------手機版的控制---------------------------------
            if (GameManager.Instance.IsPhoneMode)  
            {
                //有按住攝影機圖示的時候
                if (GameManager.Instance.inputController.Phone_Camera)
                {
                    CameraAngle_X += inputController.GetMouseX() * sensitivity_X;
                    CameraAngle_Y += inputController.GetMouseY() * sensitivity_Y;
                }
            }
            //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------





            if (IsFirsrRun)
            {
                CameraAngle_X += CameraAngle_Offset; //遊戲一開始先設定攝影機的旋轉角度朝向
            }

            CameraAngle_Y = Mathf.Clamp(CameraAngle_Y, minVerticalAngle, maxVerticalAngle); //限制 CameraAngle_Y 的最大角度與最小角度
            transform.rotation = Quaternion.Euler(CameraAngle_Y, CameraAngle_X, 0);


            //設置攝影機位置
            //滑鼠輸入
            cameraToTargetDistance += inputController.GetMouseScrollWheel() * sensitivity_ScrollWheel;
            //鍵盤輸入
            if (inputController.GetQInputHold())
            {
                cameraToTargetDistance += 0.02f;
            }
            if (inputController.GetEInputHold())
            {
                cameraToTargetDistance -= 0.02f;
            }
            cameraToTargetDistance = Mathf.Clamp(cameraToTargetDistance, cameraToTargetMinDistance, cameraToTargetMaxDistance);

            if (IsFirsrRun)
            {
                referenceObjectPosition = target.position;
                IsFirsrRun = false;
            }
            else
            {
                //追蹤方式為 攝影機追虛擬參考物(無平滑效果) 虛擬參考物追 Player (有平滑效果)
                //得到虛擬參考物的位置，因為最後有對 transform.position 加上 referenceObjectToCameraOffset，所以這邊要減回來才是 虛擬參考物應該在的位置
                referenceObjectPosition = Vector3.SmoothDamp(transform.position - referenceObjectToCameraOffset, target.position, ref currentVelocity, smoothMoveTime);
                #region //SmoothDamp() 平滑阻尼函數 : 進行跟隨移動，可以使跟隨看起來很平滑，而不顯得突兀    
                /*
                static function SmoothDamp (current : Vector3, target : Vector3, ref currentVelocity : Vector3, smoothTime : float, maxSpeed : float = Mathf.Infinity, deltaTime : float = Time.deltaTime) : Vector3
                參數含義：
                1.current 當前物體位置
                2.target 目標物體位置
                3.ref currentVelocity 當前速度，這個值由你每次調用這個函數時被修改（因為使用 ref 關鍵字，所以函數會真的改變 currentVelocity 這個變數，這代表可以在讓何時刻得到物體當前的速度 currentVelocity）
                  注意 : 變數 currentVelocity 要使用全局變量，如果定義為局部?量移動效果會出問題，參閱文章 <Unity中Lerp與SmoothDamp函數使用誤區淺析> : https://www.jianshu.com/p/8a5341c6d5a6
                4.smoothTime 到達目標的時間，較小的值將快速到達目標
                5.maxSpeed 所允許的最大速度，默認無窮大
                6.deltaTime 自上次調用這個函數的時間，默認為Time.deltaTime
                */
#endregion
                }

                referenceObjectPosition.y = HeightOffset; //指定虛擬參考物的高度為 HeightOffset (不受 Player 跳起來而改變)
            referenceObjectToCameraOffset = Quaternion.Euler(CameraAngle_Y, CameraAngle_X, 0) * new Vector3(0, 0, -cameraToTargetDistance); // Quaternion.Euler 可以乘 Vector3 ，結果為往那個方向的向量長度!!! 
            transform.position = referenceObjectPosition + referenceObjectToCameraOffset; //從虛擬參考物的位置(受 SmoothDamp() 效果影響) 在加上 referenceObjectToCameraOffset 即為攝影機該在的位置

        }
    }

    private void OnDamage()
    {
        if (behitEffect != null)
        {
            behitEffect.Play();
        }
    }

    private void OnDie_Player()
    {
        IsPlayerDeath = true;
        Invoke("playLoseSound", 1.5f);
        Lose_UI.GetComponent<Lose_UI_Controller>().Show();
    }

    void playLoseSound()
    {
        if (IsPlayerDeath == true)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(sound_Lose);
            Cursor.lockState = CursorLockMode.None; //解放滑鼠
        }
    }

    private void OnDie_Mutant()
    {
        IsMutantDeath = true;
        Invoke("playVictorySound", 1.5f);
        Victory_UI.GetComponent<Victory_UI_Controller>().Show();
    }

    void playVictorySound()
    {
        if (IsMutantDeath == true)
        {
            audioSource.Stop();
            audioSource.volume = 0.3f;
            audioSource.PlayOneShot(sound_Victory);
            Cursor.lockState = CursorLockMode.None; //解放滑鼠
        }
    }

    private void OnSprint()
    {
        if (rushEffect != null)
        {
            //先把跑步特效關掉好了
            rushEffect.Play();
        }
    }




    public void resetCamera() //會被 GameManager 中重新開始遊戲的函數 RestartGame() 呼叫到
    {
       HeightOffset = 3; //攝影機的高度
       CameraAngle_X = 0; //攝影機左右起始角度
       CameraAngle_Y = 15; //攝影機上下起始角度
       sensitivity_X = 2; //滑鼠控制攝影機左右移動的靈敏度
       sensitivity_Y = 2; //滑鼠控制攝影機上下移動的靈敏度
       sensitivity_ScrollWheel = 5; //滑鼠控制攝影機前後移動的靈敏度
       minVerticalAngle = -10; //攝影機上下移動的最小角度 (其實是往上仰視的最大角度，因為在 Unity 裡的 Edir -> Project Setting -> Input Manager -> Mouse Y 有勾選 Invert，所以上下有相反)
       maxVerticalAngle = 20; //攝影機上下移動的最大角度 (其實是往下俯視的最大角度，因為在 Unity 裡的 Edir -> Project Setting -> Input Manager -> Mouse Y 有勾選 Invert，所以上下有相反)
       cameraToTargetDistance = 10; //攝影機與目標的起始距離
       cameraToTargetMinDistance = 5; //攝影機與目標的最小距離
       cameraToTargetMaxDistance = 15; //攝影機與目標的最大距離
       CameraAngle_Offset = 270; //攝影機 Y 軸的起始旋轉角度
       IsFirsrRun = true; //第一次執行 LateUpdate() 的旗標

        //重新對玩家物件加入委託事件的函數
        PlayerController playerController = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerController>();
        playerController.onSprint += OnSprint;
        playerController.GetComponent<Health>().onDamage += OnDamage;
        playerController.GetComponent<Health>().onDie += OnDie_Player;
        //重新對魔王物件加入委託事件的函數
        MutantController mutantController = GameObject.FindGameObjectsWithTag("Mutant")[0].GetComponent<MutantController>();
        mutantController.GetComponent<Health>().onDie += OnDie_Mutant;

        //隱藏勝利與失敗的UI
        Victory_UI.SetActive(true);
        Lose_UI.SetActive(true);
        Victory_UI.GetComponent<Victory_UI_Controller>().Hied();
        Lose_UI.GetComponent<Lose_UI_Controller>().Hied();
        restartButton.gameObject.SetActive(false);


        //播放音效
        audioSource.PlayOneShot(sound_Restart);

        audioSource.volume = 0.5f;
        audioSource.loop = true;
        audioSource.clip = sound_BGM;
        audioSource.Play();

        //重置參數
        IsPlayerDeath = false;
        IsMutantDeath = false;

        //調整滑鼠焦點狀態，之後可能會再調整這個寫法，注意一下!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        Cursor.lockState = CursorLockMode.Locked;
        if (GameManager.Instance.IsPhoneMode)
        {
            Invoke("setCursorLockMode", 0.5f);
        }
    }

    void setCursorLockMode()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
