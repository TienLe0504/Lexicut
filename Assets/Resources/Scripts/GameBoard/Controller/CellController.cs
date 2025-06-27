using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellController : MonoBehaviour
{
    public CellView cellview;
    public CellModel cellmodel;
    public void CrerateLetter(char c, int i, int j)
    {
        cellmodel = new CellModel
        {
            x = i,
            y = j,
            letter = c,
            isUsed = false
        };
        cellview.SetText(c,this);
    }
    public void OnSelectLetter()
    {
        if (cellmodel.isUsed)
        {
            return;
        }
        cellmodel.isUsed = true;
        this.Broadcast(EventID.HanldeCell, this);
    }
    public void ResetIsUsed()
    {
               cellmodel.isUsed = false;
    }
    public void FalseDot()
    {
        cellview.FalseDot();
    }
    public void TrueDot(Color colorToUse)
    {
        //Color colorToUse = Color.white;
        //switch (dotColor)
        //{
        //    case DotColor.Blue:
        //        colorToUse = Color.blue;
        //        break;
        //    case DotColor.Yellow:
        //        colorToUse = Color.yellow;
        //        break;
        //    case DotColor.Green:
        //        colorToUse = Color.green;
        //        break;
        //    case DotColor.Orange:
        //        colorToUse = new Color(1f, 0.65f, 0f);
        //        break;
        //    case DotColor.Violet:
        //        colorToUse = new Color(0.56f, 0f, 1f);
        //        break;
        //}
        cellview.TrueDot(colorToUse);

    }
    //public Color GetTrueColor(DotColor dotColor)
    //{

    //    Color colorToUse = Color.white;
    //    switch (dotColor)
    //    {
    //        case DotColor.Blue:
    //            colorToUse = Color.blue;
    //            break;
    //        case DotColor.Yellow:
    //            colorToUse = Color.yellow;
    //            break;
    //        case DotColor.Green:
    //            colorToUse = Color.green;
    //            break;
    //        case DotColor.Orange:
    //            colorToUse = new Color(1f, 0.65f, 0f);
    //            break;
    //        case DotColor.Violet:
    //            colorToUse = new Color(0.56f, 0f, 1f);
    //            break;
    //    }
    //    return colorToUse;
        
    //}
    //public DotColor RandomColor()
    //{
    //    var values = System.Enum.GetValues(typeof(DotColor));
    //    int randomIndex = Random.Range(0, values.Length);
    //    return (DotColor)values.GetValue(randomIndex);
    //}
    public void SaveColor(Color color)
    {
        cellmodel.color = color;
    }
    //public Color GetColor(effectColor dotColor)
    //{
    //    Color colorToUse = Color.white;
    //    switch (dotColor)
    //    {
    //        case effectColor.Purple:
    //            colorToUse = new Color(0.5f, 0f, 0.5f);
    //            break;
    //        case effectColor.Blue:
    //            colorToUse = Color.blue;
    //            break;
    //        case effectColor.Green:
    //            colorToUse = Color.green;
    //            break;
    //        case effectColor.Red:
    //            colorToUse = Color.red;
    //            break;
    //        case effectColor.Yellow:
    //            colorToUse = Color.yellow;
    //            break;
    //    }
    //    return colorToUse;
    //}
    //public Color DecreaseColor(Color color)
    //{
    //    float lightenFactor = 0.25f;
    //    return Color.Lerp(color, Color.white, lightenFactor);
    //}
    public void CreateEffect(ref Color color,ref float strength, ref bool shoudDecrease, ref bool normalColor)
    {
        cellview.EffectColor(ref color,ref strength,ref shoudDecrease,ref normalColor);
    }
    //public void SavePos()
    //{
    //    Vector3 position = cellview.GetComponent<RectTransform>().position;
    //    cellmodel.pos = position;
    //}

}
