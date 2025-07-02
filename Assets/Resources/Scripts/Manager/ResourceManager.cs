using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;


public class ResourceManager : BaseManager<ResourceManager>
{

    public T LoadFromResources<T>(string path)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(path);

        if (jsonFile == null)
        {
            return default(T);
        }

        try
        {
            T data = JsonConvert.DeserializeObject<T>(jsonFile.text);
            return data;
        }
        catch (System.Exception e)
        {
            return default(T);
        }
    }

    public Sprite GetImage(string fullPath)
    {
        Sprite sprite = Resources.Load<Sprite>(fullPath);
        if (sprite == null)
        {
            Debug.LogWarning("Không tìm thấy ảnh tại: " + fullPath);
        }
        return sprite;
    }


    public T GetResource<T>(string path) where T : Object
    {
        T resource = Resources.Load<T>(path);
        if (resource == null)
        {
            Debug.LogWarning($"Resource of type {typeof(T)} not found at: {path}");
        }
        return resource;
    }


    public void SaveToFile<T>(string fileName, string key, T value)
    {
        try
        {
            string filePath = Path.Combine(Application.persistentDataPath, fileName);
            Dictionary<string, object> data;

            if (File.Exists(filePath))
            {
                string oldJson = File.ReadAllText(filePath);
                data = JsonConvert.DeserializeObject<Dictionary<string, object>>(oldJson);
            }
            else
            {
                data = new Dictionary<string, object>();
            }

            data[key] = value;

            string newJson = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filePath, newJson);

            Debug.Log("Saved JSON at: " + filePath + "\n" + newJson);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save JSON: " + e.Message);
        }
    }


    public T LoadFromFile<T>(string fileName, string key)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                if (data.ContainsKey(key))
                {
                    object raw = data[key];

                    if (typeof(T) == typeof(string))
                    {
                        return (T)raw;
                    }

                    return JsonConvert.DeserializeObject<T>(raw.ToString());
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load JSON: " + e.Message);
        }

        return default(T);
    }

    public void EnsureStoreFileExists()
    {
        string fileName = "STORE.json";
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        if (!File.Exists(filePath))
        {
            Dictionary<string, object> defaultData = new Dictionary<string, object>
            {
                { CONST.KEY_GOLD, 90000 }
            };

            string json = JsonConvert.SerializeObject(defaultData, Formatting.Indented);
            File.WriteAllText(filePath, json);

            Debug.Log("STORE.json created at: " + filePath);
        }
        else
        {
            Debug.Log("STORE.json already exists at: " + filePath);
        }
    }
    public void EnsureRankFileExists()
    {
        string fileName = "RANK.json";
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        if (!File.Exists(filePath))
        {
            List<User> listUser = LoadFromResources<List<User>>(CONST.PATH_RANK_ASOBLUTE);
            Dictionary<string, object> defaultData = new Dictionary<string, object>
            {
                { CONST.KEY_RANK, listUser }
            };
            string json = JsonConvert.SerializeObject(defaultData, Formatting.Indented);
            File.WriteAllText(filePath, json);

            Debug.Log("rank.json created at: " + filePath);
        }
        else
        {
            Debug.Log("rank.json already exists at: " + filePath);
        }
    }
}
