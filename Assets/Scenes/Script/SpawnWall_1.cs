using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWall_1 : MonoBehaviour //鲤_1 竲セ
{

    [SerializeField] GameObject Zombie; //鞮箇籹ン
    [SerializeField] Transform[] spawnPoint; //璶ネΘ竚 (Τ碭常皚柑)

    float spawnTime = 3f; //–鞮ネΘ丁筳丁
    int spawnAmoint = 10; //鞮璶ネΘ羆计秖

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
            int index = Random.Range(0, spawnPoint.Length); // Random.Range(int 程int 程) : 繦诀玻ネ俱计絛瞅琌 程 ~ 程(ぃ)  Random.Rang(float 程float 程) : 繦诀玻ネ疊翴计絛瞅琌 程 ~ 程() 
            Instantiate(Zombie, spawnPoint[index].position, spawnPoint[index].rotation);
            yield return new WaitForSeconds(spawnTime);
        }
    }








}
