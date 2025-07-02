using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpRankView : MonoBehaviour
{
    public ObjectRank objectRankPrefab;
    public Transform contentTransform;
    public ScrollRect scrollRect; 
    public PopUpRank popUpRank;
    public Button btnClose;
    public TextMeshProUGUI numberUserCurrent;
    public TextMeshProUGUI usernameUserCurrent;
    public TextMeshProUGUI scoreUserCurrent;
    public Tween openTween;
    public Tween closeTween;

    private void Awake()
    {
        btnClose.onClick.AddListener(() => CloseShopTranform());
    }

    public void SetUp(List<User> users)
    {
        GridLayoutGroup grid = contentTransform.GetComponent<GridLayoutGroup>();
        float cellHeight = grid.cellSize.y;
        float spacingY = grid.spacing.y;

        int childCount = contentTransform.childCount;

        for (int i = 0; i < users.Count; i++)
        {
            ObjectRank objectRank;

            if (i < childCount)
            {
                objectRank = contentTransform.GetChild(i).GetComponent<ObjectRank>();
                objectRank.gameObject.SetActive(true);
            }
            else
            {
                objectRank = Instantiate(objectRankPrefab, contentTransform);
            }

            objectRank.Setup(users[i], i + 1);

            if (users[i].id == CONST.KEY_ID)
            {
                numberUserCurrent.text = (i + 1).ToString() + ".";
                usernameUserCurrent.text = users[i].username;
                scoreUserCurrent.text = users[i].score.ToString();
            }
        }

        for (int i = users.Count; i < childCount; i++)
        {
            contentTransform.GetChild(i).gameObject.SetActive(false);
        }

        float totalHeight = users.Count * (cellHeight + spacingY) - spacingY;
        RectTransform contentRect = contentTransform.GetComponent<RectTransform>();
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, totalHeight);
        StartCoroutine(ScrollToTopNextFrame());
    }
    public void OpenTranform()
    {
        RectTransform rectTransform = this.GetComponent<RectTransform>();

        if (openTween != null && openTween.IsActive()) openTween.Kill();
        if (closeTween != null && closeTween.IsActive()) closeTween.Kill();

        rectTransform.localScale = Vector3.zero;
        openTween = rectTransform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
    }
    public void CloseShopTranform()
    {
        popUpRank.PressButton();
        RectTransform rectTransform = this.GetComponent<RectTransform>();
        if (openTween != null && openTween.IsActive()) openTween.Kill();
        if (closeTween != null && closeTween.IsActive()) closeTween.Kill();

        rectTransform.localScale = Vector3.one;
        closeTween = rectTransform.DOScale(0f, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            this.Broadcast(EventID.ShowRankButton);
            popUpRank.Hide();
        });
    }
    private IEnumerator ScrollToTopNextFrame()
    {
        yield return null; 
        scrollRect.verticalNormalizedPosition = 1f;
    }

}
