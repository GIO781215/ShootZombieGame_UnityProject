using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class MutantController : MonoBehaviour
{
    GameObject player;
    MutantNavMeshAgent mutantNavMeshAgent; //用來獲得自己寫的移動導航物件
    Health health; //用來獲得自己的血量系統物件

    bool gameOver = false;
    float timeSinceLastGameOver = 0;

    //-------------------設定動畫的參數-------------------
    Animator animatorController; //動畫播放控制器
                                 





    float viewDistance = 28f; //視野範圍，在視野範圍內就會往玩家位置移動
    float confuseTime = 3f; //當玩家在視野範圍內消失後的困惑時間
    float timeSinceLastSawPlayer = Mathf.Infinity; //初始值設為無限大

    //-------------------巡邏相關的參數-------------------
    [SerializeField] public PatrolPath patrolPath; //可以直接從 Unity 中丟給他掛有 patrolPath 腳本的物件
    float timeSinceLastArrivePatrolPoint = 0; //離上次抵達巡邏點經過的時間 
    float timeSinceLastStartPatrol = 0; //離上次開始巡邏經過的時間 (解決永遠達不到下一個巡邏點的問題)
    int GoalPatrolPoint = 0; //當前需要到達的巡邏點
    bool IsPatrol = false; //是否進入巡邏狀態
    bool IsConfuseInPatrol = false; //是否在巡邏中的困惑狀態
 


    //-------------------攻擊相關的參數-------------------
    float JumpAttackRangeRadius = 15f; //偵測跳躍攻擊半徑範圍 
    float HitAttackDamageRangeRadius = 4f; //偵測打擊與攻擊有效半徑範圍 
    float FireBallAttackRangeRadius = 50f; //偵測丟火球攻擊半徑範圍 
    float CancelFireBallAttackRangeRadius = 25f; //偵測不丟火球了的半徑範圍 

    float JumpAttackDamageRangeRadius = 5f; //跳躍攻擊有效半徑範圍 


    bool HitAttacking = false; //當前是否正在打擊攻擊中
    bool JumpAttacking = false; //當前是否正在跳躍攻擊中
    bool FireBallAttacking = false; //當前是否正在丟火球攻擊中




    //-----------打擊角度修正用變數-----------
    Vector3 attackAnglefix = Vector3.zero; //打擊攻擊的角度修正用變數(因為動畫手揮下去的位置太偏了，所以用程式來修正)
    bool IsattackAnglefix = true; //下次攻擊時是否要做角度修正
    float timeSinceLastHitAttack = Mathf.Infinity; //上次攻擊後經過的時間




    //----------------------------------------------------
    AnimatorStateInfo BaseLayer;


    /*  ToDo:
    
    普通攻擊跟跳躍攻擊的傷害不一樣多
    丟火球

    */
    //[SerializeField] fireBallProjectile; //火球子彈預製物件 <- 去寫一個火球腳本 (跟子彈類似的腳本
    [SerializeField] Transform handPosition; //手部座標


    //普通打擊(在某個範圍內) 跳躍打擊(在某個雙圏範圍區間) 火球攻擊(在某個雙圏範圍區間，且血量降到一定數值，且這時就會一直追著玩家跑(大於火球攻擊範圍時))
    //被射到會強制追人一段時間






    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); //找到在 Unity 中將標籤設置為 Player 的 GameObject 
        mutantNavMeshAgent = GetComponent<MutantNavMeshAgent>(); //獲得自己寫的移動導航物件
        animatorController = GetComponent<Animator>();

        health = GetComponent<Health>();
        health.InitHealth(1000, 1000);
        health.onDamage += OnDamage; //將自己的函數 OnDamage() 丟進 health 的事件委派容器 onDamage 中
        health.onDie += OnDie; //將自己的函數 OnDie() 丟進 health 的事件委派容器 onDie 中
    }




    void Update()
    {
        //--------------------------------------------
         if (Input.GetKeyDown(KeyCode.O))
        {
              health.TakeDamage(100);
        }
        //---------------------------------------------*/





        //-------------玩家死亡後要做的動作-------------
        if (gameOver == true)
        {
            timeSinceLastGameOver += Time.deltaTime;
            if (timeSinceLastGameOver > 3f && timeSinceLastGameOver <= 3f + Time.deltaTime)  //最後一次困惑結束
            {
                animatorController.SetBool("IsConfuse", false);
            }
            return;
        }
        if ((this.health.IsDead() || player.GetComponent<Health>().IsDead()))  //如果自己或玩家已經死了那就什麼都不做了
        {
            mutantNavMeshAgent.CancelMove(); //停止移動 
            animatorController.SetBool("IsIdle", true); //進入閒置動作
            animatorController.SetBool("IsAttack", false); //解除攻擊動畫
            animatorController.SetBool("IsConfuse", true); //最後困惑一下
            gameOver = true;
            return;
        }
        //----------------------------------------------





        /*
        //-------------攻擊-------------
        if (InFireBallAttackRange() || IsAttacking && (this.health.currentHealth < health.maxHealth / 2))  //丟火球攻擊
        {
            timeSinceLastSawPlayer = 0;
            FireBallAttack();
        }
        else if (InJumpAttackRange() || IsAttacking)  //跳躍攻擊
        {
            timeSinceLastSawPlayer = 0;
            JumpAttackBehaviour(); //攻擊行為
        }
               */
        if (InHitAttackRange() && !HitAttacking && !JumpAttacking && !FireBallAttacking) //打擊攻擊
        {
            HitAttacking = true; //如果靠動畫的回掉函數 InHitAttacking() 來讓 HitAttacking = true 還是會慢一拍的樣子，所以這邊再直接給他設 true 一次
            timeSinceLastSawPlayer = 0;
            HitAttackBehaviour(); //攻擊行為
        }
        else if(HitAttacking)
        {
            timeSinceLastHitAttack = 0;
        }
        //-------------追逐-------------
        if (InViewRange()   /*  || 被攻擊的時候*/  && !HitAttacking && !JumpAttacking && !FireBallAttacking) //如果在視野範圍內 <---------------------設計成被攻擊的時候也會去追玩家!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        {
            animatorController.SetBool("IsIdle", false);  
            animatorController.SetBool("IsConfuse", false); //解除困惑動作                                   
            timeSinceLastSawPlayer = 0;
            mutantNavMeshAgent.MoveTo(player.transform.position); //移動到玩家位置
            patrolPath.transform.position = this.transform.position; //巡邏點也會一直跟著移動，直到停下來才不會再一起動
        }
        //-------------困惑-------------
        else if ((timeSinceLastSawPlayer < confuseTime) && !HitAttacking && !JumpAttacking && !FireBallAttacking)
        {
            ConfuseBehaviour(); //困惑行為
            IsPatrol = true; //困惑完後就可以巡邏了
        }
        //-------------巡邏-------------
        else if (!HitAttacking && !JumpAttacking && !FireBallAttacking)
        {
            PatrolBehaviour(); //巡邏行為
        }



        timeSinceLastSawPlayer += Time.deltaTime;
        timeSinceLastHitAttack += Time.deltaTime;
        if (timeSinceLastHitAttack > 0.2f)
        {
            IsattackAnglefix = true;
        }
    }

















    //------------------------------ 各種 Behaviour 實作 ------------------------------


    private void FireBallAttack()
    {


    }

    private void JumpAttackBehaviour()
    {
        /*
        if (timeSinceLastAttack > timeBetweenAttack)
        {
            IsAttacking = true;
            timeSinceLastAttack = 0;
            mutantNavMeshAgent.CancelMove(); //停止移動 
            animatorController.SetBool("IsConfuse", false); //解除困惑動作  
            animatorController.SetBool("IsJumpAttack", true); //播放攻擊動畫    
        }

        if (timeSinceLastAttack + Time.deltaTime >= timeBetweenAttack && !InJumpAttackRange()) //已經播完動畫了，而且不在攻擊範圍內了，就可以去執行其他行為了   (還是要改成攻擊完才觸發一個函數 重新判斷周圍狀況比較好 ???
        {
            IsAttacking = false;
            animatorController.SetBool("IsJumpAttack", false); //停止攻擊動畫      
        }
        */
    }


    private void HitAttackBehaviour()
    {
        mutantNavMeshAgent.CancelMove(); //停止移動 
        animatorController.SetBool("IsConfuse", false); //解除困惑動作
        animatorController.SetBool("IsIdle", false); //解除閒置動作
        animatorController.SetTrigger("Attack"); //播放攻擊動畫  
    }




    private void PatrolBehaviour()
    {
        if (IsPatrol || IsConfuseInPatrol) //巡邏中或困惑中
        {
            if (!IsArrivePatrolPoint() && !IsConfuseInPatrol) //如果還沒到達巡邏點，並且不在困惑中
            {
                animatorController.SetBool("IsConfuse", false); //困惑動作                                    
                mutantNavMeshAgent.MoveTo(patrolPath.GetPatrolPointPosition(GoalPatrolPoint)); //移動到目標巡邏點 

                //有時候會完全走不到下一個巡邏點，如果行走超過一段時間，則判定走不到，直接困惑一次並結束巡邏
                timeSinceLastStartPatrol += Time.deltaTime;
                if (timeSinceLastStartPatrol > 3f) //如果走了三秒還到不了
                {
                    IsPatrol = false; //直接結束巡邏
                    mutantNavMeshAgent.CancelMove();
                    animatorController.SetBool("IsIdle", true); //進入閒置動作
                    timeSinceLastStartPatrol = 0;
                    GoalPatrolPoint = 0;
                }
            }
            else if (IsArrivePatrolPoint()) //剛到達目標巡邏點，開始困惑
            {
                mutantNavMeshAgent.CancelMove(); //停止移動 
                IsConfuseInPatrol = true;
                animatorController.SetBool("IsConfuse", true); //開始困惑動畫
                GoalPatrolPoint = patrolPath.GetNextPatrolPointNumber(GoalPatrolPoint); //將目標轉向下一個巡邏點         
                timeSinceLastArrivePatrolPoint = 0;  //重置開始困惑時間
                timeSinceLastStartPatrol = 0; //重置開始巡邏經過時間

                /*//魔王不會停止巡邏---------------------------------
                if (GoalPatrolPoint == 0) //如果所有點都巡邏完，索引值又回到 0 了
                {
                    IsPatrol = false; //巡邏完了
                    animatorController.SetBool("IsIdle", true); //進入閒置動作
                    mutantNavMeshAgent.CancelMove();
                }
                */ //------------------------------------------------
            }
            else //困惑中
            {
                timeSinceLastArrivePatrolPoint = timeSinceLastArrivePatrolPoint + Time.deltaTime; //累積困惑時間
                if (timeSinceLastArrivePatrolPoint > 3f) //如果困惑時間已經大於3秒，才解除困惑繼續巡邏
                {
                    IsConfuseInPatrol = false;
                    animatorController.SetBool("IsConfuse", false); //解除困惑動作
                }
            }
        }
        else
        {
            //進入完全呆滯狀態 
        }
    }

    private void ConfuseBehaviour()
    {
        mutantNavMeshAgent.CancelMove(); //停止移動 
        animatorController.SetBool("IsAttack", false); //停止攻擊
        animatorController.SetBool("IsConfuse", true); //困惑動作                                    
    }



    //--------------------------動畫的回掉函數--------------------------

    public void InHitAttacking()
    {
        HitAttacking = true;
        if (IsattackAnglefix == true)
        {
            IsattackAnglefix = false;

            attackAnglefix = this.transform.TransformDirection(new Vector3(-3, 0, 10));
            StartCoroutine(AttackAnglefix());
        }
    }

    private IEnumerator AttackAnglefix() //平滑地轉向修正後的角度
    {
        for (int i = 0; i < 100; i++)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation , Quaternion.LookRotation(attackAnglefix, Vector3.up), 0.01f);
            yield return new WaitForSeconds(0.01f);
        }
    }




    public void OutHitAttacking()
    {
        HitAttacking = false;
    }

    public void InJumpAttacking()
    {
        JumpAttacking = false;
    }

    public void OutJumpAttacking()
    {
        JumpAttacking = true;
    }

    public void InFireBallAttacking()
    {
        FireBallAttacking = false;
    }

    public void OutFireBallAttacking()
    {
        FireBallAttacking = true;
    }




    public void AtHitAttack()
    {
        if (InHitAttackRange()) //如果這時玩家還在攻擊範圍內
        {
            if (player != null)  //玩家就扣寫
            {
                Health health = player.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(10); //扣 100 滴血
                }
            }
        }
    }


    public void AtJumpAttack()
    {
        print("AtJumpAttack");

    }

    public void GetFireBall()
    {
        //  if(fireBallProjectile != null )
        {
            print("產生火球");//產生火球子彈
        }
    }
    //丟火球攻擊
    public void ShootFireBall()
    {
        //  if(fireBallProjectile != null )
        {
            print("丟出火球");//產生火球子彈
        }
    }








    //------------------------------------範圍判斷函數------------------------------------

    private bool IsArrivePatrolPoint() //是否到達巡邏點了
    {
        return (Vector3.Distance(this.transform.position, patrolPath.GetPatrolPointPosition(GoalPatrolPoint)) < patrolPath.CircleRadius); //當前位置與目標巡邏點的距離是否小於巡邏點半徑了
    }

    private bool InViewRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) < viewDistance;
    }

    private bool InHitAttackRange()
    {
        //return Vector3.Distance(this.transform.position , player.transform.position) < HitAttackRangeRadius; //還是放棄直接轉身攻擊的動作方式好了
        return Vector3.Distance(this.transform.position + this.transform.TransformDirection(new Vector3(2, 0, 4)), player.transform.position) < HitAttackDamageRangeRadius;
    }

    private bool InJumpAttackRange()
    {
        if (InHitAttackRange())
            return false;
        return Vector3.Distance(this.transform.position , player.transform.position) < JumpAttackRangeRadius;
    }

    private bool InFireBallAttackRange()
    {

        return Vector3.Distance(this.transform.position, player.transform.position) < FireBallAttackRangeRadius && Vector3.Distance(this.transform.position, player.transform.position) > CancelFireBallAttackRangeRadius;
    }
















    void OnDamage()
    {
        //受到攻擊時的動畫、叫聲等等

    }

    void OnDie()
    {
        //死亡叫一下
        mutantNavMeshAgent.CancelMove(); //停止移動 
        animatorController.SetTrigger("IsDead"); //播放死亡動畫

    }

















    //---------------------------------------- Debug 用 ------------------------------------------------

    //畫圖功能 Debug 用 
    void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            //Gizmos.color = Color.blue;
            //Gizmos.DrawLine(GetPatrolPointPosition(i), GetPatrolPointPosition(j)); //畫線
            //Gizmos.DrawSphere(GetPatrolPointPosition(i), CircleRadius); //畫球


            Handles.color = Color.yellow;
            Handles.DrawWireDisc(this.transform.position , new Vector3(0, 1, 0), viewDistance); //視野追逐範圍

            Handles.color = Color.red;
            Handles.DrawWireDisc(this.transform.position, new Vector3(0, 1, 0), FireBallAttackRangeRadius);  //火球攻擊範圍
            Handles.DrawWireDisc(this.transform.position, new Vector3(0, 1, 0), CancelFireBallAttackRangeRadius);  //取消火球攻擊範圍
            Handles.DrawWireDisc(this.transform.position, new Vector3(0, 1, 0), JumpAttackRangeRadius);  //跳躍攻擊範圍
            //Handles.DrawWireDisc(this.transform.position, new Vector3(0, 1, 0), HitAttackRangeRadius);  //打擊攻擊範圍

            Handles.color = Color.green;
            Handles.DrawWireDisc(this.transform.position + this.transform.TransformDirection(new Vector3(2,0,4)), Vector3.up, HitAttackDamageRangeRadius);  //打擊攻擊有效範圍
            //GameObject.transform.TransformDirection(Vector3 direction) 能把向量 direction 從物件的 local 座標系轉換到世界座標系上



            
        }
    }


    //----------------------------------------------------------------------------------------
    private void checkAnimatorState() //此函數可判斷 Animator 當前是什麼動作狀態
    {
        BaseLayer = animatorController.GetCurrentAnimatorStateInfo(0); //獲得第 0 層(BaseLayer層) 的資料
        //  print(BaseLayer.fullPathHash);
        //  print(Animator.StringToHash("BaseLayer.attack"));                                                                    

        //判斷 BaseLayer 層狀態機的 Hash 碼是否與指定動作的 Hash 碼相同
        if (BaseLayer.fullPathHash == Animator.StringToHash("Base Layer.Blend Tree"))
        {
            print("idle");
        }

        if (BaseLayer.fullPathHash == Animator.StringToHash("Base Layer.confuse"))
        {
            print("confuse");
        }

        if (BaseLayer.fullPathHash == Animator.StringToHash("Base Layer.attack"))
        {
            print("attack");
        }
    }
    //----------------------------------------------------------------------------------------




}
