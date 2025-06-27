using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ListImage", menuName = "ScriptableObjects/ListImage", order = 1)]
public class ListImage : ScriptableObject
{
    [SerializeField]
    private List<StringSpritePair> imageList = new List<StringSpritePair>();

    private Dictionary<string, Sprite> imageDictionary;

    public Dictionary<string, Sprite> ImageDictionary
    {
        get
        {
            if (imageDictionary == null)
            {
                imageDictionary = new Dictionary<string, Sprite>();
                foreach (var pair in imageList)
                {
                    if (!imageDictionary.ContainsKey(pair.key))
                        imageDictionary.Add(pair.key, pair.value);
                }
            }
            return imageDictionary;
        }
    }

    [Serializable]
    public class StringSpritePair
    {
        public string key;
        public Sprite value;
    }
}
