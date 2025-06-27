using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinOrLoseModel
{
    public List<string> word = new List<string>();
    public List<string> answer = new List<string>();
    public string category;
    public int gold;
    public string textAnswer;
    public int star;

    public void SetUp(List<string> words, List<string> answers, string category, int gold)
    {
        word = words;
        answer = answers;
        this.category = category;
        this.gold = gold;
    }
}
