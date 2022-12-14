using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("移動參數")]
    float moveSpeed = 5f; //移動速度
    float SpeedMultipler = 2.5f; //按住 Shift 時的加速倍率

    [Tooltip("旋轉速度")]
    [SerializeField] float rotationSpeed = 5f;



    [Space(20)]
    [Header("跳躍參數")]
    [Tooltip("跳躍時向上施加的力量")]
    [SerializeField] float jumpForce;

    [Tooltip("地心引力的力量")]
    [SerializeField] float gravityForce;

    float distanceToGround = 0.1f; //角色與地面之間的距離(小於此距離角色才可跳躍)



    //-------------------設定動畫的參數-------------------
    Animator animatorController; //動畫播放控制器
    float playerMovingSpeed = 0; //player 當前移動速度 (控制混成動畫的變數)
    float playerMovingSpeed_Anime = 0;
    float playerGoalSpeed = 0; //player 的目標速度
    float SpeedChangeRatio = 0.01f; //從當前速度變化到目標速度的快慢比率
    bool CanJumpAgain = true; //是否可以再次跳躍，為了解決播放跳躍動畫時會有二段跳問題而設的參數
    float JumpFreezingTime = 0.4f;//跳躍冷卻時間
    float OnGroundTime = 0; //有踏在地上時間
    //----------------------------------------------------

    //-------------------瞄準動作相關的參數-------------------
    bool IsAim = false;
    public event Action<bool> onAim; //切換成瞄準動作的事件容器
     //----------------------------------------------------

    public event Action onSprint; //用來觸發加速時的特效函數

    float idle_walk_switch_speed = 1f; //電腦版是 0.2 但手機版用 1 才正常 


    InputController inputController;
    CharacterController characterController;
    public Health health; //用來獲得自己的血量系統物件
    PlayerWeaponController playerWeaponController;


    Vector3 moveDirection; //Player 下一幀要移動到的位置
    Vector3 jumpDirection; //Player 下一幀要跳躍到的方向




    void Start()
    {
        inputController = GameManager.Instance.inputController;
        characterController = GetComponent<CharacterController>(); //獲得掛在 player 物件下的 CharacterController 組件
        playerWeaponController = GetComponentInChildren<PlayerWeaponController>();  //獲得掛在 player 物件下的 PlayerWeaponController 組件

        animatorController = GetComponentInChildren<Animator>(); //獲得動畫撥放控制器 (組件是在子物件 PlayFBX 下的，所以使用 GetComponentInChildren< > 去檢索)
        health = GetComponent<Health>();
        health.InitHealth(100, 100);
        health.onDamage += OnDamage; //將自己的函數 OnDamage() 丟進 health 的事件委派容器 onDamage 中
        health.onDie += OnDie; //將自己的函數 OnDie() 丟進 health 的事件委派容器 onDie 中
        health.onHealed += OnHealed;
    }

 
    void Update()
    {
 

        if (health.currentHealth == 0)
            return;


        AimBehaviour();
        //AimBehaviour_PhoneMode();

        MoveBehaviour();
        //MoveBehaviour_PhoneMode();

        if (!IsAim) //不在瞄準模式下才能跳躍
        {
            jumpBehaviour();
            //jumpBehaviour_PhoneMode();
        }

        //處理地心引力墜落
        jumpDirection.y -= gravityForce * Time.deltaTime; //物體無時無刻都受到向下的力
        jumpDirection.y = Mathf.Max(jumpDirection.y, -gravityForce); //向下的力最小就是 gravityForce
        characterController.Move(jumpDirection * Time.deltaTime); //實現物體受力而移動


        /*
        //印出鍵盤移動的輸入值
        if(inputController.GetMoveInput() != Vector3.zero)
        {
            Debug.Log(inputController.GetMoveInput());
        }
        */
    }


    //--------------------------------------手機 UI (滑鼠)控制操作--------------------------------------
    public void AimBehaviour_PhoneMode()
    {
            IsAim = !IsAim;
            animatorController.SetBool("IsAim", IsAim);
            onAim?.Invoke(IsAim);              
    }

    public void ShootAimBehaviour_PhoneMode()
    {
        IsAim = true;
        animatorController.SetBool("IsAim", IsAim);
        onAim?.Invoke(IsAim);
    }

    public void MoveBehaviour_PhoneMode()
    {
        print("Move");
    }

    public void jumpBehaviour_PhoneMode()
    {
        if(IsOnGround() && CanJumpAgain && !IsAim)
        {
            jumpDirection = Vector3.zero;
            jumpDirection += jumpForce * Vector3.up;

            animatorController.SetTrigger("IsJump"); //使用 animatorController 中的 SetTrigger()，就能播放之前設置 Trigger 時觸發的動畫

            //設置可跳躍旗標
            CanJumpAgain = false;
            OnGroundTime = 0;
        }
    }
    //--------------------------------------------------------------------------------------------------- 




    //處理瞄準動作
    private void AimBehaviour()
    {
        if (IsOnGround())
        {
            if (!GameManager.Instance.IsPhoneMode && (inputController.GetMouseLeftKeyDown()))// || inputController.GetSpaceInputDown()))
            {
                IsAim = true;
                animatorController.SetBool("IsAim", IsAim);
                onAim?.Invoke(IsAim); //更改 PlayerWeaponController 的 isAim 變數
            }

            if (!GameManager.Instance.IsPhoneMode && inputController.GetMouseRightKeyDown())// || inputController.GetKeyCInputDown())
            {
                IsAim = !IsAim;
                animatorController.SetBool("IsAim", IsAim);
                onAim?.Invoke(IsAim); //更改 PlayerWeaponController 的 isAim 變數                 
            }
        }
        else //如果不在地板上應該解除瞄準狀態
        {
            IsAim = false;
            animatorController.SetBool("IsAim", IsAim);
            onAim?.Invoke(IsAim);
        }
   
        
        if (inputController.GetSpaceInputHold() ) //如果按下衝刺+移動也解除瞄準狀態
        {
            IsAim = false;
            animatorController.SetBool("IsAim", IsAim);
            onAim?.Invoke(IsAim);
        }
        
    }


    //處理移動
    private void MoveBehaviour()
    {
        //獲得輸入移動方向上的單位向量
        moveDirection = Vector3.zero;
        moveDirection += inputController.GetMoveInput().z * GetCurrentCameraForward();
        moveDirection += inputController.GetMoveInput().x * GetCurrentCameraRight();
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1); //向量正規化，避免對角線速度過快
        playerGoalSpeed = 0.5f; //Player 移動速度設為 0.5 -> 走路狀態

        if (moveDirection == Vector3.zero) //如果鍵盤沒有移動輸入
        {
            playerGoalSpeed = 0f; //Player 移動速度設為 0 -> 閒置狀態
        }
        else if ((inputController.GetSpaceInputHold() || GameManager.Instance.inputController.Phone_Rush) && !IsAim) //是否按下空白鍵加速，並且不在瞄準模式
        {
            moveDirection *= SpeedMultipler;
            playerGoalSpeed = 1f;  //Player 移動速度設為 1 -> 跑步狀態
            onSprint?.Invoke();
        }

        //若鍵盤輸入不為零，也不在瞄準狀態時，才讓 player 的面朝方向轉向鍵盤輸入的移動方向
        if (moveDirection != Vector3.zero && !IsAim)
        {
            SmoothRotation(moveDirection); //讓 player 的面朝方向轉動到 moveDirection 的方向上
        }
        else if(IsAim) //如果是在瞄準狀態，就讓 Player 的朝向一直與攝影機的朝向同向
        {      
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(GetCurrentCameraForward(), Vector3.up), rotationSpeed * Time.deltaTime *　10);  //讓 player 的面朝方向轉動到攝影機的方向上，但人物的 y 軸保持向上不變
        }

        //這一幀與下一幀 player 的移動速度做差值，讓 walkSpeed 的值變動得更平滑
        if (playerMovingSpeed != playerGoalSpeed) //其實不用這個 if 判斷也沒關係
        {
            playerMovingSpeed = Mathf.Lerp(playerMovingSpeed, playerGoalSpeed, SpeedChangeRatio);
            playerMovingSpeed_Anime = Mathf.Lerp(playerMovingSpeed, playerGoalSpeed, idle_walk_switch_speed); 
        }

        animatorController.SetFloat("walkSpeed", playerMovingSpeed_Anime); //改變變數 walkSpeed 就能從 animatorController 中播放對應的動畫
        animatorController.SetFloat("Vertical", inputController.GetMoveInput().z);  
        animatorController.SetFloat("Horizontal", inputController.GetMoveInput().x);
        //上下跟左右快速切換會導致動畫跳針，限制上鍵按下鬆開一段時間後按下鍵才有移動效果，或是上鍵按下後再按下鍵過一段時間後才有移動效果，左右同理 <--------------------------之後再來實現這功能


        #region //CharacterControlle 組件與常用函數的說明 
        /*
         *  添加 CharacterController 的效果:
         *  1. 不用處理剛體，没有碰撞效果
         *  2. 添加 charactercontroller 的物體不受外力的作用
         * 
         *  常用方法:
         *  SimpleMove(Vector3 forward)
         *  1. SimpleMovez 自帶重力效果，對Y軸運動絕對控制。
         *  2. SimpleMove 返回值 Bool 類型，角色接觸地面則返回true，否則返回false。
         *  
         *  Move(Vector3 forward)
         *  1. Move，直接定位角色狀態為靜態或者動態，且沒有自帶重力效果。除 Move 以外唯一會影響運動狀態的就是各種障礙物的剛體碰撞，會使物體沿著剛體滑動.
         *  2. Move 返回一系列碰撞物體的信息。
         *  
         *  OnControllerColliderHit()
         *  當物體移動時發生碰撞就發觸發該函數
         * 
         * 
         *  官方示例1:
         *  if (characterController.isGrounded)
         *  {
         *      moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
         *      moveDirection *= speed;
         *      if (Input.GetButton("Jump"))  moveDirection.y = jumpSpeed;
         *  }
         *  moveDirection.y -= gravity * Time.deltaTime;
         *  characterController.Move(moveDirection * Time.deltaTime);
         * 
         * 
         *  官方示例2:
         *  transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);
         *  Vector3 forward = transform.TransformDirection(Vector3.forward);
         *  float curSpeed = speed * Input.GetAxis("Vertical");
         *  controller.SimpleMove(forward * curSpeed);
         * 
         */
        #endregion //
        characterController.Move(moveDirection * Time.deltaTime * moveSpeed); //使 player 往向量參數的方向移動 (乘上 Time.deltaTime 可讓移動速度看起來更平均)      
    }

    private void SmoothRotation(Vector3 moveDirection)
    {
        #region //Transform.LookAt()、Quaternion.LookRotation()、.Lerp()、.Slerp() 的解釋
        /*
  
        Transform.LookAt():
        public void LookAt(Transform target);
        public void LookAt(Transform target, Vector3 worldUp = Vector3.up);
        參數 target 為看向的目标
        參數 worldUp 為物體的向上方向，默認為 Vector3.up，(0, 1, 0)
        讓物體的 Z 軸指向 target，並指定物體的 Y 軸為 worldUp

        Quaternion.LookRotation():
        public static Quaternion LookRotation(Vector3 forward, Vector3 upwards = Vector3.up);
        參數 forward 為看向的目标
        參數 upwards 指定向上的方向，默認 Vector3.up，(0, 1, 0)
        意義與 Transform.LookAt() 相同，出來的結果為表示此旋轉方向的四元數
        應用例:
        1. 創建 form 指向 to 的 Quaternion(四元數)的方法： Quaternion.LookRotation(form - to);
        2.模仿 LookAt：       
        void myLookAt(Vector3 form, Vector3 to)
        {
            transform.rotation = Quaternion.LookRotation(form - to);
        }
       
        .Lerp()
        unity 中很多類都有 Lerp 的屬性，比如Vector3.Lerp、Quaternion.Lerp、Color.Lerp  用法都一樣，這個屬性叫做插值，兩個向量之間的線性插值。
        例如 Vector3.Lerp :
        static function Lerp (from : Vector3, to : Vector3, t : float) : Vector3
        按照數字 t 在 from 到 to 之間插值，t 是夾在 [0...1] 之間，當 t = 0 時，返回 from，當 t = 1 時，返回 to。當 t = 0.5 返回 from 和 to 的平均數。
        注意第三個參數 t，不要理解為 T 時間內從 from 到 to，平時比如移動位移的時候或做旋轉的時候，可以把 t 作為時間單位，但是一定要注意這個t的值是從 0 到 1 範圍。

        .Slerp():
        意義近乎同上
        例如 Quaternion.Slerp():
        public static Quaternion Slerp(Quaternion a, Quaternion b, float t);
        a 到 b 的"球形插值"，t 是比例。

        */
        #endregion
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDirection, Vector3.up), rotationSpeed * Time.deltaTime); 
    }

    //處理跳躍
    private void jumpBehaviour()
    {
        //Debug.DrawRay(transform.position, Vector3.down* distanceToGround, Color.red); //可看出有效接觸地板的距離
        if (inputController.GetKeyEInputDown() && IsOnGround() && CanJumpAgain)
        {
            jumpDirection = Vector3.zero;
            jumpDirection += jumpForce * Vector3.up;

            animatorController.SetTrigger("IsJump"); //使用 animatorController 中的 SetTrigger()，就能播放之前設置 Trigger 時觸發的動畫

            //設置可跳躍旗標
            CanJumpAgain = false;
            OnGroundTime = 0;
        }

        if(!CanJumpAgain && IsOnGround()) //重置可跳躍旗標
        {
            OnGroundTime += Time.deltaTime;
            if (OnGroundTime >= JumpFreezingTime) CanJumpAgain = true;
        }
    }


    void OnDamage()
    {
         //受到攻擊時的動畫、叫聲等等
    }

    void OnDie()
    {
        //死亡音效
        animatorController.SetTrigger("IsDead"); //播放死亡動畫
    }

    void OnHealed()
    {

    }


 

    //獲得當前相機的正前方方向
    private Vector3 GetCurrentCameraForward()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize(); //向量正規化
        return cameraForward;
    }

    //獲得當前相機的正右方方向
    private Vector3 GetCurrentCameraRight()
    {
        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize(); //向量正規化
        return cameraRight;
    }

    private bool IsOnGround()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1f);  //參數意義: (射線發射點, 射線方向, 射線長度)，回傳值為射線是否有射到障礙物
        //CharacterController、CapsuleCollider、PlayerController 等組件都掛載在空物件 Player 底下，而網格模型則是在 PlayerMesh 中，Player 的錨點在 PlayerMesh 的正下方，所以 Player 往下一點距離就是地板了
        //但這樣做的話要記得 CharacterController 與 CapsuleCollider 的位置都要再往上移才能與 PlayerMesh 的模型位置吻合
    }

}
