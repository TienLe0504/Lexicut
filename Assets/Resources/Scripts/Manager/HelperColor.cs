using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectColor
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
    [Header("Fruit color mapping")]
    public ListFruitColor listFruitColor;
    public Color GetColor(EffectColor color)
    {
        switch (color)
        {
            case EffectColor.Blue: return Color.blue;
            case EffectColor.Yellow: return Color.yellow;
            case EffectColor.Purple: return new Color(0.5f, 0f, 0.5f);
            case EffectColor.Green: return Color.green;
            case EffectColor.Red: return Color.red;
            case EffectColor.Orange: return new Color(1f, 0.65f, 0f);
            case EffectColor.Black: return Color.black;
            case EffectColor.White: return Color.white;
            case EffectColor.Brown: return new Color(0.6f, 0.3f, 0.1f);
            case EffectColor.Pink: return new Color(1f, 0.4f, 0.7f);
            case EffectColor.Gray: return Color.gray;
            case EffectColor.Cyan: return Color.cyan;
            case EffectColor.Magenta: return Color.magenta;
            case EffectColor.Lime: return new Color(0.75f, 1f, 0f);
            case EffectColor.Gold: return new Color(1f, 0.84f, 0f);
            case EffectColor.Teal: return new Color(0f, 0.5f, 0.5f);
            case EffectColor.Navy: return new Color(0f, 0f, 0.5f);
            case EffectColor.Maroon: return new Color(0.5f, 0f, 0f);
            case EffectColor.Olive: return new Color(0.5f, 0.5f, 0f);
            default: return Color.white;
        }
    }
    public EffectColor RandomColor(bool excludeCertainColors = false)
    {
        var values = new List<EffectColor>((EffectColor[])System.Enum.GetValues(typeof(EffectColor)));

        if (excludeCertainColors)
        {
            values.Remove(EffectColor.Red);
            values.Remove(EffectColor.Black);
            values.Remove(EffectColor.White);
        }

        return values[Random.Range(0, values.Count)];
    }


    public Color GetColorByFruitName(string fruitName)
    {
        foreach (var pair in listFruitColor.fruitColors)
        {
            if (pair.fruit == fruitName)
            {
                return GetColor(pair.color);
            }
        }

        Debug.LogWarning($"Fruit '{fruitName}' not found in ListFruitColor.");
        return Color.white;
    }



    public Color DecreaseColor(Color color, float lightenFactor)
    {
        return Color.Lerp(color, Color.white, lightenFactor);
    }

    public EffectColor MapTypeToEffectColor(string colorString)
    {
        switch (colorString)
        {
            case "GreenColor": return EffectColor.Green;
            case "YellowColor": return EffectColor.Yellow;
            case "OrangeColor": return EffectColor.Orange;
            default: return EffectColor.Gray; 
        }
    }


}
