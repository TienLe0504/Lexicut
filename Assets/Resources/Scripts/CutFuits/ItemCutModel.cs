using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum ItemType
{
    item,
    bomb
}

public class ItemCutModel : MonoBehaviour
{
    public ItemType itemType;
    public string name;
    public float posXTo;
    public float posYTo;
    public float posXFall;
    public float posYFall;

    public void SetUp(ItemType item, string name,float posxTo, float posyTo, float posxFall, float posyFall)
    {
        itemType = item;
        this.name = name;
        posXTo = posxTo;
        posYTo = posyTo;
        posXFall = posxFall;
        posYFall = posyFall;
    }
}
