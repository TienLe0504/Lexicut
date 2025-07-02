using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeScreenController : MonoBehaviour
{
    public HomeScreen homeScreen;

    private void OnEnable()
    {

        var allControllers = FindObjectsOfType<HomeScreenController>();
        this.Register(EventID.BuyItem, ReieveGold);
    }
    private void OnDisable()
    {
        this.Unregister(EventID.BuyItem, ReieveGold);
    }

    public void SaveGold(int value)
    {
        ManagerGame.Instance.gold += value;
        ResourceManager.Instance.SaveToFile<int>(CONST.KEY_FILENAME_STORE, CONST.KEY_GOLD, ManagerGame.Instance.gold);

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
        if (data is int value)
        {
            SaveGold(-value);
        }
    }

    public void PlayGame()
    {
        SoundManager.Instance.PressButton();
        UIManager.Instance.ShowPopup<GamePicker>();
    }
    public void OpenInventory()
    {
        SoundManager.Instance.PressButton();
        UIManager.Instance.ShowPopup<PopupInventory>();

    }

    public void OpenShop()
    {
        SoundManager.Instance.PressButton();
        UIManager.Instance.ShowPopup<PopupShop>();
    }
    public void ShowRank()
    {
        SoundManager.Instance.PressButton();
        UIManager.Instance.ShowPopup<PopUpRank>();

    }
}
