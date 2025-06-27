using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrailType
{
    Trail,
    Firework,
    Explosion
}

public class TrailPoolManager : BaseManager<TrailPoolManager>
{
    private Dictionary<TrailType, Queue<TrailRenderer>> poolDict = new Dictionary<TrailType, Queue<TrailRenderer>>();

    public TrailRenderer GetFromPool(TrailType type, TrailRenderer prefab, Transform parent, Vector3 position, Quaternion rotation)
    {
        if (!poolDict.ContainsKey(type))
        {
            poolDict[type] = new Queue<TrailRenderer>();
        }

        TrailRenderer trail;
        if (poolDict[type].Count > 0)
        {
            trail = poolDict[type].Dequeue();
            trail.gameObject.SetActive(true);
            trail.Clear();  
        }
        else
        {
            trail = GameObject.Instantiate(prefab, position, rotation, parent);
        }

        trail.transform.SetParent(parent);
        trail.transform.position = position;
        trail.transform.rotation = rotation;

        return trail;
    }

    public void ReturnToPool(TrailType type, TrailRenderer trail)
    {
        trail.Clear(); 
        trail.gameObject.SetActive(false);
        trail.transform.SetParent(this.transform);

        if (!poolDict.ContainsKey(type))
        {
            poolDict[type] = new Queue<TrailRenderer>();
        }

        poolDict[type].Enqueue(trail);
    }

    public bool HasAvailable(TrailType type)
    {
        return poolDict.ContainsKey(type) && poolDict[type].Count > 0;
    }

    public void ClearPool()
    {
        foreach (var kvp in poolDict)
        {
            while (kvp.Value.Count > 0)
            {
                TrailRenderer trail = kvp.Value.Dequeue();
                if (trail != null)
                {
                    Destroy(trail.gameObject);
                }
            }
        }

        poolDict.Clear();
    }
}
