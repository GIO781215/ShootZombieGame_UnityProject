using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWall_1 : MonoBehaviour //空氣牆_1 的腳本
{

    [SerializeField] GameObject Zombie; //殭屍預製物件
    [SerializeField] PatrolPath ZombiePatrolPath; //殭屍的巡邏路徑預製物件
    [SerializeField] Transform[] spawnPoint; //要生成的位置 (有好幾個，都放在陣列裡)

    float spawnTime = 1f; //每個殭屍生成的間隔時間
    int spawnAmoint = 20; //殭屍要生成的總數量

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

            GameObject zombieTemp = Instantiate(Zombie, spawnPoint[0].position + new Vector3(positionOffset_X, 0, positionOffset_Z), Quaternion.Euler(0, angleOffset, 0)); //生成殭屍
            PatrolPath zombiePatrolPathTemp = Instantiate(ZombiePatrolPath, spawnPoint[0].position + new Vector3(positionOffset_X, 0, positionOffset_Z), Quaternion.Euler(0, angleOffset, 0), GameObject.FindGameObjectsWithTag("PatroPathCollection")[0].transform); //生成僵屍的巡邏路徑，第四個參數是生成的實體要把誰當作父對象
            zombieTemp.GetComponent<ZombieController>().patrolPath = zombiePatrolPathTemp; //將巡邏路徑指定給殭屍
            zombieTemp.GetComponent<ZombieController>().alwaysPatrol = true;
        }

        for (int i = 0; i < 5; i++)
        {
            float positionOffset_X = Random.Range(0, 20f) - 1f;
            float positionOffset_Z = Random.Range(0, 20f) - 1f;
            float angleOffset = Random.Range(0, 360f);

            GameObject zombieTemp = Instantiate(Zombie, spawnPoint[1].position + new Vector3(positionOffset_X, 0, positionOffset_Z), Quaternion.Euler(0, angleOffset, 0)); //生成殭屍
            PatrolPath zombiePatrolPathTemp = Instantiate(ZombiePatrolPath, spawnPoint[1].position + new Vector3(positionOffset_X, 0, positionOffset_Z), Quaternion.Euler(0, angleOffset, 0), GameObject.FindGameObjectsWithTag("PatroPathCollection")[0].transform); //生成僵屍的巡邏路徑，第四個參數是生成的實體要把誰當作父對象
            zombieTemp.GetComponent<ZombieController>().patrolPath = zombiePatrolPathTemp; //將巡邏路徑指定給殭屍
            zombieTemp.GetComponent<ZombieController>().alwaysPatrol = true;
        }

        for (int i = 0; i < 5; i++)
        {
            float positionOffset_X = Random.Range(0, 20f) - 1f;
            float positionOffset_Z = Random.Range(0, 20f) - 1f;
            float angleOffset = Random.Range(0, 360f);

            GameObject zombieTemp = Instantiate(Zombie, spawnPoint[2].position + new Vector3(positionOffset_X, 0, positionOffset_Z), Quaternion.Euler(0, angleOffset, 0)); //生成殭屍
            PatrolPath zombiePatrolPathTemp = Instantiate(ZombiePatrolPath, spawnPoint[2].position + new Vector3(positionOffset_X, 0, positionOffset_Z), Quaternion.Euler(0, angleOffset, 0), GameObject.FindGameObjectsWithTag("PatroPathCollection")[0].transform); //生成僵屍的巡邏路徑，第四個參數是生成的實體要把誰當作父對象
            zombieTemp.GetComponent<ZombieController>().patrolPath = zombiePatrolPathTemp; //將巡邏路徑指定給殭屍
            zombieTemp.GetComponent<ZombieController>().alwaysPatrol = true;
        }



    }


    /*  //老師原本的設計
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
            int index = Random.Range(0, spawnPoint.Length); // Random.Range(int 最小值，int 最大) : 隨機產生一個整數，範圍是 最小值 ~ 最大值(不包含) ， Random.Rang(float 最小值，float 最大) : 隨機產生一個浮點數，範圍是 最小值 ~ 最大值(包含) 
            GameObject zombieTemp = Instantiate(Zombie, spawnPoint[index].position, spawnPoint[index].rotation); //生成殭屍
            PatrolPath zombiePatrolPathTemp = Instantiate(ZombiePatrolPath, spawnPoint[index].position, spawnPoint[index].rotation, GameObject.FindGameObjectsWithTag("PatroPathCollection")[0].transform); //生成僵屍的巡邏路徑，第四個參數是生成的實體要把誰當作父對象
            zombieTemp.GetComponent<ZombieController>().patrolPath = zombiePatrolPathTemp; //將巡邏路徑指定給殭屍

            yield return new WaitForSeconds(spawnTime);
        }
    }
    */







}
