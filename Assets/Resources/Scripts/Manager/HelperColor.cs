using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum effectColor
{
    Blue,
    Yellow,
    Purple,
    Green,
    Red,
    Orange,
    Black,
    White,
    Brown,
    Pink,
    Gray,
    Cyan,
    Magenta,
    Lime,
    Gold,
    Teal,
    Navy,
    Maroon,
    Olive
}

public class HelperColor : BaseManager<HelperColor>
{
    public ListFruitColor listFruitColor;
    public Color GetColor(effectColor dotColor)
    {
        Color colorToUse = Color.white;
        switch (dotColor)
        {
            case effectColor.Blue:
                colorToUse = Color.blue;
                break;
            case effectColor.Yellow:
                colorToUse = Color.yellow;
                break;
            case effectColor.Purple:
                colorToUse = new Color(0.5f, 0f, 0.5f);
                break;
            case effectColor.Green:
                colorToUse = Color.green;
                break;
            case effectColor.Red:
                colorToUse = Color.red;
                break;
            case effectColor.Orange:
                colorToUse = new Color(1f, 0.65f, 0f);
                break;
            case effectColor.Black:
                colorToUse = Color.black;
                break;
            case effectColor.White:
                colorToUse = Color.white;
                break;
            case effectColor.Brown:
                colorToUse = new Color(0.6f, 0.3f, 0.1f);
                break;
            case effectColor.Pink:
                colorToUse = new Color(1f, 0.4f, 0.7f);
                break;
            case effectColor.Gray:
                colorToUse = Color.gray;
                break;
            case effectColor.Cyan:
                colorToUse = Color.cyan;
                break;
            case effectColor.Magenta:
                colorToUse = Color.magenta;
                break;
            case effectColor.Lime:
                colorToUse = new Color(0.75f, 1f, 0f);
                break;
            case effectColor.Gold:
                colorToUse = new Color(1f, 0.84f, 0f);
                break;
            case effectColor.Teal:
                colorToUse = new Color(0f, 0.5f, 0.5f);
                break;
            case effectColor.Navy:
                colorToUse = new Color(0f, 0f, 0.5f);
                break;
            case effectColor.Maroon:
                colorToUse = new Color(0.5f, 0f, 0f);
                break;
            case effectColor.Olive:
                colorToUse = new Color(0.5f, 0.5f, 0f);
                break;
        }
        return colorToUse;
    }
    public effectColor RandomColor(bool isNotColor = false)
    {
        var values = new List<effectColor>((effectColor[])System.Enum.GetValues(typeof(effectColor)));

        if (isNotColor)
        {
            values.Remove(effectColor.Red);
            values.Remove(effectColor.Black);
            values.Remove(effectColor.White);
        }

        int randomIndex = Random.Range(0, values.Count);
        return values[randomIndex];
    }


    public Color GetColorOnName(string fruitName)
    {
        foreach (var pair in listFruitColor.fruitColors)
        {
            if (pair.fruit == fruitName)
            {
                return GetColor(pair.color);
            }
        }

        Debug.LogWarning("Fruit name not found in ListFruitColor: " + fruitName);
        return Color.white;
    }



    public Color DecreaseColor(Color color, float lightenFactor)
    {
        return Color.Lerp(color, Color.white, lightenFactor);
    }

    public effectColor MapTypeToEffectColor(string colorString)
    {
        switch (colorString)
        {
            case "GreenColor":
                return effectColor.Green;

            case "YellowColor":
                return effectColor.Yellow;

            case "OrangeColor":
                return effectColor.Orange;

            default:
                return effectColor.Gray; 
        }
    }


}
