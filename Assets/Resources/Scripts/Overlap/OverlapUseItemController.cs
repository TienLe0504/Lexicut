using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapUseItemController : MonoBehaviour
{
    public OverlapUseItem OverlapUseItem;
    public ItemInventoryController itemInventoryController;
    public void SetUpInventory(ItemInventoryController itemInventoryController)
    {
        this.itemInventoryController = itemInventoryController;
    }
    public void OnClickUseItem()
    {
        SoundManager.Instance.PressButton();
        if (itemInventoryController.inventory.isUsed)
        {
            itemInventoryController.inventory.isUsed = false;
            itemInventoryController.itemShop.SetUpShadow(false);
            SaveData("");

        }
        else
        {
            this.Broadcast(EventID.UpdateItemStatus, itemInventoryController.inventory.typeChoice);
            itemInventoryController.inventory.isUsed = true;
            itemInventoryController.itemShop.SetUpShadow(true);
            SaveData(itemInventoryController.inventory.typeInventory.ToString());

        }
        OverlapUseItem.SetupText(itemInventoryController);
    }
    public void UseItem(string key, string value)
    {
        ResourceManager.Instance.SaveToFile<string>(CONST.KEY_FILENAME_STORE, key, value);
    }
    public void SaveData(string value)
    {
        if (itemInventoryController.inventory.typeChoice == TypeChoice.color)
        {
            if(value == "")
            {
                ManagerGame.Instance.CreateColor(true);

            }
            else
            {
                ManagerGame.Instance.colorEffectCurrentString = itemInventoryController.inventory.typeInventory.ToString();
                ManagerGame.Instance.CreateColor();
            }
                UseItem(CONST.KEY_COLOR, value);
        }
        else if (itemInventoryController.inventory.typeChoice == TypeChoice.effect)
        {
            if (value == "")
            {
                ManagerGame.Instance.CreateEffectCell(true);
            }
            else
            {
                ManagerGame.Instance.effectCellName = itemInventoryController.inventory.typeInventory.ToString();
                ManagerGame.Instance.CreateEffectCell();
            }
                UseItem(CONST.KEY_EFFECT, value);
        }
        else if (itemInventoryController.inventory.typeChoice == TypeChoice.trail)
        {
            if(value == "")
            {
                ManagerGame.Instance.CreateTrail(true);
            }
            else
            {
                ManagerGame.Instance.trailName = itemInventoryController.inventory.typeInventory.ToString();
                ManagerGame.Instance.CreateTrail();
            }
                UseItem(CONST.KEY_TRAIL, value);
        }
    }

    public void PressButton()
    {
        SoundManager.Instance.PressButton();
    }

}
