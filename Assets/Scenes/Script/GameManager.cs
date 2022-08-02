using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public InputController inputController;
    [SerializeField] GameObject GameScence;


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
        if(Input.GetKeyDown(KeyCode.P))
        {
            Destroy(GameScence);

        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            GameObject gameScence = Instantiate(GameScence, GameScence.transform.position  , GameScence.transform.rotation, GameObject.FindGameObjectsWithTag("Scence_Prefab")[0].transform); //�ͦ����ͪ����޸��|�A�ĥ|�ӰѼƬO�ͦ�������n��ַ�@����H

        }
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
