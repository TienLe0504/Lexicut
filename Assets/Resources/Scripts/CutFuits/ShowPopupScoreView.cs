using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowPopupScoreView : MonoBehaviour
{
    public GameObject popup;
    public TextMeshProUGUI score;
    public Button btnContinue;
    public ShowPopupScore popupInit;
    public Image bg;

    private void Awake()
    {
        bg.GetComponent<RectTransform>().sizeDelta = new Vector2(ManagerGame.Instance.WidthScreen, ManagerGame.Instance.HeightScreen);
        btnContinue.onClick.AddListener(PressBtn);   
    }
    public void PressBtn()
    {
        popupInit.PressBtnContinue();
    }

    public void Setup(string score)
    {
        this.score.text = score;
        RectTransform scoreRect = this.score.GetComponent<RectTransform>();
        RectTransform popupRect = popup.GetComponent<RectTransform>();
        scoreRect.localScale = Vector3.zero;
        popupRect.localScale = Vector3.zero;
        popupRect.DOScale(Vector3.one, 0.8f).SetEase(Ease.OutBack);
        scoreRect.DOScale(Vector3.one, 0.8f).SetEase(Ease.OutBack);
    }

}
