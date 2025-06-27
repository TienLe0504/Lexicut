using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupChooseType : BasePopup
{
    public Button btnChooseType;
    public GameObject parent;
    List<Button> buttons = new List<Button>();
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
        ShowButton();
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
            Button newButton = Instantiate(btnChooseType, parent.transform);
            TextMeshProUGUI tmp = newButton.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = categories[i];
            newButton.onClick.AddListener(() => OnClickCategory(tmp.text));
            buttons.Add(newButton);
            CreateEffect(i, newButton);
        }
    }
    public void ShowAgain()
    {
        for(int  i = 0; i < buttons.Count; i++)
        {
            buttons[i].gameObject.SetActive(true);
            CreateEffect(i, buttons[i]);
        }
    }

    private  void CreateEffect(int i, Button newButton)
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
    

}
