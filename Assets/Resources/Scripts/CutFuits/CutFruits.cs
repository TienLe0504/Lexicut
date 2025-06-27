using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutFruits : BaseScreen
{
    public CutFruitController controller;
    public override void Hide()
    {
        base.Hide();
    }

    public override void Init()
    {
        base.Init();
        controller.StartGame();
    }

    public override void Show(object data)
    {
        base.Show(data);
    }
    public void ChangeSceence()
    {
        UIManager.Instance.ShowPopup<ShowPopupScore>(controller.uiView.textScore.text.ToString());
        Hide();
    }
}
