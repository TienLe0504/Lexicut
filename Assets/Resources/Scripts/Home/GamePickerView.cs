using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePickerView : MonoBehaviour
{
    public GamePicker gamePicker;
    public GameObject parent;
    public List<Button> buttons = new List<Button>();
    public Button btnWordChain;
    public Button btnPlay;
    public Button btnBack;
    public GameObject currentObject;

    public void ActivewOffButton()
    {
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(false);
        }
        btnPlay.gameObject.SetActive(false);
    }
    public void Init()
    {
        SetupBackButtonListener();
        AddClick();
        CreateCategoryButton();
        AnimatePlayButtonAppear();
        SetSize();
    }

    public void SetupBackButtonListener()
    {
        btnBack.onClick.AddListener(() => ShowBtnPlayGame());
    }
    public void SetSize()
    {
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(ManagerGame.Instance.WidthScreen, ManagerGame.Instance.HeightScreen);
    }
    public void ShowBtnPlayGame()
    {
        gamePicker.ButtonBack();
    }

    public  void Show()
    {

        AnimateButtons();
        AnimatePlayButtonAppear();
    }
    public void CreateCategoryButton()
    {
        List<string> categories = ManagerGame.Instance.Categories;

        for (int i = 0; i < categories.Count; i++)
        {
            CreateCategoryButton(categories, i);
        }
    }

    private void CreateCategoryButton(List<string> categories, int i)
    {
        Button newButton = Instantiate(btnWordChain, parent.transform);
        TextMeshProUGUI tmp = newButton.GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = categories[i];
        newButton.onClick.AddListener(() => gamePicker.OnClickCategory(tmp.text));
        buttons.Add(newButton);
        AnimateButtonAppear(i, newButton);
    }

    public void AnimateButtons()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].gameObject.SetActive(true);
            AnimateButtonAppear(i, buttons[i]);
        }
    }

    private void AnimateButtonAppear(int i, Button newButton)
    {
        newButton.transform.localScale = Vector3.zero;

        newButton.transform.DOScale(Vector3.one, 0.3f)
            .SetEase(Ease.OutBack)
            .SetDelay(i * 0.05f);
    }
    private void AnimatePlayButtonAppear()
    {
        btnPlay.gameObject.SetActive(true);
        btnPlay.transform.localScale = Vector3.zero;

        btnPlay.transform.DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.OutBack);
    }
    public void AddClick()
    {
        btnPlay.onClick.AddListener(() => gamePicker.PlayGameCutFruits());
    }

}
