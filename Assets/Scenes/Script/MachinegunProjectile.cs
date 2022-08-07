using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MachinegunProjectile : MonoBehaviour
{

    float projectrileSpeed = 100f; //�l�u�t��
    float maxLifeTime = 5f; //�l�u���s�b�ɶ�
    float gravityDownForce = 0.001f; //���쭫�O�U�Y���q

    Vector3 currentVelocity;

    [SerializeField] GameObject hitEffectPrefab; //�g��ؼЮɪ��S��


    private void OnEnable() //�b void Start() �e�N�|����
    {
        Destroy(this.gameObject, maxLifeTime); //�g�L maxLifeTime ���ɶ��᥻����N�|�P��
    }

    public void Start() 
    {
        currentVelocity = this.transform.forward * projectrileSpeed; //�l�u���檺�V�q��V�N�O��ͦ����¦V
    }

     void Update()
    {
        currentVelocity += Vector3.down * gravityDownForce; //�t�ר����O�v�T
        this.transform.position += currentVelocity * Time.deltaTime; //���e����
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Weapon" || other.gameObject.tag == "Player" || other.gameObject.tag == "SpawnWall") 
        {
            return; //�p�G�l�u�I��j�Ϊ��a�h�S����
        }



        if(other.gameObject.tag == "Zombie")
        {
            //print("�L�ͦ���");
            Health targetHealth = other.GetComponent<Health>();
            if(targetHealth != null && !targetHealth.IsDead())
            {
                targetHealth.TakeDamage(30);
            }
        }

        if (other.gameObject.tag == "Mutant")
        {
            //print("�]������");
            Health targetHealth = other.GetComponent<Health>();
            if (targetHealth != null && !targetHealth.IsDead())
            {
                targetHealth.TakeDamage(20);
            }
        }

        if (hitEffectPrefab != null)
        {
            GameObject hitEffect =Instantiate(hitEffectPrefab, transform.position, transform.rotation);
            Destroy(hitEffect, 1.5f);
            Destroy(gameObject); //�P�����}����������Ӫ���A�p�G�g�� this ���ܥu�O�P�����}������ҦӤw
        }

    }








}
