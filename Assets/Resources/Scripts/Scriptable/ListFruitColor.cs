using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ListFruitColor", menuName = "ScriptableObjects/ListFruitColor", order = 1)]
public class ListFruitColor : ScriptableObject
{
    [System.Serializable]
    public class FruitColorPair
    {
        public string fruit;
        public effectColor color;
    }
    public List<FruitColorPair> fruitColors = new List<FruitColorPair>();

    public Dictionary<string, effectColor> ToDictionary()
    {
        Dictionary<string, effectColor> dict = new Dictionary<string, effectColor>();
        foreach (var pair in fruitColors)
        {
            if (!dict.ContainsKey(pair.fruit))
            {
                dict[pair.fruit] = pair.color;
            }
        }
        return dict;
    }
}
