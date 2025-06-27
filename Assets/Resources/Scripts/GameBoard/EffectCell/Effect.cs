using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Effect 
{
    void PerformEffect(ref MonoBehaviour runner, ref Dictionary<Vector2, CellController> cellModels, int rows, int columns, effectColor color);
}