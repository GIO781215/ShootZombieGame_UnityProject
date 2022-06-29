using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieNavMeshAgent : MonoBehaviour
{
    NavMeshAgent navMeshAgent; //�ɯ�N�z����A�ઽ���������X���ʸ��|�M�i�沾�� (UnityEngine.AI �̶W�n�Ϊ���k����)
    private float maxMovingSpeed = 6f; //�̤j���ʳt��


    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>(); //��o�����b������U�� NavMeshAgent �ե�
    }

    public void MoveTo(Vector3 goalPosition, float movingSpeedRatio) // goalPosition : �n���ʨ쪺�ؼЦ�m �A movingSpeedRatio : ���ʳt�׽վ�ȡA�b 0~1 �����A���̤j�t�ת����v
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = maxMovingSpeed * Mathf.Clamp01(movingSpeedRatio); //Clamp01 �|�N�Ѽƭȭ���b 0 �� 1 �����A�p�G�Ȭ��t�A�h��^ 0�A�p�G�Ȥj�� 1�A�h��^ 1
        navMeshAgent.transform.position = goalPosition; //�ϱ�������H movingSpeedRatio ���t�ײ��ʨ��m goalPosition
    }

    public void CancelMove()
    {
        navMeshAgent.isStopped = true;
    }
}
