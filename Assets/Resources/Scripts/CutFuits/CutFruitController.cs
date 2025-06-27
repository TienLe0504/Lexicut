using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CutFruitController : MonoBehaviour
{
    public CutFruits cutFruits;
    public CutFruitsView uiView;
    public List<ItemCutView> listCutFruitsController = new List<ItemCutView>();
    public CutFruitsModal model = new CutFruitsModal();
    public List<int> numberWord = new List<int>() { 1, 2, 4, 5};
    public float temp = 0f;
    public float timeEachGround = 15f;
    public float timeToPlay = 75f;
    public float Width;
    public float YStart;
    public float heightItem = 150f;
    public float posRandomYFrom;
    public float posRandomYTo;
    public float size = 0.13f;
    public float rateRandomWidth = 0.06f;
    public float ligthFactor = 0.5f;
    public float sizeBlockWidthX;
    private Coroutine spawnCoroutine;
    private string nameBomb = "Bomb";
    public bool isPressed = false;
    private void Awake()
    {
        YStart = - ManagerGame.Instance.HeightScreen/2 - heightItem;
        Width = ManagerGame.Instance.WidthScreen;
        sizeBlockWidthX = size* ManagerGame.Instance.WidthScreen;
        posRandomYFrom = 0.3f* ManagerGame.Instance.HeightScreen / 2;
        posRandomYTo = 0.6f* ManagerGame.Instance.HeightScreen / 2;
    }



    public void StartGame()
    {
        model.SetDefaul();
        SelectedWord();
        uiView.ShowWord(model.wordChoosen);

    }
    public void PlayGame()
    {
        StartCoroutine(CountTimeToPlay());
    }
    IEnumerator CountTimeToPlay()
    {
        float time = 3f;
        while (time > 0) { 
           if(time<= 3f)
            {
                EffectTimeCountdown((int)time);
            }
            yield return new WaitForSeconds(1f);
            time -= 1f;
        }
        CreateNumber();
        StartCoroutine(GameLoop());

    }

IEnumerator GameLoop()
{
    float totalGameTime = timeToPlay;

    while (totalGameTime > 0)
    {
        CoreGame();
        CreateWord(model.wordCurrent);

        float roundTime = timeEachGround;

        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);
        spawnCoroutine = StartCoroutine(SpawnItemsLoop(12f)); // chỉ tạo trong 12 giây

        while (roundTime > 0)
        {
            ShowTime(totalGameTime);

            if (roundTime <= 3f)
            {
                EffectTimeCountdown((int)roundTime);
            }

            yield return new WaitForSeconds(1f);
            roundTime -= 1f;
            totalGameTime -= 1f;

            if (totalGameTime <= 0)
            {
                StopCoroutine(spawnCoroutine);
                break;
            }
        }

        // Dừng tạo trái cây khi hết vòng
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    // TODO: Hiển thị kết thúc game
    yield return new WaitForSeconds(1f);
        ShowPopupScore();

    }
    public void ShowPopupScore()
    {
        Debug.Log("Tổng số điểm: " + uiView.textScore.text);
        cutFruits.ChangeSceence();
    }


    IEnumerator SpawnItemsLoop(float duration)
    {
        yield return new WaitForSeconds(1f);

        float timer = 0f;
        while (timer < duration)
        {
            ShowItemCut(); // Gọi hàm tạo trái cây
            yield return new WaitForSeconds(3.5f);
            timer += 3.5f;
        }
    }

    public void CreateNumber()
    {
        numberWord = new List<int>() { 1, 2, 4, 5 };
        int randomIndex = Random.Range(0, 100);
        if (randomIndex > 50)
        {
            numberWord.Add(4);
        }
        else
        {
            numberWord.Add(3);
        }
    }
    

    public void SelectedWord()
    {
        string path = CONST.PATH_CUT_FRUITS;
        //List<string> listword = ResourceManager.Instance.GetList(path);
        List<string> listword = ResourceManager.Instance.LoadJson<List<string>>(path);

        int randomIndex = 0;
        for (int i = 0; i < listword.Count; i++)
        {
            randomIndex = Random.Range(i, listword.Count);
            (listword[i], listword[randomIndex]) = (listword[randomIndex], listword[i]);
        }
        model.wordDiff = listword.GetRange(5, 10);
        model.wordChoosen = listword.GetRange(0, 5);
    }

    public void CoreGame()
    {
        if (numberWord.Count > 0)
        {
            int randomIndex = Random.Range(0, numberWord.Count);
            int number = numberWord[randomIndex];
            numberWord.Remove(number);
            List<string> randomWord = new List<string>();
            model.wordCurrent = new List<string>();
            foreach(string w in model.wordChoosen)
            {
                randomWord.Add(w);
            }
            while (number > 0)
            {
                int index = Random.Range(0, randomWord.Count);
                string word = randomWord[index];
                model.wordCurrent.Add(word);
                randomWord.Remove(word);
                number--;
            } 
            uiView.ShowContent(model.wordCurrent);
        }
    }

    public void ShowItemCut()
    {
        //StartCoroutine(ShowAllItem(model.wordCurrent));
        ShowAllItem(model.wordCurrent);

    }
    void  ShowAllItem(List<string> wordlist)
    {
        List<float> listPosX = new List<float>();
        listPosX = CreatePosXList();
        int numberSubstract = 5 - wordlist.Count;
        if (numberSubstract > 0)
        {
            List<string> wordDiff = ShuffleWord(model.wordDiff, numberSubstract);
            foreach (string word in wordDiff)
            {
                CreateItemCut(listPosX, word);
            }
        }
        foreach (string word in wordlist)
        {
            CreateItemCut(listPosX, word);
        }
        int randomBomb = Random.Range(1, 10);
        if (randomBomb > 6)
        {
            CreateItemCut(listPosX, nameBomb);
        }
        //yield return new WaitForSeconds(2.5f);
    }

    private void CreateItemCut(List<float> listPosX, string word)
    {
        ItemType itemType = ItemType.item;
        float posYStartAndFall = YStart;
        int index = Random.Range(0, listPosX.Count);
        float posXStart = listPosX[index];
        listPosX.RemoveAt(index);
        //ItemCutModel item = Instantiate(uiView.prefabItemcut, uiView.tranformItemCut.transform);
        ItemCutView itemCutView = Instantiate(uiView.prefabItemcut, uiView.tranformItemCut.transform);
        ItemCutModel item = new ItemCutModel();
        itemCutView.GetComponent<RectTransform>().localPosition = new Vector3(posXStart, posYStartAndFall, 0f);
        float posYTo = Random.Range(posRandomYFrom, posRandomYTo); // y den
        float posXTo = posXStart + Random.Range(-Width * rateRandomWidth, Width * rateRandomWidth); // x den
        float posXFall = 0;
        if (posXTo <= 0)
        {
            posXFall = posXTo + Random.Range(-Width * rateRandomWidth, 0);
        }
        if (posXTo > 0)
        {
            posXFall = posXTo + Random.Range(0, Width * rateRandomWidth);
        }
        if(word == nameBomb)
        {
            itemType = ItemType.bomb;
        }
        item.SetUp(itemType, word, posXTo, posYTo, posXFall, posYStartAndFall);
        itemCutView.Setup(GetImage(word), item, this);
        AddToModel(item, itemCutView);
        uiView.ItemMove(item, itemCutView);
    }

    public List<float> CreatePosXList()
    {
        List<float> listPosX = new List<float>();
        listPosX.Add(0f);
        int i = 3;
        int j = 3;
        while (i > 0)
        {
            listPosX.Add(sizeBlockWidthX * i);
            i--;
        }
        while (j > 0)
        {
            listPosX.Add(-sizeBlockWidthX * j);
            j--;
        }
        return listPosX;
    }
    public Sprite GetImage(string word)
    {
        string path = CONST.PATH_IMG_CUT_FRUITS + word;
        //return ResourceManager.Instance.GetImage(path);
        return ResourceManager.Instance.GetResource<Sprite>(path);
    }

    public List<string> CreateWord(List<string> wordCurrent)
    {
        List<string> items = new List<string>();
        int randomIndex = Random.Range(0, 100);

        if (wordCurrent.Count == 1)
        {
            if(randomIndex > 50)
            {
                items.Add(wordCurrent[0]);
                items.Add(wordCurrent[0]);
            }
            else
            {
                items.Add(wordCurrent[0]);
            }
        }
        if(wordCurrent.Count == 2)
        {
            if (randomIndex > 50)
            {
                items = ShuffleWord(wordCurrent, 2);
            }
            else
            {
                items = ShuffleWord(wordCurrent, 1);
            }
        }
        if (wordCurrent.Count == 3)
        {
            if (randomIndex > 50)
            {
                items = ShuffleWord(wordCurrent, 3);
            }
            else
            {
                items = ShuffleWord(wordCurrent, 2);
            }
        }
        if (wordCurrent.Count == 4)
        {
            int randomNumber = Random.Range(3, 5);
            items = ShuffleWord(wordCurrent, randomNumber);
            
        }
        if (wordCurrent.Count == 5)
        {
            int randomNumber = Random.Range(4, 6);
            items = ShuffleWord(wordCurrent, randomNumber);
        }
        return items;
    }
    public List<string> ShuffleWord(List<string> wordCurrent, int limit)
    {
        List<string> shuffledWords = new List<string>();
        for (int i = 0; i < wordCurrent.Count; i++)
        {
            int randomIndex = Random.Range(i, wordCurrent.Count);
            (wordCurrent[i], wordCurrent[randomIndex]) = (wordCurrent[randomIndex], wordCurrent[i]);
        }
        shuffledWords = wordCurrent.GetRange(0, limit);
        return shuffledWords;
    }

    public void ShowTime(float timeLeft)
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);
        string formatted = $"{minutes}:{seconds:00}";
        uiView.ShowTime(formatted);
    }
    private void EffectTimeCountdown(int start)
    {
        StartCoroutine(uiView.ShowTimeCount(start));
    }
    public void CreateExplode(ItemCutModel itemCut,ItemCutView itemCutView)
    {
        if (!isPressed) return; 
        float posX = itemCutView.GetComponent<RectTransform>().localPosition.x;
        float posY = itemCutView.GetComponent<RectTransform>().localPosition.y;
        ItemType itemType = itemCut.itemType;
        string word = itemCut.name;
        
        RemoveItem(itemCut, itemCutView);
        Destroy(itemCutView.gameObject);
        if (itemType == ItemType.item)
        {
            bool isWord = IsWordCurrent(word);
            if (isWord)
            {
                model.AddCombo();
                model.CalculateScore();
                uiView.AddShowcore(model.score);
                if (model.combo >0) {
                    uiView.ShowTextCombo(model.combo);
                }
                uiView.ShowTextPerfect(CONST.TEXT_PERFECT, HelperColor.Instance.GetColor(effectColor.Green), true);
            }
            else
            {
                uiView.AddShowcore(-1);
                model.SetDefaul();
                uiView.ShowTextPerfect(CONST.TEXT_MISS, HelperColor.Instance.GetColor(effectColor.White));
            }
            Color orginal = HelperColor.Instance.GetColorOnName(word);
            Color decrease = HelperColor.Instance.DecreaseColor(orginal,ligthFactor);
            ManagerParticelSystem.Instance.CreateParticelSystem(posX, posY, ManagerParticelSystem.Instance.explodeFruit, uiView.TranformExplode, 1f, orginal, decrease, true);
        }
        else
        {
            uiView.AddShowcore(-3);
            model.SetDefaul();
            uiView.ShowTextPerfect(CONST.TEXT_EXPLODE, HelperColor.Instance.GetColor(effectColor.Yellow));
            ManagerParticelSystem.Instance.CreateParticelSystem(posX, posY, ManagerParticelSystem.Instance.explodeBomb, uiView.TranformExplode, 1f, Color.white,Color.white);
        }
    }

    public bool IsWordCurrent(string word)
    {
        return model.wordCurrent.Contains(word);
    }

    public void AddToModel(ItemCutModel item,ItemCutView itemView)
    {
        model.AddItem(item);
        //model.AddController(itemView);
        AddController(itemView);

    }
    public void RemoveItem(ItemCutModel item, ItemCutView itemView) {
        model.RemoveItem(item);
        // model.RemoveController(itemView);
        RemoveController(itemView);

    }
    public void AddController(ItemCutView controller)
    {
        listCutFruitsController.Add(controller);
    }
    public void RemoveController(ItemCutView controller)
    {
        listCutFruitsController.Remove(controller);
    }
    public void SetOnIsPress()
    {
        isPressed = true;
    }
    public void SetOffIsPress()
    {
        isPressed = false;
    }
    
    public TrailRenderer CreateTrail(TrailRenderer trail,GameObject TrailTranform, Vector3 pos)
    {
        TrailRenderer newTrail = TrailPoolManager.Instance.GetFromPool(TrailType.Trail, trail, TrailTranform.transform, pos, Quaternion.identity);
        newTrail.Clear();
        return newTrail;
    }
    public void ReturnTrailPool( TrailType type, TrailRenderer ps)
    {
        TrailPoolManager.Instance.ReturnToPool(type, ps);
    }

}
