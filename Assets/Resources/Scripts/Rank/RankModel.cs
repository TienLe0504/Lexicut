using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankModel 
{
    public List<User> users = new List<User>();
    public User userCurrent;
    public void SetUpCurrentUser(User user)
    {
        userCurrent = user;
    }
}
