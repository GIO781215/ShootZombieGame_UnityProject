using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    float projectrileSpeed = 100f; //�l�u�t��
    float maxLifeTime = 3f; //�l�u���s�b�ɶ�
    float gravityDownForce = 0.001f; //���쭫�O�U�Y���q

    Vector3 currentVelocity;


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



}
