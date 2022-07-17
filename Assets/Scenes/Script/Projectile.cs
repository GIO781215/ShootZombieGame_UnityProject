using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public enum ProjectileType //子彈種類 (其實這應該要寫成繼承類才對)
{
    machinegun,
    fiamethrower,
}
*/





public class Projectile : MonoBehaviour
{

    float projectrileSpeed = 100f; //子彈速度
    float maxLifeTime = 3f; //子彈的存在時間
    float gravityDownForce = 0.001f; //受到重力下墜的量

    Vector3 currentVelocity;

  //  [Header("子彈種類")]
  //  [SerializeField] ProjectileType projectileType;
   // [Header("射到目標時的特效")]
    [SerializeField] GameObject hitEffectPrefab;
    float damage = 30; //射到目標時殭屍扣的血量


    private void OnEnable() //在 void Start() 前就會執行
    {
        Destroy(this.gameObject, maxLifeTime); //經過 maxLifeTime 的時間後本物件就會銷毀
    }

    public void Start() 
    {
        currentVelocity = this.transform.forward * projectrileSpeed; //子彈飛行的向量方向就是其生成的朝向
    }

     void Update()
    {
        currentVelocity += Vector3.down * gravityDownForce; //速度受重力影響
        this.transform.position += currentVelocity * Time.deltaTime; //往前飛行
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Weapon" || other.gameObject.tag == "Player") return; //如果子彈碰到槍或玩家則沒反應

        if(other.gameObject.tag == "Zombie")
        {
            //print("殭屍扣血");
            Health targetHealth = other.GetComponent<Health>();
            if(targetHealth != null && !targetHealth.IsDead())
            {
                targetHealth.TakeDamage(damage);
            }
        }

        if(hitEffectPrefab != null)
        {
            GameObject hitEffect =Instantiate(hitEffectPrefab, transform.position, transform.rotation);
            Destroy(hitEffect, 1.5f);
            Destroy(gameObject); //銷毀本腳本掛載的整個物件，如果寫成 this 的話只是銷毀本腳本的實例而已
        }

    }








}
