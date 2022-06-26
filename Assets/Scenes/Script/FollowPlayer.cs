using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [Header("�l�ܥؼ�")]
    [SerializeField] Transform player;
    [Header("�j�󦹶Z���~�|�l��")]
    [SerializeField] float distanceToTarget; //�j�󦹶Z������~�|�}�l�l�� Player
    [Header("����P�l�ܥؼЪ�����")]
    [SerializeField] float startHeight; //����� Player ������
    [Header("�l�ܮɶ�")]
    [SerializeField] float smoothTime; //��v�����ưl�ܪ��骺�ɶ�

    Vector3 smoothPosition = Vector3.zero; //�����e��m
    Vector3 currentVelocity = Vector3.zero;  //Vector3.SmoothDamp() �n�ϥΨ쪺�����ܶq�ܼ� (���Ȭ����骺��e�t��)

    void Start()
    {
        transform.position = player.position + Vector3.up * startHeight;
    }

    void LateUpdate()
    {
        if(CheckDistance())
        {
            #region //SmoothDamp() ���ƪ������ : �i����H���ʡA�i�H�ϸ��H�ݰ_�ӫܥ��ơA�Ӥ���o��a    
            /*
            static function SmoothDamp (current : Vector3, target : Vector3, ref currentVelocity : Vector3, smoothTime : float, maxSpeed : float = Mathf.Infinity, deltaTime : float = Time.deltaTime) : Vector3
            �ѼƧt�q�G
            1.current ��e�����m
            2.target �ؼЪ����m
            3.ref currentVelocity ��e�t�סA�o�ӭȥѧA�C���եγo�Ө�ƮɳQ�ק�]�]���ϥ� ref ����r�A�ҥH��Ʒ|�u������ currentVelocity �o���ܼơA�o�N��i�H�b����ɨ�o�쪫���e���t�� currentVelocity�^
              �`�N : �ܼ� currentVelocity �n�ϥΥ����ܶq�A�p�G�w�q������?�q���ʮĪG�|�X���D�A�Ѿ\�峹 <Unity��Lerp�PSmoothDamp��ƨϥλ~�ϲL�R> : https://www.jianshu.com/p/8a5341c6d5a6
            4.smoothTime ��F�ؼЪ��ɶ��A���p���ȱN�ֳt��F�ؼ�
            5.maxSpeed �Ҥ��\���̤j�t�סA�q�{�L�a�j
            6.deltaTime �ۤW���եγo�Ө�ƪ��ɶ��A�q�{��Time.deltaTime
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
