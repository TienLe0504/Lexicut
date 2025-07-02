using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinOrLoseView : MonoBehaviour
{
    public float posX = 0f;
    public float posY = 200f;
    public TextMeshProUGUI title;
    public TextMeshProUGUI textContent;
    public TextMeshProUGUI txtGold;
    public GameObject tranformParrentImg;
    public GameObject tranformParrentFireWork;
    public GameObject starWin;
    public GameObject starLose;
    public Button btnHome;
    public Button btnContinue;
    public Image imgBackground;
    public Image imgItemPrefab;
    public WinOrLosePopup winOrLoseController;
    public List<Image> listImgStar = new List<Image>();
    public void Awake()
    {
        SetupTransform();
        imgBackground.GetComponent<RectTransform>().sizeDelta = new Vector2(ManagerGame.Instance.WidthScreen*5, ManagerGame.Instance.HeightScreen*5);
        imgBackground.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -250f, 0);
        btnHome.onClick.AddListener(() => PressHome());
        btnContinue.onClick.AddListener(() => PressContinue());
    }
    public void SetupTransform()
    {
        this.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, ManagerGame.Instance.HeightScreen);

    }
    public void ShowWinOrLose(string text, bool isWin, string txtAnswer, List<string>word,string category,int star, int gold)
    {

        title.text = text;
        if (isWin)
        {
            starWin.SetActive(true);
            starLose.SetActive(false);
            listImgStar = new List<Image>();
            listImgStar = starWin.GetComponentsInChildren<Image>().ToList();
        }
        else
        {
            starLose.SetActive(true);
            starWin.SetActive(false);
            listImgStar = new List<Image>();
            listImgStar = starLose.GetComponentsInChildren<Image>().ToList();
        }
        SetupText(txtAnswer, gold.ToString());
        SetActiveFalseStar();
        RectTransform rt = this.GetComponent<RectTransform>();

        rt.DOAnchorPos(new Vector2(0, posY), 1.2f)
          .SetEase(Ease.OutQuad)
          .OnComplete(() =>
          {
              SetupStar(star);
              SetupImg(word,category);
              ShowTextContent(textContent,0.3f);
              if (isWin)
              {
                  ShowFireWork();
                  ShowTextContent(txtGold, 0.4f);
              }
          });
        winOrLoseController.PlaySoundWinGame(isWin);

    }
    public void SetupText(string text, string textGold)
    {

        textContent.text =text;
        txtGold.text = textGold;
        textContent.gameObject.SetActive(false);
        txtGold.gameObject.SetActive(false);
    }
    public void ShowTextContent(TextMeshProUGUI text, float time)
    {
        text.gameObject.SetActive(true);
        text.transform.localScale = Vector3.zero;
        text.transform.DOScale(Vector3.one, time).SetEase(Ease.OutBack).OnComplete(() =>
        {
        });
    }
    private void ClearImages()
    {
        if (tranformParrentImg == null)
        {
            Transform found = transform.Find("imgParrent");
            if (found != null)
            {
            }
            else
            {
                Debug.LogError("imgParrent GameObject not found in children of UIWordChainView");
                return;
            }
        }

        List<Image> imglist = tranformParrentImg.GetComponentsInChildren<Image>()
            .Where(img => img.gameObject != tranformParrentImg.gameObject)
            .ToList();
        if (imglist.Count > 0)
        {
            foreach (Image img in imglist)
            {
                if (img != null && img.gameObject != tranformParrentImg)
                {
                    Destroy(img.gameObject);
                }
            }
        }
    }
    public void SetActiveFalseStar()
    {
        foreach (Image img in listImgStar)
        {
            img.gameObject.SetActive(false);
        }
    }
    public void SetupImg(List<string> word, string category)
    {
        ClearImages();
        foreach(string item in word)
        {
            Image img = Instantiate(imgItemPrefab, tranformParrentImg.transform);
            string path = CONST.PATH_IMG +category+"/" + item;
            Debug.Log(" path " + path);
            Sprite sprite = ResourceManager.Instance.GetResource<Sprite>(path);

            img.sprite = sprite;
            img.transform.localScale = Vector3.zero;
            img.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                img.transform.DOShakeScale(0.3f, 0.1f, 10, 90, false);
            });
        }
    }
    public void SetupStar(int star)
    {
        CreatStarEffect(star);
    }
    private void CreatStarEffect(int j)
    {
        for (int i = 0; i < listImgStar.Count; i++)
        {
            if (j == i)
            {
                break;
            }
            listImgStar[i].gameObject.SetActive(true);

            Image img = listImgStar[i];
            img.transform.localScale = Vector3.zero;

            img.transform
                .DOScale(Vector3.one, 0.3f)
                .SetEase(Ease.OutBack)
                .SetDelay(i * 0.1f);
        }
    }
    public void PressHome()
    {
        SetupTransform();
        winOrLoseController.PressHome();
    }
    public void PressContinue()
    {
        SetupTransform();
        winOrLoseController.PressContinue();
    }
    public void ShowFireWork()
    {
        StartCoroutine(ExplodeFrireWork());
    }

    IEnumerator ExplodeFrireWork()
    {
        yield return new WaitForSeconds(1f);
        float temp = 30f;
        while(temp > 0f)
        {
            int i = ManagerParticelSystem.Instance.fireworkPrefabs.Count() - 1;
            while (i > -1)
            {
                Vector2 pos = ManagerParticelSystem.Instance.fireworkPositions[i];
                GameObject firework = ManagerParticelSystem.Instance.fireworkPrefabs[i];
                ManagerParticelSystem.Instance.CreateParticleEffect(pos.x, pos.y, firework, tranformParrentFireWork, 1.5f, Color.white, Color.white);
                yield return new WaitForSeconds(1.5f);
                i--;
                temp -= 1f;
            }
        }

    }

}
