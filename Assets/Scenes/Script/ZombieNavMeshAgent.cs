using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieNavMeshAgent : MonoBehaviour
{
    NavMeshAgent navMeshAgent; //導航代理物件，能直接幫角色算出移動路徑和進行移動 (UnityEngine.AI 裡超好用的方法物件)
    private float maxMovingSpeed = 8f; //最大移動速度

    //-------------------設定動畫的參數-------------------
    Animator animatorController; //動畫播放控制器
    float MovingSpeed = 0; //當前正要的移動速度
    float GoalSpeed = 0; //目標速度
    float SpeedChangeRatio = 0.01f; //從當前速度變化到目標速度的快慢比率
    //----------------------------------------------------



    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>(); //獲得掛載在此物件下的 NavMeshAgent 組件
        animatorController = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        navMeshAgent.speed = 0;
    }

    
    public void SetNavMeshAgentSpeed(int Speed) //直接對 SetNavMeshAgentSpeed 設定速度，因為其會直接影響動畫，所以把方法開出來
    {
        navMeshAgent.speed = Speed;
    }

    private void Update()
    {
        UpdateAnimation(); //將 navMeshAgent.speed 的速度值反映到控制動畫的 WalkSpeed 上 (WalkSpeed 的值確實應該在 NavMeshAgent 這裡來獲得比較適合，而不是在 ZombieController 中設定)
    }

    private void UpdateAnimation()
    {
        GoalSpeed = navMeshAgent.speed / maxMovingSpeed; //反算回 maxMovingSpeed 作為 GoalSpeed
        /*
         老師這邊是用 (this.transform.InverseTransformDirection(navmeshAgent.velocity)).z 來作為 GoalSpeed，但我認為不用這麼麻煩
         GameObject.transform.InverseTransformDirection(Vector3 direction) 能把向量 direction 從世界座標系轉換到物件的 local 座標系上
         GameObject.transform.TransformDirection(Vector3 direction) 能把向量 direction 從物件的 local 座標系轉換到世界座標系上
        */
        MovingSpeed = Mathf.Lerp(MovingSpeed, GoalSpeed, SpeedChangeRatio); //這一幀與下一幀的移動速度做差值，讓 WalkSpeed 的值變動得更平滑       
        animatorController.SetFloat("WalkSpeed", MovingSpeed);
    }

    public void MoveTo(Vector3 goalPosition, float movingSpeedRatio) // goalPosition : 要移動到的目標位置 ， movingSpeedRatio : 移動速度調整值，在 0~1 之間，為最大速度的倍率
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = maxMovingSpeed * Mathf.Clamp01(movingSpeedRatio); //Clamp01 會將參數值限制在 0 到 1 之間，如果值為負，則返回 0，如果值大於 1，則返回 1
        navMeshAgent.destination = goalPosition; //使掛載物件以 navMeshAgent.speed 的速度移動到目標位置 goalPosition
    }

    public void CancelMove()
    {
        navMeshAgent.isStopped = true;
    }





}
