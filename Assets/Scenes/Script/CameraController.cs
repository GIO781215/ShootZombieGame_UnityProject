using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameManager gameManager;
    InputController inputController;
    [SerializeField] GameObject Victory_UI;
    [SerializeField] GameObject Lose_UI;

    public bool IsPlayerDeath = false; //���a�O�_���`�X��
    public bool IsMutantDeath = false; //�]���O�_���`�X��

    [SerializeField] AudioClip sound_BGM; //BGM
    [SerializeField] AudioClip sound_Restart; //���s�}�l����
    [SerializeField] AudioClip sound_Victory; //�ӧQ����
    [SerializeField] GameObject restartButton; //���s�}�l�����s

    [SerializeField] AudioClip sound_Lose; //���ѭ���
    AudioSource audioSource;


    [Header("�۾����H���ؼ�")]
    public Transform target; //private �i�ٲ��A�w�]�N�O private
    //[SerializeField] private Transform target_temp; //�R�����a����v���n�ܧ󪺰l�H�ؼ�

    [SerializeField] GameObject player; //�����˯S�ĥΡA�n�o�쪱�a����q�t��
    [SerializeField] GameObject mutant; //�P�O�����]������X�ӧQUI�Ϊ�

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
    float WSAD_speed = 0.1f; // W S A D ��J���������ʪ��ֺC
    float WSAD_Max_speed = 0.8f; // W S A D ��J���������ʪ��̤j�t��
    float W_value = 0;
    float S_value = 0;
    float A_value = 0;
    float D_value = 0;





    void Start()
    {
        inputController = GameManager.Instance.inputController;
        player.GetComponent<Health>().onDamage += OnDamage;
        player.GetComponent<Health>().onDie += OnDie_Player;
        player.GetComponent<PlayerController>().onSprint += OnSprint;

        mutant.GetComponent<Health>().onDie += OnDie_Mutant;
        audioSource = GetComponent<AudioSource>();

        //�]�m�ӧQ�P���Ѫ�UI
        Victory_UI.SetActive(true);
        Victory_UI.GetComponent<Victory_UI_Controller>().Hied();
        Lose_UI.SetActive(true);
        Lose_UI.GetComponent<Lose_UI_Controller>().Hied();

        //�@�}�l������ BGM
        audioSource.volume = 0.5f;
        audioSource.loop = true;
        audioSource.clip = sound_BGM;
        audioSource.Play();

    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && (IsPlayerDeath || IsMutantDeath))
        {
            GameManager.Instance.RestartGame();
        }
    }

    private void LateUpdate() //�b Update �����
    {
        if (Cursor.lockState == CursorLockMode.Locked || GameManager.Instance.IsPhoneMode)
        {


            //�]�m��v������----------------------------------------------------------------------------------------------------------------------------------------------------------------------------

             //---------------------------------�q����������---------------------------------
            if (!GameManager.Instance.IsPhoneMode) 
            {
                //�ƹ���J
                CameraAngle_X += inputController.GetMouseX() * sensitivity_X;
                //��L��J
                if (inputController.GetDInputHold())
                {
                    D_value = Mathf.Lerp(D_value, WSAD_Max_speed, WSAD_speed);
                }
                else
                {
                    D_value = 0;
                }
                CameraAngle_X += D_value;

                if (inputController.GetAInputHold())
                {
                    A_value = Mathf.Lerp(A_value, WSAD_Max_speed, WSAD_speed);
                }
                else
                {
                    A_value = 0;
                }
                CameraAngle_X -= A_value;

                //------------------- 

                //�ƹ���J
                CameraAngle_Y += inputController.GetMouseY() * sensitivity_Y;
                //��L��J
                if (inputController.GetWInputHold())
                {
                    W_value = Mathf.Lerp(W_value, WSAD_Max_speed, WSAD_speed);
                }
                else
                {
                    W_value = 0;
                }
                CameraAngle_Y -= W_value;

                if (inputController.GetSInputHold())
                {
                    S_value = Mathf.Lerp(S_value, WSAD_Max_speed, WSAD_speed);
                }
                else
                {
                    S_value = 0;
                }
                CameraAngle_Y += S_value;
            }
             
            //---------------------------------�����������---------------------------------
            if (GameManager.Instance.IsPhoneMode)  
            {
                //��������v���ϥܪ��ɭ�
                if (GameManager.Instance.inputController.Phone_Camera)
                {
                    CameraAngle_X += inputController.GetMouseX() * sensitivity_X;
                    CameraAngle_Y += inputController.GetMouseY() * sensitivity_Y;
                }
            }
            //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------





            if (IsFirsrRun)
            {
                CameraAngle_X += CameraAngle_Offset; //�C���@�}�l���]�w��v�������ਤ�״¦V
            }

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

            if (IsFirsrRun)
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
        if (behitEffect != null)
        {
            behitEffect.Play();
        }
    }

    private void OnDie_Player()
    {
        IsPlayerDeath = true;
        Invoke("playLoseSound", 1.5f);
        Lose_UI.GetComponent<Lose_UI_Controller>().Show();
    }

    void playLoseSound()
    {
        if (IsPlayerDeath == true)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(sound_Lose);
            Cursor.lockState = CursorLockMode.None; //�ѩ�ƹ�
        }
    }

    private void OnDie_Mutant()
    {
        IsMutantDeath = true;
        Invoke("playVictorySound", 1.5f);
        Victory_UI.GetComponent<Victory_UI_Controller>().Show();
    }

    void playVictorySound()
    {
        if (IsMutantDeath == true)
        {
            audioSource.Stop();
            audioSource.volume = 0.3f;
            audioSource.PlayOneShot(sound_Victory);
            Cursor.lockState = CursorLockMode.None; //�ѩ�ƹ�
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




    public void resetCamera() //�|�Q GameManager �����s�}�l�C������� RestartGame() �I�s��
    {
       HeightOffset = 3; //��v��������
       CameraAngle_X = 0; //��v�����k�_�l����
       CameraAngle_Y = 15; //��v���W�U�_�l����
       sensitivity_X = 2; //�ƹ�������v�����k���ʪ��F�ӫ�
       sensitivity_Y = 2; //�ƹ�������v���W�U���ʪ��F�ӫ�
       sensitivity_ScrollWheel = 5; //�ƹ�������v���e�Ჾ�ʪ��F�ӫ�
       minVerticalAngle = -10; //��v���W�U���ʪ��̤p���� (���O���W�������̤j���סA�]���b Unity �̪� Edir -> Project Setting -> Input Manager -> Mouse Y ���Ŀ� Invert�A�ҥH�W�U���ۤ�)
       maxVerticalAngle = 20; //��v���W�U���ʪ��̤j���� (���O���U�������̤j���סA�]���b Unity �̪� Edir -> Project Setting -> Input Manager -> Mouse Y ���Ŀ� Invert�A�ҥH�W�U���ۤ�)
       cameraToTargetDistance = 10; //��v���P�ؼЪ��_�l�Z��
       cameraToTargetMinDistance = 5; //��v���P�ؼЪ��̤p�Z��
       cameraToTargetMaxDistance = 15; //��v���P�ؼЪ��̤j�Z��
       CameraAngle_Offset = 270; //��v�� Y �b���_�l���ਤ��
       IsFirsrRun = true; //�Ĥ@������ LateUpdate() ���X��

        //���s�缾�a����[�J�e�U�ƥ󪺨��
        PlayerController playerController = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerController>();
        playerController.onSprint += OnSprint;
        playerController.GetComponent<Health>().onDamage += OnDamage;
        playerController.GetComponent<Health>().onDie += OnDie_Player;
        //���s���]������[�J�e�U�ƥ󪺨��
        MutantController mutantController = GameObject.FindGameObjectsWithTag("Mutant")[0].GetComponent<MutantController>();
        mutantController.GetComponent<Health>().onDie += OnDie_Mutant;

        //���óӧQ�P���Ѫ�UI
        Victory_UI.SetActive(true);
        Lose_UI.SetActive(true);
        Victory_UI.GetComponent<Victory_UI_Controller>().Hied();
        Lose_UI.GetComponent<Lose_UI_Controller>().Hied();
        restartButton.gameObject.SetActive(false);


        //���񭵮�
        audioSource.PlayOneShot(sound_Restart);

        audioSource.volume = 0.5f;
        audioSource.loop = true;
        audioSource.clip = sound_BGM;
        audioSource.Play();

        //���m�Ѽ�
        IsPlayerDeath = false;
        IsMutantDeath = false;

        //�վ�ƹ��J�I���A�A����i��|�A�վ�o�Ӽg�k�A�`�N�@�U!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        Cursor.lockState = CursorLockMode.Locked;
        if (GameManager.Instance.IsPhoneMode)
        {
            Invoke("setCursorLockMode", 0.5f);
        }
    }

    void setCursorLockMode()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
