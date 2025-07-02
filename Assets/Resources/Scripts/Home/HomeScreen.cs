using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeScreen : BaseScreen
{
    public HomeScreenController controller;
    public TextMeshProUGUI gold;
    public Image bgImage;
    public Button btnPlay;
    public Button btnShop;
    public Button btnInventory;
    public Button btnRank;
    private Tween goldTween;
    public override void Init()
    {
        base.Init();
        bgImage.rectTransform.sizeDelta = new Vector2(ManagerGame.Instance.WidthScreen, ManagerGame.Instance.HeightScreen);
        btnPlay.onClick.AddListener(() => OpenChooseCategory());
        btnShop.onClick.AddListener(() => OpenShop());
        btnInventory.onClick.AddListener(() => OpenInventory());
        btnRank.onClick.AddListener(() => ShowRank());
        ShowGold(false);
    }
    private void OnEnable()
    {
        this.Register(EventID.SetStoreActive, ShowShop);
        this.Register(EventID.Back, ShowBtn);
        this.Register(EventID.ShowInventoryButton, ShowBtnInventory);
        this.Register(EventID.ShowRankButton, ShowBtnRank);
    }
    private void OnDisable()
    {
        this.Unregister(EventID.SetStoreActive, ShowShop);
        this.Unregister(EventID.Back, ShowBtn);
        this.Unregister(EventID.ShowInventoryButton, ShowBtnInventory);
        this.Unregister(EventID.ShowRankButton, ShowBtnRank);

    }
    public override void Show(object data)
    {
        base.Show(data);
        if(data is int value)
        {
            if (value != 0)
            {
                controller.SaveGold(value);

            }
        }
    }
    public override void Hide()
    {
        base.Hide();
        ShowBtn();
    }
    public void OpenChooseCategory()
    {
        controller.PlayGame();
        btnPlay.gameObject.SetActive(false);
    }
    public void OpenShop()
    {
        btnShop.gameObject.SetActive(false);
        controller.OpenShop();
    }
    public void ShowShop(object data = null)
    {
        btnShop.gameObject.SetActive(true);

    }
    public void OpenInventory()
    {
        btnInventory.gameObject.SetActive(false);
        controller.OpenInventory();
    }
    public void ShowRank()
    {
        btnRank.gameObject.SetActive(false);
        controller.ShowRank();
    }
    public void ShowBtnRank(object data = null)
    {
        btnRank.gameObject.SetActive(true);
    }
    public void ShowBtnInventory(object data = null)
    {
        btnInventory.gameObject.SetActive(true);
    }
    public void ShowBtn(object data = null)
    {
        btnPlay.gameObject.SetActive(true);
    }
    public void ShowGold(bool isEffect = false)
    {
        if (!isEffect)
        {
            gold.text = ManagerGame.Instance.gold.ToString();
            return;
        }
        int currentGold = int.Parse(gold.text);  
        int targetGold = ManagerGame.Instance.gold;  

        if (goldTween != null && goldTween.IsActive())
        {
            goldTween.Kill();
        }
        goldTween = DOVirtual.Int(currentGold, targetGold, 0.5f, value => {
            gold.text = value.ToString();
        });
    }
    
}
