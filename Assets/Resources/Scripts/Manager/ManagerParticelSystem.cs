using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ManagerParticelSystem : BaseManager<ManagerParticelSystem>
{
    public List<Vector2> posFireword = new List<Vector2>();
    public List<GameObject> firework = new List<GameObject>();
    public GameObject explodeBomb;
    public GameObject explodeFruit;
    public GameObject effectPerfect;
    public void CreateParticelSystem(float x, float y, GameObject explode,GameObject tranformExplode, float time, Color colorOriginal, Color colorDecrease, bool changeColor = false)
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
