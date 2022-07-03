using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    GameObject player;
    ZombieNavMeshAgent zombieNavMeshAgent; //�Ψ���o�ۤv�g�����ʾɯ誫��
    Health health; //�Ψ���o�ۤv����q�t�Ϊ���

    float viewDistance = 10f; //�L�͵����d��
    float confuseTime = 3f; //���a�b�L�ͪ������d�򤺮������L�ͪ��x�b�ɶ�
    float timeSinceLastSawPlayer = Mathf.Infinity; //��l�ȳ]���L���j



    //-------------------�]�w�ʵe���Ѽ�-------------------
    Animator animatorController; //�ʵe���񱱨
    //----------------------------------------------------


    //-------------------���ެ������Ѽ�-------------------
    [SerializeField] public ZombiePatrolPath zombiePatrolPath; //�i�H�����q Unity ���ᵹ�L���� ZombiePatrolPath �}��������
    float timeSinceLastArrivePatrolPoint = 0; //���W����F�����I�g�L���ɶ� 
    float timeSinceLastStartPatrol = 0; //���W���}�l���޸g�L���ɶ� (�ѨM�û��F����U�@�Ө����I�����D)
    int GoalPatrolPoint = 0; //��e�ݭn��F�������I
    bool IsPatrol = false; //�O�_�i�J���ު��A
    bool IsConfuseInPatrol = false; //�O�_�b���ޤ����x�b���A
    //----------------------------------------------------







    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); //���b Unity ���N���ҳ]�m�� Player �� GameObject 
        zombieNavMeshAgent = GetComponent<ZombieNavMeshAgent>(); //��o�ۤv�g�����ʾɯ誫��
        animatorController = GetComponentInChildren<Animator>();

        health = GetComponent<Health>();
        health.onDamage += OnDamage; //�N�ۤv����� OnDamage() ��i health ���ƥ�e���e�� onDamage ��
        health.onDie += OnDie; //�N�ۤv����� OnDie() ��i health ���ƥ�e���e�� onDie ��

    }




    void Update()
    {
        //--------------------------------------------
        print("��q:" + health.GetCurrentHealth());
        if(Input.GetKeyDown(KeyCode.S))
        {
            health.TakeDamage(30);
        }
        //---------------------------------------------*/



        if (health.IsDead()) return; //�p�G����w�g���F���N���򳣤����F

        if (InViewRange()) //�p�G�b�L�ͪ������d��
        {
            animatorController.SetBool("IsConfuse", false); //�Ѱ��x�b�ʧ@                                   

            timeSinceLastSawPlayer = 0;
            zombieNavMeshAgent.MoveTo(player.transform.position, 1); //���ʨ쪱�a��m
            zombiePatrolPath.transform.position = this.transform.position; //�����I�]�|�@������L�Ͳ��ʡA�����L�Ͱ��U�Ӥ~���|�A�@�_��
        }
        else if (timeSinceLastSawPlayer < confuseTime)
        {
            ConfuseBehaviour(); //�x�b�欰
            IsPatrol = true; //�x�b����N�i�H���ޤF
        }
        else
        {
            PatrolBehaviour(); //���ަ欰
        }

    }


    private void PatrolBehaviour()
    {     
        if(IsPatrol || IsConfuseInPatrol) //���ޤ��Χx�b��
        {
            if(!IsArrivePatrolPoint() && !IsConfuseInPatrol) //�p�G�٨S��F�����I�A�åB���b�x�b��
            {
                animatorController.SetBool("IsConfuse", false); //�x�b�ʧ@                                    
                zombieNavMeshAgent.MoveTo(zombiePatrolPath.GetPatrolPointPosition(GoalPatrolPoint), 0.2f); //���ʨ�ؼШ����I 

                //���ɭԷ|����������U�@�Ө����I�A�p�G�樫�W�L�@�q�ɶ��A�h�P�w������A�����x�b�@���õ�������
                timeSinceLastStartPatrol += Time.deltaTime;
                if (timeSinceLastStartPatrol > 8f) //�p�G���F�K���٨줣�F
                {
                    IsPatrol = false; //������������
                    zombieNavMeshAgent.SetNavMeshAgentSpeed(0); //�N����ʵe���ܼ� WalkSpeed �]�� 0 �~�|���� idle �ʵe
                    timeSinceLastStartPatrol = 0;
                    GoalPatrolPoint = 0;
                }
            }
            else if(IsArrivePatrolPoint()) //���F�ؼШ����I�A�}�l�x�b
            {
                zombieNavMeshAgent.CancelMove(); //����� 
                IsConfuseInPatrol = true;
                animatorController.SetBool("IsConfuse", true); //�}�l�x�b�ʵe
                GoalPatrolPoint = zombiePatrolPath.GetNextPatrolPointNumber(GoalPatrolPoint); //�N�ؼ���V�U�@�Ө����I         
                timeSinceLastArrivePatrolPoint = 0;  //���m�}�l�x�b�ɶ�
                timeSinceLastStartPatrol = 0; //���m�}�l���޸g�L�ɶ�

                if (GoalPatrolPoint == 0) //�p�G�Ҧ��I�����ާ��A���ޭȤS�^�� 0 �F
                {
                    IsPatrol = false; //���ާ��F
                    zombieNavMeshAgent.SetNavMeshAgentSpeed(0); //�N����ʵe���ܼ� WalkSpeed �]�� 0 �~�|���� idle �ʵe
                }
            } //�x�b��
            else 
            {
                timeSinceLastArrivePatrolPoint = timeSinceLastArrivePatrolPoint + Time.deltaTime; //�ֿn�x�b�ɶ�
                if (timeSinceLastArrivePatrolPoint > 3f) //�p�G�x�b�ɶ��w�g�j��3��A�~�Ѱ��x�b�~����
                {
                    IsConfuseInPatrol = false;
                    animatorController.SetBool("IsConfuse", false); //�Ѱ��x�b�ʧ@
                }
            }
        }
        else 
        {
            //�i�J�����b�����A 
        }
    }

    private bool IsArrivePatrolPoint() //�O�_��F�����I�F
    {
        return (Vector3.Distance(this.transform.position, zombiePatrolPath.GetPatrolPointPosition(GoalPatrolPoint)) < zombiePatrolPath.CircleRadius); //��e��m�P�ؼШ����I���Z���O�_�p�����I�b�|�F
    }


    private void ConfuseBehaviour()
    {
        zombieNavMeshAgent.CancelMove(); //����� 
                                         //animatorController.SetBool("IsAttack", false); //�������
        animatorController.SetBool("IsConfuse", true); //�x�b�ʧ@                                    
        timeSinceLastSawPlayer += Time.deltaTime; //�}�l�֭p�x�b�ɶ�
    }

    private bool InViewRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) < viewDistance;  
    }





    void OnDamage()
    {
        //��������ɪ��ʵe�B�s�n����

    }

    void OnDie()
    {
        //���`�s�@�U
        zombieNavMeshAgent.CancelMove(); //����� 
        animatorController.SetTrigger("IsDead"); //���񦺤`�ʵe

    }










}
