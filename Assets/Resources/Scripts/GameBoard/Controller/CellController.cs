using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellController : MonoBehaviour
{
    public CellView cellview;
    public CellModel cellmodel;
    public void CrerateLetter(char c, int i, int j)
    {
        cellmodel = new CellModel
        {
            x = i,
            y = j,
            letter = c,
            isUsed = false
        };
        cellview.SetText(c,this);
    }
    public void OnSelectLetter()
    {
        if (cellmodel.isUsed)
        {
            return;
        }
        cellmodel.isUsed = true;
        this.Broadcast(EventID.HanldeCell, this);
    }
    public void ResetIsUsed()
    {
               cellmodel.isUsed = false;
    }
    public void FalseDot()
    {
        cellview.FalseDot();
    }
    public void TrueDot(Color colorToUse)
    {
        cellview.TrueDot(colorToUse);

    }
   
    public void SaveColor(Color color)
    {
        cellmodel.color = color;
    }
    
    public void CreateEffect(ref Color color,ref float strength, ref bool shoudDecrease, ref bool normalColor)
    {
        cellview.EffectColor(ref color,ref strength,ref shoudDecrease,ref normalColor);
    }


}
