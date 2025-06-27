using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupInventoryModel 
{
    public List<GameObject> items = new List<GameObject>();
    public void AddItem(GameObject item)

    {
        if(item == null)
        {
            Debug.Log(" null roio");
        }
        Debug.Log(" co null khon");
        items.Add(item);
    }
}
