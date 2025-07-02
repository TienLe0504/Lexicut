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
        controller.InitializeGame();
    }

    public override void Show(object data)
    {
        base.Show(data);
    }
    public void ChangeSceence()
    {
        SoundManager.Instance.StopLoopingMusic();
        UIManager.Instance.ShowPopup<ShowPopupScore>(controller.cutFruitUI.textScore.text.ToString());
        Hide();
    }
}
