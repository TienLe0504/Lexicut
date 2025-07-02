using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CutFruitsView : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    // === Controller ===
    public CutFruitController controller;

    // === Main Camera ===
    private Camera mainCamera;

    // === Trail Handling ===
    public TrailRenderer trail;                 // Trail prefab từ ManagerGame
    public TrailRenderer newTrail;             // Trail instance đang vẽ
    public GameObject trailTranForm;           // Vị trí parent chứa trail

    // === Item Cut Setup ===
    public GameObject tranformItemCut;         // Transform chứa các ItemCut
    public ItemCutView prefabItemcut;          // Prefab để spawn ItemCut
    public GameObject TranformExplode;         // Transform chứa hiệu ứng nổ

    // === Word Display UI ===
    public GameObject popupShowWord;           // Popup hiển thị từ vựng
    public GameObject prefabImgWord;           // Prefab chứa ảnh + chữ từ vựng
    public GameObject tranformImgWord;         // Transform chứa các prefabImgWord

    // === UI - Text Components ===
    public TextMeshProUGUI textContent;        // Hiển thị từ cần chém
    public TextMeshProUGUI timeText;           // Hiển thị thời gian đếm ngược chính
    public TextMeshProUGUI timeCount;          // Hiển thị thời gian đếm lùi (3,2,1)
    public TextMeshProUGUI textScore;          // Hiển thị điểm số
    public TextMeshProUGUI textCombo;          // Hiển thị combo
    public TextMeshProUGUI textPerfect;        // Hiển thị "Perfect" hoặc "Miss"

    // === Buttons ===
    public Button btnPlayGame;                 // Nút bắt đầu chơi

    // === Effects & Animation ===
    public GameObject effectParrent;           // Vị trí spawn hiệu ứng
    private Tween tweenCombo;                  // Tween combo
    private Tween tweenPerfect;                // Tween perfect/miss
    private Tween tweenScore;                  // Tween điểm số
    private Tween tweenWord;                   // Tween từ vựng



    private void Awake()
    {
        SetupBackGround();
        popupShowWord.SetActive(false);
        btnPlayGame.onClick.AddListener(()=>PlayGame());

        mainCamera = Camera.main;
    }
    public void SetupBackGround()
    {
        Image bg = this.transform.GetComponent<Image>();
        bg.rectTransform.sizeDelta = new Vector2(ManagerGame.Instance.WidthScreen, ManagerGame.Instance.HeightScreen);
        timeCount.gameObject.SetActive(false);
    }

    public void ShowScore(int score)
    {
        textScore.text = score.ToString();
        if (tweenScore != null && tweenScore.IsActive() && tweenScore.IsPlaying())
        {
            tweenScore.Kill();
        }
        tweenScore = textScore.transform.DOScale(1.1f, 0.2f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            tweenScore = textScore.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
        });

    }
    public void ShowContent(List<string> word)
    {
        textContent.text = string.Join(", ", word);
        if(tweenWord != null && tweenWord.IsActive() && tweenWord.IsPlaying())
        {
            tweenWord.Kill();
        }
        tweenWord = textContent.transform.DOScale(1.2f, 0.2f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            tweenWord = textContent.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
        });
    }
    public void ShowTime(string time)
    {
        timeText.text = time;
    }
    public IEnumerator ShowTimeCount(int start)
    {
        for (int i = start; i > 0; i--)
        {
            controller.PlayCountdownSound();
            timeCount.text = i.ToString();
            timeCount.transform.localScale = Vector3.zero;
            timeCount.gameObject.SetActive(true);

            timeCount.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(1f);
        }

        timeCount.gameObject.SetActive(false);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        controller.SetOnIsPress();
        newTrail = controller.CreateTrail(ManagerGame.Instance.trailCurrent, trailTranForm, GetWorldPosition(eventData));
        newTrail.GetComponent<RectTransform>().position = GetWorldPosition(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        newTrail.GetComponent<RectTransform>().position = GetWorldPosition(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        controller.SetOffIsPress();
        controller.ReturnTrailPool(TrailType.Trail, newTrail);
    }

    private Vector3 GetWorldPosition(PointerEventData eventData)
    {
        return mainCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, 10));
    }
    public void ItemMove(ItemCutModel item, ItemCutView itemCutView)
    {
        RectTransform itemRect = itemCutView.GetComponent<RectTransform>();

        itemRect.DORotate(new Vector3(0f, 0f, 360f), 3f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1);

        Sequence mySequence = DOTween.Sequence();

        mySequence.Append(
            itemRect.DOLocalMove(new Vector3(item.posXTo, item.posYTo, 0f), 1.4f)
                    .SetEase(Ease.OutSine)
        );

        mySequence.Append(
            itemRect.DOLocalMove(new Vector3(item.posXFall, item.posYFall, 0f), 1.4f)
                    .SetEase(Ease.InQuad) 
        );

        mySequence.OnComplete(() => {
            controller.RemoveItem(item,itemCutView);
            Destroy(itemCutView.gameObject);
        });
    }
    public void ShowWord(List<string> words)
    {
        ShowScore(0);
        popupShowWord.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -ManagerGame.Instance.HeightScreen, 0);
        popupShowWord.SetActive(true);

        foreach (Transform child in tranformImgWord.transform)
        {
            Destroy(child.gameObject);
        }

        popupShowWord.GetComponent<RectTransform>()
            .DOLocalMove(new Vector3(0, 200f, 0), 1.2f)
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
               WaiteToCreateWord(words);
            });
    }
    public void WaiteToCreateWord(List<string> word)
    {
                StartCoroutine(CreateWord(word));
    }
    IEnumerator CreateWord(List<string> words)
    {
        foreach (string word in words)
        {
            controller.PlayWordAppearSound();
            GameObject imgWord = Instantiate(prefabImgWord, tranformImgWord.transform);
            imgWord.GetComponentInChildren<TextMeshProUGUI>().text = " - " + word;

            Sprite sprite = controller.LoadItemSprite(word);
            imgWord.GetComponent<Image>().sprite = sprite;

            imgWord.transform.localScale = Vector3.zero;
            imgWord.transform.DOScale(1f, 0.8f).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(0.2f);
        }
    }
    public void AddShowcore(int score)
    {
        int currentScore = int.Parse(textScore.text);
        currentScore += score;
        if (currentScore<=0)
        {
            currentScore = 0;
        }
        ShowScore(currentScore);
    }

    public void PlayGame()
    {
        controller.PressButton();
        popupShowWord.SetActive(false);
        controller.StartGameplay();
    }

    public void ShowTextCombo(int combo)
    {
        if (tweenCombo != null && tweenCombo.IsActive() && tweenCombo.IsPlaying())
        {
            tweenCombo.Kill();
        }
        textCombo.text = "x" + combo.ToString();
        textCombo.transform.localScale = Vector3.one;
        textCombo.gameObject.SetActive(true);
        tweenCombo = textCombo.transform.DOScale(1.1f, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            DOVirtual.DelayedCall(0.3f, () =>
            {
                textCombo.gameObject.SetActive(false);
            });
        });
    }
    public void ShowTextPerfect(string perfect, Color color, bool isEffect = false)
    {
        if (tweenPerfect != null && tweenPerfect.IsActive())
        {
            tweenPerfect.Kill();
        }
        if (isEffect)
        {
            ManagerParticelSystem.Instance.CreateParticleEffect(effectParrent.GetComponent<RectTransform>().position.x, effectParrent.GetComponent<RectTransform>().position.y, ManagerParticelSystem.Instance.perfectEffectPrefab, effectParrent, 1f, color, color);
        }
        textPerfect.color = color;
        textPerfect.text = perfect;
        textPerfect.transform.localScale = Vector3.zero;
        textPerfect.gameObject.SetActive(true);

        tweenPerfect = textPerfect.transform.DOScale(1.2f, 0.25f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                tweenPerfect = textPerfect.transform.DOScale(0f, 0.8f)
                    .SetEase(Ease.InQuad)
                    .OnComplete(() =>
                    {
                        textPerfect.gameObject.SetActive(false);
                    });
            });
    }


}
