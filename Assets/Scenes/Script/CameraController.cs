using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    InputController inputController;

    [Header("�۾����H���ؼ�")]
    [SerializeField] private Transform target; //private �i�ٲ��A�w�]�N�O private
    [SerializeField] GameObject player; //�����˯S�ĥΡA�n�o�쪱�a����q�t��
    [SerializeField] ParticleSystem behitEffect; //���ˮɪ��S��
    [SerializeField] ParticleSystem rushEffect; //�Ĩ�ɪ��S��



    [Header("��v��������")]
    [SerializeField] float HeightOffset = 3; //��v��������
    float CameraAngle_X = 0; //��v�����k�_�l����
    float CameraAngle_Y = 15; //��v���W�U�_�l����
    float sensitivity_X = 2; //�ƹ�������v�����k���ʪ��F�ӫ�
    float sensitivity_Y = 2; //�ƹ�������v���W�U���ʪ��F�ӫ�
    float sensitivity_ScrollWheel = 5; //�ƹ�������v���e�Ჾ�ʪ��F�ӫ�
    float minVerticalAngle = -10; //��v���W�U���ʪ��̤p���� (���O���W�������̤j���סA�]���b Unity �̪� Edir -> Project Setting -> Input Manager -> Mouse Y ���Ŀ� Invert�A�ҥH�W�U���ۤ�)
    float maxVerticalAngle = 20; //��v���W�U���ʪ��̤j���� (���O���U�������̤j���סA�]���b Unity �̪� Edir -> Project Setting -> Input Manager -> Mouse Y ���Ŀ� Invert�A�ҥH�W�U���ۤ�)
    float cameraToTargetDistance = 10; //��v���P�ؼЪ��_�l�Z��
    float cameraToTargetMinDistance = 5; //��v���P�ؼЪ��̤p�Z��
    float cameraToTargetMaxDistance = 15; //��v���P�ؼЪ��̤j�Z��
    [SerializeField] float CameraAngle_Offset; //��v�� Y �b���_�l���ਤ��
    bool IsFirsrRun = true; //�Ĥ@������ LateUpdate() ���X��

    //-----------------����v�����ưl�ܪ���ݭn���ܼ�//-----------------
    [Header("��v�����ưl�ܪ��骺�ɶ�")]
    [SerializeField] float smoothMoveTime = 0.2f; //��v�����ưl�ܪ��骺�ɶ�
    Vector3 referenceObjectPosition = Vector3.zero; //�����ѦҪ����y�Ц�m
    Vector3 currentVelocity = Vector3.zero; //Vector3.SmoothDamp() �n�ϥΨ쪺�����ܶq�ܼ� (���Ȭ������ѦҪ�����e�t��)
    Vector3 referenceObjectToCameraOffset = Vector3.zero; //�����ѦҪ��Z����v�����Z��

    //�ΨӼȦs W S A D ��J���ܼ�
    float WSAD_move_Speed = 0.5f;
    float W_value = 0;
    float S_value = 0;
    float A_value = 0;
    float D_value = 0;



    void Start()
    {
        inputController = GameManager.Instance.inputController;
        player.GetComponent<Health>().onDamage += OnDamage;
        player.GetComponent<PlayerController>().onSprint += OnSprint;

    }

    private void LateUpdate() //�b Update �����
    {
        if(Cursor.lockState == CursorLockMode.Locked)
        {
            //�]�m��v������
            //�ƹ���J
            CameraAngle_X += inputController.GetMouseX() * sensitivity_X ; 
            //��L��J
            if (inputController.GetDInputHold())
            {
                D_value += WSAD_move_Speed * Time.deltaTime * 1.2f;
                D_value = Mathf.Clamp(D_value, 0, 0.3f);
            }
            else
            {
                D_value = 0;
            }
            CameraAngle_X += D_value;

            if (inputController.GetAInputHold())
            {
                A_value += WSAD_move_Speed * Time.deltaTime * 1.2f;
                A_value = Mathf.Clamp(A_value, 0, 0.3f);
            }
            else
            {
                A_value = 0;
            }
            CameraAngle_X -= A_value;
            //------------------------------
            if (IsFirsrRun)
            {
                CameraAngle_X += CameraAngle_Offset; //�C���@�}�l���]�w��v�������ਤ�״¦V
            }

            //�ƹ���J
            CameraAngle_Y += inputController.GetMouseY() * sensitivity_Y;
            //��L��J
            if (inputController.GetWInputHold())
            {
                W_value += WSAD_move_Speed * Time.deltaTime * 1.2f;
                W_value = Mathf.Clamp(W_value, 0, 0.3f);
            }
            else
            {
                W_value = 0;
            }
            CameraAngle_Y += W_value;

            if (inputController.GetSInputHold())
            {
                S_value += WSAD_move_Speed * Time.deltaTime * 1.2f;
                S_value = Mathf.Clamp(S_value, 0, 0.3f);
            }
            else
            {
                S_value = 0;
            }
            CameraAngle_Y -= S_value;
            //------------------------------
            CameraAngle_Y = Mathf.Clamp(CameraAngle_Y, minVerticalAngle, maxVerticalAngle); //���� CameraAngle_Y ���̤j���׻P�̤p����
            transform.rotation = Quaternion.Euler(CameraAngle_Y, CameraAngle_X, 0);


            //�]�m��v����m
            //�ƹ���J
            cameraToTargetDistance += inputController.GetMouseScrollWheel() * sensitivity_ScrollWheel;
            //��L��J
            if (inputController.GetQInputHold())
            {
                cameraToTargetDistance += 0.02f;
            }
            if (inputController.GetEInputHold())
            {
                cameraToTargetDistance -= 0.02f;
            }
            cameraToTargetDistance = Mathf.Clamp(cameraToTargetDistance, cameraToTargetMinDistance, cameraToTargetMaxDistance);

            if(IsFirsrRun)
            {
                referenceObjectPosition = target.position;
                IsFirsrRun = false;
            }
            else
            {
                //�l�ܤ覡�� ��v���l�����ѦҪ�(�L���ƮĪG) �����ѦҪ��l Player (�����ƮĪG)
                //�o������ѦҪ�����m�A�]���̫ᦳ�� transform.position �[�W referenceObjectToCameraOffset�A�ҥH�o��n��^�Ӥ~�O �����ѦҪ����Ӧb����m
                referenceObjectPosition = Vector3.SmoothDamp(transform.position - referenceObjectToCameraOffset, target.position, ref currentVelocity, smoothMoveTime);
                #region //SmoothDamp() ���ƪ������ : �i����H���ʡA�i�H�ϸ��H�ݰ_�ӫܥ��ơA�Ӥ���o��a    
                /*
                static function SmoothDamp (current : Vector3, target : Vector3, ref currentVelocity : Vector3, smoothTime : float, maxSpeed : float = Mathf.Infinity, deltaTime : float = Time.deltaTime) : Vector3
                �ѼƧt�q�G
                1.current ��e�����m
                2.target �ؼЪ����m
                3.ref currentVelocity ��e�t�סA�o�ӭȥѧA�C���եγo�Ө�ƮɳQ�ק�]�]���ϥ� ref ����r�A�ҥH��Ʒ|�u������ currentVelocity �o���ܼơA�o�N��i�H�b����ɨ�o�쪫���e���t�� currentVelocity�^
                  �`�N : �ܼ� currentVelocity �n�ϥΥ����ܶq�A�p�G�w�q������?�q���ʮĪG�|�X���D�A�Ѿ\�峹 <Unity��Lerp�PSmoothDamp��ƨϥλ~�ϲL�R> : https://www.jianshu.com/p/8a5341c6d5a6
                4.smoothTime ��F�ؼЪ��ɶ��A���p���ȱN�ֳt��F�ؼ�
                5.maxSpeed �Ҥ��\���̤j�t�סA�q�{�L�a�j
                6.deltaTime �ۤW���եγo�Ө�ƪ��ɶ��A�q�{��Time.deltaTime
                */
                #endregion
            }

            referenceObjectPosition.y = HeightOffset; //���w�����ѦҪ������׬� HeightOffset (���� Player ���_�Ӧӧ���)
            referenceObjectToCameraOffset = Quaternion.Euler(CameraAngle_Y, CameraAngle_X, 0) * new Vector3(0, 0, -cameraToTargetDistance); // Quaternion.Euler �i�H�� Vector3 �A���G�������Ӥ�V���V�q����!!! 
            transform.position = referenceObjectPosition + referenceObjectToCameraOffset; //�q�����ѦҪ�����m(�� SmoothDamp() �ĪG�v�T) �b�[�W referenceObjectToCameraOffset �Y����v���Ӧb����m
            
        }
    }

    private void OnDamage()
    {
        if(behitEffect != null)
        {
            behitEffect.Play();
        }
    }

    private void OnSprint()
    {
        if (rushEffect != null)
        {
            //����]�B�S�������n�F
            rushEffect.Play();
        }
    }






}
