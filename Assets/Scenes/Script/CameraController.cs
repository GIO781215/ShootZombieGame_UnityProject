using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    InputController inputController;
    float CameraAngle_X = 0;
    float CameraAngle_Y = 20;

    [Header("�۾����H���ؼ�")]
    [SerializeField] private Transform target; //private �i�ٲ��A�w�]�N�O private

    [Header("�۾����k���ʪ��F�ӫ�")]
    [SerializeField] float sensitivity_X = 2;
    [Header("�۾��W�U���ʪ��F�ӫ�")]
    [SerializeField] float sensitivity_Y = 2;
    [Header("�u���F�ӫ�")]
    [SerializeField] float sensitivity_ScrollWheel = 5;

    /*
    [Header("�۾��W�U���ʪ��̤p����")]
    [SerializeField] float minVerticalAngle = -10;
    [Header("�۾��W�U���ʪ��̤j����")]
    [SerializeField] float maxVerticalAngle = 85;

    [Header("�۾����׽վ�")]
    [SerializeField] float HeightOffset = 2;
    [Header("�۾��P�ؼЪ��Z��")]
    [SerializeField] float cameraToTargetDistance = 10;
    [Header("�۾��P�ؼЪ��̤p�Z��")]
    [SerializeField] float cameraToTargetMinDistance = 2;
    [Header("�۾��P�ؼЪ��̤j�Z��")]
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

    private void LateUpdate() //�b Update �����
    {
        if(Cursor.lockState == CursorLockMode.Locked)
        {
            //�]�m��v������
            CameraAngle_X += inputController.GetMouseX() * sensitivity_X;
            CameraAngle_Y += inputController.GetMouseY() * sensitivity_Y;
            CameraAngle_Y = Mathf.Clamp(CameraAngle_Y, minVerticalAngle, maxVerticalAngle); //���� CameraAngle_Y ���̤j���׻P�̤p����
            transform.rotation = Quaternion.Euler(CameraAngle_Y, CameraAngle_X, 0);

            //�]�m��v����m
            cameraToTargetDistance += inputController.GetMouseScrollWheel() * sensitivity_ScrollWheel;
            cameraToTargetDistance = Mathf.Clamp(cameraToTargetDistance, cameraToTargetMinDistance, cameraToTargetMaxDistance);
            transform.position = Vector3.up * HeightOffset + target.position + Quaternion.Euler(CameraAngle_Y, CameraAngle_X, 0) * new Vector3(0, 0, -cameraToTargetDistance); // Quaternion.Euler �i�H�� Vector3 �A���G�������Ӥ�V���V�q����!!! 
        }



    }
}
