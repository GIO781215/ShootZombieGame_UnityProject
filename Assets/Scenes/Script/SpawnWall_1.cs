using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWall_1 : MonoBehaviour //空氣牆_1 的腳本
{

    [SerializeField] GameObject Zombie; //殭屍預製物件
    [SerializeField] Transform[] spawnPoint; //要生成的位置 (有好幾個，都放在陣列裡)

    float spawnTime = 3f; //每個殭屍生成的間隔時間
    int spawnAmoint = 10; //殭屍要生成的總數量

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
            int index = Random.Range(0, spawnPoint.Length); // Random.Range() 隨機產生一個整數，範圍 : 最小值 ~ 最大值(不包含)。
            print(index);
            Instantiate(Zombie, spawnPoint[index].position, spawnPoint[index].rotation);
            yield return new WaitForSeconds(spawnTime);
        }
    }








}
