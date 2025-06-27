using System.Collections.Generic;
using UnityEngine;
public enum typeInventory
{
    GrayColor,
    HorizontalEffect,
    VerticalEffect,
    DiagonalEffect,
    BlueGreenTrail,
    BlueTrail,
    WhiteTrail,
    YellowTrail,
    GreenColor,
    YellowColor,
    OrangeColor,
    purpleTrail,

}
public enum TypeChoice
{
    trail,
    effect,
    color
}
[CreateAssetMenu(fileName = "InventoryData", menuName = "Data/InventoryData")]
public class InventoryData : ScriptableObject
{
    public List<Inventory> inventoryList = new List<Inventory>();
}

[System.Serializable]
public class Inventory
{
    public string name;
    public Sprite image;
    public int gold;
    public typeInventory typeInventory;
    public bool isUsed = false;
    public TypeChoice typeChoice;
}