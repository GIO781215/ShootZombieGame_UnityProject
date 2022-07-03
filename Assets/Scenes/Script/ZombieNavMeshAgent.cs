using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieNavMeshAgent : MonoBehaviour
{
    NavMeshAgent navMeshAgent; //�ɯ�N�z����A�ઽ���������X���ʸ��|�M�i�沾�� (UnityEngine.AI �̶W�n�Ϊ���k����)
    private float maxMovingSpeed = 8f; //�̤j���ʳt��

    //-------------------�]�w�ʵe���Ѽ�-------------------
    Animator animatorController; //�ʵe���񱱨
    float MovingSpeed = 0; //��e���n�����ʳt��
    float GoalSpeed = 0; //�ؼгt��
    float SpeedChangeRatio = 0.01f; //�q��e�t���ܤƨ�ؼгt�ת��ֺC��v
    //----------------------------------------------------



    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>(); //��o�����b������U�� NavMeshAgent �ե�
        animatorController = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        navMeshAgent.speed = 0;
    }

    
    public void SetNavMeshAgentSpeed(int Speed) //������ SetNavMeshAgentSpeed �]�w�t�סA�]����|�����v�T�ʵe�A�ҥH���k�}�X��
    {
        navMeshAgent.speed = Speed;
    }

    private void Update()
    {
        UpdateAnimation(); //�N navMeshAgent.speed ���t�׭ȤϬM�챱��ʵe�� WalkSpeed �W (WalkSpeed ���ȽT�����Ӧb NavMeshAgent �o�̨���o����A�X�A�Ӥ��O�b ZombieController ���]�w)
    }

    private void UpdateAnimation()
    {
        GoalSpeed = navMeshAgent.speed / maxMovingSpeed; //�Ϻ�^ maxMovingSpeed �@�� GoalSpeed
        /*
         �Ѯv�o��O�� (this.transform.InverseTransformDirection(navmeshAgent.velocity)).z �ӧ@�� GoalSpeed�A���ڻ{�����γo��·�
         GameObject.transform.InverseTransformDirection(Vector3 direction) ���V�q direction �q�@�ɮy�Шt�ഫ�쪫�� local �y�Шt�W
         GameObject.transform.TransformDirection(Vector3 direction) ���V�q direction �q���� local �y�Шt�ഫ��@�ɮy�Шt�W
        */
        MovingSpeed = Mathf.Lerp(MovingSpeed, GoalSpeed, SpeedChangeRatio); //�o�@�V�P�U�@�V�����ʳt�װ��t�ȡA�� WalkSpeed �����ܰʱo�󥭷�       
        animatorController.SetFloat("WalkSpeed", MovingSpeed);
    }

    public void MoveTo(Vector3 goalPosition, float movingSpeedRatio) // goalPosition : �n���ʨ쪺�ؼЦ�m �A movingSpeedRatio : ���ʳt�׽վ�ȡA�b 0~1 �����A���̤j�t�ת����v
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = maxMovingSpeed * Mathf.Clamp01(movingSpeedRatio); //Clamp01 �|�N�Ѽƭȭ���b 0 �� 1 �����A�p�G�Ȭ��t�A�h��^ 0�A�p�G�Ȥj�� 1�A�h��^ 1
        navMeshAgent.destination = goalPosition; //�ϱ�������H navMeshAgent.speed ���t�ײ��ʨ�ؼЦ�m goalPosition
    }

    public void CancelMove()
    {
        navMeshAgent.isStopped = true;
    }





}
