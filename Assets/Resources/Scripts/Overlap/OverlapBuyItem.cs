using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OverlapBuyItem : BaseOverlap
{
    public Image imgItem;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemPrice;
    public Button btnBuy;
    public Button btnClose;
    public TextMeshProUGUI txtNotEnoughMoney;
    private Tween openTween;
    private Tween closeTween;
    public OverlapBuyItemController controller;


    public override void Hide()
    {
        this.Broadcast(EventID.InActiveBGShop);
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
        txtNotEnoughMoney.text = "Not enough money";
        RectTransform rectTransform = txtNotEnoughMoney.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 0);
        Color color = txtNotEnoughMoney.color;
        color.a = 1;
        txtNotEnoughMoney.color = color;
        txtNotEnoughMoney.transform.DOLocalMoveY(200f, 0.4f).SetRelative(false).SetEase(Ease.OutQuad);
        txtNotEnoughMoney.DOFade(0f, 0.4f);

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
        RectTransform rectTransform = this.GetComponent<RectTransform>();

        if (openTween != null && openTween.IsActive()) openTween.Kill();
        if (closeTween != null && closeTween.IsActive()) closeTween.Kill();

        rectTransform.localScale = Vector3.one;
        closeTween = rectTransform.DOScale(0f, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            this.Broadcast(EventID.InActiveBGShop);
            base.Hide();
        });
    }
}
