using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class UIWordchainController : MonoBehaviour
{
    public UIWordChainView uiView;
    private bool needToRegisterEvents = false;

    private void Awake()
    {
        uiView = GetComponent<UIWordChainView>();
    }
    private void OnEnable()
    {
        this.Register(EventID.SendImage, OnReceiveImage);
        this.Register(EventID.Moves, CalculateMoves);
        this.Register(EventID.SendWordToUI, Recieve);
    }
    private void OnDisable()
    {
        this.Unregister(EventID.SendImage, OnReceiveImage);
        this.Unregister(EventID.Moves, CalculateMoves);
        this.Unregister(EventID.SendWordToUI, Recieve);
    }
    void Start()
    {
        uiView = GetComponent<UIWordChainView>();
    }
    public void Recieve(object data)
    {
        if (uiView == null)
        {
            if(data is UIWordChainView value)
            {
                uiView = value;
            }
        }
    }

    public void CalculateMoves(object data)
    {
       uiView.numberMoves -= 1; 
        int number = uiView.numberMoves;
        if (number>-1)
        {
            uiView.ShowMoves(number);
        }
        if (number == 0)
        {
            this.Broadcast(EventID.ShowEndGamePopup);
        }
    }


    private void OnReceiveImage(object data)
    {
        if (data is Dictionary<string, List<string>> value)
        {
            SetupView(value);
        }
        else
        {
            Debug.LogError("Invalid data type received in SendImage event.");
        }
    }

    private void SetupView(Dictionary<string, List<string>> value)
    {
        string category = null;
        foreach (var kvp in value)
        {
            category = kvp.Key;
            break;
        }
        if (uiView == null)
        {
            uiView = GetComponent<UIWordChainView>();
            Debug.Log("null");
        }
        uiView.TurnOffShowBG();
        uiView.SetupTitleAndImg(category);
        List<string> listWord = value[category];
        Debug.Log("HandleCellSelected called with listWord: " + string.Join(", ", listWord) + " " + category + "...............");
        List<Sprite> listImage = new List<Sprite>();
        foreach (string word in listWord)
        {
            string path = CONST.PATH_IMG + category + "/" + word;
            Sprite sprite = ResourceManager.Instance.GetResource<Sprite>(path);

            if (sprite != null)
            {
                Debug.Log($"Image found at path: {path}");
                listImage.Add(sprite);
            }
            else
            {
                Debug.LogWarning($"Image not found at path: {path}");
            }
        }
        if (listImage.Count > 0)
        {
            uiView.DisplayImages(listImage);
        }
        else
        {
            Debug.LogWarning("No images to display.");
        }
    }
    public void ExitGame() {
        UIManager.Instance.ShowScreen<HomeScreen>();
        SoundManager.Instance.StopLoopingMusic();
        SoundManager.Instance.PlayLoopingMusic(SoundManager.Instance.backgroundGame);
    }
    public void PlayGameAgain()
    {
        uiView.SetupGame();
        uiView.ShowMoves(uiView.numberMoves);
        this.Broadcast(EventID.PlayGameAgain);
    }
    public Sprite GetImage(string type, ListImage listImage)
    {
        if (listImage.ImageDictionary.TryGetValue(type, out Sprite sprite))
        {
            return sprite;
        }
        return null;
    }
    public void PressButton()
    {
        SoundManager.Instance.PressButton();
    }
}
