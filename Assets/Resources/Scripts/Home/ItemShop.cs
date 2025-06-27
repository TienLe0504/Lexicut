using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemShop : MonoBehaviour
{
    public TextMeshProUGUI gold;
    public Image img;
    public GameObject imgShadow;
    public ItemShopController controller;
    public Button btnBuy;


    private void Awake()
    {
        btnBuy.onClick.AddListener(() => BuyItem());
    }
    public void SetUp(string gold, Sprite sprite, bool isActiveGold)
    {
        if (!isActiveGold)
        {
            this.gold.text = gold;
        }
        this.img.sprite = sprite;   
    }
    public void SetUpShadow(bool isActive)
    {
        imgShadow.SetActive(isActive);
    }
    public void BuyItem()
    {
        controller.PressButton();
    }
}
