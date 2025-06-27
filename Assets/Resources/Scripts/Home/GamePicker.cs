using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GamePicker : BasePopup
{
    public GameObject parent;
    public List<Button> buttons = new List<Button>();
    public Button btnWordChain;
    public Button btnPlay;
    public Button btnBack;
    public GameObject currentObject;
    public override void Hide()
    {
        base.Hide();
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(false);
        }
        btnPlay.gameObject.SetActive(false);
    }

    public override void Init()
    {
        base.Init();
        HideBtn();
        AddClick();
        ShowButton();
        CreateEffectBtnPlay();
        SetSize();
    }
    public void SetSize()
    {
        this.GetComponent<RectTransform>().sizeDelta =new Vector2(ManagerGame.Instance.WidthScreen, ManagerGame.Instance.HeightScreen);
    }
    public void ShowBtnPlayGame()
    {
        this.Broadcast(EventID.Back);

        Hide();
    }
    public void HideBtn()
    {
        btnBack.onClick.AddListener(()=>ShowBtnPlayGame());
    }
    public override void Show(object data)
    {
        base.Show(data);
        this.Broadcast(EventID.ShowBtnInventory);
        this.Broadcast(EventID.SetActiveStore);
        ShowAgain();
        CreateEffectBtnPlay();
    }
    public void ShowButton()
    {
        List<string> categories = ManagerGame.Instance.Categories;

        for (int i = 0; i < categories.Count; i++)
        {
            Button newButton = Instantiate(btnWordChain, parent.transform);
            TextMeshProUGUI tmp = newButton.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = categories[i];
            newButton.onClick.AddListener(() => OnClickCategory(tmp.text));
            buttons.Add(newButton);
            CreateEffect(i, newButton);
        }
    }
    public void ShowAgain()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].gameObject.SetActive(true);
            CreateEffect(i, buttons[i]);
        }
    }

    private void CreateEffect(int i, Button newButton)
    {
        newButton.transform.localScale = Vector3.zero;

        newButton.transform.DOScale(Vector3.one, 0.3f)
            .SetEase(Ease.OutBack)
            .SetDelay(i * 0.05f);
    }
    private void CreateEffectBtnPlay()
    {
        btnPlay.gameObject.SetActive(true);
        btnPlay.transform.localScale = Vector3.zero;

        btnPlay.transform.DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.OutBack);
    }
    public void AddClick()
    {
        btnPlay.onClick.AddListener(() => PlayGameCutFruits());
    }
    private void PlayGameCutFruits()
    {
        UIManager.Instance.ShowScreen<CutFruits>();
        Hide();
    }


    private void OnClickCategory(string categoryName)
    {
        Debug.Log("Category selected: " + categoryName);
        UIManager.Instance.ShowScreen<WordChainGame>(categoryName);
        Hide();
    }

}
