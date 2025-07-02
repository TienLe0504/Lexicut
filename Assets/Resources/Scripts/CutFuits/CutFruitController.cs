using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CutFruitController : MonoBehaviour
{
    // === References ===
    public CutFruits cutFruitManager;
    public CutFruitsView cutFruitUI;
    public CutFruitsModal cutFruitModel = new CutFruitsModal();
    public List<ItemCutView> activeItemView = new List<ItemCutView>();

    // === Game Configurations ===
    public List<int> wordCountsPerRound = new List<int>() { 1, 2, 4, 5 };
    public float roundDuration = 15f;
    public float totalGameTime = 75f;

    // === Screen & Layout Settings ===
    public float screenWidth;
    public float startYPosition;
    public float itemHeight = 150f;
    public float itemScale = 0.13f;
    public float itemSpacingX;

    // === Randomization & Effects ===
    public float randomYMin;
    public float randomYMax;
    public float horizontalRandomFactor = 0.06f;
    public float lightenFactor = 0.5f;

    // === Game State ===
    private Coroutine spawnItemCoroutine;
    private string bombKeyword = "Bomb";
    public bool inputPressed = false;

    // === Temporary or Debug ===
    public float temp = 0f;

    private void Awake()
    {
        startYPosition = - ManagerGame.Instance.HeightScreen/2 - itemHeight;
        screenWidth = ManagerGame.Instance.WidthScreen;
        itemSpacingX  = itemScale * ManagerGame.Instance.WidthScreen;
        randomYMin = 0.3f* ManagerGame.Instance.HeightScreen / 2;
        randomYMax = 0.6f* ManagerGame.Instance.HeightScreen / 2;
    }



    public void InitializeGame()
    {
        cutFruitModel.SetDefaul();
        ShuffleAndSelectWords();
        PlayGameMusic();
        cutFruitUI.ShowWord(cutFruitModel.wordChoosen);

    }
    public void StartGameplay()
    {
        StartCoroutine(StartCountdownToStart());
    }
    public void PlayGameMusic()
    {
        SoundManager.Instance.PlayLoopingMusic(SoundManager.Instance.cutFruitsGame,CONST.KEYMUSIC,false,1f);
    }
    IEnumerator StartCountdownToStart()
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
        GenerateWordCountPerRound();
        StartCoroutine(RunGameLoop());

    }

