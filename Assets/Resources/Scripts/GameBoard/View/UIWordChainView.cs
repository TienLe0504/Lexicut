using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIWordChainView : MonoBehaviour
{
    private List<Image> currentImages = new List<Image>();
    public TextMeshProUGUI title;
    public TextMeshProUGUI moves;
    public Image ShadowBG;
    public GameObject stopGame;
    public GameObject WinOrLoseGame;
    public Image bgImage;
    public Image imgTitle;
    float temp = 0;
    public int movesCount = 5;
    public int numberMoves = 5;
    public Button btnStopGame;
    public Button btnContinueGame;
    public Button btnPlayAgain;
    public Button btnExitGame;
    public float timeStop = 8f;
    public UIWordchainController controller;
    public Image imgItem;
    public Image imgParrent;
    private void Awake()
    {
        numberMoves = movesCount;
        this.Register(EventID.SendWordToController, Recieve);
        controller = GetComponent<UIWordchainController>();
        bgImage.rectTransform.sizeDelta = new Vector2(ManagerGame.Instance.WidthScreen, ManagerGame.Instance.HeightScreen);
        ShadowBG.rectTransform.sizeDelta = new Vector2(ManagerGame.Instance.WidthScreen, ManagerGame.Instance.HeightScreen);
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(ManagerGame.Instance.WidthScreen, ManagerGame.Instance.HeightScreen);
        ContinueGame();
        btnStopGame.onClick.AddListener(() => { StopGame(); });
        btnContinueGame.onClick.AddListener(() => { ContinueGame(); });
        btnExitGame.onClick.AddListener(() => { ExitGame(); });
        btnPlayAgain.onClick.AddListener(() => { PlayGameAgain(); });

    }
    public void SetupGame()
    {
        numberMoves = movesCount;
        temp = 0;
    }
    private void Start()
    {
        ShowMoves(numberMoves);
    }
    public void Update()
    {
        temp += Time.deltaTime;
        if (temp > timeStop)
        {
            temp = 0f;
            StartCoroutine(ShakeImage());
        }
    }
    public void Recieve(object data)
    {
        if (controller == null)
        {
            if (data is UIWordchainController value)
            {
                controller = value;
            }
        }
        SetupGame();
    }
    public void SetupTitleAndImg(string text)
    {
        title.text = text;
        Sprite sprite = controller.GetImage(text, ManagerGame.Instance.listImage);
        imgTitle.sprite = sprite;
        Sprite bgSprite = controller.GetImage(text, ManagerGame.Instance.listBGImage);
        bgImage.sprite = bgSprite;
    }


    public void ShowImage(Sprite sprite)
    {



        Image img = Instantiate(imgItem, imgParrent.transform);
        img.sprite = sprite;
        img.transform.localScale = Vector3.zero;
        img.transform.localPosition = Vector3.zero;
        currentImages.Add(img);
        img.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            img.transform.DOShakeScale(0.3f, 0.1f, 10, 90, false);
        });
    }

    public void DisplayImages(List<Sprite> images)
    {

        ClearImages();

        foreach (Sprite sprite in images)
        {
            ShowImage(sprite);

        }
    }

    private void ClearImages()
    {
        if (imgParrent == null)
        {
            Transform found = transform.Find("imgParrent");
            if (found != null)
            {
                //imgParrent = found.gameObject;
            }
            else
            {
                Debug.LogError("imgParrent GameObject not found in children of UIWordChainView");
                return;
            }
        }

        List<Image> imglist = imgParrent.GetComponentsInChildren<Image>()
            .Where(img => img.gameObject != imgParrent.gameObject)
            .ToList();
        currentImages = new List<Image>();
        if (imglist.Count > 0)
        {
            foreach (Image img in imglist)
            {
                if (img != null && img.gameObject != imgParrent)
                {
                    Destroy(img.gameObject);
                }
            }
        }
    }

    private IEnumerator ShakeImage()
    {
        for (int i = 0; i < currentImages.Count; i++)
        {
            Image imgObj = currentImages[i];
            Vector3 originalPos = imgObj.transform.localPosition;
            imgObj.transform.DOShakePosition(0.2f, strength: 8f, vibrato: 15, randomness: 90, fadeOut: true).OnComplete(() =>
            {
                imgObj.transform.localPosition = originalPos;

            });
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(0.2f);
        for (int i = currentImages.Count - 1; i >= 0; i--)
        {
            Image imgObj = currentImages[i];
            Vector3 originalPos = imgObj.transform.localPosition;
            imgObj.transform.DOShakePosition(0.2f, strength: 8f, vibrato: 15, randomness: 90, fadeOut: true).OnComplete(() =>
            {
                imgObj.transform.localPosition = originalPos;

            });
            yield return new WaitForSeconds(0.2f);
        }
    }
    public void ShowMoves(int numberMoves)
    {
        moves.text = "Moves: " + numberMoves.ToString();
    }
    public void StopGame()
    {
        ShowBG();
        controller.PressButton();
        stopGame.SetActive(true);
        stopGame.transform.localScale = Vector3.zero;
        stopGame.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).OnComplete(() => {
        });
    }
    public void ContinueGame()
    {
        controller.PressButton();
        ShadowBG.gameObject.SetActive(false);
        stopGame.SetActive(false);
    }
    public void ExitGame()
    {
        controller.PressButton();
        controller.ExitGame();
    }
    public void PlayGameAgain()
    {
        ContinueGame();
        controller.PlayGameAgain();
    }
    public void ShowBG()
    {
        ShadowBG.gameObject.SetActive(true);
    }
    public void ShowEffectBackground()
    {
        ShadowBG.gameObject.SetActive(true);
        ShadowBG.rectTransform.localScale = Vector3.zero;   
        ShadowBG.rectTransform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack).OnComplete(() => {
        });
    }
    public void TurnOffShowBG()
    {
        ShadowBG.gameObject.SetActive(false);
    }
}