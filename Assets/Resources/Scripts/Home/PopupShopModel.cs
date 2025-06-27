using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupShopModel
{
    public List<ItemShopController> shopEffect;
    public List<ItemShopController> shopTrail;
    public List<ItemShopController> shopColor;

    public void AddToList(List<ItemShopController> shops, ItemShopController item)
    {
        shops.Add(item);
    }
}
