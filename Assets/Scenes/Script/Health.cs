using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    private float maxHealth = 100f; //最大血量
    private float currentHealth; //當前血量

    public event Action onDamage; //受到傷害時的事件委派容器
    public event Action nHealed; //受到傷害時的事件委派容器
    public event Action onDie; //受到傷害時的事件委派容器

    private bool isDead = false;


    void Start()
    {
        currentHealth = maxHealth;
    }


    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetHealthRatio()
    {
        return currentHealth / maxHealth;
    }

    public bool IsDead()
    {
        return isDead;
    }

    //受到傷害
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0); //避免扣血完血量變負值

        if(currentHealth>0)
        {
            onDamage?.Invoke();  
        }
        /*相等於
        if(currentHealth > 0 && onDamage != null)
        {
            onDamage.Invoke();  
        }
        */
        if(currentHealth <= 0)
        {
            isDead = true;
            onDie?.Invoke();
            //Destroy(GameObject); //將角色消除
        }




    }










}
