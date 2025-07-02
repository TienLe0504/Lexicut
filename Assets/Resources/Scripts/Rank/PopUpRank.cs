using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpRank : BasePopup
{
    public RankModel rankModel = new RankModel();
    public PopUpRankView popUpRankView;
    public override void Hide()
    {
        base.Hide();
    }

    public override void Init()
    {
        base.Init();

    }

    public override void Show(object data)
    {
        base.Show(data);
        this.Broadcast(EventID.Back);
        this.Broadcast(EventID.SetStoreActive);
        this.Broadcast(EventID.ShowInventoryButton);
        LoadListUser();
        SortListUser();
        GetCurrentUser();
        SetUp();
        gameObject.SetActive(true);
        popUpRankView.OpenTranform();

    }
    public void LoadListUser()
    {
        rankModel.users = ResourceManager.Instance.LoadFromFile<List<User>>(CONST.PATH_RANK, CONST.KEY_RANK);
        Debug.Log("rank model "+ rankModel.users.Count);
    }
    public void SortListUser()
    {
        rankModel.users.Sort((a, b) => b.score.CompareTo(a.score));
    }
    public void GetCurrentUser()
    {
        User currentUser = rankModel.users.Find(u => u.id == CONST.KEY_ID);
        rankModel.SetUpCurrentUser(currentUser);
    }
    public void SetUp()
    {
        popUpRankView.SetUp(rankModel.users);
    }
    public void PressButton()
    {
        SoundManager.Instance.PressButton();
    }
}
