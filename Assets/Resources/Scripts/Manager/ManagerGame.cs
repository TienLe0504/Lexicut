using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerGame : BaseManager<ManagerGame>
{
    [Header("Canvas Reference")]
    public Canvas canvas;
    public float WidthScreen;
    public float HeightScreen;
    [Header("Category Reference")]
    public ListImage listImage;
    public ListImage listBGImage;
    public ListTrailRender listTrail;
    // === Inventory / Shop Data ===
    public InventoryData shopEffect;
    public InventoryData shopTrail;
    public InventoryData shopColor;
    // === State Inventory ===
    public List<string> inventory = new List<string>();
    public List<Inventory> InventoryData = new List<Inventory>();
    public Dictionary<string,Inventory> bagInventory = new Dictionary<string, Inventory>();
    // === Categories ===
    public List<string> Categories => new List<string>(listImage.ImageDictionary.Keys);
    // === Color Effect ===
    public int gold;
    // === Effect Cell ===
    public EffectColor colorEffectCurrent;
    public string colorEffectCurrentString;
    public Effect effectCell;
    public bool isUseEffectCell = false;
    public string effectCellName;
    public string trailName;
    public TrailRenderer trailCurrent;
    private void Awake()
    {
        GetSizeScreen();
    }
    void Start()
    {
        ResourceManager.Instance.EnsureStoreFileExists();
        ResourceManager.Instance.EnsureRankFileExists();
        
        AddScoreForUsers();
        LoadColorEffect();
        LoadEffectCell();
        LoadTrail();
        LoadInventoryFromStore();
        LoadGold();
        ApplyShopItemsToInventory();
        UpdateListInventory();
        MarkUsedItemsInInventory();

        UIManager.Instance.ShowScreen<HomeScreen>();
        PlayBackgroundMusic();

    }
    private void GetSizeScreen()
    {
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        WidthScreen = canvasRect.rect.width;
        HeightScreen = canvasRect.rect.height;
    }

    public void PlayBackgroundMusic()
    {
        AudioClip backgroundMusic = SoundManager.Instance.RandomMusic();
        SoundManager.Instance.PlayLoopingMusic(SoundManager.Instance.backgroundGame);

    }


    public void AddScoreForUsers()
    {
        int globalChance = Random.Range(0, 101); 
        if (globalChance > 55)
        {
            List<User> users = ResourceManager.Instance.LoadFromFile<List<User>>(CONST.PATH_RANK, CONST.KEY_RANK);

            foreach (User user in users)
            {
                if (user.id != CONST.KEY_ID)
                {
                    int individualChance = Random.Range(0, 101);
                    if (individualChance > 50) 
                    {
                        int bonus = Random.Range(50, 181); 
                        user.score += bonus;
                    }
                }
            }
            ResourceManager.Instance.SaveToFile<List<User>>(CONST.PATH_RANK, CONST.KEY_RANK, users);
        }
    }

    public void LoadGold()
    {
        gold = ResourceManager.Instance.LoadFromFile<int>(CONST.KEY_FILENAME_STORE, CONST.KEY_GOLD);
    }
    public void LoadInventoryFromStore()
    {
        inventory = ResourceManager.Instance.LoadFromFile<List<string>>(CONST.KEY_FILENAME_STORE, CONST.KEY_INVENTORY);
        if(inventory == null)
        {
            inventory = new List<string>();
            bagInventory = new Dictionary<string, Inventory>();
            return;
        }
        foreach (var item in inventory)
        {
            bagInventory.Add(item, new Inventory());
        }

    }


    public void ApplyShopItemsToInventory()
    {
        AddBagInventory(shopTrail);
        AddBagInventory(shopColor);
        AddBagInventory(shopEffect);
    }

    public void AddBagInventory(InventoryData data)
    {
        for (int i = 0; i < data.inventoryList.Count; i++)
        {
            Inventory inventoryType = data.inventoryList[i];
            if (inventory.Contains(inventoryType.typeInventory.ToString()))
            {
                inventoryType.isUsed = true;
                bagInventory[data.inventoryList[i].typeInventory.ToString()] = DuplicateInventoryItem(data.inventoryList[i]);
            }
        }
    }
    public void UpdateListInventory()
    {
        foreach (KeyValuePair<string, Inventory> item in bagInventory)
        {
            InventoryData.Add(item.Value); 
        }
    }
    private Inventory DuplicateInventoryItem(Inventory source)
    {
        return new Inventory
        {
            name = source.name,
            gold = source.gold,
            image = source.image,
            typeInventory = source.typeInventory,
            typeChoice = source.typeChoice,
            isUsed = false,
            
        };
    }
    public void LoadColorEffect()
    {
        colorEffectCurrentString = ResourceManager.Instance.LoadFromFile<string>(CONST.KEY_FILENAME_STORE, CONST.KEY_COLOR);

        CreateColor();

    }

    public void CreateColor(bool isUse = false)
    {
        if (string.IsNullOrEmpty(colorEffectCurrentString)||isUse)
        {
            colorEffectCurrentString = "";
        }

         colorEffectCurrent = HelperColor.Instance.MapTypeToEffectColor(colorEffectCurrentString);
    }
    public void LoadEffectCell()
    {
        effectCellName = ResourceManager.Instance.LoadFromFile<string>(CONST.KEY_FILENAME_STORE, CONST.KEY_EFFECT);
        CreateEffectCell();
    }
    public void CreateEffectCell(bool isUse = false)
    {
        if (string.IsNullOrEmpty(effectCellName) || isUse)
        {
            isUseEffectCell = false;
            return;
        }

        switch (effectCellName)
        {
            case var name when name == typeInventory.HorizontalEffect.ToString():
                effectCell = new HorizontalLeftEffect();
                break;

            case var name when name == typeInventory.VerticalEffect.ToString():
                effectCell = new VerticelAboveEffect();
                break;

            case var name when name == typeInventory.DiagonalEffect.ToString():
                effectCell = new DiagonalLeftEfect();
                break;

            default:
                isUseEffectCell = false;
                return;
        }

        isUseEffectCell = true;
    }
    public void LoadTrail()
    {
        trailName = ResourceManager.Instance.LoadFromFile<string>(CONST.KEY_FILENAME_STORE, CONST.KEY_TRAIL);
        CreateTrail();
    }
    public void CreateTrail(bool isUse = false)
    {
        if (string.IsNullOrEmpty(trailName) || isUse)
        {
            GetTrail(typeInventory.WhiteTrail.ToString());
            return;
        }
        GetTrail(trailName);
    }
    public void GetTrail(string name)
    {
        TrailPoolManager.Instance.ClearPool();
        foreach (TrailEntry item in listTrail.trailEntries)
        {
            if (item.type.ToString() == name)
            {
                trailCurrent = item.trail;
                break;
            }
        }
    }
    public void MarkUsedItemsInInventory()
    {
        HashSet<string> usedTypes = new HashSet<string>
        {
            trailName,
            effectCellName,
            colorEffectCurrentString
        };

        foreach (Inventory item in InventoryData)
        {
            if (usedTypes.Contains(item.typeInventory.ToString()))
            {
                item.isUsed = true;
            }
        }
    }

}

