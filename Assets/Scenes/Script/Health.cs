using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    private float maxHealth = 100f; //�̤j��q
    private float currentHealth; //��e��q

    public event Action onDamage; //����ˮ`�ɪ��ƥ�e���e��
    public event Action nHealed; //����ˮ`�ɪ��ƥ�e���e��
    public event Action onDie; //����ˮ`�ɪ��ƥ�e���e��

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

    //����ˮ`
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0); //�קK���姹��q�ܭt��

        if(currentHealth>0)
        {
            onDamage?.Invoke();  
        }
        /*�۵���
        if(currentHealth > 0 && onDamage != null)
        {
            onDamage.Invoke();  
        }
        */
        if(currentHealth <= 0)
        {
            isDead = true;
            onDie?.Invoke();
            //Destroy(GameObject); //�N�������
        }




    }










}
