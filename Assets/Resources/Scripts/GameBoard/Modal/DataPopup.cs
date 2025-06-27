using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPopup 
{
    public string category;
    public List<string> word;
    public List<string> answer;
    public int gold;
    public void Setup(string cate, List<string> word, List<string> answer, int gold)
    {
        this.category = cate;
        this.word = word;
        this.answer = answer;
        this.gold = gold;
    }
}
