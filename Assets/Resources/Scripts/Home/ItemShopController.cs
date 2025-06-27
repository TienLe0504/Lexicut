using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShopController : MonoBehaviour
{
    public ItemShop itemShop;
    public Inventory inventory;

    public void SetupData(Inventory inventory, bool isActiveGold = false)
    {
        this.inventory = inventory;
        this.inventory.image = GetSprite(inventory.typeInventory.ToString());
        itemShop.SetUp(inventory.gold.ToString(), this.inventory.image, isActiveGold);
        itemShop.SetUpShadow(inventory.isUsed);
    }
    public Sprite GetSprite(string name)
    { 
        string path = CONST.KEY_PATH_IMG_SHOP+ name;
        return ResourceManager.Instance.GetResource<Sprite>(path);
    }
    public virtual void PressButton(){}
    //public void PressButton()
    //{
    //    if (inventory.isUsed) return;
    //    this.Broadcast(EventID.ActiveBGShop);
    //    UIManager.Instance.ShowOverlap<OverlapBuyItem>(this,true);

    //}

    public void BuyItemSuccess(object data = null)
    {
        inventory.isUsed = true;
        itemShop.SetUpShadow(inventory.isUsed);
    }
}
