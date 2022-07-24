using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*//----------------------------
 
由於魔王 Mutant 的 Animator 的 Apply Root Motion 有勾選，所以跟控制殭屍動畫的移動方式不同

不是全部由程式指定位置，而是會運用到動畫本身的位移來做移動控制

註 : 給 navMeshAgent.speed 設值好像就會失去效用，會依照動畫本身的移動速度來套用角色的移動速度

*///----------------------------



public class MutantNavMeshAgent : MonoBehaviour
{
    NavMeshAgent navMeshAgent; //導航代理物件，能直接幫角色算出移動路徑和進行移動 (UnityEngine.AI 裡超好用的方法物件)
    Animator animatorController; //動畫播放控制器

    //private float movingSpeed = 8f; //移動速度
    float rotateSpeed = 3f; //旋轉速度



    //-------------------設定動畫的參數-------------------
    //float MovingSpeed = 0; //當前正要的移動速度
    //float GoalSpeed = 0; //目標速度
    //float SpeedChangeRatio = 0.01f; //從當前速度變化到目標速度的快慢比率
    //----------------------------------------------------







    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>(); //獲得掛載在此物件下的 NavMeshAgent 組件
        //animatorController = GetComponentInChildren<Animator>();
        animatorController = GetComponent<Animator>();

    }

    private void Start()
    {
        navMeshAgent.updateRotation = false; //關掉 navMeshAgent 的旋轉導航功能，讓旋轉改由 Update() 中的程式來控制
        //navMeshAgent.speed = movingSpeed;
    }

    private void Update()
    {
        if(navMeshAgent != null && !navMeshAgent.isStopped)
        {
            //讓角色的朝向能夠朝位移方向旋轉
            Vector3 targetPosition = navMeshAgent.steeringTarget; //得到 navMeshAgent 算出的接下來要移動到的位置
            Vector3 movingVector = targetPosition - transform.position; //得到接下來的位移方向向量
            if (movingVector != Vector3.zero)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movingVector, Vector3.up), rotateSpeed * Time.deltaTime);
            }
        }

    }



    /* 原來這個不需要
    private void OnAnimatorMove() //當動畫中的角色移動時就會觸發這個函數!
    {
        navMeshAgent.velocity = animatorController.deltaPosition / Time.deltaTime; // 將 navMeshAgent 的移動速度設為與動畫中角色的移動速度相同 (animatorController.deltaPosition 為動畫中兩個影格間角色移動的位移量)
    }
    */




    public void MoveTo(Vector3 goalPosition) // goalPosition : 要移動到的目標位置 
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.destination = goalPosition; //使掛載物件以 navMeshAgent.speed 的速度移動到目標位置 goalPosition
        animatorController.SetBool("Move", true);
    }

    public void CancelMove()
    {
        navMeshAgent.isStopped = true;
        animatorController.SetBool("Move", false);
    }

 










}
