using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerGame : BaseManager<ManagerGame>
{
    // Start is called before the first frame update
    [Header("Canvas Reference")]
    public Canvas canvas;
    public float WidthScreen;
    public float HeightScreen;
    [Header("Category Reference")]
    public ListImage listImage;
    public ListImage listBGImage;
    public List<string> Categories => new List<string>(listImage.ImageDictionary.Keys);
    public int gold;
    public InventoryData shopEffect;
    public InventoryData shopTrail;
    public InventoryData shopColor;
    public List<string> inventory = new List<string>();
    public List<Inventory> InventoryData = new List<Inventory>();
    public Dictionary<string,Inventory> bagInventory = new Dictionary<string, Inventory>();
    public effectColor colorEffectCurrent;
    public string colorEffectCurrentString;
    public Effect effectCell;
    public bool isUseEffectCell = false;
    public string effectCellName;
    public string trailName;
    public TrailRenderer trailCurrent;
    public ListTrailRender listTrail;
    private void Awake()
    {
        GetSizeScreen();
    }


    private void GetSizeScreen()
    {
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        WidthScreen = canvasRect.rect.width;
        HeightScreen = canvasRect.rect.height;
    }

    void Start()
    {
        ResourceManager.Instance.CreateStoreJsonIfNotExists();
        LoadColorEffect();
        LoadEffectCell();
        LoadTrail();
        LoadInventory();
        LoadGold();
        UpdateInventory();
        UpdateListInventory();
        SetupItemUsed();
        UIManager.Instance.ShowScreen<HomeScreen>();
    }
    public void LoadGold()
    {
        gold = ResourceManager.Instance.LoadJson<int>(CONST.KEY_FILENAME_STORE, CONST.KEY_GOLD);
    }
    public void LoadInventory()
    {
        inventory = ResourceManager.Instance.LoadJson<List<string>>(CONST.KEY_FILENAME_STORE, CONST.KEY_INVENTORY);
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
    public void UpdateInventory()
    {
        AddBagInventory(shopTrail);
        AddBagInventory(shopColor);
        AddBagInventory(shopEffect);
        Debug.Log("");
    }

    public void AddBagInventory(InventoryData data)
    {
        for (int i = 0; i < data.inventoryList.Count; i++)
        {
            if (inventory.Contains(data.inventoryList[i].typeInventory.ToString()))
            {
                //InventoryData.Add(CloneInventory(data.inventoryList[i]));
                bagInventory[data.inventoryList[i].typeInventory.ToString()] = CloneInventory(data.inventoryList[i]);
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
    private Inventory CloneInventory(Inventory source)
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
        colorEffectCurrentString = ResourceManager.Instance.LoadJson<string>(CONST.KEY_FILENAME_STORE, CONST.KEY_COLOR);

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
        effectCellName = ResourceManager.Instance.LoadJson<string>(CONST.KEY_FILENAME_STORE, CONST.KEY_EFFECT);
        CreateEffect();
    }
    public void CreateEffect(bool isUse = false)
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
        trailName = ResourceManager.Instance.LoadJson<string>(CONST.KEY_FILENAME_STORE, CONST.KEY_TRAIL);
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
    public void SetupItemUsed()
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

