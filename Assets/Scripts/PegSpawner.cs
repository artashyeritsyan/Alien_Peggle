using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PegSpawner : MonoBehaviour
{
    public float minX, maxX, minY, maxY, xInterval, yInterval;
    public List<GameObject> pegs;


    void Start()
    {
        CreateLevel();
    }

    public void CreateLevel()
    {
        for (float i = minY; i <= maxY; i += yInterval)
        {
            for (float j = minX; j <= maxX; j += xInterval)
            {
                int pegIndex = Random.Range(0, pegs.Count);
                if (i % 2 == 0)
                {
                    float offsetX = j + xInterval / 2;
                    if (offsetX > maxX) continue;
                    Instantiate(pegs[pegIndex], new Vector2(offsetX, i), Quaternion.identity);
                }
                else
                {
                    Instantiate(pegs[pegIndex], new Vector2(j, i), Quaternion.identity);
                }
            }
        }
    }

}
