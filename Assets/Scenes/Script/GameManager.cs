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
            inputController = this.GetComponent<InputController>(); //得到同樣掛載在 GameManager 物件底下的 inputController 腳本產生的 class 實體 (用 GetComponentInParent 也可以耶)
        }
        else
        {
            Debug.LogWarning("初始化時 GameManager 已經有實體，程式可能有問題請檢察");
        }
    }

 

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
            //銷毀場景裡的所有角色
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
        //重建場景裡的所有角色
        GameObject gameScence = Instantiate(Game_Scence_Prefab, Game_Scence_Root.transform.position, Game_Scence_Root.transform.rotation, GameObject.FindGameObjectsWithTag("Scence_Prefab")[0].transform); //生成僵屍的巡邏路徑，第四個參數是生成的實體要把誰當作父對象
        cameraController.target = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        cameraController.resetCamera();

    }



    /*---------不喜歡這樣寫------------
     * 
    private GameObject GameManagerObject;

    private static GameManager _Instance;
    public static GameManager Instance //接口
    {
        get
        {
            if( _Instance == null )
            {
                _Instance = new GameManager(); //自己
                _Instance.GameManagerObject = new GameObject("GameManagerObject"); //新增一個 GameObject 用來把其他的 Script 掛在他底下，在之後取用
                _Instance.GameManagerObject.AddComponent<inputController>(); //掛上 inputController 腳本
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
