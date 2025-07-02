using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OverlapBuyItem : BaseOverlap
{
    // === UI References ===
    public Image imgItem;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemPrice;
    public TextMeshProUGUI txtNotEnoughMoney;
    public Button btnBuy;
    public Button btnClose;

    // === Controller Reference ===
    public OverlapBuyItemController controller;

    // === Tween Animations ===
    private Tween openTween;
    private Tween closeTween;
    public Tween buyItem;
    public Tween fadeItem;



    public override void Hide()
    {
        this.Broadcast(EventID.DeactivateBackgroundShop);
        base.Hide();
    }

    public override void Init()
    {
        base.Init();
        btnClose.onClick.AddListener(() => CloseOverlap());
        btnBuy.onClick.AddListener(() => Buy());
    }

    public override void Show(object data)
    {

        this.gameObject.SetActive(true);
        base.Show(data);
        txtNotEnoughMoney.gameObject.SetActive(false);
        SetupData(data);
        OpenOverlap();
    }
    public void ReciveInventory(object data)
    {
        SetupData(data);
    }
    public void SetupData(object data)
    {
        if(data is ItemShopController inventoryController)
        {
            Inventory inventory = inventoryController.inventory;
            controller.SetupData(inventory, inventoryController);
            imgItem.sprite = inventory.image;
            itemName.text = inventory.name;
            itemPrice.text = inventory.gold.ToString();
        }
    }
    public void Buy()
    {
       bool isBuy = controller.BuyItem();
        if (isBuy)
        {
            CloseOverlap();
        }
        else
        {
            ShowNotEnoughMoney();
        }
    }
    public void ShowNotEnoughMoney()
    {
        if (buyItem != null && buyItem.IsActive()) buyItem.Kill();
        if (fadeItem != null && fadeItem.IsActive()) fadeItem.Kill();
        txtNotEnoughMoney.gameObject.SetActive(true);
        txtNotEnoughMoney.text = "Not enough money";
        RectTransform rectTransform = txtNotEnoughMoney.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, 0);
        Color color = txtNotEnoughMoney.color;
        color.a = 1;
        txtNotEnoughMoney.color = color;
        buyItem = txtNotEnoughMoney.transform.DOLocalMoveY(200f, 1.3f).SetRelative(false).SetEase(Ease.OutQuad);
        fadeItem = txtNotEnoughMoney.DOFade(0f, 1.3f);

    }
    public void OpenOverlap()
    {
        RectTransform rectTransform = this.GetComponent<RectTransform>();

        if (openTween != null && openTween.IsActive()) openTween.Kill();
        if (closeTween != null && closeTween.IsActive()) closeTween.Kill();

        rectTransform.localScale = Vector3.zero;
        openTween = rectTransform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
    }

    public void CloseOverlap(object data = null)
    {
        controller.PressButton();
        RectTransform rectTransform = this.GetComponent<RectTransform>();

        if (openTween != null && openTween.IsActive()) openTween.Kill();
        if (closeTween != null && closeTween.IsActive()) closeTween.Kill();

        rectTransform.localScale = Vector3.one;
        closeTween = rectTransform.DOScale(0f, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            this.Broadcast(EventID.DeactivateBackgroundShop);
            base.Hide();
        });
    }
}
