using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordChainGame : BaseScreen
{
    public BoardController boardController;
    public UIWordChainView uiview;
    public UIWordchainController controller;
    public override void Hide()
    {
        base.Hide();
    }

    public override void Init()
    {
        base.Init();
        boardController = GetComponentInChildren<BoardController>();

    }

    public override void Show(object data)
    {
        base.Show(data);
        if (data is string categoryName)
        {
            this.Broadcast(EventID.sendWordUI, uiview);
            this.Broadcast(EventID.sendWordController, controller);
            boardController.StartGame(categoryName);
        }

    }
}
