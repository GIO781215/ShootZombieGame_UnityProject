using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class ZombieController : MonoBehaviour
{
    GameObject player;
    ZombieNavMeshAgent zombieNavMeshAgent; //用來獲得自己寫的移動導航物件
    Health health; //用來獲得自己的血量系統物件

    bool gameOver = false;


    public float viewDistance = 15f; //殭屍視野範圍
    float confuseDelayTime = 0f; //玩家在殭屍的視野範圍消失後到困惑的延遲時間  (解決殭屍會一起困惑太整齊的問題)
    bool InConfuseBehaviour = false; //進入 InConfuseBehaviour 的旗標
    float confuseTime = 3f; //當玩家在殭屍的視野範圍內消失後殭屍的困惑時間
    float timeSinceLastSawPlayer = Mathf.Infinity; //初始值設為無限大


    //-------------------設定動畫的參數-------------------
    Animator animatorController; //動畫播放控制器
    //----------------------------------------------------


    //-------------------巡邏相關的參數-------------------
    [SerializeField] public PatrolPath patrolPath; //可以直接從 Unity 中丟給他掛有 patrolPath 腳本的物件
    float timeSinceLastArrivePatrolPoint = 0; //離上次抵達巡邏點經過的時間 
    float timeSinceLastStartPatrol = 0; //離上次開始巡邏經過的時間 (解決永遠達不到下一個巡邏點的問題)
    int GoalPatrolPoint = 0; //當前需要到達的巡邏點
    bool IsPatrol = false; //是否進入巡邏狀態
    bool IsConfuseInPatrol = false; //是否在巡邏中的困惑狀態
    //----------------------------------------------------


    //-------------------攻擊相關的參數-------------------
    float AttackRangeRadius = 2f; //攻擊範圍半徑
    float timeSinceLastAttack = Mathf.Infinity; //上次攻擊後經過的時間
    float timeBetweenAttack = 1.1f; //能再次攻擊的時間間隔
    bool IsAttacking = false; //是否正在攻擊中
                              //----------------------------------------------------


    bool IsTrapped = false; //殭屍莫名狀況卡住時的旗標
    public bool alwaysPatrol = false; //殭屍是否會一直巡邏
    public bool OnDamageIsChasing = false; //殭屍被打到時是否會追玩家

    MutantController mutantController; //魔王物件，要知道如果魔王死了自己也要死

    AnimatorStateInfo BaseLayer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); //找到在 Unity 中將標籤設置為 Player 的 GameObject 
        mutantController = GameObject.FindGameObjectWithTag("Mutant").GetComponent<MutantController>();

        zombieNavMeshAgent = GetComponent<ZombieNavMeshAgent>(); //獲得自己寫的移動導航物件
        animatorController = GetComponentInChildren<Animator>();

        health = GetComponent<Health>();
        health.InitHealth(100, 100);
        health.onDamage += OnDamage; //將自己的函數 OnDamage() 丟進 health 的事件委派容器 onDamage 中
        health.onDie += OnDie; //將自己的函數 OnDie() 丟進 health 的事件委派容器 onDie 中


        //為參數添加一些隨機值，讓每隻殭屍的基本能力有些不一樣
        viewDistance  +=  UnityEngine.Random.Range(0, 5f); // 僵屍的視野範圍在 15~ 20 之間
        confuseDelayTime +=  UnityEngine.Random.Range(0, 0.5f);


        GoalPatrolPoint = patrolPath.GetNextRandomPatrolPointNumber();
    }




    void Update()
    {




        if(gameOver == true )
        {
            return;
        }
        if ((this.health.IsDead() || player.GetComponent<Health>().IsDead()))  //如果殭屍或玩家已經死了那就什麼都不做了
        {
            zombieNavMeshAgent.CancelMove(); //停止移動 
            zombieNavMeshAgent.SetNavMeshAgentSpeed(0); //將控制動畫的變數 WalkSpeed 設為 0 才會播放 idle 動畫
            animatorController.SetBool("IsAttack", false); //解除攻擊動畫
            Invoke("playerDieConfuse", confuseDelayTime);  //最後困惑一下
            gameOver = true;
            return;
        }

        if(mutantController.GetComponent<Health>().currentHealth <=0) //如果魔王沒血了自己直接扣 100 滴血死掉
        {
            health.TakeDamage(100);
        }




        timeSinceLastAttack += Time.deltaTime;
        timeSinceLastSawPlayer += Time.deltaTime;
        if (InConfuseBehaviour) return;




        if (InAttackRange() || IsAttacking) //如果在殭屍的攻擊範圍內，或是已經正在攻擊中
        {
            timeSinceLastSawPlayer = 0;
            AttackBehaviour(); //攻擊行為
      
        }
        else if (InViewRange() && !IsAttacking) //如果在殭屍的視野範圍內
        {
            animatorController.SetBool("IsConfuse", false); //解除困惑動作                                   
            timeSinceLastSawPlayer = 0;
            IsTrapped = false; //解除卡住旗標
            zombieNavMeshAgent.MoveTo(player.transform.position, 1); //移動到玩家位置
            patrolPath.transform.position = this.transform.position; //巡邏點也會一直跟著殭屍移動，直到殭屍停下來才不會再一起動
            patrolPath.transform.rotation = this.transform.rotation;
        }
        else if ((timeSinceLastSawPlayer < confuseTime) && !IsAttacking)   
        {
            InConfuseBehaviour = true;
            ConfuseBehaviour(); //困惑行為
        }
        else if(!IsAttacking)
        {
            PatrolBehaviour(); //巡邏行為
        }



    }

    private void AttackBehaviour()
    {

        if (timeSinceLastAttack > timeBetweenAttack)
        {
            IsAttacking = true;
            timeSinceLastAttack = 0;
            zombieNavMeshAgent.CancelMove(); //停止移動 
            animatorController.SetBool("IsConfuse", false); //解除困惑動作
            animatorController.SetBool("IsAttack", true); //播放攻擊動畫    
        }

        if (timeSinceLastAttack + Time.deltaTime >= timeBetweenAttack && !InAttackRange()) //已經播完動畫了，而且不在攻擊範圍內了，就可以去執行其他行為了   (還是要改成攻擊完才觸發一個函數 重新判斷周圍狀況比較好 ???
        {
            IsAttacking = false;
            animatorController.SetBool("IsAttack", false); //停止攻擊動畫      
        }
    }

    public void AtHit()
    {
        if(InAttackRange()) //如果這時玩家還在攻擊範圍內
        {
            if (player != null)  //玩家就扣寫
            {
                Health health = player.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(30); //扣 30 滴血
                }
            }
        }
    }



    private void PatrolBehaviour()
    {     
        if(IsPatrol || IsConfuseInPatrol) //巡邏中或困惑中
        {
            if(!IsArrivePatrolPoint() && !IsConfuseInPatrol) //如果還沒到達巡邏點，並且不在困惑中
            {
 
                animatorController.SetBool("IsConfuse", false); //困惑動作                                    
                zombieNavMeshAgent.MoveTo(patrolPath.GetPatrolPointPosition(GoalPatrolPoint), 0.2f); //移動到目標巡邏點 

                //有時候會完全走不到下一個巡邏點，如果行走超過一段時間，則判定走不到，直接困惑一次並結束巡邏
                timeSinceLastStartPatrol += Time.deltaTime;
                if (timeSinceLastStartPatrol > 10f) //如果走了十秒還到不了
                {
                    IsPatrol = false; //直接結束巡邏
                    patrolPath.ResethasBeenPatrol();
                    zombieNavMeshAgent.CancelMove();
                    zombieNavMeshAgent.SetNavMeshAgentSpeed(0); //將控制動畫的變數 WalkSpeed 設為 0 才會播放 idle 動畫
                    IsTrapped = true;
                    timeSinceLastStartPatrol = 0;
                }
            }
            else if(IsArrivePatrolPoint()) //剛到達目標巡邏點，開始困惑
            {
 
                zombieNavMeshAgent.CancelMove(); //停止移動 
                IsConfuseInPatrol = true;
                animatorController.SetBool("IsConfuse", true); //開始困惑動畫

                //GoalPatrolPoint = patrolPath.GetNextPatrolPointNumber(GoalPatrolPoint); //將目標轉向下一個巡邏點
                GoalPatrolPoint = patrolPath.GetNextRandomPatrolPointNumber();

                timeSinceLastArrivePatrolPoint = 0;  //重置開始困惑時間
                timeSinceLastStartPatrol = 0; //重置開始巡邏經過時間

                if (patrolPath.PatrolOver == true) //如果所有點都巡邏完，索引值又回到 0 了
                {
                    IsPatrol = false; //巡邏完了
                    zombieNavMeshAgent.SetNavMeshAgentSpeed(0); //將控制動畫的變數 WalkSpeed 設為 0 才會播放 idle 動畫
                    if (alwaysPatrol == true) //如果殭屍設定為會一直巡邏
                    {
                        IsPatrol = true;
                        patrolPath.PatrolOver = false;
                    }
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
            //如果有設定要一直巡邏才會繼續巡邏
            if (alwaysPatrol == true && IsTrapped == false) IsPatrol = true;
        }
    }

    private bool IsArrivePatrolPoint() //是否到達巡邏點了
    {
        return (Vector3.Distance(this.transform.position, patrolPath.GetPatrolPointPosition(GoalPatrolPoint)) < patrolPath.CircleRadius); //當前位置與目標巡邏點的距離是否小於巡邏點半徑了
    }


    private void ConfuseBehaviour()
    {
        zombieNavMeshAgent.CancelMove(); //停止移動 
        zombieNavMeshAgent.SetNavMeshAgentSpeed(0); //將控制動畫的變數 WalkSpeed 設為 0 才會播放 idle 動畫
        animatorController.SetBool("IsAttack", false); //停止攻擊
        Invoke("ConfuseBehaviour_", confuseDelayTime); //經過 confuseDelayTime 時間延遲後才會去執行函數 ConfuseDelayTime()                   
    }

    private bool InViewRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) < viewDistance;  
    }

    private bool InAttackRange()
    {
        return Vector3.Distance(this.transform.position + this.transform.TransformDirection(Vector3.forward * 1.5f), player.transform.position) < AttackRangeRadius;
    }



    void OnDamage()
    {
        if(OnDamageIsChasing)
          StartCoroutine(keepChasing(10f));
        //可以再添加受到攻擊時的動畫、叫聲等等
    }

    void OnDie()
    {
        //死亡叫一下

        zombieNavMeshAgent.CancelMove(); //停止移動 
        animatorController.SetTrigger("IsDead"); //播放死亡動畫
        Invoke("DestroyCollider", 2.5f);
    }
    void DestroyCollider()
    {
        Destroy(GetComponent<CapsuleCollider>()); //銷毀碰撞組件
    }



    private IEnumerator keepChasing(float time) //瘋狂追逐玩家的函數  
    {
        float _viewDistance = viewDistance;
        viewDistance = 10000;
        yield return new WaitForSeconds(time); //等待 time 秒後
        viewDistance = _viewDistance;
    }

    private void ConfuseBehaviour_() //延遲函數 (玩家在殭屍的視野範圍消失後到困惑的延遲時間)
    {
        animatorController.SetBool("IsConfuse", true); //困惑動作
        InConfuseBehaviour = false;
        IsPatrol = true; //困惑完後就可以巡邏了     
        patrolPath.PatrolOver = false;
    }

    private void playerDieConfuse() //玩家死掉後的困惑動作
    {
        animatorController.SetBool("IsConfuse", true); //困惑動作
        Invoke("playerDieConfuseOver", 2.5f + UnityEngine.Random.Range(0, 0.5f));
    }
    private void playerDieConfuseOver()  
    {
        animatorController.SetBool("IsConfuse", false);
    }

    //-------------------------------------------------------------------------------

    //畫圖功能 Debug 用 
    void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            //Gizmos.color = Color.blue;
            //Gizmos.DrawLine(GetPatrolPointPosition(i), GetPatrolPointPosition(j)); //畫線
            //Gizmos.DrawSphere(GetPatrolPointPosition(i), CircleRadius); //畫球
            Handles.color = Color.green;
            Handles.DrawWireDisc(this.transform.position  + this.transform.TransformDirection(Vector3.forward * 1.5f), new Vector3(0, 1, 0), AttackRangeRadius); //畫圓，參數: 圓盤中心、圓盤法線、圓盤半徑 (須 using UnityEditor)
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
