using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    GameObject player;

    float viewDistance = 10f; //�L�͵����d��
    float confuseTime = 5f; //���a�b�L�ͪ������d�򤺮������L�ͪ��x�b�ɶ�
    float timeSinceLastSawPlayer = Mathf.Infinity; //��l�ȳ]���L���j
    private Vector3 beginPosition; //�L�Ͱ_�l��m


    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player"); //���b Unity ���N���ҳ]�m�� Player �� GameObject
        beginPosition = transform.position;
    }


    void Update()
    {
        if (InViewRange()) //�p�G�b�L�ͪ������d��
        {
            timeSinceLastSawPlayer = 0;
            //���ʨ쪱�a��m
        }
        else if (timeSinceLastSawPlayer < confuseTime)
        {
            //����ʩM����
            //�x�b�ʧ@

            timeSinceLastSawPlayer += Time.deltaTime; //�}�l�֭p�x�b�ɶ�
        }
        else
        {
            //�^�쨵���I
        }
    }

    private bool InViewRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) < viewDistance;
    }
}
