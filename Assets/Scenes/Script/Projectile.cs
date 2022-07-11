using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    float projectrileSpeed = 100f; //子彈速度
    float maxLifeTime = 3f; //子彈的存在時間
    float gravityDownForce = 0.001f; //受到重力下墜的量

    Vector3 currentVelocity;


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



}
