using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalLeftEffect : Effect
{
    public void PerformEffect(ref MonoBehaviour runner, ref Dictionary<Vector2, CellController> cellModels, int rows, int columns, EffectColor color)
    {
        runner.StartCoroutine(PerformWaveCoroutine(runner, cellModels, columns, rows, color));
    }

    private IEnumerator PerformWaveCoroutine(MonoBehaviour runner, Dictionary<Vector2, CellController> cellModels, int columns, int rows, EffectColor color)
    {
        int numberZero = 0;

        float strength = 20f;
        bool shouldDecrease = false;
        bool normalColor = false;

        // Wave 1
        yield return runner.StartCoroutine(CreateWaveColor(cellModels, color, rows, columns, numberZero, shouldDecrease, normalColor, strength));

    }


    private IEnumerator CreateWaveColor(Dictionary<Vector2, CellController> cellModels, EffectColor color, int rows, int columns, int numberZero, bool shouldDecrease, bool normalColor, float strength)
    {
        for (int i = 0; i < rows; i++)
        {
            CreateEffect(numberZero, i, cellModels, color, false, false, strength, rows);
            yield return new WaitForSeconds(0.07f);

            if (i > 0)
            {
                CreateEffect(numberZero, i - 1,  cellModels, color, true, false, 16f, rows);
            }
            yield return new WaitForSeconds(0.07f);

            if (i > 1)
            {
                CreateEffect(numberZero, i - 2, cellModels, color, true, true, 13f, rows);
            }

            yield return new WaitForSeconds(0.07f);
        }

        CreateEffect(numberZero, rows - 2, cellModels, color, true, true, 13f, rows);
        yield return new WaitForSeconds(0.07f);
        CreateEffect(numberZero, rows - 1, cellModels, color, true, false, 16f, rows);
        yield return new WaitForSeconds(0.07f);
        CreateEffect(numberZero, rows - 1, cellModels, color, true, true, 13f, rows);


    }
    public void CreateEffect(int x, int y, Dictionary<Vector2, CellController> cellControllers, EffectColor color, bool shouldDecrease, bool normalColor, float strength, float rows)
    {
        int currentX = x;
        int currentY = y;


        while (currentX < rows)
        {
            ColorCell(ref currentX, ref currentY, ref cellControllers, ref color, ref strength, ref shouldDecrease, ref normalColor);
            currentX++;
        }
        ColorCell(ref currentX, ref currentY, ref cellControllers, ref color, ref strength, ref shouldDecrease, ref normalColor);

    }


    private void ColorCell(ref int x, ref int y, ref Dictionary<Vector2, CellController> cellControllers,
                           ref EffectColor color, ref float strength, ref bool shouldDecrease, ref bool normalColor)
    {
        Vector2 key = new Vector2(x, y);
        if (!cellControllers.ContainsKey(key))
        {
            // Bỏ qua vị trí không tồn tại
            return;
        }

        CellController cellController = cellControllers[key];
        //Color getColor = cellController.GetColor(color);
        Color getColor = HelperColor.Instance.GetColor(color);
        cellController.CreateEffect(ref getColor, ref strength, ref shouldDecrease, ref normalColor);
    }

}
