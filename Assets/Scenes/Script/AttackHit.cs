using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AttackHit : MonoBehaviour //此腳本會掛在能攻擊的角色的 FBX 模型下，作為角色的 FBX 模型執行攻擊動畫時會呼叫的函數
{
    void Hit() //當角色做出攻擊動作此函數被觸發時，會直接呼叫掛載在父物件角色下的 角色_Controller 腳本中的 AtHit() 函數
    {
        GameObject Character = this.transform.parent.gameObject;
        if (Character.GetComponent<ZombieController>())
            Character.GetComponent<ZombieController>().AtHit();
    }
}
