using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallProjectile : MonoBehaviour
{
    float projectrileSpeed = 50f; //���y�t��
    float maxLifeTime = 5f; //���y���s�b�ɶ�
    float gravityDownForce = 0f; //���쭫�O�U�Y���q
    public bool isShoot=false; //���y�O�_�|�g�X�h

    Vector3 currentVelocity;

    [SerializeField] GameObject hitEffectPrefab; //�g��ؼЮɪ��S��


    private void OnEnable() //�b void Start() �e�N�|����

    {
        Destroy(this.gameObject, maxLifeTime); //�g�L maxLifeTime ���ɶ��᥻����N�|�P��
    }

    public void Start()
    {
        currentVelocity = this.transform.forward * projectrileSpeed; //���y���檺�V�q��V�N�O��ͦ����¦V
    }

    void Update()
    {
        if (isShoot)
        {
            currentVelocity += Vector3.down * gravityDownForce; //�t�ר����O�v�T
            this.transform.position += currentVelocity * Time.deltaTime; //���e����
        }
    }

    private void OnParticleCollision(GameObject other) //����K�ɤl�S�Ī��I���ƥ�o�ͮɷ|Ĳ�o�o�Ө��
    {
        if (other.gameObject.tag == "Mutant" || isShoot == false) return; //�p�G���y�I��ۤv�γo�����y���O���ӥ�X�h�����y�A�h�S����

        if (other.gameObject.tag == "Player")
        {
            //print("���a����");
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
            Destroy(gameObject); //�P�����}����������Ӫ���A�p�G�g�� this ���ܥu�O�P�����}������ҦӤw
        }
    }








}
