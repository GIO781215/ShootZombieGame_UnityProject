using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWall_1 : MonoBehaviour //�Ů���_1 ���}��
{

    [SerializeField] GameObject Zombie; //�L�͹w�s����
    [SerializeField] PatrolPath ZombiePatrolPath; //�L�ͪ����޸��|�w�s����
    [SerializeField] Transform[] spawnPoint; //�n�ͦ�����m (���n�X�ӡA����b�}�C��)

    float spawnTime = 1f; //�C���L�ͥͦ������j�ɶ�
    int spawnAmoint = 20; //�L�ͭn�ͦ����`�ƶq

    bool hasBeenTrigger = false;



    private void OnTriggerEnter(Collider other)
    {
        if (hasBeenTrigger) return;

        if (other.gameObject.tag == "Player")
        {
            hasBeenTrigger = true;
            SpawnZombie();
        }
    }

    void SpawnZombie()
    {
        for (int i = 0; i < 5; i++) 
        {
            float positionOffset_X = Random.Range(0, 20f) - 1f;
            float positionOffset_Z = Random.Range(0, 20f) - 1f;
            float angleOffset = Random.Range(0, 360f);

            GameObject zombieTemp = Instantiate(Zombie, spawnPoint[0].position + new Vector3(positionOffset_X, 0, positionOffset_Z), Quaternion.Euler(0, angleOffset, 0)); //�ͦ��L��
            PatrolPath zombiePatrolPathTemp = Instantiate(ZombiePatrolPath, spawnPoint[0].position + new Vector3(positionOffset_X, 0, positionOffset_Z), Quaternion.Euler(0, angleOffset, 0), GameObject.FindGameObjectsWithTag("PatroPathCollection")[0].transform); //�ͦ����ͪ����޸��|�A�ĥ|�ӰѼƬO�ͦ�������n��ַ�@����H
            zombieTemp.GetComponent<ZombieController>().patrolPath = zombiePatrolPathTemp; //�N���޸��|���w���L��
            zombieTemp.GetComponent<ZombieController>().alwaysPatrol = true;
        }

        for (int i = 0; i < 5; i++)
        {
            float positionOffset_X = Random.Range(0, 20f) - 1f;
            float positionOffset_Z = Random.Range(0, 20f) - 1f;
            float angleOffset = Random.Range(0, 360f);

            GameObject zombieTemp = Instantiate(Zombie, spawnPoint[1].position + new Vector3(positionOffset_X, 0, positionOffset_Z), Quaternion.Euler(0, angleOffset, 0)); //�ͦ��L��
            PatrolPath zombiePatrolPathTemp = Instantiate(ZombiePatrolPath, spawnPoint[1].position + new Vector3(positionOffset_X, 0, positionOffset_Z), Quaternion.Euler(0, angleOffset, 0), GameObject.FindGameObjectsWithTag("PatroPathCollection")[0].transform); //�ͦ����ͪ����޸��|�A�ĥ|�ӰѼƬO�ͦ�������n��ַ�@����H
            zombieTemp.GetComponent<ZombieController>().patrolPath = zombiePatrolPathTemp; //�N���޸��|���w���L��
            zombieTemp.GetComponent<ZombieController>().alwaysPatrol = true;
        }

        for (int i = 0; i < 5; i++)
        {
            float positionOffset_X = Random.Range(0, 20f) - 1f;
            float positionOffset_Z = Random.Range(0, 20f) - 1f;
            float angleOffset = Random.Range(0, 360f);

            GameObject zombieTemp = Instantiate(Zombie, spawnPoint[2].position + new Vector3(positionOffset_X, 0, positionOffset_Z), Quaternion.Euler(0, angleOffset, 0)); //�ͦ��L��
            PatrolPath zombiePatrolPathTemp = Instantiate(ZombiePatrolPath, spawnPoint[2].position + new Vector3(positionOffset_X, 0, positionOffset_Z), Quaternion.Euler(0, angleOffset, 0), GameObject.FindGameObjectsWithTag("PatroPathCollection")[0].transform); //�ͦ����ͪ����޸��|�A�ĥ|�ӰѼƬO�ͦ�������n��ַ�@����H
            zombieTemp.GetComponent<ZombieController>().patrolPath = zombiePatrolPathTemp; //�N���޸��|���w���L��
            zombieTemp.GetComponent<ZombieController>().alwaysPatrol = true;
        }



    }


    /*  //�Ѯv�쥻���]�p
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
            GameObject zombieTemp = Instantiate(Zombie, spawnPoint[index].position, spawnPoint[index].rotation); //�ͦ��L��
            PatrolPath zombiePatrolPathTemp = Instantiate(ZombiePatrolPath, spawnPoint[index].position, spawnPoint[index].rotation, GameObject.FindGameObjectsWithTag("PatroPathCollection")[0].transform); //�ͦ����ͪ����޸��|�A�ĥ|�ӰѼƬO�ͦ�������n��ַ�@����H
            zombieTemp.GetComponent<ZombieController>().patrolPath = zombiePatrolPathTemp; //�N���޸��|���w���L��

            yield return new WaitForSeconds(spawnTime);
        }
    }
    */







}
