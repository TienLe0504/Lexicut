using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PopupInventory : BasePopup
{
    public Button btnClose;
    public GameObject tranformContent;
    public GameObject inventoryPrefab;
    public ScrollRect scrollRect;
    public Tween openTween;
    public Tween closeTween;
    public PopupInventoryController controller;
    //private void Awake()
    //{
    //    controller = GetComponent<PopupInventoryController>();
    //    if (controller == null)
    //    {
    //        controller = gameObject.AddComponent<PopupInventoryController>();
    //    }
    //    controller.popupInventory = this;
    //}
    public override void Hide()
    {
        base.Hide();
    }

    public override void Init()
    {
        base.Init();
        btnClose.onClick.AddListener(()=>CloseShopTranform());
    }

    public override void Show(object data)
    {
        base.Show(data);
        this.Broadcast(EventID.Back);
        this.Broadcast(EventID.SetActiveStore);
        ShowInventory(ManagerGame.Instance.InventoryData, tranformContent);
        StartCoroutine(ResetScrollPosition());
        OpenShopTranform();
    }
    public void ShowInventory(List<Inventory> inventoryData, GameObject parrentTranform)
    {
        ItemShop[] itemChilds = parrentTranform.GetComponentsInChildren<ItemShop>();
        int currentCount = itemChilds.Length;
        int newCount = inventoryData.Count;
        for (int i = currentCount; i < newCount; i++)
        {
            GameObject item = Instantiate(inventoryPrefab, parrentTranform.transform);
            item.GetComponent<ItemShopController>().SetupData(inventoryData[i], true);
            controller.AddItem(item);
        }
        AdjustContentHeight(newCount);
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
    private void AdjustContentHeight(int itemCount)
    {
        RectTransform contentRect = tranformContent.GetComponent<RectTransform>();

        GridLayoutGroup gridLayout = tranformContent.GetComponent<GridLayoutGroup>();
        if (gridLayout != null)
        {
            float itemHeight = gridLayout.cellSize.y;
            float spacing = gridLayout.spacing.y;
            int constraintCount = gridLayout.constraintCount > 0 ? gridLayout.constraintCount : 1;
            int rowCount = Mathf.CeilToInt((float)itemCount / constraintCount);

            float totalHeight = rowCount * itemHeight + (rowCount - 1) * spacing + gridLayout.padding.top + gridLayout.padding.bottom;

            Vector2 size = contentRect.sizeDelta;
            size.y = totalHeight;
            contentRect.sizeDelta = size;
        }
        else
        {
            ItemShop itemShop = tranformContent.GetComponentInChildren<ItemShop>();
            if (itemShop != null)
            {
                RectTransform itemRect = itemShop.GetComponent<RectTransform>();
                float itemHeight = itemRect.rect.height;

                float spacing = 0f; 
                VerticalLayoutGroup layoutGroup = tranformContent.GetComponent<VerticalLayoutGroup>();
                if (layoutGroup != null)
                {
                    spacing = layoutGroup.spacing;
                }

                float totalHeight = itemCount * itemHeight + (itemCount - 1) * spacing;

                Vector2 size = contentRect.sizeDelta;
                size.y = totalHeight;
                contentRect.sizeDelta = size;
            }
        }
    }

    public void CloseShopTranform()
    {
        RectTransform rectTransform = this.GetComponent<RectTransform>();
        if (openTween != null && openTween.IsActive()) openTween.Kill();
        if (closeTween != null && closeTween.IsActive()) closeTween.Kill();

        rectTransform.localScale = Vector3.one;
        closeTween = rectTransform.DOScale(0f, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            this.Broadcast(EventID.ShowBtnInventory);
            this.Broadcast(EventID.InActiveOverlapUseItem);
            Hide();
        });
    }
}
