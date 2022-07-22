using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//1是1號槍 machinegun，2是2號槍 flamethrower


public class PlayerWeaponController : MonoBehaviour
{
    /*//---------------------------------------------------------------------------
    有個搞不太清楚的地方???
    把 Weapon 腳本掛到 GameObject 上後，竟然可以把這個 GameObject 指定給本腳本的 List<Weapon>， List 裡面的類不是應該是要 GameObject 才對嗎?
    推測是在 Unity 中可以把掛有該腳本的 GameObject 直接指定給該腳本的 class 變數，而得到的依然是這個 class 組件而不是丟給他的 GameObject
    *///---------------------------------------------------------------------------
    [SerializeField] List<Weapon> AllWeaponsList = new List<Weapon>(); //遊戲中所有種類的武器，在 Unity 中就把所有武器的 prefab 都先丟進來給他 (只要把 Weapon 腳本掛在武器模型的預製物件下，就可以直接先丟進去)
    public Image[] machinegunUI = new Image[3];
    public Image[] flamethrowerUI = new Image[3];
    //public Sprite image; //可直接放圖片資源


    Weapon[] PlayerWeaponSlot = new Weapon[3]; //Player 的武器槽，Player 最多只能得到三個武器，但這遊戲現在最多就只有兩把武器而已，所以武器 weaponUI 就只有先設 2 而已

    int currentWeaponSlotIndex; //目前在武器槽中選擇的武器的索引值，-1 代表目前沒有選擇任何槍
    [SerializeField] Transform equipPosition; //裝備武器的位置 (從 Unity 中指定給他)
    bool isAim; //使否處於瞄準狀態
    bool toAim = false; //true 代表為剛拿起武器的狀態，false 代表沒拿武器或已經拿武器的狀態
    PlayerController playerController;
    InputController inputController;





    void Start()
    {

        inputController = GameManager.Instance.inputController;
        playerController = GetComponent<PlayerController>();
        playerController.onAim += OnAim;



        currentWeaponSlotIndex = -1; //一開始設定成沒有拿武器
        AddWeapon(AllWeaponsList[0]); //加入武器 machinegun (一開始就得到 machinegun)
        /* //讓玩家得到所有武器 
        foreach (Weapon weapon in AllWeaponsList) 
        {
            AddWeapon(weapon);
        }
          */




        //初始化武器 UI，變小變灰所有武器的 UI
        machinegunUI[0].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        flamethrowerUI[0].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        machinegunUI[0].color = new Color(170f / 255f, 170f / 255f, 170f / 255f);
        machinegunUI[1].color = new Color(170f / 255f, 170f / 255f, 170f / 255f);
        machinegunUI[2].color = new Color(170f / 255f, 170f / 255f, 170f / 255f);
        flamethrowerUI[0].color = new Color(170f / 255f, 170f / 255f, 170f / 255f);
        flamethrowerUI[1].color = new Color(170f / 255f, 170f / 255f, 170f / 255f);
        flamethrowerUI[2].color = new Color(170f / 255f, 170f / 255f, 170f / 255f);

    }


    void Update()
    {
        hasSwitchWeaponInput(); //判斷有沒有按下切換武器鍵要切換武器

        //處理射擊
        if (currentWeaponSlotIndex != -1 && isAim && !toAim) //如果目前有選擇武器，且為瞄準狀態
        {
            PlayerWeaponSlot[currentWeaponSlotIndex].HandleShootInput(inputController.GetMouseLeftKeyDown(), inputController.GetMouseLeftKeyHeldDown(), inputController.GetMouseLeftKeyUp()); //處理射擊
        }
 

       
        //不停更新所有武器的 UI 的彈藥量條
        foreach (Weapon weapon in PlayerWeaponSlot)
        {
            if (weapon != null && weapon.weaponType == WeaponType.machinegun)
            {
                machinegunUI[1].fillAmount = Mathf.Lerp(machinegunUI[1].fillAmount, weapon.CurrentAmmoRatio_machinegun(), 0.05f);
            }
            else if (weapon != null && weapon.weaponType == WeaponType.flamethrower)
            {
                flamethrowerUI[1].fillAmount = Mathf.Lerp(flamethrowerUI[1].fillAmount, weapon.CurrentAmmoRatio_flamethrower(), 0.3f);
            }
        }
    }







    private void AddWeapon(Weapon weapon) //將武器加入玩家的武器槽中
    {
        if (weapon == null)
            return;

        if (HasWeapon(weapon)) //如果已經有 weapon 這把武器
        {
            print("已經有相同的武器，無法再加入一次");
            return;
        }
        for (int i = 0; i < PlayerWeaponSlot.Length; i++)
        {
            if (PlayerWeaponSlot[i] == null)
            {
                Weapon weaponInstance = Instantiate(weapon, equipPosition); //實例化一個 Weapon 腳本 
                weaponInstance.sourcePrefab = weapon.gameObject; //為這個 Weapon 腳本的 sourcePrefab 指定初始值         
                PlayerWeaponSlot[i] = weaponInstance; //---------------------------------------------------------------------所以 AllWeaponsList 中的 Weapon 還不是實體，而 PlayerWeaponSlot[] 中的 Weapon 是實體嗎???
                PlayerWeaponSlot[i].HiddenWeapon(); //先不顯示

                //顯示加入的武器的UI
                if (weapon == AllWeaponsList[0]) //武器是 machinegun
                {
                    machinegunUI[2].enabled = false;

                }
                else if (weapon == AllWeaponsList[1])  //武器是 flamethrower
                {
                    flamethrowerUI[2].enabled = false;
                }

                //print("成功加入武器 " + weaponInstance);
                return;
            }
        }
        print("武器已經滿了，無法再加入武器");
        return;
    }

