using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] spaceshipPrefubs;
    public Texture2D[] spaceshipTextures;

    public static GameManager Instnace;

    private int currentSpaceshipIndex = 0;
    public int CurrentSpaceshipIndex => currentSpaceshipIndex;

    public GameObject currentSpaceship => spaceshipPrefubs[currentSpaceshipIndex];

   

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
