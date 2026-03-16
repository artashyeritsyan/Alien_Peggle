using System.Collections.Generic;
using UnityEngine;

public class DataHolder : MonoBehaviour
{
    // For every level. Saving in array (list)
    [Header("Player Score for Levels")]
    public int LevelsCount = 15;
    private int[] levelsBestShots;
    private int[] levelsBestTimes;

    public string Data { get; private set; }
}
