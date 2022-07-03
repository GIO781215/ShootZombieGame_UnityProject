using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    GameObject player;
    ZombieNavMeshAgent zombieNavMeshAgent; //用來獲得自己寫的移動導航物件
    Health health; //用來獲得自己的血量系統物件

    float viewDistance = 10f; //殭屍視野範圍
    float confuseTime = 3f; //當玩家在殭屍的視野範圍內消失後殭屍的困惑時間
    float timeSinceLastSawPlayer = Mathf.Infinity; //初始值設為無限大



    //-------------------設定動畫的參數-------------------
    Animator animatorController; //動畫播放控制器
    //----------------------------------------------------


    //-------------------巡邏相關的參數-------------------
    [SerializeField] public ZombiePatrolPath zombiePatrolPath; //可以直接從 Unity 中丟給他掛有 ZombiePatrolPath 腳本的物件
    float timeSinceLastArrivePatrolPoint = 0; //離上次抵達巡邏點經過的時間 
    float timeSinceLastStartPatrol = 0; //離上次開始巡邏經過的時間 (解決永遠達不到下一個巡邏點的問題)
    int GoalPatrolPoint = 0; //當前需要到達的巡邏點
    bool IsPatrol = false; //是否進入巡邏狀態
    bool IsConfuseInPatrol = false; //是否在巡邏中的困惑狀態
    //----------------------------------------------------







    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); //找到在 Unity 中將標籤設置為 Player 的 GameObject 
        zombieNavMeshAgent = GetComponent<ZombieNavMeshAgent>(); //獲得自己寫的移動導航物件
        animatorController = GetComponentInChildren<Animator>();

        health = GetComponent<Health>();
        health.onDamage += OnDamage; //將自己的函數 OnDamage() 丟進 health 的事件委派容器 onDamage 中
        health.onDie += OnDie; //將自己的函數 OnDie() 丟進 health 的事件委派容器 onDie 中

    }




    void Update()
    {
        //--------------------------------------------
        print("血量:" + health.GetCurrentHealth());
        if(Input.GetKeyDown(KeyCode.S))
        {
            health.TakeDamage(30);
        }
        //---------------------------------------------*/



        if (health.IsDead()) return; //如果角色已經死了那就什麼都不做了

        if (InViewRange()) //如果在殭屍的視野範圍內
        {
            animatorController.SetBool("IsConfuse", false); //解除困惑動作                                   

            timeSinceLastSawPlayer = 0;
            zombieNavMeshAgent.MoveTo(player.transform.position, 1); //移動到玩家位置
            zombiePatrolPath.transform.position = this.transform.position; //巡邏點也會一直跟著殭屍移動，直到殭屍停下來才不會再一起動
        }
        else if (timeSinceLastSawPlayer < confuseTime)
        {
            ConfuseBehaviour(); //困惑行為
            IsPatrol = true; //困惑完後就可以巡邏了
        }
        else
        {
            PatrolBehaviour(); //巡邏行為
        }

    }


    private void PatrolBehaviour()
    {     
        if(IsPatrol || IsConfuseInPatrol) //巡邏中或困惑中
        {
            if(!IsArrivePatrolPoint() && !IsConfuseInPatrol) //如果還沒到達巡邏點，並且不在困惑中
            {
                animatorController.SetBool("IsConfuse", false); //困惑動作                                    
                zombieNavMeshAgent.MoveTo(zombiePatrolPath.GetPatrolPointPosition(GoalPatrolPoint), 0.2f); //移動到目標巡邏點 

                //有時候會完全走不到下一個巡邏點，如果行走超過一段時間，則判定走不到，直接困惑一次並結束巡邏
                timeSinceLastStartPatrol += Time.deltaTime;
                if (timeSinceLastStartPatrol > 8f) //如果走了八秒還到不了
                {
                    IsPatrol = false; //直接結束巡邏
                    zombieNavMeshAgent.SetNavMeshAgentSpeed(0); //將控制動畫的變數 WalkSpeed 設為 0 才會播放 idle 動畫
                    timeSinceLastStartPatrol = 0;
                    GoalPatrolPoint = 0;
                }
            }
            else if(IsArrivePatrolPoint()) //剛到達目標巡邏點，開始困惑
            {
                zombieNavMeshAgent.CancelMove(); //停止移動 
                IsConfuseInPatrol = true;
                animatorController.SetBool("IsConfuse", true); //開始困惑動畫
                GoalPatrolPoint = zombiePatrolPath.GetNextPatrolPointNumber(GoalPatrolPoint); //將目標轉向下一個巡邏點         
                timeSinceLastArrivePatrolPoint = 0;  //重置開始困惑時間
                timeSinceLastStartPatrol = 0; //重置開始巡邏經過時間

                if (GoalPatrolPoint == 0) //如果所有點都巡邏完，索引值又回到 0 了
                {
                    IsPatrol = false; //巡邏完了
                    zombieNavMeshAgent.SetNavMeshAgentSpeed(0); //將控制動畫的變數 WalkSpeed 設為 0 才會播放 idle 動畫
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
        return (Vector3.Distance(this.transform.position, zombiePatrolPath.GetPatrolPointPosition(GoalPatrolPoint)) < zombiePatrolPath.CircleRadius); //當前位置與目標巡邏點的距離是否小於巡邏點半徑了
    }


    private void ConfuseBehaviour()
    {
        zombieNavMeshAgent.CancelMove(); //停止移動 
                                         //animatorController.SetBool("IsAttack", false); //停止攻擊
        animatorController.SetBool("IsConfuse", true); //困惑動作                                    
        timeSinceLastSawPlayer += Time.deltaTime; //開始累計困惑時間
    }

    private bool InViewRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) < viewDistance;  
    }





    void OnDamage()
    {
        //受到攻擊時的動畫、叫聲等等

    }

    void OnDie()
    {
        //死亡叫一下
        zombieNavMeshAgent.CancelMove(); //停止移動 
        animatorController.SetTrigger("IsDead"); //播放死亡動畫

    }










}
