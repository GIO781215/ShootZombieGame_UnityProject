using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AttackHit : MonoBehaviour //���}���|���b����������⪺ FBX �ҫ��U�A�@�����⪺ FBX �ҫ���������ʵe�ɷ|�I�s�����
{
    void Hit() //���ⰵ�X�����ʧ@����ƳQĲ�o�ɡA�|�����I�s�����b�����󨤦�U�� ����_Controller �}������ AtHit() ���
    {
        GameObject Character = this.transform.parent.gameObject;
        if (Character.GetComponent<ZombieController>())
            Character.GetComponent<ZombieController>().AtHit();
    }
}
