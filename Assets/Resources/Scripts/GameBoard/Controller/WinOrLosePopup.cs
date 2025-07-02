using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinOrLosePopup : BasePopup
{
    public WinOrLoseModel model = new WinOrLoseModel();
    public WinOrLoseView uiView;
    public bool isWin;
    public string textTitle;
    public override void Init()
    {
        base.Init();
    }
    public override void Hide()
    {
        base.Hide();
    }
    public override void Show(object data)
    {
        base.Show(data);
        if(data is DataPopup value)
        {
            model.SetUp(value.word, value.answer, value.category, value.gold);
            StartShowPopup();
            SetupText();
            SetupStar();
            uiView.ShowWinOrLose(textTitle, isWin, model.textAnswer, model.word, model.category, model.star,model.gold);
        }
    }
    public void StartShowPopup()
    {

        if (model.answer.Count < 5)
        {
            isWin = true;
            textTitle = "You Win!";
        }
        else
        {
            isWin = false;
            textTitle = "You Lose!";
        }
    }
    public void SetupText()
    {
        if (model.word.Count < 5)
        {
            model.word.Add("Empty");
            model.word.Add("Empty");
            model.word.Add("Empty");
            model.word.Add("Empty");
        }
        model.textAnswer = "1. " + model.word[0] + " - " +
                           "2. " + model.word[1] + "\n" +
                           "3. " + model.word[2] + " - " +
                           "4. " + model.word[3] + "\n" +
                           "5. " + model.word[4];
    }

    public void SetupStar()
    {
        if (model.answer.Count == 5)
        {
            model.star = 3;
            return;
        }
        if (model.answer.Count == 0)
        {
            model.star = 3;
            return;

        }
        if (model.answer.Count <= 2)
        {
            model.star = 2;
            return;

        }
        if (model.answer.Count <= 4)
        {

            model.star = 1;
            return;

        }
    }
    public void PressHome()
    {
        SoundManager.Instance.PressButton();
        UIManager.Instance.ShowScreen<HomeScreen>(model.gold);
        Hide();
        SoundManager.Instance.PlayLoopingMusic(SoundManager.Instance.backgroundGame);
    }
    public void PressContinue()
    {
        SoundManager.Instance.PressButton();
        UIManager.Instance.ShowScreen<WordChainGame>(model.category);
        Hide();
        SoundManager.Instance.PlayLoopingMusic(SoundManager.Instance.backgroundGame);
    }
    public void PlaySoundWinGame(bool isWin)
    {
        SoundManager.Instance.StopLoopingMusic();
        if (isWin)
        {
            PlaySoundOneShot(SoundManager.Instance.winGame);
        }
        else
        {
            PlaySoundOneShot(SoundManager.Instance.loseGame);

        }
    }
    public void PlaySoundOneShot(AudioClip audio)
    {
        SoundManager.Instance.PlayOneShotSound(audio);

    }
}
