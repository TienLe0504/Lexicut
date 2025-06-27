using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupInventoryController : MonoBehaviour
{
    public PopupInventory popupInventory;
    public PopupInventoryModel popupInventoryModel = new PopupInventoryModel();
    private void OnEnable()
    {
        this.Register(EventID.UpdateStatusItem, UpdateStatusItem);

    }
    private void OnDisable()
    {
        this.Unregister(EventID.UpdateStatusItem, UpdateStatusItem);
    }
    public void AddItem(GameObject item)
    {
        popupInventoryModel.AddItem(item);
    }
    public void UpdateStatusItem(object data)
    {
        if (data is TypeChoice type)
        {
            foreach (GameObject item in popupInventoryModel.items)
            {
                ItemShopController itemController = item.GetComponent<ItemShopController>();
                if(itemController.inventory.typeChoice == type)
                {
                    itemController.inventory.isUsed = false;
                    itemController.itemShop.SetUpShadow(itemController.inventory.isUsed);
                }
            }
        }
    }
}
