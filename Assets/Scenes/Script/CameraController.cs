using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    InputController inputController;
    float CameraAngle_X = 0;
    float CameraAngle_Y = 20;

    [Header("相機跟隨的目標")]
    [SerializeField] private Transform target; //private 可省略，預設就是 private

    [Header("相機左右移動的靈敏度")]
    [SerializeField] float sensitivity_X = 2;
    [Header("相機上下移動的靈敏度")]
    [SerializeField] float sensitivity_Y = 2;
    [Header("滾輪靈敏度")]
    [SerializeField] float sensitivity_ScrollWheel = 5;

    /*
    [Header("相機上下移動的最小角度")]
    [SerializeField] float minVerticalAngle = -10;
    [Header("相機上下移動的最大角度")]
    [SerializeField] float maxVerticalAngle = 85;

    [Header("相機高度調整")]
    [SerializeField] float HeightOffset = 2;
    [Header("相機與目標的距離")]
    [SerializeField] float cameraToTargetDistance = 10;
    [Header("相機與目標的最小距離")]
    [SerializeField] float cameraToTargetMinDistance = 2;
    [Header("相機與目標的最大距離")]
    [SerializeField] float cameraToTargetMaxDistance = 25;
    */

    float minVerticalAngle = -10;
    float maxVerticalAngle = 70;

    float HeightOffset = 2;
    float cameraToTargetDistance = 8;
    float cameraToTargetMinDistance = 2;
    float cameraToTargetMaxDistance = 15;

    void Start()
    {
        inputController = GameManager.Instance.inputController;
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
            transform.position = Vector3.up * HeightOffset + target.position + Quaternion.Euler(CameraAngle_Y, CameraAngle_X, 0) * new Vector3(0, 0, -cameraToTargetDistance); // Quaternion.Euler 可以乘 Vector3 ，結果為往那個方向的向量長度!!! 
        }



    }
}
