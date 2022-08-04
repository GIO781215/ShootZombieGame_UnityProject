using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public InputController inputController;
    public CameraController cameraController;
    [SerializeField] GameObject Game_Scence;
    [SerializeField] GameObject Game_Scence_Root;
    [SerializeField] GameObject Game_Scence_Prefab;


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

 

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
            //�P�������̪��Ҧ�����
            cameraController.target = this.transform;
            Destroy(Game_Scence);
            if (Game_Scence == null)
            {
                Destroy(GameObject.FindGameObjectsWithTag("Scence_Prefab_")[0]);
            }

            Invoke("ResetScence", 0.001f);
        }

    }

    private void ResetScence()
    {
        //���س����̪��Ҧ�����
        GameObject gameScence = Instantiate(Game_Scence_Prefab, Game_Scence_Root.transform.position, Game_Scence_Root.transform.rotation, GameObject.FindGameObjectsWithTag("Scence_Prefab")[0].transform); //�ͦ����ͪ����޸��|�A�ĥ|�ӰѼƬO�ͦ�������n��ַ�@����H
        cameraController.target = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        cameraController.resetCamera();

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