IEnumerator RunGameLoop()
{
    float totalGameTime = this.totalGameTime;

    while (totalGameTime > 0)
    {
        SetupRoundWords();
        GenerateWordSet(cutFruitModel.wordCurrent);

        float roundTime = roundDuration;

        if (spawnItemCoroutine != null)
            StopCoroutine(spawnItemCoroutine);
        spawnItemCoroutine = StartCoroutine(SpawnItemsLoop(12f)); 

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
                StopCoroutine(spawnItemCoroutine);
                break;
            }
        }

        if (spawnItemCoroutine != null)
        {
            StopCoroutine(spawnItemCoroutine);
            spawnItemCoroutine = null;
        }
    }

    yield return new WaitForSeconds(1f);
        DisplayFinalScorePopup();

    }
    public void DisplayFinalScorePopup()
    {
        cutFruitManager.ChangeSceence();
    }


    IEnumerator SpawnItemsLoop(float duration)
    {
        yield return new WaitForSeconds(1f);

        float timer = 0f;
        while (timer < duration)
        {
            DisplayCuttableItems(); 
            yield return new WaitForSeconds(3.5f);
            timer += 3.5f;
        }
    }

    public void GenerateWordCountPerRound()
    {
        wordCountsPerRound = new List<int>() { 1, 2, 4, 5 };
        int randomIndex = Random.Range(0, 100);
        if (randomIndex > 50)
        {
            wordCountsPerRound.Add(4);
        }
        else
        {
            wordCountsPerRound.Add(3);
        }
    }
    

    public void ShuffleAndSelectWords()
    {
        string path = CONST.PATH_CUT_FRUITS;
        List<string> listword = ResourceManager.Instance.LoadFromResources<List<string>>(path);

        int randomIndex = 0;
        for (int i = 0; i < listword.Count; i++)
        {
            randomIndex = Random.Range(i, listword.Count);
            (listword[i], listword[randomIndex]) = (listword[randomIndex], listword[i]);
        }
        cutFruitModel.wordDiff = listword.GetRange(5, 10);
        cutFruitModel.wordChoosen = listword.GetRange(0, 5);
    }

    public void SetupRoundWords()
    {
        if (wordCountsPerRound.Count > 0)
        {
            int randomIndex = Random.Range(0, wordCountsPerRound.Count);
            int number = wordCountsPerRound[randomIndex];
            wordCountsPerRound.Remove(number);
            List<string> randomWord = new List<string>();
            cutFruitModel.wordCurrent = new List<string>();
            foreach(string w in cutFruitModel.wordChoosen)
            {
                randomWord.Add(w);
            }
            while (number > 0)
            {
                int index = Random.Range(0, randomWord.Count);
                string word = randomWord[index];
                cutFruitModel.wordCurrent.Add(word);
                randomWord.Remove(word);
                number--;
            }
            PlayCorrectWordSound();
            cutFruitUI.ShowContent(cutFruitModel.wordCurrent);
        }
    }
    public void PlayCorrectWordSound()
    {
        SoundManager.Instance.PlayOneShotSound(SoundManager.Instance.correctWord);
    }

    public void DisplayCuttableItems()
    {
        SpawnAllItems(cutFruitModel.wordCurrent);

    }
    void  SpawnAllItems(List<string> wordlist)
    {
        List<float> listPosX = new List<float>();
        listPosX = GenerateSpawnXPositions();
        int numberSubstract = 5 - wordlist.Count;
        if (numberSubstract > 0)
        {
            List<string> wordDiff = ShuffleAndPickWords(cutFruitModel.wordDiff, numberSubstract);
            foreach (string word in wordDiff)
            {
                SpawnItemCutView(listPosX, word);
            }
        }
        foreach (string word in wordlist)
        {
            SpawnItemCutView(listPosX, word);
        }
        int randomBomb = Random.Range(1, 10);
        if (randomBomb > 6)
        {
            SpawnItemCutView(listPosX, bombKeyword  );
        }
    }

    private void SpawnItemCutView(List<float> listPosX, string word)
    {
        ItemType itemType = ItemType.item;
        float posYStartAndFall = startYPosition;
        int index = Random.Range(0, listPosX.Count);
        float posXStart = listPosX[index];
        listPosX.RemoveAt(index);
        ItemCutView itemCutView = Instantiate(cutFruitUI.prefabItemcut, cutFruitUI.tranformItemCut.transform);
        ItemCutModel item = new ItemCutModel();
        itemCutView.GetComponent<RectTransform>().localPosition = new Vector3(posXStart, posYStartAndFall, 0f);
        float posYTo = Random.Range(randomYMin, randomYMax);
        float posXTo = posXStart + Random.Range(-screenWidth * horizontalRandomFactor, screenWidth * horizontalRandomFactor); 
        float posXFall = 0;
        if (posXTo <= 0)
        {
            posXFall = posXTo + Random.Range(-screenWidth * horizontalRandomFactor, 0);
        }
        if (posXTo > 0)
        {
            posXFall = posXTo + Random.Range(0, screenWidth * horizontalRandomFactor);
        }
        if(word == bombKeyword  )
        {
            itemType = ItemType.bomb;
        }
        item.SetUp(itemType, word, posXTo, posYTo, posXFall, posYStartAndFall);
        itemCutView.Setup(LoadItemSprite(word), item, this);
        AddToModel(item, itemCutView);
        cutFruitUI.ItemMove(item, itemCutView);
    }

    public List<float> GenerateSpawnXPositions()
    {
        List<float> listPosX = new List<float>();
        listPosX.Add(0f);
        int i = 3;
        int j = 3;
        while (i > 0)
        {
            listPosX.Add(itemSpacingX  * i);
            i--;
        }
        while (j > 0)
        {
            listPosX.Add(-itemSpacingX  * j);
            j--;
        }
        return listPosX;
    }
    public Sprite LoadItemSprite(string word)
    {
        string path = CONST.PATH_IMG_CUT_FRUITS + word;
        return ResourceManager.Instance.GetResource<Sprite>(path);
    }

    public List<string> GenerateWordSet(List<string> wordCurrent)
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
                items = ShuffleAndPickWords(wordCurrent, 2);
            }
            else
            {
                items = ShuffleAndPickWords(wordCurrent, 1);
            }
        }
        if (wordCurrent.Count == 3)
        {
            if (randomIndex > 50)
            {
                items = ShuffleAndPickWords(wordCurrent, 3);
            }
            else
            {
                items = ShuffleAndPickWords(wordCurrent, 2);
            }
        }
        if (wordCurrent.Count == 4)
        {
            int randomNumber = Random.Range(3, 5);
            items = ShuffleAndPickWords(wordCurrent, randomNumber);
            
        }
        if (wordCurrent.Count == 5)
        {
            int randomNumber = Random.Range(4, 6);
            items = ShuffleAndPickWords(wordCurrent, randomNumber);
        }
        return items;
    }
    public List<string> ShuffleAndPickWords(List<string> wordCurrent, int limit)
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
        cutFruitUI.ShowTime(formatted);
    }
    private void EffectTimeCountdown(int start)
    {
        StartCoroutine(cutFruitUI.ShowTimeCount(start));
    }
    public void PlayCountdownSound()
    {
        SoundManager.Instance.PlayOneShotSound(SoundManager.Instance.chooseWord, 0.7f);
    }
    public void CreateExplode(ItemCutModel itemCut,ItemCutView itemCutView)
    {
        if (!inputPressed ) return;
        float posX = itemCutView.GetComponent<RectTransform>().localPosition.x;
        float posY = itemCutView.GetComponent<RectTransform>().localPosition.y;
        ItemType itemType = itemCut.itemType;
        string word = itemCut.name;
        
        RemoveItem(itemCut, itemCutView);
        Destroy(itemCutView.gameObject);
        if (itemType == ItemType.item)
        {
            PlayOneShot(SoundManager.Instance.slice);
            bool isWord = IsWordCurrent(word);
            if (isWord)
            {
                cutFruitModel.AddCombo();
                cutFruitModel.CalculateScore();
                cutFruitUI.AddShowcore(cutFruitModel.score);
                if (cutFruitModel.combo >0) {
                    cutFruitUI.ShowTextCombo(cutFruitModel.combo);
                }
                cutFruitUI.ShowTextPerfect(CONST.TEXT_PERFECT, HelperColor.Instance.GetColor(EffectColor.Green), true);
            }
            else
            {
                cutFruitUI.AddShowcore(-1);
                cutFruitModel.SetDefaul();
                cutFruitUI.ShowTextPerfect(CONST.TEXT_MISS, HelperColor.Instance.GetColor(EffectColor.White));
            }
            Color orginal = HelperColor.Instance.GetColorByFruitName(word);
            Color decrease = HelperColor.Instance.DecreaseColor(orginal,lightenFactor);
            ManagerParticelSystem.Instance.CreateParticleEffect(posX, posY, ManagerParticelSystem.Instance.fruitExplosionPrefab, cutFruitUI.TranformExplode, 1f, orginal, decrease, true);
        }
        else
        {
            PlayOneShot(SoundManager.Instance.bomb);
            cutFruitUI.AddShowcore(-3);
            cutFruitModel.SetDefaul();
            cutFruitUI.ShowTextPerfect(CONST.TEXT_EXPLODE, HelperColor.Instance.GetColor(EffectColor.Yellow));
            ManagerParticelSystem.Instance.CreateParticleEffect(posX, posY, ManagerParticelSystem.Instance.bombExplosionPrefab, cutFruitUI.TranformExplode, 1f, Color.white,Color.white);
        }
    }
    public void PlayOneShot(AudioClip audioClip)
    {
        SoundManager.Instance.PlayOneShotSound(audioClip);
    }
    public bool IsWordCurrent(string word)
    {
        return cutFruitModel.wordCurrent.Contains(word);
    }

    public void AddToModel(ItemCutModel item,ItemCutView itemView)
    {
        cutFruitModel.AddItem(item);
        AddController(itemView);

    }
    public void RemoveItem(ItemCutModel item, ItemCutView itemView) {
        cutFruitModel.RemoveItem(item);
        RemoveController(itemView);

    }
    public void AddController(ItemCutView controller)
    {
        activeItemView.Add(controller);
    }
    public void RemoveController(ItemCutView controller)
    {
        activeItemView.Remove(controller);
    }
    public void SetOnIsPress()
    {
        inputPressed  = true;
    }
    public void SetOffIsPress()
    {
        inputPressed  = false;
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

    public void PlayWordAppearSound()
    {
        SoundManager.Instance.PlayOneShotSound(SoundManager.Instance.wordApear,0.7f);
    }
    public void PressButton()
    {
        SoundManager.Instance.PressButton();
    }
}
