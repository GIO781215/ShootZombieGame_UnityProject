using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    InputController inputController;

    [Header("相機跟隨的目標")]
    [SerializeField] private Transform target; //private 可省略，預設就是 private
    [SerializeField] GameObject player; //做受傷特效用，要得到玩家的血量系統
    [SerializeField] ParticleSystem behitEffect; //受傷時的特效
    [SerializeField] ParticleSystem rushEffect; //衝刺時的特效



    [Header("攝影機的高度")]
    [SerializeField] float HeightOffset = 3; //攝影機的高度
    float CameraAngle_X = 0; //攝影機左右起始角度
    float CameraAngle_Y = 15; //攝影機上下起始角度
    float sensitivity_X = 2; //滑鼠控制攝影機左右移動的靈敏度
    float sensitivity_Y = 2; //滑鼠控制攝影機上下移動的靈敏度
    float sensitivity_ScrollWheel = 5; //滑鼠控制攝影機前後移動的靈敏度
    float minVerticalAngle = -10; //攝影機上下移動的最小角度 (其實是往上仰視的最大角度，因為在 Unity 裡的 Edir -> Project Setting -> Input Manager -> Mouse Y 有勾選 Invert，所以上下有相反)
    float maxVerticalAngle = 20; //攝影機上下移動的最大角度 (其實是往下俯視的最大角度，因為在 Unity 裡的 Edir -> Project Setting -> Input Manager -> Mouse Y 有勾選 Invert，所以上下有相反)
    float cameraToTargetDistance = 10; //攝影機與目標的起始距離
    float cameraToTargetMinDistance = 5; //攝影機與目標的最小距離
    float cameraToTargetMaxDistance = 15; //攝影機與目標的最大距離

    //-----------------讓攝影機平滑追蹤物體需要的變數//-----------------
    [Header("攝影機平滑追蹤物體的時間")]
    [SerializeField] float smoothMoveTime = 0.2f; //攝影機平滑追蹤物體的時間
    Vector3 referenceObjectPosition = Vector3.zero; //虛擬參考物的座標位置
    Vector3 currentVelocity = Vector3.zero; //Vector3.SmoothDamp() 要使用到的全局變量變數 (此值為虛擬參考物的當前速度)
    Vector3 referenceObjectToCameraOffset = Vector3.zero; //虛擬參考物距離攝影機的距離


    void Start()
    {
        inputController = GameManager.Instance.inputController;
        player.GetComponent<Health>().onDamage += OnDamage;
        player.GetComponent<PlayerController>().onSprint += OnSprint;

    }

    private void LateUpdate() //在 Update 後執行
    {
        if(Cursor.lockState == CursorLockMode.Locked)
        {
            //設置攝影機角度
            CameraAngle_X += inputController.GetMouseX() * sensitivity_X;
            CameraAngle_Y += inputController.GetMouseY() * sensitivity_Y;
            CameraAngle_Y = Mathf.Clamp(CameraAngle_Y, minVerticalAngle, maxVerticalAngle); //限制 CameraAngle_Y 的最大角度與最小角度
            transform.rotation = Quaternion.Euler(CameraAngle_Y, CameraAngle_X, 0);


            //設置攝影機位置
            cameraToTargetDistance += inputController.GetMouseScrollWheel() * sensitivity_ScrollWheel;
            cameraToTargetDistance = Mathf.Clamp(cameraToTargetDistance, cameraToTargetMinDistance, cameraToTargetMaxDistance);


            //追蹤方式為 攝影機追虛擬參考物(無平滑效果) 虛擬參考物追 Player (有平滑效果)
            //得到虛擬參考物的位置，因為最後有對 transform.position 加上 referenceObjectToCameraOffset，所以這邊要減回來才是 虛擬參考物應該在的位置
            referenceObjectPosition = Vector3.SmoothDamp(transform.position - referenceObjectToCameraOffset, target.position, ref currentVelocity, smoothMoveTime);
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

            referenceObjectPosition.y = HeightOffset; //指定虛擬參考物的高度為 HeightOffset (不受 Player 跳起來而改變)
            referenceObjectToCameraOffset = Quaternion.Euler(CameraAngle_Y, CameraAngle_X, 0) * new Vector3(0, 0, -cameraToTargetDistance); // Quaternion.Euler 可以乘 Vector3 ，結果為往那個方向的向量長度!!! 
            transform.position = referenceObjectPosition + referenceObjectToCameraOffset; //從虛擬參考物的位置(受 SmoothDamp() 效果影響) 在加上 referenceObjectToCameraOffset 即為攝影機該在的位置
            
        }
    }

    private void OnDamage()
    {
        if(behitEffect != null)
        {
            behitEffect.Play();
        }
    }

    private void OnSprint()
    {
        if (rushEffect != null)
        {
            //先把跑步特效關掉好了
            rushEffect.Play();
        }
    }






}
