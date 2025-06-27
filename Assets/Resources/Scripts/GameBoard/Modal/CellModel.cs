using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellModel 
{
    public int x { get; set; }
    public int y { get; set; }
    public char letter { get; set; }
    public bool isUsed { get; set; }
    public Color color { get; set; }
    public Vector3 pos { get; set; }
    public void SaveColor(Color color)
    {
        this.color = color;
    }
}
