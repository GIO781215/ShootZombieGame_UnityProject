using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*//----------------------------
 
�ѩ��]�� Mutant �� Animator �� Apply Root Motion ���Ŀ�A�ҥH�򱱨��L�Ͱʵe�����ʤ覡���P

���O�����ѵ{�����w��m�A�ӬO�|�B�Ψ�ʵe�������첾�Ӱ����ʱ���

�� : �� navMeshAgent.speed �]�Ȧn���N�|���h�ĥΡA�|�̷Ӱʵe���������ʳt�רӮM�Ψ��⪺���ʳt��

*///----------------------------



public class MutantNavMeshAgent : MonoBehaviour
{
    NavMeshAgent navMeshAgent; //�ɯ�N�z����A�ઽ���������X���ʸ��|�M�i�沾�� (UnityEngine.AI �̶W�n�Ϊ���k����)
    Animator animatorController; //�ʵe���񱱨

    //private float movingSpeed = 8f; //���ʳt��
    float rotateSpeed = 3f; //����t��



    //-------------------�]�w�ʵe���Ѽ�-------------------
    //float MovingSpeed = 0; //��e���n�����ʳt��
    //float GoalSpeed = 0; //�ؼгt��
    //float SpeedChangeRatio = 0.01f; //�q��e�t���ܤƨ�ؼгt�ת��ֺC��v
    //----------------------------------------------------







    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>(); //��o�����b������U�� NavMeshAgent �ե�
        //animatorController = GetComponentInChildren<Animator>();
        animatorController = GetComponent<Animator>();

    }

    private void Start()
    {
        navMeshAgent.updateRotation = false; //���� navMeshAgent ������ɯ�\��A�������� Update() �����{���ӱ���
        //navMeshAgent.speed = movingSpeed;
    }

    private void Update()
    {
        if(navMeshAgent != null && !navMeshAgent.isStopped)
        {
            //�����⪺�¦V����¦첾��V����
            Vector3 targetPosition = navMeshAgent.steeringTarget; //�o�� navMeshAgent ��X�����U�ӭn���ʨ쪺��m
            Vector3 movingVector = targetPosition - transform.position; //�o�챵�U�Ӫ��첾��V�V�q
            if (movingVector != Vector3.zero)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movingVector, Vector3.up), rotateSpeed * Time.deltaTime);
            }
        }

    }



    /* ��ӳo�Ӥ��ݭn
    private void OnAnimatorMove() //��ʵe�������Ⲿ�ʮɴN�|Ĳ�o�o�Ө��!
    {
        navMeshAgent.velocity = animatorController.deltaPosition / Time.deltaTime; // �N navMeshAgent �����ʳt�׳]���P�ʵe�����⪺���ʳt�׬ۦP (animatorController.deltaPosition ���ʵe����Ӽv�涡���Ⲿ�ʪ��첾�q)
    }
    */




    public void MoveTo(Vector3 goalPosition) // goalPosition : �n���ʨ쪺�ؼЦ�m 
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.destination = goalPosition; //�ϱ�������H navMeshAgent.speed ���t�ײ��ʨ�ؼЦ�m goalPosition
        animatorController.SetBool("Move", true);
    }

    public void CancelMove()
    {
        navMeshAgent.isStopped = true;
        animatorController.SetBool("Move", false);
    }

 










}
