using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieNavMeshAgent : MonoBehaviour
{
    NavMeshAgent navMeshAgent; //導航代理物件，能直接幫角色算出移動路徑和進行移動 (UnityEngine.AI 裡超好用的方法物件)
    private float maxMovingSpeed = 6f; //最大移動速度


    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>(); //獲得掛載在此物件下的 NavMeshAgent 組件
    }

    public void MoveTo(Vector3 goalPosition, float movingSpeedRatio) // goalPosition : 要移動到的目標位置 ， movingSpeedRatio : 移動速度調整值，在 0~1 之間，為最大速度的倍率
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = maxMovingSpeed * Mathf.Clamp01(movingSpeedRatio); //Clamp01 會將參數值限制在 0 到 1 之間，如果值為負，則返回 0，如果值大於 1，則返回 1
        navMeshAgent.transform.position = goalPosition; //使掛載物件以 movingSpeedRatio 的速度移動到位置 goalPosition
    }

    public void CancelMove()
    {
        navMeshAgent.isStopped = true;
    }
}
