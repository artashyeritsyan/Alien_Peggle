using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PegSpawner : MonoBehaviour
{
    public float minX, maxX, minY, maxY, xInterval, yInterval;
    public List<GameObject> pegsPrefabs;

    public float randomStrength;

    private List<GameObject> spawnedPegs;


    void Start()
    {
        spawnedPegs = new List<GameObject>();
        CreateLevel();
    }

    public void CreateLevel()
    {
        for (int i = 0; i < spawnedPegs.Count; i++)
        {
            Destroy(spawnedPegs[i]);
        }
        spawnedPegs.Clear();

        for (float i = minY; i <= maxY; i += yInterval)
        {
            for (float j = minX; j <= maxX; j += xInterval)
            {
                int pegIndex = Random.Range(0, pegsPrefabs.Count);
                if (i % 2 == 0)
                {
                    float offsetX = j + xInterval / 2;
                    if (offsetX > maxX) continue;
                    //Instantiate(pegs[pegIndex], new Vector2(offsetX, i), Quaternion.identity);
                    GameObject newPeg = Instantiate(pegsPrefabs[pegIndex], new Vector2(offsetX + randomStrength * (Random.value - 0.5f), i + randomStrength * (Random.value - 0.5f)), Quaternion.identity);
                    spawnedPegs.Add(newPeg);
                }
                else
                {
                    GameObject newPeg = Instantiate(pegsPrefabs[pegIndex], new Vector2(j, i), Quaternion.identity);
                    spawnedPegs.Add(newPeg);

                }
            }
        }
    }

    public void SetXInterval(float interval)
    {
        xInterval = interval;
    }

    public void SetYInterval(float interval)
    {
        yInterval = interval;
    }

    public void SetRandomStrength(float value)
    {
        randomStrength = value;
    }

    public int GetPegsCount ()
    { 
        return spawnedPegs.Count;
    }

}
