using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

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
        SoundManager.Instance.PlayOneShotSound(SoundManager.Instance.winGame);
        if (data is string scoreText)
        {
            score = int.Parse(scoreText);
            uiView.Setup(scoreText);
        }
        ManagerGame.Instance.AddScoreForUsers();
        AddScoreForUser();

    }
    public void AddScoreForUser()
    {
        List<User> listUser = ResourceManager.Instance.LoadFromFile<List<User>>(CONST.PATH_RANK,CONST.KEY_RANK);
        User currentUser = listUser.Find(u => u.id == CONST.KEY_ID);
        currentUser.score += score;
        ResourceManager.Instance.SaveToFile<List<User>>(CONST.PATH_RANK, CONST.KEY_RANK, listUser);
    }    
    public void PressBtnContinue()
    {
        SoundManager.Instance.PressButton();
        UIManager.Instance.ShowScreen<HomeScreen>(score);
        Hide();
        SoundManager.Instance.PlayLoopingMusic(SoundManager.Instance.backgroundGame);
    }
}