using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeScreenController : MonoBehaviour
{
    public HomeScreen homeScreen;
    //public int gold;
    private void Awake()
    {
        //this.Register(EventID.BuyItem, ReieveGold);
    }
    private void OnEnable()
    {
        Debug.Log($"HomeScreenController on {gameObject.name} OnEnable");

        // Tìm tất cả HomeScreenController trong scene
        var allControllers = FindObjectsOfType<HomeScreenController>();
        Debug.Log($"Total HomeScreenController instances: {allControllers.Length}");
        foreach (var controller in allControllers)
        {
            Debug.Log($"  - Controller on: {controller.gameObject.name}, enabled: {controller.enabled}");
        }

        this.Register(EventID.BuyItem, ReieveGold);
    }
    private void OnDisable()
    {
        this.Unregister(EventID.BuyItem, ReieveGold);
    }
    private void Start()
    {
    }

    public void SaveGold(int value)
    {
        //gold+= value;
        ManagerGame.Instance.gold +=value;
        //ResourceManager.Instance.SaveJson<int>(CONST.PATH_STORE_ABSOLUTE, CONST.KEY_GOLD,gold);
        ResourceManager.Instance.SaveJson<int>(CONST.KEY_FILENAME_STORE, CONST.KEY_GOLD, ManagerGame.Instance.gold);

        if (homeScreen != null)
        {
            homeScreen.ShowGold(true);
        }
        else
        {
            Debug.LogWarning("HomeScreen reference is null in HomeScreenController.SaveGold()");
        }
    }
    public void ReieveGold(object data)
    {
        if(data is int value)
        {
            SaveGold(-value);
        }
    }

}
