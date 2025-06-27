using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListennerBoardGameManager : BaseManager<ListennerBoardGameManager>
{
    public System.Action<CellController> OnCellSelected;
    public System.Action<List<string>, string> OnGetImage;
    public System.Action MovesFalse;

    public void CellSelected(CellController cell)
    {
        OnCellSelected?.Invoke(cell);
    }
    public void SendImage(List<string> image, string category)
    {
        OnGetImage?.Invoke(image, category);
    }
    public void MovesFalseAction()
    {
        MovesFalse?.Invoke();

    }
}
