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
    //public bool isFalseDot = false;

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

        //isFalseDot = true;
        cell.color = Color.red;

        Vector3 originalPos = transform.localPosition; 

        transform.DOShakePosition(0.3f, strength: 15f, vibrato: 15, randomness: 90, fadeOut: true).OnComplete(() =>
        {
            transform.localPosition = originalPos;  
            cell.color = cellcontroller.cellmodel.color;
            cellcontroller.ResetIsUsed();
            //isFalseDot = false;
        });
    }
    public void EffectColor(ref Color color, ref float strength, ref bool shouldDecrease, ref bool normalColor)
    {
        bg.gameObject.SetActive(true);
        textmesh.raycastTarget = false; // Ngăn pointer trigger
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

        // Phóng to lên 1.05, sau đó thu nhỏ về 1
        transform.DOScale(1.05f, 0.1f).OnComplete(() =>
        {
            transform.DOScale(originalScale, 0.1f).OnComplete(() =>
            {
                if (isNormal)
                {
                    textmesh.color = Color.black;
                    bg.gameObject.SetActive(false);
                }

                // Bật lại raycast
                textmesh.raycastTarget = true;
                cell.raycastTarget = true;
            });
        });
    }


    //public void EffectColor(ref Color color,ref float strength, ref bool shouldDecrease , ref bool normalColor)
    //{

    //    bg.gameObject.SetActive(true);
    //    Color colorText = Color.white;

    //    if (!shouldDecrease)
    //    {
    //        bg.color = color;
    //        colorText = color;


    //    }
    //    else
    //    {
    //        Color colorCurrent = bg.color;
    //        bg.color = cellcontroller.DecreaseColor(colorCurrent);
    //        colorText = cellcontroller.DecreaseColor(colorCurrent);
    //    }
    //    //if (!isFalseDot)
    //    //{
    //    //    cell.color = cellcontroller.cellmodel.color;

    //    //}
    //    Vector3 originalPos = transform.localPosition;
    //    textmesh.color = colorText; 
    //    bool isNormal = normalColor;

    //    transform.DOShakePosition(0.06f, strength: strength, vibrato: 15, randomness: 90, fadeOut: true).OnComplete(() =>
    //    {
    //        transform.localPosition = originalPos;
    //        if (isNormal)
    //        {
    //            //bg.color = Color.white;
    //            textmesh.color = Color.black;
    //            //Image img = GetComponentInChildren<Image>();
    //            //img.color = cellcontroller.cellmodel.color;
    //            //if (!isFalseDot)
    //            //{
    //            //    cell.color = cellcontroller.cellmodel.color;

    //            //}

    //            bg.gameObject.SetActive(false);

    //        }
    //    });

    //}
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
