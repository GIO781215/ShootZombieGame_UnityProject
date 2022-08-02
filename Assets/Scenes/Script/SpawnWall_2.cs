using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWall_2 : MonoBehaviour
{
    [SerializeField] GameObject Zombie; //�L�͹w�s����
    [SerializeField] PatrolPath ZombiePatrolPath; //�L�ͪ����޸��|�w�s����
    [SerializeField] Transform[] spawnPoint; //�n�ͦ�����m (���n�X�ӡA����b�}�C��)


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

        for (int i = 0; i < 50; i++)
        {
            float positionOffset_X = Random.Range(0, 10f) - 10f;
            float positionOffset_Z = Random.Range(0, 10f) - 10f;
            float angleOffset = Random.Range(0, 360f);

            GameObject zombieTemp = Instantiate(Zombie, spawnPoint[0].position + new Vector3(positionOffset_X, 0, positionOffset_Z), Quaternion.Euler(0, angleOffset, 0), GameObject.FindGameObjectsWithTag("Scence_Prefab_")[0].transform); //�ͦ��L��
            PatrolPath zombiePatrolPathTemp = Instantiate(ZombiePatrolPath, spawnPoint[0].position + new Vector3(positionOffset_X, 0, positionOffset_Z), Quaternion.Euler(0, angleOffset, 0), GameObject.FindGameObjectsWithTag("PatroPathCollection")[0].transform); //�ͦ����ͪ����޸��|�A�ĥ|�ӰѼƬO�ͦ�������n��ַ�@����H
            zombieTemp.GetComponent<ZombieController>().patrolPath = zombiePatrolPathTemp; //�N���޸��|���w���L��
            zombieTemp.GetComponent<ZombieController>().alwaysPatrol = true;
            zombieTemp.GetComponent<ZombieController>().OnDamageIsChasing = true;

        }

        for (int i = 0; i < 50; i++)
        {
            float positionOffset_X = Random.Range(0, 20f) - 10f;
            float positionOffset_Z = Random.Range(0, 20f) - 10f;
            float angleOffset = Random.Range(0, 360f);

            GameObject zombieTemp = Instantiate(Zombie, spawnPoint[1].position + new Vector3(positionOffset_X, 0, positionOffset_Z), Quaternion.Euler(0, angleOffset, 0), GameObject.FindGameObjectsWithTag("Scence_Prefab_")[0].transform); //�ͦ��L��
            PatrolPath zombiePatrolPathTemp = Instantiate(ZombiePatrolPath, spawnPoint[1].position + new Vector3(positionOffset_X, 0, positionOffset_Z), Quaternion.Euler(0, angleOffset, 0), GameObject.FindGameObjectsWithTag("PatroPathCollection")[0].transform); //�ͦ����ͪ����޸��|�A�ĥ|�ӰѼƬO�ͦ�������n��ַ�@����H
            zombieTemp.GetComponent<ZombieController>().patrolPath = zombiePatrolPathTemp; //�N���޸��|���w���L��
            zombieTemp.GetComponent<ZombieController>().alwaysPatrol = true;
            zombieTemp.GetComponent<ZombieController>().OnDamageIsChasing = true;

        }

        for (int i = 0; i < 50; i++)
        {
            float positionOffset_X = Random.Range(0, 20f) - 10f;
            float positionOffset_Z = Random.Range(0, 20f) - 10f;
            float angleOffset = Random.Range(0, 360f);

            GameObject zombieTemp = Instantiate(Zombie, spawnPoint[2].position + new Vector3(positionOffset_X, 0, positionOffset_Z), Quaternion.Euler(0, angleOffset, 0), GameObject.FindGameObjectsWithTag("Scence_Prefab_")[0].transform); //�ͦ��L��
            PatrolPath zombiePatrolPathTemp = Instantiate(ZombiePatrolPath, spawnPoint[2].position + new Vector3(positionOffset_X, 0, positionOffset_Z), Quaternion.Euler(0, angleOffset, 0), GameObject.FindGameObjectsWithTag("PatroPathCollection")[0].transform); //�ͦ����ͪ����޸��|�A�ĥ|�ӰѼƬO�ͦ�������n��ַ�@����H
            zombieTemp.GetComponent<ZombieController>().patrolPath = zombiePatrolPathTemp; //�N���޸��|���w���L��
            zombieTemp.GetComponent<ZombieController>().alwaysPatrol = true;
            zombieTemp.GetComponent<ZombieController>().OnDamageIsChasing = true;

        }

        for (int i = 0; i < 50; i++)
        {
            float positionOffset_X = Random.Range(0, 20f) - 10f;
            float positionOffset_Z = Random.Range(0, 20f) - 10f;
            float angleOffset = Random.Range(0, 360f);

            GameObject zombieTemp = Instantiate(Zombie, spawnPoint[3].position + new Vector3(positionOffset_X, 0, positionOffset_Z), Quaternion.Euler(0, angleOffset, 0), GameObject.FindGameObjectsWithTag("Scence_Prefab_")[0].transform); //�ͦ��L��
            PatrolPath zombiePatrolPathTemp = Instantiate(ZombiePatrolPath, spawnPoint[3].position + new Vector3(positionOffset_X, 0, positionOffset_Z), Quaternion.Euler(0, angleOffset, 0), GameObject.FindGameObjectsWithTag("PatroPathCollection")[0].transform); //�ͦ����ͪ����޸��|�A�ĥ|�ӰѼƬO�ͦ�������n��ַ�@����H
            zombieTemp.GetComponent<ZombieController>().patrolPath = zombiePatrolPathTemp; //�N���޸��|���w���L��
            zombieTemp.GetComponent<ZombieController>().alwaysPatrol = true;
            zombieTemp.GetComponent<ZombieController>().OnDamageIsChasing = true;

        }












        for (int i = 0; i < 33; i++)
        {
            float positionOffset_X = Random.Range(0, 30f) - 15f;
            float positionOffset_Z = Random.Range(0, 30f) - 15f;
            float angleOffset = Random.Range(0, 360f);

            GameObject zombieTemp = Instantiate(Zombie, spawnPoint[4].position + new Vector3(positionOffset_X, 0, positionOffset_Z), Quaternion.Euler(0, angleOffset, 0), GameObject.FindGameObjectsWithTag("Scence_Prefab_")[0].transform); //�ͦ��L��
            PatrolPath zombiePatrolPathTemp = Instantiate(ZombiePatrolPath, spawnPoint[4].position + new Vector3(positionOffset_X, 0, positionOffset_Z), Quaternion.Euler(0, angleOffset, 0), GameObject.FindGameObjectsWithTag("PatroPathCollection")[0].transform); //�ͦ����ͪ����޸��|�A�ĥ|�ӰѼƬO�ͦ�������n��ַ�@����H
            zombieTemp.GetComponent<ZombieController>().patrolPath = zombiePatrolPathTemp; //�N���޸��|���w���L��
            zombieTemp.GetComponent<ZombieController>().alwaysPatrol = true;
            zombieTemp.GetComponent<ZombieController>().OnDamageIsChasing = true;
            zombieTemp.GetComponent<ZombieController>().viewDistance= 10000;
        }

        for (int i = 0; i < 33; i++)
        {
            float positionOffset_X = Random.Range(0, 30f) - 15f;
            float positionOffset_Z = Random.Range(0, 30f) - 15f;
            float angleOffset = Random.Range(0, 360f);

            GameObject zombieTemp = Instantiate(Zombie, spawnPoint[5].position + new Vector3(positionOffset_X, 0, positionOffset_Z), Quaternion.Euler(0, angleOffset, 0), GameObject.FindGameObjectsWithTag("Scence_Prefab_")[0].transform); //�ͦ��L��
            PatrolPath zombiePatrolPathTemp = Instantiate(ZombiePatrolPath, spawnPoint[5].position + new Vector3(positionOffset_X, 0, positionOffset_Z), Quaternion.Euler(0, angleOffset, 0), GameObject.FindGameObjectsWithTag("PatroPathCollection")[0].transform); //�ͦ����ͪ����޸��|�A�ĥ|�ӰѼƬO�ͦ�������n��ַ�@����H
            zombieTemp.GetComponent<ZombieController>().patrolPath = zombiePatrolPathTemp; //�N���޸��|���w���L��
            zombieTemp.GetComponent<ZombieController>().alwaysPatrol = true;
            zombieTemp.GetComponent<ZombieController>().OnDamageIsChasing = true;
            zombieTemp.GetComponent<ZombieController>().viewDistance = 10000;
        }

        for (int i = 0; i < 33; i++)
        {
            float positionOffset_X = Random.Range(0, 30f) - 15f;
            float positionOffset_Z = Random.Range(0, 30f) - 15f;
            float angleOffset = Random.Range(0, 360f);

            GameObject zombieTemp = Instantiate(Zombie, spawnPoint[6].position + new Vector3(positionOffset_X, 0, positionOffset_Z), Quaternion.Euler(0, angleOffset, 0), GameObject.FindGameObjectsWithTag("Scence_Prefab_")[0].transform); //�ͦ��L��
            PatrolPath zombiePatrolPathTemp = Instantiate(ZombiePatrolPath, spawnPoint[6].position + new Vector3(positionOffset_X, 0, positionOffset_Z), Quaternion.Euler(0, angleOffset, 0), GameObject.FindGameObjectsWithTag("PatroPathCollection")[0].transform); //�ͦ����ͪ����޸��|�A�ĥ|�ӰѼƬO�ͦ�������n��ַ�@����H
            zombieTemp.GetComponent<ZombieController>().patrolPath = zombiePatrolPathTemp; //�N���޸��|���w���L��
            zombieTemp.GetComponent<ZombieController>().alwaysPatrol = true;
            zombieTemp.GetComponent<ZombieController>().OnDamageIsChasing = true;
            zombieTemp.GetComponent<ZombieController>().viewDistance = 10000;
        }

    
    }

 
     

}