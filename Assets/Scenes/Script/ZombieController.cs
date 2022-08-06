using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class ZombieController : MonoBehaviour
{
    GameObject player;
    ZombieNavMeshAgent zombieNavMeshAgent; //�Ψ���o�ۤv�g�����ʾɯ誫��
    Health health; //�Ψ���o�ۤv����q�t�Ϊ���

    bool gameOver = false;


    public float viewDistance = 15f; //�L�͵����d��
    float confuseDelayTime = 0f; //���a�b�L�ͪ������d��������x�b������ɶ�  (�ѨM�L�ͷ|�@�_�x�b�Ӿ�������D)
    bool InConfuseBehaviour = false; //�i�J InConfuseBehaviour ���X��
    float confuseTime = 3f; //���a�b�L�ͪ������d�򤺮������L�ͪ��x�b�ɶ�
    float timeSinceLastSawPlayer = Mathf.Infinity; //��l�ȳ]���L���j


    //-------------------�]�w�ʵe���Ѽ�-------------------
    Animator animatorController; //�ʵe���񱱨
    //----------------------------------------------------


    //-------------------���ެ������Ѽ�-------------------
    [SerializeField] public PatrolPath patrolPath; //�i�H�����q Unity ���ᵹ�L���� patrolPath �}��������
    float timeSinceLastArrivePatrolPoint = 0; //���W����F�����I�g�L���ɶ� 
    float timeSinceLastStartPatrol = 0; //���W���}�l���޸g�L���ɶ� (�ѨM�û��F����U�@�Ө����I�����D)
    int GoalPatrolPoint = 0; //��e�ݭn��F�������I
    bool IsPatrol = false; //�O�_�i�J���ު��A
    bool IsConfuseInPatrol = false; //�O�_�b���ޤ����x�b���A
    //----------------------------------------------------


    //-------------------�����������Ѽ�-------------------
    float AttackRangeRadius = 2f; //�����d��b�|
    float timeSinceLastAttack = Mathf.Infinity; //�W��������g�L���ɶ�
    float timeBetweenAttack = 1.1f; //��A���������ɶ����j
    bool IsAttacking = false; //�O�_���b������
                              //----------------------------------------------------


    bool IsTrapped = false; //�L�Ͳ��W���p�d��ɪ��X��
    public bool alwaysPatrol = false; //�L�ͬO�_�|�@������
    public bool OnDamageIsChasing = false; //�L�ͳQ����ɬO�_�|�l���a

    MutantController mutantController; //�]������A�n���D�p�G�]�����F�ۤv�]�n��

    AnimatorStateInfo BaseLayer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); //���b Unity ���N���ҳ]�m�� Player �� GameObject 
        mutantController = GameObject.FindGameObjectWithTag("Mutant").GetComponent<MutantController>();

        zombieNavMeshAgent = GetComponent<ZombieNavMeshAgent>(); //��o�ۤv�g�����ʾɯ誫��
        animatorController = GetComponentInChildren<Animator>();

        health = GetComponent<Health>();
        health.InitHealth(100, 100);
        health.onDamage += OnDamage; //�N�ۤv����� OnDamage() ��i health ���ƥ�e���e�� onDamage ��
        health.onDie += OnDie; //�N�ۤv����� OnDie() ��i health ���ƥ�e���e�� onDie ��


        //���ѼƲK�[�@���H���ȡA���C���L�ͪ��򥻯�O���Ǥ��@��
        viewDistance  +=  UnityEngine.Random.Range(0, 5f); // ���ͪ������d��b 15~ 20 ����
        confuseDelayTime +=  UnityEngine.Random.Range(0, 0.5f);


        GoalPatrolPoint = patrolPath.GetNextRandomPatrolPointNumber();
    }




    void Update()
    {




        if(gameOver == true )
        {
            return;
        }
        if ((this.health.IsDead() || player.GetComponent<Health>().IsDead()))  //�p�G�L�ͩΪ��a�w�g���F���N���򳣤����F
        {
            zombieNavMeshAgent.CancelMove(); //����� 
            zombieNavMeshAgent.SetNavMeshAgentSpeed(0); //�N����ʵe���ܼ� WalkSpeed �]�� 0 �~�|���� idle �ʵe
            animatorController.SetBool("IsAttack", false); //�Ѱ������ʵe
            Invoke("playerDieConfuse", confuseDelayTime);  //�̫�x�b�@�U
            gameOver = true;
            return;
        }

        if(mutantController.GetComponent<Health>().currentHealth <=0) //�p�G�]���S��F�ۤv������ 100 �w�妺��
        {
            health.TakeDamage(100);
        }




        timeSinceLastAttack += Time.deltaTime;
        timeSinceLastSawPlayer += Time.deltaTime;
        if (InConfuseBehaviour) return;




        if (InAttackRange() || IsAttacking) //�p�G�b�L�ͪ������d�򤺡A�άO�w�g���b������
        {
            timeSinceLastSawPlayer = 0;
            AttackBehaviour(); //�����欰
      
        }
        else if (InViewRange() && !IsAttacking) //�p�G�b�L�ͪ������d��
        {
            animatorController.SetBool("IsConfuse", false); //�Ѱ��x�b�ʧ@                                   
            timeSinceLastSawPlayer = 0;
            IsTrapped = false; //�Ѱ��d��X��
            zombieNavMeshAgent.MoveTo(player.transform.position, 1); //���ʨ쪱�a��m
            patrolPath.transform.position = this.transform.position; //�����I�]�|�@������L�Ͳ��ʡA�����L�Ͱ��U�Ӥ~���|�A�@�_��
            patrolPath.transform.rotation = this.transform.rotation;
        }
        else if ((timeSinceLastSawPlayer < confuseTime) && !IsAttacking)   
        {
            InConfuseBehaviour = true;
            ConfuseBehaviour(); //�x�b�欰
        }
        else if(!IsAttacking)
        {
            PatrolBehaviour(); //���ަ欰
        }



    }

    private void AttackBehaviour()
    {

        if (timeSinceLastAttack > timeBetweenAttack)
        {
            IsAttacking = true;
            timeSinceLastAttack = 0;
            zombieNavMeshAgent.CancelMove(); //����� 
            animatorController.SetBool("IsConfuse", false); //�Ѱ��x�b�ʧ@
            animatorController.SetBool("IsAttack", true); //��������ʵe    
        }

        if (timeSinceLastAttack + Time.deltaTime >= timeBetweenAttack && !InAttackRange()) //�w�g�����ʵe�F�A�ӥB���b�����d�򤺤F�A�N�i�H�h�����L�欰�F   (�٬O�n�令�������~Ĳ�o�@�Ө�� ���s�P�_�P�򪬪p����n ???
        {
            IsAttacking = false;
            animatorController.SetBool("IsAttack", false); //��������ʵe      
        }
    }

    public void AtHit()
    {
        if(InAttackRange()) //�p�G�o�ɪ��a�٦b�����d��
        {
            if (player != null)  //���a�N���g
            {
                Health health = player.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(30); //�� 30 �w��
                }
            }
        }
    }



    private void PatrolBehaviour()
    {     
        if(IsPatrol || IsConfuseInPatrol) //���ޤ��Χx�b��
        {
            if(!IsArrivePatrolPoint() && !IsConfuseInPatrol) //�p�G�٨S��F�����I�A�åB���b�x�b��
            {
 
                animatorController.SetBool("IsConfuse", false); //�x�b�ʧ@                                    
                zombieNavMeshAgent.MoveTo(patrolPath.GetPatrolPointPosition(GoalPatrolPoint), 0.2f); //���ʨ�ؼШ����I 

                //���ɭԷ|����������U�@�Ө����I�A�p�G�樫�W�L�@�q�ɶ��A�h�P�w������A�����x�b�@���õ�������
                timeSinceLastStartPatrol += Time.deltaTime;
                if (timeSinceLastStartPatrol > 10f) //�p�G���F�Q���٨줣�F
                {
                    IsPatrol = false; //������������
                    patrolPath.ResethasBeenPatrol();
                    zombieNavMeshAgent.CancelMove();
                    zombieNavMeshAgent.SetNavMeshAgentSpeed(0); //�N����ʵe���ܼ� WalkSpeed �]�� 0 �~�|���� idle �ʵe
                    IsTrapped = true;
                    timeSinceLastStartPatrol = 0;
                }
            }
            else if(IsArrivePatrolPoint()) //���F�ؼШ����I�A�}�l�x�b
            {
 
                zombieNavMeshAgent.CancelMove(); //����� 
                IsConfuseInPatrol = true;
                animatorController.SetBool("IsConfuse", true); //�}�l�x�b�ʵe

                //GoalPatrolPoint = patrolPath.GetNextPatrolPointNumber(GoalPatrolPoint); //�N�ؼ���V�U�@�Ө����I
                GoalPatrolPoint = patrolPath.GetNextRandomPatrolPointNumber();

                timeSinceLastArrivePatrolPoint = 0;  //���m�}�l�x�b�ɶ�
                timeSinceLastStartPatrol = 0; //���m�}�l���޸g�L�ɶ�

                if (patrolPath.PatrolOver == true) //�p�G�Ҧ��I�����ާ��A���ޭȤS�^�� 0 �F
                {
                    IsPatrol = false; //���ާ��F
                    zombieNavMeshAgent.SetNavMeshAgentSpeed(0); //�N����ʵe���ܼ� WalkSpeed �]�� 0 �~�|���� idle �ʵe
                    if (alwaysPatrol == true) //�p�G�L�ͳ]�w���|�@������
                    {
                        IsPatrol = true;
                        patrolPath.PatrolOver = false;
                    }
                }
            } //�x�b��
            else 
            {
 
                timeSinceLastArrivePatrolPoint = timeSinceLastArrivePatrolPoint + Time.deltaTime; //�ֿn�x�b�ɶ�
                if (timeSinceLastArrivePatrolPoint > 3f) //�p�G�x�b�ɶ��w�g�j��3��A�~�Ѱ��x�b�~����
                {
                    IsConfuseInPatrol = false;
                    animatorController.SetBool("IsConfuse", false); //�Ѱ��x�b�ʧ@
                }
            }
        }
        else
        {
            //�i�J�����b�����A 
            //�p�G���]�w�n�@�����ޤ~�|�~����
            if (alwaysPatrol == true && IsTrapped == false) IsPatrol = true;
        }
    }

    private bool IsArrivePatrolPoint() //�O�_��F�����I�F
    {
        return (Vector3.Distance(this.transform.position, patrolPath.GetPatrolPointPosition(GoalPatrolPoint)) < patrolPath.CircleRadius); //��e��m�P�ؼШ����I���Z���O�_�p�����I�b�|�F
    }


    private void ConfuseBehaviour()
    {
        zombieNavMeshAgent.CancelMove(); //����� 
        zombieNavMeshAgent.SetNavMeshAgentSpeed(0); //�N����ʵe���ܼ� WalkSpeed �]�� 0 �~�|���� idle �ʵe
        animatorController.SetBool("IsAttack", false); //�������
        Invoke("ConfuseBehaviour_", confuseDelayTime); //�g�L confuseDelayTime �ɶ������~�|�h������ ConfuseDelayTime()                   
    }

    private bool InViewRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) < viewDistance;  
    }

    private bool InAttackRange()
    {
        return Vector3.Distance(this.transform.position + this.transform.TransformDirection(Vector3.forward * 1.5f), player.transform.position) < AttackRangeRadius;
    }



    void OnDamage()
    {
        if(OnDamageIsChasing)
          StartCoroutine(keepChasing(10f));
        //�i�H�A�K�[��������ɪ��ʵe�B�s�n����
    }

    void OnDie()
    {
        //���`�s�@�U

        zombieNavMeshAgent.CancelMove(); //����� 
        animatorController.SetTrigger("IsDead"); //���񦺤`�ʵe
        Invoke("DestroyCollider", 2.5f);
    }
    void DestroyCollider()
    {
        Destroy(GetComponent<CapsuleCollider>()); //�P���I���ե�
    }



    private IEnumerator keepChasing(float time) //�ƨg�l�v���a�����  
    {
        float _viewDistance = viewDistance;
        viewDistance = 10000;
        yield return new WaitForSeconds(time); //���� time ���
        viewDistance = _viewDistance;
    }

    private void ConfuseBehaviour_() //������ (���a�b�L�ͪ������d��������x�b������ɶ�)
    {
        animatorController.SetBool("IsConfuse", true); //�x�b�ʧ@
        InConfuseBehaviour = false;
        IsPatrol = true; //�x�b����N�i�H���ޤF     
        patrolPath.PatrolOver = false;
    }

    private void playerDieConfuse() //���a�����᪺�x�b�ʧ@
    {
        animatorController.SetBool("IsConfuse", true); //�x�b�ʧ@
        Invoke("playerDieConfuseOver", 2.5f + UnityEngine.Random.Range(0, 0.5f));
    }
    private void playerDieConfuseOver()  
    {
        animatorController.SetBool("IsConfuse", false);
    }

    //-------------------------------------------------------------------------------

    //�e�ϥ\�� Debug �� 
    void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            //Gizmos.color = Color.blue;
            //Gizmos.DrawLine(GetPatrolPointPosition(i), GetPatrolPointPosition(j)); //�e�u
            //Gizmos.DrawSphere(GetPatrolPointPosition(i), CircleRadius); //�e�y
            Handles.color = Color.green;
            Handles.DrawWireDisc(this.transform.position  + this.transform.TransformDirection(Vector3.forward * 1.5f), new Vector3(0, 1, 0), AttackRangeRadius); //�e��A�Ѽ�: ��L���ߡB��L�k�u�B��L�b�| (�� using UnityEditor)
            //GameObject.transform.TransformDirection(Vector3 direction) ���V�q direction �q���� local �y�Шt�ഫ��@�ɮy�Шt�W
        }
    }




//----------------------------------------------------------------------------------------
    private void checkAnimatorState() //����ƥi�P�_ Animator ��e�O����ʧ@���A
    {
        BaseLayer = animatorController.GetCurrentAnimatorStateInfo(0); //��o�� 0 �h(BaseLayer�h) �����
        //  print(BaseLayer.fullPathHash);
        //  print(Animator.StringToHash("BaseLayer.attack"));                                                                    

        //�P�_ BaseLayer �h���A���� Hash �X�O�_�P���w�ʧ@�� Hash �X�ۦP
        if (BaseLayer.fullPathHash == Animator.StringToHash("Base Layer.Blend Tree"))
        {
            print("idle");
        }

        if (BaseLayer.fullPathHash == Animator.StringToHash("Base Layer.confuse"))
        {
            print("confuse");
        }

        if (BaseLayer.fullPathHash == Animator.StringToHash("Base Layer.attack"))
        {
            print("attack");
        }
    }
    //----------------------------------------------------------------------------------------


    

}
