using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShopChildController : ItemShopController
{

    public override void PressButton()
    {
        base.PressButton();
        if (inventory.isUsed) return;
        this.Broadcast(EventID.ActivateBackgroundShop);
        UIManager.Instance.ShowOverlap<OverlapBuyItem>(this, true);
    }
}
