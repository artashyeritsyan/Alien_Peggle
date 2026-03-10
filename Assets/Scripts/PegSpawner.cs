using UnityEngine;

public class PegSpawner : MonoBehaviour
{
    public float minX, maxX, minY, maxY, xInterval, yInterval;
    public GameObject peg;


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
                if (i % 2 == 0)
                {
                    float offsetX = j + xInterval / 2;
                    if (offsetX > maxX) continue;
                    Instantiate(peg, new Vector2(offsetX, i), Quaternion.identity);
                }
                else
                {
                    Instantiate(peg, new Vector2(j, i), Quaternion.identity);
                }
            }
        }
    }

}
