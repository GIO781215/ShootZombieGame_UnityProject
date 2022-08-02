using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PatrolPath : MonoBehaviour
{
    [SerializeField] public float CircleRadius = 0.5f;

    //�H�������I�n�Ψ쪺�ܼ�
    bool hasBeenPatrol_1 = false;
    bool hasBeenPatrol_2 = false;
    bool hasBeenPatrol_3 = false;
    bool hasBeenPatrol_4 = false;

    [HideInInspector] public bool PatrolOver = false; //�������޺X��



    //��o�U�@�Ө����I�s��
    public int GetNextPatrolPointNumber(int PatrolPointNumber) //�ѼƬ������I�s���A�q 0 �}�l
    {
        if(PatrolPointNumber + 1 > transform.childCount-1) //PatrolPointNumber �|���}�C���ޭȡA�ҥH�o�䵹�L�[ 1
        {
            //PatrolOver = true; //�Q�e�ϥ\��v�T��F (debug de�b�ѴN�O�A�`��.....
            return 0; //�W�L�l����ƶq�^�� 0 ���s�ư_
        }
        return PatrolPointNumber + 1;
    }


    public int GetNextRandomPatrolPointNumber() //�H���o��U�@�Ө����I 
    {

        if (hasBeenPatrol_1 == false && hasBeenPatrol_2 == false && hasBeenPatrol_3 == false) //�Ĥ@���i��
        {
            int i = Random.Range(0, 3);

            if (i == 0)
            {
                hasBeenPatrol_1 = true;
                return 0;
            }
            if (i == 1)
            {
                hasBeenPatrol_2 = true;
                return 1;
            }
            if (i == 2)
            {
                hasBeenPatrol_3 = true;
                return 2;
            }

           
        }


        //�@�}�l�O 0 �����p
        if (hasBeenPatrol_1 == true && hasBeenPatrol_2 == false && hasBeenPatrol_3 == false)
        {            hasBeenPatrol_2 = true;
            return 1;
        }
        if (hasBeenPatrol_1 == true && hasBeenPatrol_2 == true && hasBeenPatrol_3 == false)
        {
            hasBeenPatrol_3 = true;
            return 2;
        }


        //�@�}�l�O 1 �����p
        if (hasBeenPatrol_1 == false && hasBeenPatrol_2 == true && hasBeenPatrol_3 == false)
        {
            hasBeenPatrol_3 = true;
            return 2;
        }
        if (hasBeenPatrol_1 == false && hasBeenPatrol_2 == true && hasBeenPatrol_3 == true)//�@�}�l�O 2 �����p
        {
            hasBeenPatrol_1 = true;
            return 0;
        }


        //�@�}�l�O 2 �����p
        if (hasBeenPatrol_1 == false && hasBeenPatrol_2 == false && hasBeenPatrol_3 == true)
        {
            hasBeenPatrol_1 = true;
            return 0;
        }
        if (hasBeenPatrol_1 == true && hasBeenPatrol_2 == false && hasBeenPatrol_3 == true)//�@�}�l�O 2 �����p
        {
            hasBeenPatrol_2 = true;
            return 1;
        }


        //---------------------------------
        if (hasBeenPatrol_1 == true && hasBeenPatrol_2 == true && hasBeenPatrol_3 == true && hasBeenPatrol_4 == false)
        {
            hasBeenPatrol_4 = true;
            return 3;
        }

        if (hasBeenPatrol_1 == true && hasBeenPatrol_2 == true && hasBeenPatrol_3 == true && hasBeenPatrol_4 == true)
        {
            ResethasBeenPatrol();
        }
        return 0;

    }

    public void ResethasBeenPatrol()
    {
        hasBeenPatrol_1 = false;
        hasBeenPatrol_2 = false;
        hasBeenPatrol_3 = false;
        hasBeenPatrol_4 = false;
        PatrolOver = true;
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
