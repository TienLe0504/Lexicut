using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class BoardView : MonoBehaviour
{
    public int rows = 5;
    public int columns = 5;
    public GameObject CellPrefab;
    public float cellSize = 150f;
    public Vector2 additionalOffset = Vector2.zero;
    private float timer = 0f;
    public float interval = 10f;
    GridLayoutGroup gridLayoutGroup;
    public BoardController boardController;
    public Dictionary<Vector2, CellController> cellModels = new Dictionary<Vector2, CellController>();
    public EffectStrategy effectStrategy = new EffectStrategy();

    public void CreateBoard(BoardModel board , BoardController controller)
    {
        float scaleY = 1f;
        float scaleX = 1f;
        boardController = controller;
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        RectTransform parentRect = transform.parent.GetComponent<RectTransform>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(parentRect);
        Canvas parentCanvas = GetComponentInParent<Canvas>();
        RectTransform canvasRect = parentCanvas.GetComponent<RectTransform>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(canvasRect); 
        gridLayoutGroup.constraintCount = columns;
        float spacing = gridLayoutGroup.spacing.x;
        cellSize = gridLayoutGroup.cellSize.x;
        float totalWidth = columns * cellSize + (columns - 1) * spacing;
        float totalHeight = rows * cellSize +(columns -1)*spacing;
        float containerWidth = Screen.width;
        float containerHeight = Screen.height;
        float containerWidth1 = canvasRect.rect.width * 0.85f;
        float containerHeight1 = canvasRect.rect.height*0.85f;
        float currentScreenHeight = containerHeight1 / 2f + totalHeight;
        if(totalWidth> containerWidth1)
        {
            scaleX = (containerWidth1 / 2) / (totalWidth / 2);
        }
        if (currentScreenHeight > containerHeight1)
        {
            float decreaseScale = 0.01f;
            while (currentScreenHeight > containerHeight1)
            {
                scaleY -= decreaseScale;
                currentScreenHeight = containerHeight1 / 2f + totalHeight * scaleY;
            }


        }
        float finalScale = Mathf.Min(scaleX, scaleY);
        GetComponent<RectTransform>().localScale = new Vector3(finalScale, finalScale, 1);
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        cellModels.Clear();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                CellModel cellModel = board.cells[i, j];
                GameObject obj = Instantiate(CellPrefab, this.transform);
                CellController cell = obj.GetComponentInChildren<CellController>();
                cell.CrerateLetter(cellModel.letter, i,j);
                cell.SaveColor(Color.white);
                cellModels[new Vector2(i, j)] = cell;
            }
        }
    }
    private void Update()
    {
        if (ManagerGame.Instance.isUseEffectCell)
        {
            if (cellModels.Count > 0)
            {
                timer += Time.deltaTime;
                if (timer >= 5f)
                {
                    timer = 0f;
                    //Effect effect = new HorizontalLeftEffect();
                    effectStrategy.SetEffect(ManagerGame.Instance.effectCell);
                    MonoBehaviour runner = this;
                    effectColor effectColor = ManagerGame.Instance.colorEffectCurrent;
                    effectStrategy.SetParameters(ref runner,ref cellModels,ref rows,ref columns,ref effectColor);
                    effectStrategy.PerformEffect();
                }
            }
        }

    }
}
