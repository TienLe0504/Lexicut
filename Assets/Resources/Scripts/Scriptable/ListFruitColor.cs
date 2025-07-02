using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ListFruitColor", menuName = "ScriptableObjects/ListFruitColor", order = 1)]
public class ListFruitColor : ScriptableObject
{
    [System.Serializable]
    public class FruitColorPair
    {
        public string fruit;
        public EffectColor color;
    }
    public List<FruitColorPair> fruitColors = new List<FruitColorPair>();

    public Dictionary<string, EffectColor> ToDictionary()
    {
        Dictionary<string, EffectColor> dict = new Dictionary<string, EffectColor>();
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
