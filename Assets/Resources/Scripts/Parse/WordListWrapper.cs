using System.Collections.Generic;

[System.Serializable]
public class WordListWrapper
{
    public List<Entry> entries;

    [System.Serializable]
    public class Entry
    {
        public string id;
        public List<string> words;
    }
}
