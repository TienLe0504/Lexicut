using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCategory", menuName = "ScriptableObjects/Category", order = 1)]
public class Category : ScriptableObject
{
    [Header("List of category names")]
    public List<string> categories = new List<string>();
}
