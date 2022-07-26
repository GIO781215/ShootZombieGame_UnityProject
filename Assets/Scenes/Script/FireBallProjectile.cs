using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallProjectile : MonoBehaviour
{
    float projectrileSpeed = 50f; //火球速度
    float maxLifeTime = 5f; //火球的存在時間
    float gravityDownForce = 0f; //受到重力下墜的量
    public bool isShoot=false; //火球是否會射出去

    Vector3 currentVelocity;

    [SerializeField] GameObject hitEffectPrefab; //射到目標時的特效


    private void OnEnable() //在 void Start() 前就會執行

    {
        Destroy(this.gameObject, maxLifeTime); //經過 maxLifeTime 的時間後本物件就會銷毀
    }

    public void Start()
    {
        currentVelocity = this.transform.forward * projectrileSpeed; //火球飛行的向量方向就是其生成的朝向
    }

    void Update()
    {
        if (isShoot)
        {
            currentVelocity += Vector3.down * gravityDownForce; //速度受重力影響
            this.transform.position += currentVelocity * Time.deltaTime; //往前飛行
        }
    }

    private void OnParticleCollision(GameObject other) //當火焰粒子特效的碰撞事件發生時會觸發這個函數
    {
        if (other.gameObject.tag == "Mutant" || isShoot == false) return; //如果火球碰到自己或這顆火球不是拿來丟出去的火球，則沒反應

        if (other.gameObject.tag == "Player")
        {
            //print("玩家扣血");
            Health targetHealth = other.GetComponent<Health>();
            if (targetHealth != null && !targetHealth.IsDead())
            {
                targetHealth.TakeDamage(45);
            }
        }

        if (hitEffectPrefab != null)
        {
            GameObject hitEffect = Instantiate(hitEffectPrefab, transform.position, transform.rotation);
            Destroy(hitEffect, 2f);
            Destroy(gameObject); //銷毀本腳本掛載的整個物件，如果寫成 this 的話只是銷毀本腳本的實例而已
        }
    }








}
