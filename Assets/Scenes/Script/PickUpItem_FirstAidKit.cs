using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PickUpItem_FirstAidKit : MonoBehaviour
{
    public GameObject rootObject; //整組待撿起道具的最上層物件
 
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


    }

    void Update()
    {
        transform.position = startPosition + Vector3.up * (((Mathf.Sin(Time.time * verticalMoveSpeed) * 0.5f) + 0.5f) * bobbingDistance);
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self); //神奇!!!!!!!!!!!!!!!
    }



    [System.Obsolete]
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.GetComponent<PlayerWeaponController>())  
            {
                other.GetComponent<PlayerController>().health.TakeHealed(100); //血量+100
            }
            Destroy(this.gameObject, 0.1f); //銷毀自己
            rootObject.GetComponent<PickUp_firstAidKit>().DestroySelf(); //銷毀最上層父物件
        }
    }







}
