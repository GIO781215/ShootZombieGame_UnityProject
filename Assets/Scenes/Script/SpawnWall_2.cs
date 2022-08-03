using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWall_2 : MonoBehaviour
{
    [SerializeField] GameObject Zombie; //殭屍預製物件
    [SerializeField] PatrolPath ZombiePatrolPath; //殭屍的巡邏路徑預製物件
    [SerializeField] Transform[] spawnPoint; //要生成的位置 (有好幾個，都放在陣列裡)


    bool hasBeenTrigger = false;


    private void OnTriggerEnter(Collider other)
    {
        if (hasBeenTrigger) return;

        if (other.gameObject.tag == "Player")
        {
            hasBeenTrigger = true;
            SpawnZombie(); //在橋下生成一堆殭屍

            StartCoroutine(SpawnZombieAlways()); //在固定3個地方一直生成殭屍
        }
    }

    void SpawnZombie()
    {

        for (int i = 0; i < 50; i++)
        {
            float positionOffset_X = Random.Range(0, 10f) - 10f;
            float positionOffset_Z = Random.Range(0, 10f) - 10f;
            float angleOffset = Random.Range(0, 360f);

            GameObject zombieTemp = Instantiate(Zombie, spawnPoint[0].position + new Vector3(positionOffset_X, 0, positionOffset_Z), Quaternion.Euler(0, angleOffset, 0), GameObject.FindGameObjectsWithTag("Scence_Prefab_")[0].transform); //生成殭屍
            PatrolPath zombiePatrolPathTemp = Instantiate(ZombiePatrolPath, spawnPoint[0].position + new Vector3(positionOffset_X, 0, positionOffset_Z), Quaternion.Euler(0, angleOffset, 0), GameObject.FindGameObjectsWithTag("PatroPathCollection")[0].transform); //生成僵屍的巡邏路徑，第四個參數是生成的實體要把誰當作父對象
            zombieTemp.GetComponent<ZombieController>().patrolPath = zombiePatrolPathTemp; //將巡邏路徑指定給殭屍
            zombieTemp.GetComponent<ZombieController>().alwaysPatrol = true;
            zombieTemp.GetComponent<ZombieController>().OnDamageIsChasing = true;

        }

        for (int i = 0; i < 50; i++)
        {
            float positionOffset_X = Random.Range(0, 20f) - 10f;
            float positionOffset_Z = Random.Range(0, 20f) - 10f;
            float angleOffset = Random.Range(0, 360f);

            GameObject zombieTemp = Instantiate(Zombie, spawnPoint[1].position + new Vector3(positionOffset_X, 0, positionOffset_Z), Quaternion.Euler(0, angleOffset, 0), GameObject.FindGameObjectsWithTag("Scence_Prefab_")[0].transform); //生成殭屍
            PatrolPath zombiePatrolPathTemp = Instantiate(ZombiePatrolPath, spawnPoint[1].position + new Vector3(positionOffset_X, 0, positionOffset_Z), Quaternion.Euler(0, angleOffset, 0), GameObject.FindGameObjectsWithTag("PatroPathCollection")[0].transform); //生成僵屍的巡邏路徑，第四個參數是生成的實體要把誰當作父對象
            zombieTemp.GetComponent<ZombieController>().patrolPath = zombiePatrolPathTemp; //將巡邏路徑指定給殭屍
            zombieTemp.GetComponent<ZombieController>().alwaysPatrol = true;
            zombieTemp.GetComponent<ZombieController>().OnDamageIsChasing = true;

        }

        for (int i = 0; i < 50; i++)
        {
            float positionOffset_X = Random.Range(0, 20f) - 10f;
            float positionOffset_Z = Random.Range(0, 20f) - 10f;
            float angleOffset = Random.Range(0, 360f);

            GameObject zombieTemp = Instantiate(Zombie, spawnPoint[2].position + new Vector3(positionOffset_X, 0, positionOffset_Z), Quaternion.Euler(0, angleOffset, 0), GameObject.FindGameObjectsWithTag("Scence_Prefab_")[0].transform); //生成殭屍
            PatrolPath zombiePatrolPathTemp = Instantiate(ZombiePatrolPath, spawnPoint[2].position + new Vector3(positionOffset_X, 0, positionOffset_Z), Quaternion.Euler(0, angleOffset, 0), GameObject.FindGameObjectsWithTag("PatroPathCollection")[0].transform); //生成僵屍的巡邏路徑，第四個參數是生成的實體要把誰當作父對象
            zombieTemp.GetComponent<ZombieController>().patrolPath = zombiePatrolPathTemp; //將巡邏路徑指定給殭屍
            zombieTemp.GetComponent<ZombieController>().alwaysPatrol = true;
            zombieTemp.GetComponent<ZombieController>().OnDamageIsChasing = true;

        }

        for (int i = 0; i < 50; i++)
        {
            float positionOffset_X = Random.Range(0, 20f) - 10f;
            float positionOffset_Z = Random.Range(0, 20f) - 10f;
            float angleOffset = Random.Range(0, 360f);

            GameObject zombieTemp = Instantiate(Zombie, spawnPoint[3].position + new Vector3(positionOffset_X, 0, positionOffset_Z), Quaternion.Euler(0, angleOffset, 0), GameObject.FindGameObjectsWithTag("Scence_Prefab_")[0].transform); //生成殭屍
            PatrolPath zombiePatrolPathTemp = Instantiate(ZombiePatrolPath, spawnPoint[3].position + new Vector3(positionOffset_X, 0, positionOffset_Z), Quaternion.Euler(0, angleOffset, 0), GameObject.FindGameObjectsWithTag("PatroPathCollection")[0].transform); //生成僵屍的巡邏路徑，第四個參數是生成的實體要把誰當作父對象
            zombieTemp.GetComponent<ZombieController>().patrolPath = zombiePatrolPathTemp; //將巡邏路徑指定給殭屍
            zombieTemp.GetComponent<ZombieController>().alwaysPatrol = true;
            zombieTemp.GetComponent<ZombieController>().OnDamageIsChasing = true;

        }

    }





    IEnumerator SpawnZombieAlways()
    {
        while(true)
        {
            int index = Random.Range(4, 7); // Random.Range(int 最小值，int 最大) : 隨機產生一個整數，範圍是 最小值 ~ 最大值(不包含) ， Random.Rang(float 最小值，float 最大) : 隨機產生一個浮點數，範圍是 最小值 ~ 最大值(包含) 

            float angleOffset = Random.Range(0, 360f);
            GameObject zombieTemp = Instantiate(Zombie, spawnPoint[index].position, Quaternion.Euler(0, angleOffset, 0), GameObject.FindGameObjectsWithTag("Scence_Prefab_")[0].transform); //生成殭屍
            PatrolPath zombiePatrolPathTemp = Instantiate(ZombiePatrolPath, spawnPoint[index].position, Quaternion.Euler(0, angleOffset, 0), GameObject.FindGameObjectsWithTag("PatroPathCollection")[0].transform); //生成僵屍的巡邏路徑，第四個參數是生成的實體要把誰當作父對象
            zombieTemp.GetComponent<ZombieController>().patrolPath = zombiePatrolPathTemp; //將巡邏路徑指定給殭屍
            zombieTemp.GetComponent<ZombieController>().alwaysPatrol = true;
            zombieTemp.GetComponent<ZombieController>().OnDamageIsChasing = true;
            zombieTemp.GetComponent<ZombieController>().viewDistance = 10000;

            yield return new WaitForSeconds(10f);
        }
    }


}