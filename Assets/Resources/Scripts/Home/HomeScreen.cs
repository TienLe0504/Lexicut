using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeScreen : BaseScreen
{
    public HomeScreenController controller;
    public Image bgImage;
    public Button btnPlay;
    public TextMeshProUGUI gold;
    public Button btnShop;
    public Button btnInventory;
    private Tween goldTween;
    public override void Init()
    {
        base.Init();
        //controller.SetupGold();
        bgImage.rectTransform.sizeDelta = new Vector2(ManagerGame.Instance.WidthScreen, ManagerGame.Instance.HeightScreen);
        
        btnPlay.onClick.AddListener(() => OpenChooseCategory());
        btnShop.onClick.AddListener(() => OpenShop());
        btnInventory.onClick.AddListener(() => OpenInventory());
        ShowGold(false);
    }
    private void OnEnable()
    {
        this.Register(EventID.SetActiveStore, ShowShop);
        this.Register(EventID.Back, ShowBtn);
        this.Register(EventID.ShowBtnInventory, ShowBtnInventory);

    }
    private void OnDisable()
    {
        this.Unregister(EventID.SetActiveStore, ShowShop);
        this.Unregister(EventID.Back, ShowBtn);
        this.Unregister(EventID.ShowBtnInventory, ShowBtnInventory);

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
            Debug.Log("-------------- gold ----------"+value);
        }
        // Add any HomeScreen-specific show logic here
    }
    public override void Hide()
    {
        base.Hide();
        ShowBtn();
        // Add any HomeScreen-specific hide logic here
    }
    public void OpenChooseCategory()
    {
        //UIManager.Instance.ShowPopup<PopupChooseType>();
        UIManager.Instance.ShowPopup<GamePicker>();
        btnPlay.gameObject.SetActive(false);
    }
    public void OpenShop()
    {
        btnShop.gameObject.SetActive(false);
        UIManager.Instance.ShowPopup<PopupShop>();
    }
    public void ShowShop(object data = null)
    {
        btnShop.gameObject.SetActive(true);

    }
    public void OpenInventory()
    {
        btnInventory.gameObject.SetActive(false);
        UIManager.Instance.ShowPopup<PopupInventory>();
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
