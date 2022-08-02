using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PickUpItem : MonoBehaviour
{
    public GameObject rootObject; //��իݾ߰_�D�㪺�̤W�h����
    public WeaponType weaponType; //�ۤv�O����D��(�Z��)

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

        Weapon weapon = this.gameObject.GetComponent<Weapon>();
        if(weapon != null)
        {
            weaponType = weapon.weaponType; //�o��ۤv�O�����������Z��
        }
        
    }

     void Update()
    {
        transform.position = startPosition + Vector3.up * (  ((Mathf.Sin(Time.time * verticalMoveSpeed) * 0.5f) + 0.5f) * bobbingDistance);
        transform.Rotate(Vector3.up,rotationSpeed*Time.deltaTime,Space.Self); //���_!!!!!!!!!!!!!!!
    }



    [System.Obsolete]
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (other.GetComponent<PlayerWeaponController>())
            {
                other.GetComponent<PlayerWeaponController>().PickUpWeapon(this.gameObject); //�i�D Player �ߨ�ۤv(���K�j)
                other.GetComponent<PlayerWeaponController>().SwitchWeaponToFlamethrower(); //������ Player ���������K�j
            }

            Destroy(this.gameObject, 0.1f); //�P���ۤv
            rootObject.GetComponent<PickUpFlamethrower>().DestroySelf(); //�P���̤W�h������
        }
    }
 






}
