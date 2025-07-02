using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DiagonalLeftEfect : Effect
{

    public void PerformEffect(ref MonoBehaviour runner, ref Dictionary<Vector2, CellController> cellModels, int rows, int columns, EffectColor color)
    {
        runner.StartCoroutine(PerformWaveCoroutine(runner,cellModels, columns,rows, color));

    }
    private IEnumerator PerformWaveCoroutine(MonoBehaviour runner, Dictionary<Vector2, CellController> cellModels, int columns,int rows, EffectColor color)
    {
        int numberZero = 0;

        float strength = 20f;
        bool shouldDecrease = false;
        bool normalColor = false;
        yield return runner.StartCoroutine(CreateWaveColor(cellModels, color, rows,rows, numberZero, shouldDecrease, normalColor, strength)); 

    }

 
    private IEnumerator CreateWaveColor(Dictionary<Vector2, CellController> cellModels, EffectColor color, int rows,int columns, int numberZero, bool shouldDecrease, bool normalColor, float strength)
    {
        for (int i = 0; i < rows; i++)
        {
            // Hiệu ứng 1: Đổi màu đậm
            CreateEffect(i, numberZero, cellModels, color, false, false, strength);
            yield return new WaitForSeconds(0.07f);

            // Nếu đã xử lý cột trước đó, áp dụng hiệu ứng 2 (giảm độ đậm) cho nó
            if (i > 0)
            {
                CreateEffect(i - 1, numberZero, cellModels, color, true, false, 16f);
            }
            yield return new WaitForSeconds(0.07f);

            // Nếu đã xử lý cột i-2 trước đó, áp dụng hiệu ứng 3 (trở về màu ban đầu) cho nó
            if (i > 1)
            {
                CreateEffect(i - 2, numberZero, cellModels, color, true, true, 13f);
            }

            // Chờ để tạo hiệu ứng sóng di chuyển
            yield return new WaitForSeconds(0.07f);
        }

        //// Xử lý 2 cột cuối cùng (xử lý hiệu ứng 2 và 3 cho chúng)
        //// Hiệu ứng 2 cho cột kế cuối
        CreateEffect(rows - 2, numberZero, cellModels, color, true, true, 13f);
        yield return new WaitForSeconds(0.07f);

        //// Hiệu ứng 3 cho cột kế cuối


        //// Hiệu ứng 3 cho cột cuối cùng (trở về màu ban đầu)



        for (int j = 1; j < columns; j++)
        {
            // Không cần thiết lập numberNew vì bây giờ điểm bắt đầu luôn là columns-1
            // Hiệu ứng 1: Đổi màu đậm
            CreateEffectBelow(columns-1, j, cellModels, color, false, false, strength, rows, columns);
            yield return new WaitForSeconds(0.07f);
            if (j == 1)
            {
                CreateEffect(rows - 1, numberZero, cellModels, color, true, false, 16f);
                yield return new WaitForSeconds(0.07f);
            }


            // Nếu đã xử lý hàng trước đó, áp dụng hiệu ứng 2 (giảm độ đậm) cho nó
            if (j > 1)
            {
                CreateEffectBelow(columns-1, j - 1, cellModels, color, true, false, 16f, rows, columns);
                yield return new WaitForSeconds(0.07f);
            }
            if (j == 2)
            {
                CreateEffect(rows - 1, numberZero, cellModels, color, true, true, 13f);
                yield return new WaitForSeconds(0.07f);
            }
            // Nếu đã xử lý hàng j-2 trước đó, áp dụng hiệu ứng 3 (trở về màu ban đầu) cho nó
            if (j > 2)
            {
                CreateEffectBelow(columns-1, j - 2, cellModels, color, true, true, 13f, rows, columns);
                yield return new WaitForSeconds(0.07f);
            }

        }

        // Cần xử lý các hiệu ứng cuối cùng cho các hàng cuối
        if (columns > 1)
        {
            // Hiệu ứng 2 cho hàng kế cuối
            CreateEffectBelow(columns - 1, columns - 1, cellModels, color, true, false, 16f, rows, columns);
            yield return new WaitForSeconds(0.07f);
        }

        if (columns > 2)
        {
            // Hiệu ứng 3 cho hàng kế cuối
            CreateEffectBelow(columns - 1, columns - 2, cellModels, color, true, true, 13f, rows, columns);
            yield return new WaitForSeconds(0.07f);
        }

        // Hiệu ứng 3 cho hàng cuối cùng
        CreateEffectBelow(columns - 1, columns - 1, cellModels, color, true, true, 13f, rows, columns);


    }
    public void CreateEffect(int x ,int y, Dictionary<Vector2, CellController> cellControllers, EffectColor color, bool shouldDecrease , bool normalColor,  float strength)
    {
        int currentX = x;
        int currentY = y;

        if (currentX == 0 && currentY == 0)
        {
            ColorCell(ref currentX,ref currentY,ref cellControllers,ref color,ref strength,ref shouldDecrease,ref normalColor);
        }
        else
        {
            while (currentX!= 0)
            {
                ColorCell(ref currentX, ref currentY, ref cellControllers, ref color, ref strength, ref shouldDecrease, ref normalColor);
                currentX--;
                currentY++;
            }
            ColorCell(ref currentX, ref currentY, ref cellControllers, ref color, ref strength, ref shouldDecrease, ref normalColor);
        }

    }
    public void CreateEffectBelow(int x, int y, Dictionary<Vector2, CellController> cellControllers, EffectColor color, bool shouldDecrease, bool normalColor, float strength, int rows, int columns)
    {
        // Luôn bắt đầu từ cột cuối cùng (columns-1) và hàng tương ứng y
        int currentX = columns - 1;  // Điểm bắt đầu luôn từ cột cuối cùng
        int currentY = y;

        // Xử lý từng điểm trên đường chéo
        // Điều kiện phù hợp với x++ (tăng dần) và y-- (giảm dần)
        while (currentY < rows-1)
        {
            Vector2 key = new Vector2(currentX, currentY);
            if (cellControllers.ContainsKey(key))
            {
                // Debug.Log($"EffectBelow: Áp dụng màu tại ({currentX},{currentY})");
            }
            ColorCell(ref currentX, ref currentY, ref cellControllers, ref color, ref strength, ref shouldDecrease, ref normalColor);

            // Di chuyển theo đường chéo xuống (tăng x, giảm y)
            currentX--;
            currentY++;
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
        Color getColor = HelperColor.Instance.GetColor(color);
        cellController.CreateEffect(ref getColor, ref strength, ref shouldDecrease, ref normalColor);
    }



}
