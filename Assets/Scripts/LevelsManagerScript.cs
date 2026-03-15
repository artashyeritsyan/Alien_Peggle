using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelsManagerScript : MonoBehaviour
{
    [Header("Level Buttons Parameters")]
    [SerializeField] int levelsCount = 15;
    [SerializeField] Transform levelsLayout;
    [SerializeField] GameObject levelButtonPrefab;
    private List<GameObject> levelButtons;
    

    [Header("Star Sprites")]
    [SerializeField] Sprite emptyStarSprite;
    [SerializeField] Sprite FilledStarSprite;


    //private DataHolder savedData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    
}
