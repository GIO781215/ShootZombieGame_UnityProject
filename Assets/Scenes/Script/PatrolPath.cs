using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PatrolPath : MonoBehaviour
{
    [SerializeField] public float CircleRadius = 0.5f;

    //隨機巡邏點要用到的變數
    bool hasBeenPatrol_1 = false;
    bool hasBeenPatrol_2 = false;
    bool hasBeenPatrol_3 = false;
    bool hasBeenPatrol_4 = false;
    public bool PatrolOver = false; //結束巡邏旗標




    //獲得下一個巡邏點編號
    public int GetNextPatrolPointNumber(int PatrolPointNumber) //參數為巡邏點編號，從 0 開始
    {
        if(PatrolPointNumber + 1 > transform.childCount-1) //PatrolPointNumber 會給陣列索引值，所以這邊給他加 1
        {
            PatrolOver = true;
            return 0; //超過子物件數量回傳 0 重新數起
        }
        return PatrolPointNumber + 1;
    }

    /*
    public int GetRandomPatrolPointNumber(int PatrolPointNumber)                 //這邊做一個巡邏點為隨機的處理

    {
        if (PatrolPointNumber + 1 > transform.childCount - 1) //PatrolPointNumber 會給陣列索引值，所以這邊給他加 1
        {
            confuseDelayTime += UnityEngine.Random.Range(0, 1f);





            return 0; //超過子物件數量回傳 0 重新數起
        }
        return PatrolPointNumber + 1;
    }

    */





    public int aaa()
    {
        return 10;
    }

    //獲得子物件(巡邏點)的位置
    public Vector3 GetPatrolPointPosition(int PatrolPointNumber) //參數為巡邏點編號，從 0 開始
    {
        return transform.GetChild(PatrolPointNumber).position;
    }


    //獲得巡邏點總數
    public int GetPatrolPointNumber() 
    {
        return transform.childCount;
    }

    //畫圖功能 Debug 用 (沒有寫在 void Start 裡也會被執行到，很神奇@@ ........................................................???
    void OnDrawGizmos() 
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Gizmos.color = Color.blue;
            int j = GetNextPatrolPointNumber(i); 
            Gizmos.DrawLine(GetPatrolPointPosition(i), GetPatrolPointPosition(j));
            //Gizmos.DrawSphere(GetPatrolPointPosition(i), CircleRadius); //畫球
            Handles.color = Color.red;
            Handles.DrawWireDisc(GetPatrolPointPosition(i), new Vector3(0, 1, 0), CircleRadius); //畫圓，參數: 圓盤中心、圓盤法線、圓盤半徑 (須 using UnityEditor)
        }
    }



}
