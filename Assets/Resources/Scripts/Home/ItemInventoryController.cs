using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventoryController : ItemShopController
{
    public override void PressButton()
    {
        base.PressButton();
        UIManager.Instance.ShowOverlap<OverlapUseItem>(this, true);
    }
}
