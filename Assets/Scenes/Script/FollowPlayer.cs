using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [Header("追蹤目標")]
    [SerializeField] Transform player;
    [Header("大於此距離才會追蹤")]
    [SerializeField] float distanceToTarget; //大於此距離物體才會開始追中 Player
    [Header("方塊與追蹤目標的高度")]
    [SerializeField] float startHeight; //方塊離 Player 的高度
    [Header("追蹤時間")]
    [SerializeField] float smoothTime; //攝影機平滑追蹤物體的時間

    Vector3 smoothPosition = Vector3.zero; //物體當前位置
    Vector3 currentVelocity = Vector3.zero;  //Vector3.SmoothDamp() 要使用到的全局變量變數 (此值為物體的當前速度)

    void Start()
    {
        transform.position = player.position + Vector3.up * startHeight;
    }

    void LateUpdate()
    {
        if(CheckDistance())
        {
            #region //SmoothDamp() 平滑阻尼函數 : 進行跟隨移動，可以使跟隨看起來很平滑，而不顯得突兀    
            /*
            static function SmoothDamp (current : Vector3, target : Vector3, ref currentVelocity : Vector3, smoothTime : float, maxSpeed : float = Mathf.Infinity, deltaTime : float = Time.deltaTime) : Vector3
            參數含義：
            1.current 當前物體位置
            2.target 目標物體位置
            3.ref currentVelocity 當前速度，這個值由你每次調用這個函數時被修改（因為使用 ref 關鍵字，所以函數會真的改變 currentVelocity 這個變數，這代表可以在讓何時刻得到物體當前的速度 currentVelocity）
              注意 : 變數 currentVelocity 要使用全局變量，如果定義為局部?量移動效果會出問題，參閱文章 <Unity中Lerp與SmoothDamp函數使用誤區淺析> : https://www.jianshu.com/p/8a5341c6d5a6
            4.smoothTime 到達目標的時間，較小的值將快速到達目標
            5.maxSpeed 所允許的最大速度，默認無窮大
            6.deltaTime 自上次調用這個函數的時間，默認為Time.deltaTime
            */
            #endregion
            smoothPosition = Vector3.SmoothDamp(transform.position, player.position + Vector3.up* startHeight, ref currentVelocity, smoothTime);
            transform.position = smoothPosition;
        }

    }

    private bool CheckDistance()
    {
        return Vector3.Distance(transform.position, player.position) > distanceToTarget;
    }
}
