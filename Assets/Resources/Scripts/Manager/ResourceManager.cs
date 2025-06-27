//using System.Collections.Generic;
//using UnityEngine;
//using Newtonsoft.Json;

//public class ResourceManager : BaseManager<ResourceManager>
//{
//    public List<string> GetList(string path)
//    {
//        TextAsset jsonFile = Resources.Load<TextAsset>(path);

//        if (jsonFile == null)
//        {
//            Debug.LogError("JSON file not found at: " + path);
//            return new List<string>();
//        }

//        try
//        {
//            List<string> data = JsonConvert.DeserializeObject<List<string>>(jsonFile.text);
//            return data;
//        }
//        catch (System.Exception e)
//        {
//            Debug.LogError("Failed to deserialize JSON: " + e.Message);
//            return new List<string>();
//        }
//    }
//    public Sprite GetImage(string fullPath)
//    {
//        Sprite sprite = Resources.Load<Sprite>(fullPath);
//        if (sprite == null)
//        {
//            Debug.LogWarning("Không tìm thấy ảnh tại: " + fullPath);
//        }
//        return sprite;
//    }

//}

using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;


public class ResourceManager : BaseManager<ResourceManager>
{

    public T LoadJson<T>(string path)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(path);

        if (jsonFile == null)
        {
            Debug.LogError("JSON file not found at: " + path);
            return default(T);
        }

        try
        {
            T data = JsonConvert.DeserializeObject<T>(jsonFile.text);
            return data;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to deserialize JSON at " + path + ": " + e.Message);
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


    public void SaveJson<T>(string fileName, string key, T value)
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


    public T LoadJson<T>(string fileName, string key)
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

                    // Nếu là kiểu string thì ép kiểu luôn
                    if (typeof(T) == typeof(string))
                    {
                        return (T)raw;
                    }

                    // Còn lại thì deserialize như cũ
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

    public void CreateStoreJsonIfNotExists()
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

}
