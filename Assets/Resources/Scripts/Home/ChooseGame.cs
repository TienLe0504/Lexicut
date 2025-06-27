using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public enum ColorBtn
{
    Green,
    Orange,
    Violet,
    Blue
}
public class ChooseGame : BaseScreen
{
    public Image Choosegame;
    public float heightBGFronHide = 200f;
    public Image BGFrontHide;
    public Image BGFront;
    public Image content;
    public Image imgWordChain;
    public Image CutFruits;
    public GameObject parentBtn;
    public Button btnWordChain;
    public List<Button> buttons = new List<Button>();
    public Button btnBack;
    public override void Hide()
    {
        base.Hide();
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(false);
        }
    }

    public override void Init()
    {
        base.Init();
        Setup();
        ShowButton();
    }
    public void Setup()
    {
        Choosegame.rectTransform.sizeDelta = new Vector2(ManagerGame.Instance.WidthScreen, ManagerGame.Instance.HeightScreen);
        BGFrontHide.rectTransform.sizeDelta = new Vector2(ManagerGame.Instance.WidthScreen, heightBGFronHide);
        BGFront.rectTransform.sizeDelta = new Vector2(ManagerGame.Instance.WidthScreen * 0.95f, heightBGFronHide);
        content.rectTransform.sizeDelta = new Vector2(ManagerGame.Instance.WidthScreen, ManagerGame.Instance.HeightScreen - heightBGFronHide);
        content.rectTransform.position = new Vector3(content.rectTransform.position.x, content.rectTransform.position.y, 0);
        GridLayoutGroup gridContent = content.GetComponent<GridLayoutGroup>();
        gridContent.cellSize = new Vector2(ManagerGame.Instance.WidthScreen, gridContent.cellSize.y);
        imgWordChain.rectTransform.sizeDelta = new Vector2(ManagerGame.Instance.WidthScreen * 0.97f, imgWordChain.rectTransform.sizeDelta.y);
        CutFruits.rectTransform.sizeDelta = new Vector2(ManagerGame.Instance.WidthScreen * 0.97f, CutFruits.rectTransform.sizeDelta.y);
        btnBack.onClick.AddListener(() => OnClickBack());
    }
    public void OnClickBack()
    {
        UIManager.Instance.ShowScreen<HomeScreen>();
    }
    public override void Show(object data)
    {
        base.Show(data);
        ShowAgain();
    }
    public void ShowButton()
    {
        List<string> categories = ManagerGame.Instance.Categories;

        for (int i = 0; i < categories.Count; i++)
        {
            Button newButton = Instantiate(btnWordChain, parentBtn.transform);
            TextMeshProUGUI tmp = newButton.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = categories[i];
            Color color = GetColor(i);
            newButton.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 220f / 255f);

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
    private void OnClickCategory(string categoryName)
    {
        Debug.Log("Category selected: " + categoryName);
        // Handle the category selection logic here
        //Hide();
    }
    public Color GetColor(int index)
    {
        ColorBtn colorBtn = (ColorBtn)index;
        Color color = Color.white;

        switch (colorBtn)
        {
            case ColorBtn.Green:
                color = Color.green;
                break;
            case ColorBtn.Orange:
                color = new Color(1f, 0.5f, 0f); 
                break;
            case ColorBtn.Violet:
                color = new Color(0.54f, 0.17f, 0.89f);
                break;
            case ColorBtn.Blue:
                color = Color.blue;
                break;
            default:
                color = Color.white;
                break;
        }

        return color;
    }
}