    private bool HasWeapon(Weapon weapon) //檢查是否已經有 weapon 這把武器
    {
        foreach (Weapon PWS_weapon in PlayerWeaponSlot)
        {
            if (PWS_weapon != null && PWS_weapon.weaponType == weapon.weaponType)
            {
                return true;
            }
        }
        return false;
    }

    private bool HasWeaponAtIndex(Weapon weaponPrefab) //檢查在這個陣列索引值上是否有武器
    {
        foreach (Weapon weapon in PlayerWeaponSlot)
        {
            if (weapon != null && weapon.sourcePrefab == weaponPrefab)
            {
                return true;
            }
        }
        return false;
    }


    private void hasSwitchWeaponInput() //判斷有沒有按下切換武器鍵要切換武器
    {
        if (inputController.GetKeyCInput())
        {
            //往右移動 index
            currentWeaponSlotIndex += 1;
            if (currentWeaponSlotIndex > PlayerWeaponSlot.Length - 1) 
                currentWeaponSlotIndex = -1;
            if (currentWeaponSlotIndex != -1 && PlayerWeaponSlot[currentWeaponSlotIndex] == null)
                currentWeaponSlotIndex = -1;

            //如果已經在瞄準動作 無法切成不拿槍的狀態 <--- 我這樣寫是預設了永遠都會有一把武器，可能不太好，以後要改再說
            if (currentWeaponSlotIndex == -1 && isAim)
                currentWeaponSlotIndex += 1;

            switchWeapon(currentWeaponSlotIndex); //切換成那把武器
        }
        else if (inputController.GetKeyVInput())
        {
            //往左移動 index
            currentWeaponSlotIndex -= 1;
            if (currentWeaponSlotIndex <  -1)
                currentWeaponSlotIndex = PlayerWeaponSlot.Length - 1;
            while (currentWeaponSlotIndex != -1 && PlayerWeaponSlot[currentWeaponSlotIndex] == null) //跳過空槽
            {
                currentWeaponSlotIndex -= 1;
            }

            //如果已經在瞄準動作 無法切成不拿槍的狀態 <--- 我這樣寫是預設了永遠都會有一把武器，可能不太好，以後要改再說
            if (currentWeaponSlotIndex == -1 && isAim)
                currentWeaponSlotIndex = PlayerWeaponSlot.Length - 1;
            while (currentWeaponSlotIndex != -1 && PlayerWeaponSlot[currentWeaponSlotIndex] == null) //跳過空槽
            {
                currentWeaponSlotIndex -= 1;
            }

            switchWeapon(currentWeaponSlotIndex); //切換成那把武器
        }
    }

    private void switchWeapon(int index) //切換武器，其實只是在做武器的隱藏/顯示而已
    {
        if (index >= -1 && index < PlayerWeaponSlot.Length)
        {
            //先隱藏所有武器
            foreach (Weapon weapon in PlayerWeaponSlot)
            {
                if (weapon != null)
                {
                    weapon.HiddenWeapon();
                }
            }
            //變小變灰所有武器的 UI
            machinegunUI[0].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            flamethrowerUI[0].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            machinegunUI[0].color = new Color(170f / 255f, 170f / 255f, 170f / 255f);
            machinegunUI[1].color = new Color(170f / 255f, 170f / 255f, 170f / 255f);
            machinegunUI[2].color = new Color(170f / 255f, 170f / 255f, 170f / 255f);
            flamethrowerUI[0].color = new Color(170f / 255f, 170f / 255f, 170f / 255f);
            flamethrowerUI[1].color = new Color(170f / 255f, 170f / 255f, 170f / 255f);
            flamethrowerUI[2].color = new Color(170f / 255f, 170f / 255f, 170f / 255f);


            //只顯示那把武器
            if (index != -1)
            {
                //print("切換成武器 " + PlayerWeaponSlot[index]);
                PlayerWeaponSlot[index].ShowWeapon();

                //顯示武器的 UI
                if (PlayerWeaponSlot[index].weaponType == WeaponType.machinegun)
                {
                    machinegunUI[0].transform.localScale = new Vector3(1f, 1f, 1f);
                    machinegunUI[1].color = Color.white;
                }
                else if(PlayerWeaponSlot[index].weaponType == WeaponType.flamethrower)
                {
                    flamethrowerUI[0].transform.localScale = new Vector3(1f, 1f, 1f);
                    flamethrowerUI[1].color = Color.white;
                }
            }
            else
            {
                //print("不拿武器 " );
            }
        }
    }



    private void  OnAim(bool value)
    {
        if(isAim == false && value == true) //如果剛剛不是瞄準動作，突然變成瞄準動作
        {
            toAim = true;
        }
        else
        {
            toAim = false;
        }

        //如果剛剛沒拿槍，突然變成瞄準動作 -> 統一變成拿第一把槍
        if (currentWeaponSlotIndex == -1)
        {
            currentWeaponSlotIndex = 0;
            switchWeapon(currentWeaponSlotIndex);
        }

        isAim = value;
    }


    public void PickUpWeapon(GameObject gameObject)
    {     
        if (gameObject != null  )
        {
            PickUpItem pickUpItem = gameObject.GetComponent<PickUpItem>();
            if(pickUpItem != null )
            {
                if(pickUpItem.weaponType == WeaponType.flamethrower)
                {
                    AddWeapon(AllWeaponsList[1]); //撿到火焰槍 -> 加入武器 flamethrower
                }
            }
        }
    }


}
