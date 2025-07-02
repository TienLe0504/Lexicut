using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ManagerParticelSystem : BaseManager<ManagerParticelSystem>
{
    public List<Vector2> fireworkPositions = new List<Vector2>();
    public List<GameObject> fireworkPrefabs = new List<GameObject>();
    public GameObject bombExplosionPrefab;
    public GameObject fruitExplosionPrefab;
    public GameObject perfectEffectPrefab;
    public void CreateParticleEffect(float x, float y, GameObject explode,GameObject tranformExplode, float time, Color colorOriginal, Color colorDecrease, bool changeColor = false)
    {
        GameObject particelSystem = Instantiate(explode, tranformExplode.transform);
        particelSystem.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
        if (changeColor)
        {
            ParticleSystem[] allParticles = particelSystem.GetComponentsInChildren<ParticleSystem>();
            if (allParticles.Length > 0)
            {
                allParticles[0].startColor = colorDecrease; 

                if (allParticles.Length > 1)
                {
                    allParticles[1].startColor = colorOriginal; 
                }
            }
        }

        particelSystem.GetComponent<ParticleSystem>().Play();
        Destroy(particelSystem, time);
    }
    
}
