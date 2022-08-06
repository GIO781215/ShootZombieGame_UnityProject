using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerCollision : MonoBehaviour
{



    /*//-----------------------------------------------------------------------------------------------------
    Collision Quality : 使用下拉????置粒子碰撞的?量。此?置?影?有多少粒子可以穿?碰撞体。在?低的?量水平下，粒子有??穿?碰撞体，但需要的?算?源?少。
        ? Collision Quality ?置? High ?，碰撞始?使用物理系????碰撞?果。此?置是最耗??源但也是最准确的??。
        ? Collision Quality ?置? Medium(Static Colliders) ?，碰撞使用一?体素??存先前的碰撞，?而在以后的?中更快地重用。
        ? Collision Quality ?置? Low(Static Colliders) ?，碰撞使用一?体素??存先前的碰撞，?而在以后的?中更快地重用。
        Medium 和 Low 之?的唯一??是粒子系?在每?查?物理系?的次?。 Medium 每?的查?次?多于 Low。?注意，此?置?适用于?不移?的??碰撞体。

    Collides With : 粒子只?与所??上的?象?生碰撞。
    Max Collision Shapes : 粒子碰撞可包括的碰撞形???。多余的形??被忽略，且地形优先。
    Voxel Size 体素(voxel) : 表示三?空?中的常?网格上的值。使用 Medium 或 Low ?量碰撞?，Unity ?在网格?构中?存碰撞。此?置控制?网格大小。?小的值可提供更高的准确性，但?占用更多?存，效率也?降低。注意：?? Collision Quality ?置? Medium 或 Low ?，才能??此?性。

    * ? Collision Quality ?置? Low，并?高 Voxel Size 比?能做出粒子能穿透物体的效果，并且也能触? OnParticleCollision() 函? <--- 我??找到其他更好的解法...
    *///-----------------------------------------------------------------------------------------------------



    private void OnParticleCollision(GameObject other) //當火焰粒子特效的碰撞事件發生時會觸發這個函數
    {
        print(other);

        if (other.gameObject.tag == "Weapon" || other.gameObject.tag == "Player") return; //如果子彈碰到槍或玩家則沒反應

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
    
     
    private void OnParticleTrigger() //當火焰粒子特效的碰撞事件發生時會觸發這個函數，但是沒辦法獲取碰撞的對象...
    {
        print("粒子發生碰撞");
    }


 

}
