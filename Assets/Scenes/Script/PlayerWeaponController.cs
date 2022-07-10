using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//1是1號槍 machinegun，2是2號槍 flamethrower


public class PlayerWeaponController : MonoBehaviour
{
    /*//---------------------------------------------------------------------------
    有個搞不太清楚的地方???
    把 Weapon 腳本掛到 GameObject 上後，竟然可以把這個 GameObject 指定給本腳本的 List<Weapon>， List 裡面的類不是應該是要 GameObject 才對嗎?
    推測是在 Unity 中可以把掛有該腳本的 GameObject 直接指定給該腳本的 class 變數，而得到的依然是這個 class 組件而不是丟給他的 GameObject
    *///---------------------------------------------------------------------------
    [SerializeField] List<Weapon> AllWeaponsList = new List<Weapon>(); //遊戲中所有種類的武器，在 Unity 中就把所有武器的 prefab 都先丟進來給他 (只要把 Weapon 腳本掛在武器模型的預製物件下，就可以直接先丟進去)

    Weapon[] PlayerWeaponSlot = new Weapon[3]; //Player 的武器槽，Player 最多只能得到三個武器
    int currentWeaponSlotIndex; //目前在武器槽中選擇的武器的索引值，-1 代表目前沒有選擇任何槍
    [SerializeField] Transform equipPosition; //裝備武器的位置 (從 Unity 中指定給他)
    bool isAim; //使否處於瞄準狀態

    PlayerController playerController;
    InputController inputController;

 


    void Start()
    {
        currentWeaponSlotIndex = -1; //一開始設定成沒有拿武器

        inputController = GameManager.Instance.inputController;
        playerController = GetComponent<PlayerController>();
        playerController.onAim += OnAim;

        //    playerController.onAim += OnAim; //讓 playerController 腳本能與此腳本的 isAim 旗標同步
        //playerController的omAim呢??????????

        foreach (Weapon weapon in AllWeaponsList) //這邊先讓玩家在一開始就得到所有武器 
        {
            AddWeapon(weapon);
        }


     }


    void Update()
    {
        hasSwitchWeaponInput(); //判斷有沒有按下切換武器鍵要切換武器

        //處理射擊
        if (currentWeaponSlotIndex != -1 && isAim) //如果目前有選擇武器，且為瞄準狀態
        {
            PlayerWeaponSlot[currentWeaponSlotIndex].HandleShootInput(inputController.GetMouseLeftKeyDown(), inputController.GetMouseLeftKeyHeldDown(), inputController.GetMouseLeftKeyUp()); //處理射擊
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
                //print("成功加入武器 " + weaponInstance);
                return;
            }
        }
        print("武器已經滿了，無法再加入武器");
        return;
    }

    private bool HasWeapon(Weapon weaponPrefab) //檢查是否已經有 weaponPrefab 這把武器
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
            //只顯示那把武器
            if (index != -1)
            {
                //print("切換成武器 " + PlayerWeaponSlot[index]);
                PlayerWeaponSlot[index].ShowWeapon();
            }
            else
            {
                //print("不拿武器 " );
            }
        }
    }



    private void  OnAim(bool value)
    {
        //如果剛剛沒拿槍，突然變成瞄準動作 -> 統一變成拿第一把槍
        if(currentWeaponSlotIndex == -1)
        {
            currentWeaponSlotIndex = 0;
            switchWeapon(currentWeaponSlotIndex);
        }

        isAim = value;
    }





}
