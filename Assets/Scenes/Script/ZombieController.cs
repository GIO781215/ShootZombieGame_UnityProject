using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    GameObject player;

    float viewDistance = 10f; //殭屍視野範圍
    float confuseTime = 5f; //當玩家在殭屍的視野範圍內消失後殭屍的困惑時間
    float timeSinceLastSawPlayer = Mathf.Infinity; //初始值設為無限大
    private Vector3 beginPosition; //殭屍起始位置


    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player"); //找到在 Unity 中將標籤設置為 Player 的 GameObject
        beginPosition = transform.position;
    }


    void Update()
    {
        if (InViewRange()) //如果在殭屍的視野範圍內
        {
            timeSinceLastSawPlayer = 0;
            //移動到玩家位置
        }
        else if (timeSinceLastSawPlayer < confuseTime)
        {
            //停止移動和攻擊
            //困惑動作

            timeSinceLastSawPlayer += Time.deltaTime; //開始累計困惑時間
        }
        else
        {
            //回到巡邏點
        }
    }

    private bool InViewRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) < viewDistance;
    }
}
