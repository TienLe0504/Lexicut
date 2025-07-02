using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectRank : MonoBehaviour
{
    public TextMeshProUGUI number;
    public TextMeshProUGUI username;
    public TextMeshProUGUI score;
    public void Setup(User user, int rank)
    {
        number.text = rank.ToString()+".";
        username.text = user.username;
        score.text = user.score.ToString();
    }
}
