using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPopupScore : BasePopup
{
    public ShowPopupScoreView uiView;
    public int score;
    public override void Hide()
    {
        base.Hide();
    }

    public override void Init()
    {
        base.Init();
    }

    public override void Show(object data)
    {
        base.Show(data);
        if (data is string scoreText)
        {
            score = int.Parse(scoreText);
            uiView.Setup(scoreText);
        }

    }
    public void PressBtnContinue()
    {
        UIManager.Instance.ShowScreen<HomeScreen>(score);
        Hide();
    }
}