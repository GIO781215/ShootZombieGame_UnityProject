using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public CameraController cameraController;
    public PlayerController playerController;
    public PlayerWeaponController playerWeaponController;
    [HideInInspector] public InputController inputController;
    [SerializeField] GameObject Game_Scence;
    [SerializeField] GameObject Game_Scence_Root;
    [SerializeField] GameObject Game_Scence_Prefab;

    GameObject restartGameScence;

    public bool IsPhoneMode = false; //�w�]���q�����Ҧ�(�D����Ҧ�)


     
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            inputController = this.GetComponent<InputController>(); //�o��P�˱����b GameManager ���󩳤U�� inputController �}�����ͪ� class ���� (�� GetComponentInParent �]�i�H�C)
        }
        else
        {
            Debug.LogWarning("��l�Ʈ� GameManager �w�g������A�{���i�঳���D���˹�");
        }

    }


    public void RestartGame() //���s�}�l�C��
    {
        cameraController.target = this.transform; //�קK  cameraController.target = NULL ����L�a�誺�{���X��
        Destroy(Game_Scence);

        if (Game_Scence == null)
        {
            Destroy(GameObject.FindGameObjectsWithTag("Scence_Prefab_")[0]);
        }

        Invoke("ResetScence", 0.001f);
    }


    private void ResetScence()
    {
        if (restartGameScence == null)
        {
            //���س����̪��Ҧ�����
            restartGameScence = Instantiate(Game_Scence_Prefab, Game_Scence_Root.transform.position, Game_Scence_Root.transform.rotation, GameObject.FindGameObjectsWithTag("Scence_Prefab")[0].transform); //�ͦ����ͪ����޸��|�A�ĥ|�ӰѼƬO�ͦ�������n��ַ�@����H
            cameraController.target = GameObject.FindGameObjectsWithTag("Player")[0].transform; //�o��A�� cameraController.target ���w�^��
            cameraController.resetCamera(); //��L�n���m���@�ǰѼƦb CameraController ��

            playerController = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerController>();
            playerWeaponController = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerWeaponController>();

        }
    }


    public void setPhoneMode()
    {
        IsPhoneMode = true;
    }


    public void setComputerMode()
    {
        IsPhoneMode = false;
    }

    /*---------�����w�o�˼g------------
     * 
    private GameObject GameManagerObject;

    private static GameManager _Instance;
    public static GameManager Instance //���f
    {
        get
        {
            if( _Instance == null )
            {
                _Instance = new GameManager(); //�ۤv
                _Instance.GameManagerObject = new GameObject("GameManagerObject"); //�s�W�@�� GameObject �Ψӧ��L�� Script ���b�L���U�A�b�������
                _Instance.GameManagerObject.AddComponent<inputController>(); //���W inputController �}��
            }
            return _Instance;
        }

    }

    private inputController _InputController;
    public  inputController inputController
    {
        get
        {
            if( _InputController == null )
            {
                _InputController = GameManagerObject.GetComponent<inputController>();
            }
            return _InputController;
        }
    }
    */

}
