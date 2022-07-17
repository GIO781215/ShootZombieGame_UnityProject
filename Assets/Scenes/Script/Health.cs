using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [HideInInspector] public float maxHealth = 0f; //�̤j��q
    [HideInInspector] public float currentHealth = 0f; //��e��q

    public event Action onDamage; //����ˮ`�ɪ��ƥ�e���e��
    public event Action onHealed; //����v¡�ɪ��ƥ�e���e��
    public event Action onDie; //����ˮ`�ɪ��ƥ�e���e��

    private bool isDead = false;


    void Start()
    {

    }

    public void InitHealth(float _maxHealth, float _currentHealth) //��q��l�ơA�Ѽ�: �̤j��q�B��e��q
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
