using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapBuyItemController : MonoBehaviour
{
    public OverlapBuyItem overlap;
    public Inventory inventory;
    public ItemShopController itemShopController;
    public void SetupData(Inventory inventory, ItemShopController itemShopController)
    {
        this.inventory = inventory;
        this.itemShopController = itemShopController;
    }
    public bool BuyItem()
    {
        PressButton();
        if (ManagerGame.Instance.gold < inventory.gold || inventory.isUsed)
        {

            return false;
        }

        ManagerGame.Instance.inventory.Add(inventory.typeInventory.ToString());
        Inventory item = CloneItem();
        ManagerGame.Instance.InventoryData.Add(item);
        ResourceManager.Instance.SaveToFile<List<string>>(CONST.KEY_FILENAME_STORE, CONST.KEY_INVENTORY, ManagerGame.Instance.inventory);

        itemShopController.BuyItemSuccess();
        this.Broadcast(EventID.BuyItem, inventory.gold);
        return true;

    }

    private Inventory CloneItem()
    {
        Inventory item = new Inventory();
        item.typeInventory = inventory.typeInventory;
        item.name = inventory.name;
        item.image = inventory.image;
        item.gold = inventory.gold;
        item.typeChoice = inventory.typeChoice;
        item.isUsed = false;
        return item;
    }

    public void PressButton()
    {
        SoundManager.Instance.PressButton();
    }
}
