using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [HideInInspector] public float maxHealth = 0f; //最大血量
    [HideInInspector] public float currentHealth = 0f; //當前血量

    public event Action onDamage; //受到傷害時的事件委派容器
    public event Action onHealed; //受到治癒時的事件委派容器
    public event Action onDie; //受到傷害時的事件委派容器

    private bool isDead = false;


    void Start()
    {

    }

    public void InitHealth(float _maxHealth, float _currentHealth) //血量初始化，參數: 最大血量、當前血量
    {
        maxHealth = _maxHealth;
        currentHealth = _currentHealth;
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
