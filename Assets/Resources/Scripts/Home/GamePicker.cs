using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GamePicker : BasePopup
{
    public GamePickerView gamePickerView;
    
    public override void Hide()
    {
        base.Hide();
        gamePickerView.ActivewOffButton();
    }

    public override void Show(object data)
    {
        base.Show(data);
        this.Broadcast(EventID.ShowInventoryButton);
        this.Broadcast(EventID.SetStoreActive);
        gamePickerView.Show();
    }
    public override void Init()
    {
        base.Init();
        gamePickerView.Init();
    }
    public void PlayGameCutFruits()
    {
        SoundManager.Instance.PressButton();
        UIManager.Instance.ShowScreen<CutFruits>();
        Hide();
    }


    public void OnClickCategory(string categoryName)
    {
        SoundManager.Instance.PressButton();
        SoundManager.Instance.StopLoopingMusic();
        UIManager.Instance.ShowScreen<WordChainGame>(categoryName);
        Hide();
    }
    public void ButtonBack()
    {
        SoundManager.Instance.PressButton();
        this.Broadcast(EventID.Back);
        Hide();
    }
    public void PlayBtnAppear()
    {
        SoundManager.Instance.PlayAppearSound();
    }
}
