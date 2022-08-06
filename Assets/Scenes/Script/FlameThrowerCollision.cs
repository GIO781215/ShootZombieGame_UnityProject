using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerCollision : MonoBehaviour
{



    /*//-----------------------------------------------------------------------------------------------------
    Collision Quality : �ϥΤU��????�m�ɤl�I����?�q�C��?�m?�v?���h�ֲɤl�i�H��?�I���^�C�b?�C��?�q�����U�A�ɤl��??��?�I���^�A���ݭn��?��?��?�֡C
        ? Collision Quality ?�m? High ?�A�I���l?�ϥΪ��z�t????�I��?�G�C��?�m�O�̯�??�����]�O�̭��̪�??�C
        ? Collision Quality ?�m? Medium(Static Colliders) ?�A�I���ϥΤ@?�^��??�s���e���I���A?�Ӧb�H�Z��?����֦a���ΡC
        ? Collision Quality ?�m? Low(Static Colliders) ?�A�I���ϥΤ@?�^��??�s���e���I���A?�Ӧb�H�Z��?����֦a���ΡC
        Medium �M Low ��?���ߤ@??�O�ɤl�t?�b�C?�d?���z�t?����?�C Medium �C?���d?��?�h�_ Low�C?�`�N�A��?�m?��Τ_?����?��??�I���^�C

    Collides With : �ɤl�u?�O��??�W��?�H?�͸I���C
    Max Collision Shapes : �ɤl�I���i�]�A���I����???�C�h�E����??�Q�����A�B�a��ɬ���C
    Voxel Size �^��(voxel) : ��ܤT?��?�����`?�I��W���ȡC�ϥ� Medium �� Low ?�q�I��?�AUnity ?�b�I��?�ۤ�?�s�I���C��?�m����?�I��j�p�C?�p���ȥi���ѧ󰪪����̩ʡA��?�e�Χ�h?�s�A�Ĳv�]?���C�C�`�N�G?? Collision Quality ?�m? Medium �� Low ?�A�~��??��?�ʡC

    * ? Collision Quality ?�m? Low�A�}?�� Voxel Size ��?�వ�X�ɤl���z���^���ĪG�A�}�B�]���D? OnParticleCollision() ��? <--- ��??����L��n���Ѫk...
    *///-----------------------------------------------------------------------------------------------------



    private void OnParticleCollision(GameObject other) //����K�ɤl�S�Ī��I���ƥ�o�ͮɷ|Ĳ�o�o�Ө��
    {
        print(other);

        if (other.gameObject.tag == "Weapon" || other.gameObject.tag == "Player") return; //�p�G�l�u�I��j�Ϊ��a�h�S����

        if (other.gameObject.tag == "Zombie")
        {
            Health targetHealth = other.GetComponent<Health>();
            if (targetHealth != null && !targetHealth.IsDead())
            {
                targetHealth.TakeDamage(10);
            }
        }

        if (other.gameObject.tag == "Mutant")
        {
            Health targetHealth = other.GetComponent<Health>();
            if (targetHealth != null && !targetHealth.IsDead())
            {
                targetHealth.TakeDamage(5);
            }
        }

    }
    
     
    private void OnParticleTrigger() //����K�ɤl�S�Ī��I���ƥ�o�ͮɷ|Ĳ�o�o�Ө�ơA���O�S��k����I������H...
    {
        print("�ɤl�o�͸I��");
    }


 

}
