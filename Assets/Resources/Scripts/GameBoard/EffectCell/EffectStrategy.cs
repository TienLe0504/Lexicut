using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class EffectStrategy
{
    private Effect effect;
    private Dictionary<Vector2, CellController> cellModels;
    private int rows, columns;
    private effectColor color;
    private MonoBehaviour runner;
    public void SetParameters(ref MonoBehaviour runner,ref Dictionary<Vector2,CellController> cellModels,ref int rows,ref int columns,ref effectColor color)
    {
        this.cellModels = cellModels;
        this.rows = rows;
        this.columns = columns;
        this.color = color;
        this.runner = runner;
    }
    public void SetEffect(Effect effect)
    {
        this.effect = effect;
    }
    public void PerformEffect()
    {
        if (effect != null)
        {
            effect.PerformEffect(ref runner,ref cellModels, rows, columns, color);
            Debug.Log($"Effect {effect.GetType().Name} performed with color {color} on a grid of size {rows}x{columns}.");
        }
    }
}
