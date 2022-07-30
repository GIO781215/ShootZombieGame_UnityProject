using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWall_1 : MonoBehaviour //�Ů���_1 ���}��
{

    [SerializeField] GameObject Zombie; //�L�͹w�s����
    [SerializeField] Transform[] spawnPoint; //�n�ͦ�����m (���n�X�ӡA����b�}�C��)

    float spawnTime = 3f; //�C���L�ͥͦ������j�ɶ�
    int spawnAmoint = 10; //�L�ͭn�ͦ����`�ƶq

    bool hasBeenTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasBeenTrigger) return;

        if(other.gameObject.tag == "Player")
        {
            hasBeenTrigger = true;
            StartCoroutine(SpawnZombie());
        }
    }


    IEnumerator SpawnZombie()
    {
        for(int i = 0; i < spawnAmoint; i++)
        {
            int index = Random.Range(0, spawnPoint.Length); // Random.Range(int �̤p�ȡAint �̤j) : �H�����ͤ@�Ӿ�ơA�d��O �̤p�� ~ �̤j��(���]�t) �A Random.Rang(float �̤p�ȡAfloat �̤j) : �H�����ͤ@�ӯB�I�ơA�d��O �̤p�� ~ �̤j��(�]�t) 
            Instantiate(Zombie, spawnPoint[index].position, spawnPoint[index].rotation);
            yield return new WaitForSeconds(spawnTime);
        }
    }








}
