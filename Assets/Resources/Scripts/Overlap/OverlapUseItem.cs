using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class OverlapUseItem : BaseOverlap
{
    public OverlapUseItemController controller;
    public TextMeshProUGUI txtName;
    public Button btnClose;
    public Button btnUse;
    public Image imgitem;
    private Tween openTween;
    private Tween closeTween;
    public override void Hide()
    {
        base.Hide();
    }
    private void OnEnable()
    {
        this.Register(EventID.DeactivateOverlappingItem, CloseOverlap);
    }
    private void OnDisable()
    {
        this.Unregister(EventID.DeactivateOverlappingItem, CloseOverlap);
    }
    public override void Init()
    {
        base.Init();
        btnUse.onClick.AddListener(()=>OnClickUseItem());
        btnClose.onClick.AddListener(() => CloseOverlap(true));
    }

    public override void Show(object data)
    {
        base.Show(data);
        ShowData(data);
        OpenOverlap();
    }
    public void ShowData(object data)
    {
        if(data is ItemInventoryController itemInventoryController)
        {

            controller.SetUpInventory(itemInventoryController);
            imgitem.sprite = itemInventoryController.inventory.image;
            txtName.text = itemInventoryController.inventory.name;
            SetupText(itemInventoryController);
        }

    }

    public void SetupText(ItemInventoryController itemInventoryController)
    {
        if (itemInventoryController.inventory.isUsed)
        {
            btnUse.GetComponentInChildren<TextMeshProUGUI>().text = "Not Use";
        }
        else
        {
            btnUse.GetComponentInChildren<TextMeshProUGUI>().text = "Use";
        }
    }

    public void OnClickUseItem()
    {
        controller.OnClickUseItem();
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
        if (data is bool isPressButton) {
           if (isPressButton)
            {
                controller.PressButton();
            }
        }
        RectTransform rectTransform = this.GetComponent<RectTransform>();

        if (openTween != null && openTween.IsActive()) openTween.Kill();
        if (closeTween != null && closeTween.IsActive()) closeTween.Kill();

        rectTransform.localScale = Vector3.one;
        closeTween = rectTransform.DOScale(0f, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
           Hide();
        });
    }
}
