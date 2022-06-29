using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    GameObject player;
    ZombieNavMeshAgent zombieNavMeshAgent; //用來獲得自己寫的移動導航物件

    float viewDistance = 10f; //殭屍視野範圍
    float confuseTime = 5f; //當玩家在殭屍的視野範圍內消失後殭屍的困惑時間
    float timeSinceLastSawPlayer = Mathf.Infinity; //初始值設為無限大
    private Vector3 beginPosition; //殭屍起始位置



    void Start()
    {
        beginPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player"); //找到在 Unity 中將標籤設置為 Player 的 GameObject 
        zombieNavMeshAgent = GetComponent<ZombieNavMeshAgent>(); //獲得自己寫的移動導航物件
    }




    void Update()
    {

        if (InViewRange()) //如果在殭屍的視野範圍內
        {
            timeSinceLastSawPlayer = 0;
            zombieNavMeshAgent.MoveTo(player.transform.position, 1); //移動到玩家位置
        }
        else if (timeSinceLastSawPlayer < confuseTime)
        {
            zombieNavMeshAgent.CancelMove(); //停止移動

            //停止攻擊
            //困惑動作                                   

            timeSinceLastSawPlayer += Time.deltaTime; //開始累計困惑時間
        }
        else
        {
            zombieNavMeshAgent.MoveTo(beginPosition, 0.5f); //回到原點或巡邏點
        }

    }

    private bool InViewRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) < viewDistance;  
    }
}
