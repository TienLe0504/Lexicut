using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticelAboveEffect : Effect
{
    public void PerformEffect(ref MonoBehaviour runner,ref Dictionary<Vector2, CellController> cellModels, int rows, int columns, effectColor color)
    {
        runner.StartCoroutine(PerformWaveCoroutine(runner, cellModels, columns, rows, color));
    }

    private IEnumerator PerformWaveCoroutine(MonoBehaviour runner, Dictionary<Vector2, CellController> cellModels, int columns, int rows, effectColor color)
    {
        int numberZero = 0;

        float strength = 20f;
        bool shouldDecrease = false;
        bool normalColor = false;

        // Wave 1
        yield return runner.StartCoroutine(CreateWaveColor(cellModels, color, rows, columns, numberZero, shouldDecrease, normalColor, strength));

    }


    private IEnumerator CreateWaveColor(Dictionary<Vector2, CellController> cellModels, effectColor color, int rows, int columns, int numberZero, bool shouldDecrease, bool normalColor, float strength)
    {
        // Duyệt qua từng cột để tạo hiệu ứng sóng từ trái sang phải
        for (int i = 0; i < rows; i++)
        {
            CreateEffect( i, numberZero, cellModels, color, false, false, strength, columns);
            yield return new WaitForSeconds(0.07f);

            // Nếu đã xử lý cột trước đó, áp dụng hiệu ứng 2 (giảm độ đậm) cho nó
            if (i > 0)
            {
                CreateEffect( i - 1, numberZero, cellModels, color, true, false, 16f, columns);
            }
            yield return new WaitForSeconds(0.07f);

            // Nếu đã xử lý cột i-2 trước đó, áp dụng hiệu ứng 3 (trở về màu ban đầu) cho nó
            if (i > 1)
            {
                CreateEffect( i - 2, numberZero, cellModels, color, true, true, 13f, columns);
            }

            // Chờ để tạo hiệu ứng sóng di chuyển
            yield return new WaitForSeconds(0.07f);
        }

        //// Xử lý 2 cột cuối cùng (xử lý hiệu ứng 2 và 3 cho chúng)
        //// Hiệu ứng 2 cho cột kế cuối
        CreateEffect(rows - 2, numberZero, cellModels, color, true, true, 13f, columns);
        yield return new WaitForSeconds(0.07f);
        CreateEffect( rows - 1, numberZero, cellModels, color, true, false, 16f, columns);
        yield return new WaitForSeconds(0.07f);
        CreateEffect( rows - 1, numberZero, cellModels, color, true, true, 13f, columns);


    }
    public void CreateEffect(int x, int y, Dictionary<Vector2, CellController> cellControllers, effectColor color, bool shouldDecrease, bool normalColor, float strength, float columns)
    {
        int currentX = x;
        int currentY = y;


        while (currentY < columns)
        {
            ColorCell(ref currentX, ref currentY, ref cellControllers, ref color, ref strength, ref shouldDecrease, ref normalColor);
            currentY++;
        }
        ColorCell(ref currentX, ref currentY, ref cellControllers, ref color, ref strength, ref shouldDecrease, ref normalColor);
        
    }


    private void ColorCell(ref int x, ref int y, ref Dictionary<Vector2, CellController> cellControllers,
                           ref effectColor color, ref float strength, ref bool shouldDecrease, ref bool normalColor)
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
