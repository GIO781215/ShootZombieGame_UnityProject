using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PatrolPath : MonoBehaviour
{
    [SerializeField] public float CircleRadius = 0.5f;


    //��o�U�@�Ө����I�s��
    public int GetNextPatrolPointNumber(int PatrolPointNumber) //�ѼƬ������I�s���A�q 0 �}�l
    {
        if(PatrolPointNumber + 1 > transform.childCount-1) //PatrolPointNumber �|���}�C���ޭȡA�ҥH�o�䵹�L�[ 1
        {
            return 0; //�W�L�l����ƶq�^�� 0 ���s�ư_
        }
        return PatrolPointNumber + 1;
    }


    public int aaa()
    {
        return 10;
    }

    //��o�l����(�����I)����m
    public Vector3 GetPatrolPointPosition(int PatrolPointNumber) //�ѼƬ������I�s���A�q 0 �}�l
    {
        return transform.GetChild(PatrolPointNumber).position;
    }


    //��o�����I�`��
    public int GetPatrolPointNumber() 
    {
        return transform.childCount;
    }

    //�e�ϥ\�� Debug �� (�S���g�b void Start �̤]�|�Q�����A�ܯ��_@@ ........................................................???
    void OnDrawGizmos() 
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Gizmos.color = Color.blue;
            int j = GetNextPatrolPointNumber(i); 
            Gizmos.DrawLine(GetPatrolPointPosition(i), GetPatrolPointPosition(j));
            //Gizmos.DrawSphere(GetPatrolPointPosition(i), CircleRadius); //�e�y
            Handles.color = Color.red;
            Handles.DrawWireDisc(GetPatrolPointPosition(i), new Vector3(0, 1, 0), CircleRadius); //�e��A�Ѽ�: ��L���ߡB��L�k�u�B��L�b�| (�� using UnityEditor)
        }
    }



}
