using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User 
{
    public int id;
    public string username;
    public int score;
    public User(int id, string name, int score)
    {
        this.id = id;
        this.username = name;
        this.score = score;
    }
}
