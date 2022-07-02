using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    GameObject player;
    ZombieNavMeshAgent zombieNavMeshAgent; //�Ψ���o�ۤv�g�����ʾɯ誫��

    float viewDistance = 10f; //�L�͵����d��
    float confuseTime = 3f; //���a�b�L�ͪ������d�򤺮������L�ͪ��x�b�ɶ�
    float timeSinceLastSawPlayer = Mathf.Infinity; //��l�ȳ]���L���j
    private Vector3 beginPosition; //�L�Ͱ_�l��m

    //-------------------�]�w�ʵe���Ѽ�-------------------
    Animator animatorController; //�ʵe���񱱨
    //----------------------------------------------------




    void Start()
    {
        beginPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player"); //���b Unity ���N���ҳ]�m�� Player �� GameObject 
        zombieNavMeshAgent = GetComponent<ZombieNavMeshAgent>(); //��o�ۤv�g�����ʾɯ誫��
        animatorController = GetComponent<Animator>();
    }




    void Update()
    {

        if (InViewRange()) //�p�G�b�L�ͪ������d��
        {
            animatorController.SetBool("IsConfuse", false); //�Ѱ��x�b�ʧ@                                   

            timeSinceLastSawPlayer = 0;
            zombieNavMeshAgent.MoveTo(player.transform.position, 1); //���ʨ쪱�a��m
        }
        else if (timeSinceLastSawPlayer < confuseTime)
        {
            zombieNavMeshAgent.CancelMove(); //�����

            //�������
            animatorController.SetBool("IsConfuse", true); //�x�b�ʧ@                                   

            timeSinceLastSawPlayer += Time.deltaTime; //�}�l�֭p�x�b�ɶ�
        }
        else
        {
            animatorController.SetBool("IsConfuse", false); //�Ѱ��x�b�ʧ@
                                                         


            // ���ӬO�n��a���޻P�o�b
            zombieNavMeshAgent.MoveTo(beginPosition, 0.2f); //�^����I�Ψ����I  

            /*
            if( �u���^����I��N���n�ʤF )
            {
               �� idle �ʧ@
            }
            */
        }

    }

    private bool InViewRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) < viewDistance;  
    }
}
