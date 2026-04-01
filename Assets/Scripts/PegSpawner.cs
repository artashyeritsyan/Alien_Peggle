using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PegSpawner : MonoBehaviour
{
    public float minX, maxX, minY, maxY, xInterval, yInterval;
    public GameObject[] pegsPrefabs;

    public float randomStrength;
    public bool createRandomLevel;

    //private List<GameObject> spawnedPegs;
    public Transform pegsParent;


    void Start()
    {
        //spawnedPegs = new List<GameObject>();
        CreateLevel();
    }

    public void CreateLevel()
    {
        ClearPegs();

        for (float i = minY; i <= maxY; i += yInterval)
        {
            for (float j = minX; j <= maxX; j += xInterval)
            {
                int pegIndex = Random.Range(0, pegsPrefabs.Length);
                if (i % 2 == 0)
                {
                    float offsetX = j + xInterval / 2;
                    if (offsetX > maxX) continue;
                    //Instantiate(pegs[pegIndex], new Vector2(offsetX, i), Quaternion.identity);
                     Instantiate(pegsPrefabs[pegIndex], new Vector2(offsetX + randomStrength * (Random.value - 0.5f), i + randomStrength * (Random.value - 0.5f)), Quaternion.identity, pegsParent);
                    //spawnedPegs.Add(newPeg);
                }
                else
                {
                    Instantiate(pegsPrefabs[pegIndex], new Vector2(j, i), Quaternion.identity, pegsParent);
                    //spawnedPegs.Add(newPeg);

                }
            }
        }
    }

    public void ClearPegs()
    {
        if (pegsParent.childCount > 0)
        {
            for (int i = 0; i < pegsParent.childCount; i++)
                Destroy(pegsParent.GetChild(i).gameObject);
        }
    }

    public void CreateLevelWithPegCount(int pegsCount, float minDistance = 1f)
    {
        ClearPegs();

        float yInterval = minDistance;

        int rowCount = Mathf.FloorToInt((maxY - minY) / yInterval) + 1;
        int pegsPerRow = Mathf.CeilToInt((float)pegsCount / rowCount);

        float xInterval = (maxX - minX) / pegsPerRow;

        int spawned = 0;
        float centerX = (minX + maxX) / 2f;

        for (int row = 0; row < rowCount; row++)
        {
            float y = minY + row * yInterval;

            float offset = (row % 2 == 0) ? 0f : xInterval / 2f;

            float rowWidth = (pegsPerRow - 1) * xInterval;
            float startX = centerX - rowWidth / 2f;

            for (int col = 0; col < pegsPerRow; col++)
            {
                if (spawned >= pegsCount)
                    return;

                float x = startX + col * xInterval + offset;

                if (x < minX || x > maxX)
                    continue;

                int pegIndex = Random.Range(0, pegsPrefabs.Length);

                Vector2 pos = new Vector2(
                    x + randomStrength * (Random.value - 0.5f),
                    y + randomStrength * (Random.value - 0.5f)
                );

                Instantiate(pegsPrefabs[pegIndex], pos, Quaternion.identity, pegsParent);
                //spawnedPegs.Add(newPeg);

                spawned++;
            }
        }
    }


    public void SetCanShoot(bool canShoot)
    {
        
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
        return pegsParent.childCount;
    }

    public void SetSpawnedPegsCount()
    {

    }

    public void SetPegPrefabs(GameObject[] pegs)
    {
        pegsPrefabs = pegs;
    }

}
