using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PickUpItem : MonoBehaviour
{
    public GameObject rootObject; //整組待撿起道具的最上層物件
    public WeaponType weaponType; //自己是什麼道具(武器)

    float rotationSpeed = 40f; //道具旋轉的速度
    float verticalMoveSpeed = 1.5f; //道具上下移動的速度
    float bobbingDistance = 0.3f; //道具上下移動的距離

    public event Action<GameObject> onPick;

    Vector3 startPosition;



     void Start()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        Collider collider = GetComponent<Collider>();

        rigidbody.isKinematic = true; //運動不受物理引擎影響
        collider.isTrigger = true; //碰撞不受物理引擎影響，且使其能夠觸發 OnTrigger() 函數

        startPosition = this.transform.position;

        Weapon weapon = this.gameObject.GetComponent<Weapon>();
        if(weapon != null)
        {
            weaponType = weapon.weaponType; //得到自己是什麼類型的武器
        }
        
    }

     void Update()
    {
        transform.position = startPosition + Vector3.up * (  ((Mathf.Sin(Time.time * verticalMoveSpeed) * 0.5f) + 0.5f) * bobbingDistance);
        transform.Rotate(Vector3.up,rotationSpeed*Time.deltaTime,Space.Self); //神奇!!!!!!!!!!!!!!!
    }



    [System.Obsolete]
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (other.GetComponent<PlayerWeaponController>())
            {
                other.GetComponent<PlayerWeaponController>().PickUpWeapon(this.gameObject); //告訴 Player 撿到自己(火焰槍)
                other.GetComponent<PlayerWeaponController>().SwitchWeaponToFlamethrower(); //直接讓 Player 切換成火焰槍
            }

            Destroy(this.gameObject, 0.1f); //銷毀自己
            rootObject.GetComponent<PickUpFlamethrower>().DestroySelf(); //銷毀最上層父物件
        }
    }
 






}
