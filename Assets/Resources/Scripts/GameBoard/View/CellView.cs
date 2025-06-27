using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using Unity.VisualScripting;

public enum DotColor
{
    Blue,
    Yellow,
    Green,
    Orange,
    Violet
}

public class CellView : MonoBehaviour, IPointerEnterHandler
{
    public TextMeshProUGUI textmesh;
    public CellController cellcontroller;
    public Image bg;
    public Image cell;
    public int i;
    public int j;
    public float ligthFator = 0.25f;

    private void Start()
    {
        bg.gameObject.SetActive(false);
    }

    public void SetText(char c, CellController cell)
    {
        cellcontroller = cell;
        textmesh = GetComponentInChildren<TextMeshProUGUI>();
        textmesh.text = c.ToString();
        transform.localScale = Vector3.zero;
        i = cell.cellmodel.x;
        j = cell.cellmodel.y;
        transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            transform.DOShakeScale(0.3f, 0.1f, 10, 90, false);
        });
    }


    private Tween falseDotTween;
    public void OnPointerEnter(PointerEventData eventData)
    {
        cellcontroller.OnSelectLetter();
    }
    public void FalseDot()
    {

        cell.color = Color.red;

        Vector3 originalPos = transform.localPosition; 

        transform.DOShakePosition(0.3f, strength: 15f, vibrato: 15, randomness: 90, fadeOut: true).OnComplete(() =>
        {
            transform.localPosition = originalPos;  
            cell.color = cellcontroller.cellmodel.color;
            cellcontroller.ResetIsUsed();
        });
    }
    public void EffectColor(ref Color color, ref float strength, ref bool shouldDecrease, ref bool normalColor)
    {
        bg.gameObject.SetActive(true);
        textmesh.raycastTarget = false; 
        cell.raycastTarget = false;

        Color colorText = Color.white;

        if (!shouldDecrease)
        {
            bg.color = color;
            colorText = color;
        }
        else
        {
            Color colorCurrent = bg.color;
            bg.color = HelperColor.Instance.DecreaseColor(colorCurrent, ligthFator);
            colorText = HelperColor.Instance.DecreaseColor(colorCurrent,ligthFator);
        }

        textmesh.color = colorText;
        bool isNormal = normalColor;

        Vector3 originalScale = transform.localScale;

        transform.DOScale(1.05f, 0.1f).OnComplete(() =>
        {
            transform.DOScale(originalScale, 0.1f).OnComplete(() =>
            {
                if (isNormal)
                {
                    textmesh.color = Color.black;
                    bg.gameObject.SetActive(false);
                }

                textmesh.raycastTarget = true;
                cell.raycastTarget = true;
            });
        });
    }


    
    public void TrueDot(Color color)
    {

        cell.color = color;
        Vector3 originalPos = transform.localPosition;

        transform.DOShakePosition(0.3f, strength: 15f, vibrato: 15, randomness: 90, fadeOut: true).OnComplete(() =>
        {
            transform.localPosition = originalPos; 
                                                    
        });
    }

}
