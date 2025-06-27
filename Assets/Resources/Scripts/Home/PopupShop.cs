using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PopupShop : BasePopup
{
    public Button btnClose;
    public GameObject content;
    public GameObject parrentTrail;
    public GameObject parrentEffect;
    public GameObject parrentColor;
    public GameObject itemShopPrefab;
    public Image imgShadow;
    public Tween openTween;
    public Tween closeTween;
    public ScrollRect scrollRect;
    public override void Hide()
    {
        base.Hide();
    }

    public override void Init()
    {

        base.Init();
        btnClose.onClick.AddListener(() => CloseShopTranform());
        SetInActiveBG();
        this.Broadcast(EventID.Back);
        this.Broadcast(EventID.ShowBtnInventory);
        //this.Register(EventID.InActiveBGShop, SetInActiveBG);
        //this.Register(EventID.ActiveBGShop, SetActiveBG);
        CreateShop(ManagerGame.Instance.shopColor, parrentColor);
        CreateShop(ManagerGame.Instance.shopEffect, parrentEffect);
        CreateShop(ManagerGame.Instance.shopTrail, parrentTrail);
    }
    public void OnEnable()
    {
        this.Register(EventID.InActiveBGShop, SetInActiveBG);
        this.Register(EventID.ActiveBGShop, SetActiveBG);
    }
    public void OnDisable()
    {
        this.Unregister(EventID.ActiveBGShop, SetActiveBG);
        this.Unregister(EventID.InActiveBGShop, SetInActiveBG);
    }
    public override void Show(object data)
    {
        base.Show(data);

        StartCoroutine(ResetScrollPosition());

        OpenShopTranform();
    }
    public void CreateShop(InventoryData inventoryData, GameObject parrentTranform)
    {
        for(int i = 0; i < inventoryData.inventoryList.Count; i++)
        {
            GameObject item = Instantiate(itemShopPrefab, parrentTranform.transform);
            item.GetComponent<ItemShopController>().SetupData(inventoryData.inventoryList[i]);
        }
    }
    public void SetActiveBG(object data = null)
    {
        imgShadow.gameObject.SetActive(true);   

    }
    public void SetInActiveBG(object data = null)
    {
        imgShadow.gameObject.SetActive(false);

    }
    public void OpenShopTranform()
    {
        RectTransform rectTransform = this.GetComponent<RectTransform>();

        if (openTween != null && openTween.IsActive()) openTween.Kill();
        if (closeTween != null && closeTween.IsActive()) closeTween.Kill();

        rectTransform.localScale = Vector3.zero;
        openTween = rectTransform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
    }
    private IEnumerator ResetScrollPosition()
    {
        yield return null;
        scrollRect.verticalNormalizedPosition = 1f;
    }

    public void CloseShopTranform()
    {
        RectTransform rectTransform = this.GetComponent<RectTransform>();
        if (openTween != null && openTween.IsActive()) openTween.Kill();
        if (closeTween != null && closeTween.IsActive()) closeTween.Kill();

        rectTransform.localScale = Vector3.one;
        closeTween = rectTransform.DOScale(0f, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            this.Broadcast(EventID.SetActiveStore);
            Hide();
        });
    }

}
