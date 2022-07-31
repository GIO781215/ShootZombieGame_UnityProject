using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PickUpItem_FirstAidKit : MonoBehaviour
{
    public GameObject rootObject; //��իݾ߰_�D�㪺�̤W�h����
 
    float rotationSpeed = 40f; //�D����઺�t��
    float verticalMoveSpeed = 1.5f; //�D��W�U���ʪ��t��
    float bobbingDistance = 0.3f; //�D��W�U���ʪ��Z��

    public event Action<GameObject> onPick;

    Vector3 startPosition;



    void Start()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        Collider collider = GetComponent<Collider>();

        rigidbody.isKinematic = true; //�B�ʤ������z�����v�T
        collider.isTrigger = true; //�I���������z�����v�T�A�B�Ϩ���Ĳ�o OnTrigger() ���

        startPosition = this.transform.position;


    }

    void Update()
    {
        transform.position = startPosition + Vector3.up * (((Mathf.Sin(Time.time * verticalMoveSpeed) * 0.5f) + 0.5f) * bobbingDistance);
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self); //���_!!!!!!!!!!!!!!!
    }



    [System.Obsolete]
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.GetComponent<PlayerWeaponController>())  
            {
                other.GetComponent<PlayerController>().health.TakeHealed(100); //��q+100
            }
            Destroy(this.gameObject, 0.1f); //�P���ۤv
            rootObject.GetComponent<PickUp_firstAidKit>().DestroySelf(); //�P���̤W�h������
        }
    }







}
