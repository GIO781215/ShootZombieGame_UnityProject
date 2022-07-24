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


    float viewDistance = 10f; //殭屍視野範圍
    float confuseTime = 3f; //當玩家在殭屍的視野範圍內消失後殭屍的困惑時間
    float timeSinceLastSawPlayer = Mathf.Infinity; //初始值設為無限大


    //-------------------設定動畫的參數-------------------
    Animator animatorController; //動畫播放控制器
    //----------------------------------------------------


    //-------------------巡邏相關的參數-------------------
    [SerializeField] public PatrolPath PatrolPath; //可以直接從 Unity 中丟給他掛有 PatrolPath 腳本的物件
    float timeSinceLastArrivePatrolPoint = 0; //離上次抵達巡邏點經過的時間 
    float timeSinceLastStartPatrol = 0; //離上次開始巡邏經過的時間 (解決永遠達不到下一個巡邏點的問題)
    int GoalPatrolPoint = 0; //當前需要到達的巡邏點
    bool IsPatrol = false; //是否進入巡邏狀態
    bool IsConfuseInPatrol = false; //是否在巡邏中的困惑狀態
    //----------------------------------------------------


    //-------------------攻擊相關的參數-------------------
    float AttackRangeRadius = 1f; //攻擊範圍半徑
    float timeSinceLastAttack = Mathf.Infinity; //上次攻擊後經過的時間
    float timeBetweenAttack = 1.1f; //能再次攻擊的時間間隔
    bool IsAttacking = false; //是否正在攻擊中


    float JumpAttackRangeRadius = 2f; //跳躍攻擊距離

    //----------------------------------------------------
    AnimatorStateInfo BaseLayer;


    /*  ToDo:
    
    普通攻擊跟跳躍攻擊的傷害不一樣多
    丟火球

    */
    //[SerializeField] fireBallProjectile; //火球子彈預製物件 <- 去寫一個火球腳本 (跟子彈類似的腳本
    [SerializeField] Transform handPosition; //手部座標


    //普通打擊(在某個範圍內) 跳躍打擊(在某個雙圏範圍區間) 火球攻擊(在某個雙圏範圍區間，且血量降到一定數值)
    //被射到會強制追人一段時間






    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); //找到在 Unity 中將標籤設置為 Player 的 GameObject 
        mutantNavMeshAgent = GetComponent<MutantNavMeshAgent>(); //獲得自己寫的移動導航物件
        animatorController = GetComponentInChildren<Animator>();

        health = GetComponent<Health>();
        health.InitHealth(100, 100);
        health.onDamage += OnDamage; //將自己的函數 OnDamage() 丟進 health 的事件委派容器 onDamage 中
        health.onDie += OnDie; //將自己的函數 OnDie() 丟進 health 的事件委派容器 onDie 中

    }




    void Update()
    {
        //--------------------------------------------
        //print("焦屍血量:" + health.GetCurrentHealth());
        if (Input.GetKeyDown(KeyCode.V))
        {
            //  health.TakeDamage(40);
        }
        //---------------------------------------------*/




        if (gameOver == true)
        {
            timeSinceLastGameOver += Time.deltaTime;
            if (timeSinceLastGameOver > 3f && timeSinceLastGameOver <= 3f + Time.deltaTime)  //最後一次困惑結束
            {
                animatorController.SetBool("IsConfuse", false);
            }
            return;
        }
        if ((this.health.IsDead() || player.GetComponent<Health>().IsDead()))  //如果殭屍或玩家已經死了那就什麼都不做了
        {
            mutantNavMeshAgent.CancelMove(); //停止移動 
            //mutantNavMeshAgent.SetNavMeshAgentSpeed(0); //將控制動畫的變數 WalkSpeed 設為 0 才會播放 idle 動畫
            animatorController.SetBool("IsAttack", false); //解除攻擊動畫
            animatorController.SetBool("IsConfuse", true); //最後困惑一下
            gameOver = true;
            return;
        }






        if (InAttackRange() || IsAttacking) //如果在殭屍的攻擊範圍內，或是已經正在攻擊中
        {
            timeSinceLastSawPlayer = 0;
            AttackBehaviour(); //攻擊行為
        }
        else if(InJumpAttackRange() || IsAttacking)
        {
            timeSinceLastSawPlayer = 0;
            JumpAttackBehaviour(); //攻擊行為
        }
        else if (InViewRange() && !IsAttacking || false /*被攻擊的時候*/  ) //如果在殭屍的視野範圍內 <---------------------設計成被攻擊的時候也會去追玩家!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        {
            animatorController.SetBool("IsConfuse", false); //解除困惑動作                                   
            timeSinceLastSawPlayer = 0;
            mutantNavMeshAgent.MoveTo(player.transform.position); //移動到玩家位置
            PatrolPath.transform.position = this.transform.position; //巡邏點也會一直跟著殭屍移動，直到殭屍停下來才不會再一起動
            PatrolPath.transform.rotation = this.transform.rotation;
        }
        else if ((timeSinceLastSawPlayer < confuseTime) && !IsAttacking)
        {
            ConfuseBehaviour(); //困惑行為
            IsPatrol = true; //困惑完後就可以巡邏了
        }
        else if (!IsAttacking)
        {
            PatrolBehaviour(); //巡邏行為
        }


        timeSinceLastAttack += Time.deltaTime;
        timeSinceLastSawPlayer += Time.deltaTime;


    }



    private void AttackBehaviour()
    {

        if (timeSinceLastAttack > timeBetweenAttack)
        {
            IsAttacking = true;
            timeSinceLastAttack = 0;
            mutantNavMeshAgent.CancelMove(); //停止移動 
            animatorController.SetBool("IsConfuse", false); //解除困惑動作
            animatorController.SetBool("IsAttack", true); //播放攻擊動畫    
        }

        if (timeSinceLastAttack + Time.deltaTime >= timeBetweenAttack && !InAttackRange()) //已經播完動畫了，而且不在攻擊範圍內了，就可以去執行其他行為了   (還是要改成攻擊完才觸發一個函數 重新判斷周圍狀況比較好 ???
        {
            IsAttacking = false;
            animatorController.SetBool("IsAttack", false); //停止攻擊動畫      
        }
    }

    private void JumpAttackBehaviour()
    {

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
    }






    public void AtHit()
    {
        if (InAttackRange()) //如果這時玩家還在攻擊範圍內
        {
            if (player != null)  //玩家就扣寫
            {
                Health health = player.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(90); //扣 90 滴血
                }
            }
        }
    }



    private void PatrolBehaviour()
    {
        if (IsPatrol || IsConfuseInPatrol) //巡邏中或困惑中
        {
            if (!IsArrivePatrolPoint() && !IsConfuseInPatrol) //如果還沒到達巡邏點，並且不在困惑中
            {
                animatorController.SetBool("IsConfuse", false); //困惑動作                                    
                mutantNavMeshAgent.MoveTo(PatrolPath.GetPatrolPointPosition(GoalPatrolPoint)); //移動到目標巡邏點 

                //有時候會完全走不到下一個巡邏點，如果行走超過一段時間，則判定走不到，直接困惑一次並結束巡邏
                timeSinceLastStartPatrol += Time.deltaTime;
                if (timeSinceLastStartPatrol > 8f) //如果走了八秒還到不了
                {
                    IsPatrol = false; //直接結束巡邏
                    //mutantNavMeshAgent.SetNavMeshAgentSpeed(0); //將控制動畫的變數 WalkSpeed 設為 0 才會播放 idle 動畫
                    timeSinceLastStartPatrol = 0;
                    GoalPatrolPoint = 0;
                }
            }
            else if (IsArrivePatrolPoint()) //剛到達目標巡邏點，開始困惑
            {
                mutantNavMeshAgent.CancelMove(); //停止移動 
                IsConfuseInPatrol = true;
                animatorController.SetBool("IsConfuse", true); //開始困惑動畫
                GoalPatrolPoint = PatrolPath.GetNextPatrolPointNumber(GoalPatrolPoint); //將目標轉向下一個巡邏點         
                timeSinceLastArrivePatrolPoint = 0;  //重置開始困惑時間
                timeSinceLastStartPatrol = 0; //重置開始巡邏經過時間

                if (GoalPatrolPoint == 0) //如果所有點都巡邏完，索引值又回到 0 了
                {
                    IsPatrol = false; //巡邏完了
                    //mutantNavMeshAgent.SetNavMeshAgentSpeed(0); //將控制動畫的變數 WalkSpeed 設為 0 才會播放 idle 動畫
                }
            } //困惑中
            else
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

    private bool IsArrivePatrolPoint() //是否到達巡邏點了
    {
        return (Vector3.Distance(this.transform.position, PatrolPath.GetPatrolPointPosition(GoalPatrolPoint)) < PatrolPath.CircleRadius); //當前位置與目標巡邏點的距離是否小於巡邏點半徑了
    }


    private void ConfuseBehaviour()
    {
        mutantNavMeshAgent.CancelMove(); //停止移動 
        animatorController.SetBool("IsAttack", false); //停止攻擊
        animatorController.SetBool("IsConfuse", true); //困惑動作                                    
    }

    private bool InViewRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) < viewDistance;
    }

    private bool InAttackRange()
    {
        return Vector3.Distance(this.transform.position + this.transform.TransformDirection(Vector3.forward * 1f), player.transform.position) < AttackRangeRadius;
    }

    private bool InJumpAttackRange()
    {
        return Vector3.Distance(this.transform.position + this.transform.TransformDirection(Vector3.forward * 1f), player.transform.position) < JumpAttackRangeRadius;
    }





    //丟火球攻擊
    private void ShootFireBall()
    {
      //  if(fireBallProjectile != null )
        {
            //產生火球子彈
        }
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



    //畫圖功能 Debug 用 
    void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            //Gizmos.color = Color.blue;
            //Gizmos.DrawLine(GetPatrolPointPosition(i), GetPatrolPointPosition(j)); //畫線
            //Gizmos.DrawSphere(GetPatrolPointPosition(i), CircleRadius); //畫球
            Handles.color = Color.green;
            Handles.DrawWireDisc(this.transform.position + this.transform.TransformDirection(Vector3.forward * 1f), new Vector3(0, 1, 0), AttackRangeRadius); //畫圓，參數: 圓盤中心、圓盤法線、圓盤半徑 (須 using UnityEditor)
                                                                                                                                                              //GameObject.transform.TransformDirection(Vector3 direction) 能把向量 direction 從物件的 local 座標系轉換到世界座標系上
            Handles.color = Color.yellow;
            Handles.DrawWireDisc(this.transform.position + this.transform.TransformDirection(Vector3.forward * 1f), new Vector3(0, 1, 0), JumpAttackRangeRadius);
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
