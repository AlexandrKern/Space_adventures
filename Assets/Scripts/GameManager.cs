using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] spaceshipPrefubs;
    public Texture2D[] spaceshipTextures;
    public int[] spaceshipPrice = new int[] { 0, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000, 1100, 1200, 1300, 1400, 1500, 1600, 1700, 1800, 1900 };

    public static GameManager Instnace;

    private int currentSpaceshipIndex = 0;
    public int CurrentSpaceshipIndex => currentSpaceshipIndex;

    public GameObject currentSpaceship => spaceshipPrefubs[currentSpaceshipIndex];

    public int curentLevelIndex = 0;

   

    private void Awake()
    {
        if (Instnace == null)
        {
            Instnace = this;
        }

        spaceshipTextures = new Texture2D[spaceshipPrefubs.Length];
        for (int i = 0; i < spaceshipPrefubs.Length; i++)
        {
            GameObject prefub = spaceshipPrefubs[i];
            Texture2D texture = AssetPreview.GetAssetPreview(prefub);
            spaceshipTextures[i] = texture;
        }
        DontDestroyOnLoad(gameObject);
       
    }

    public void ChangeCurrentSpaceship(int index)
    {
        currentSpaceshipIndex = index;
    }
}
