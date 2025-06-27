using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutFruitsModal
{
    public List<ItemCutModel> itemCuts = new List<ItemCutModel>();
    ////public List<ItemCutView> listCutFruitsController = new List<ItemCutView>();
    public List<string> wordDiff = new List<string>();
    public List<string> wordChoosen = new List<string>();
    public List<string> wordCurrent = new List<string>();
    public int combo;
    public int score = 1;
    public void AddItem(ItemCutModel item)
    {
        itemCuts.Add(item);
    }
    public void RemoveItem(ItemCutModel item) {
        itemCuts.Remove(item);
    }
    //public void AddController(ItemCutView controller)
    //{
    //    listCutFruitsController.Add(controller);
    //}
    //public void RemoveController(ItemCutView controller)
    //{
    //    listCutFruitsController.Remove(controller);
    //}
    public void SetDefaul()
    {
        combo = -1;
    }
    public void AddCombo()
    {
        combo += 1;
    }
    public void CalculateScore()
    {
        if (combo <= 4)
        {
            score = 1;
            return;
        }
        if (combo <= 10)
        {
            score = 2;
            return;
        }
        else
        {
            score = 3;
            return;
        }
    }
}
