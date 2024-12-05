using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Data : MonoBehaviour
{
    public static Data Instance;
    private string filePath;
    private int currerntGold;
    private void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "saveData.json");
        if (Instance == null)
        {
            Instance = this;
            Load();
            ResetValues();
        }
    }

    private SaveInfo saveInfo;

    public void Save()
    {
        string serializeObj = Serialize(saveInfo);
        File.WriteAllText(filePath, serializeObj);
        Debug.Log("Сохранился");
    }

    private void Load()
    {
        if (File.Exists(filePath))
        {
            saveInfo = DeSerializer();
        }
        else
        {
            Debug.Log("Создан файл сохранения");
            saveInfo = new SaveInfo();
            Save();
        }
    }

    private void ResetValues()
    {
        saveInfo.gold = 0;
        saveInfo.levelIsComplitedMapOne = -1;
        saveInfo.levelIsComplitedMapTwo = -1;
        saveInfo.levelIsComplitedMapThree = -1;
        saveInfo.ownedSpaceship = new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        Save();
    }

    public bool IsSpaceshipOwned(int index)
    {
        if (saveInfo.ownedSpaceship[index] == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void PrchaseSpaceship(int index)
    {
        saveInfo.ownedSpaceship[index] = 1;
        Save();
    }

    public void RemoveGold(int amt)
    {
        saveInfo.gold -= amt;
        Save();
    }

    public void CompletedNextLevel(int numberMup)
    {
        if (numberMup == 1)
        {
            saveInfo.levelIsComplitedMapOne++;
        }
        if (numberMup == 2)
        {
            saveInfo.levelIsComplitedMapTwo++;
        }
        if (numberMup == 3)
        {
            saveInfo.levelIsComplitedMapThree++;
        }
        Save();
    }

    public int GetLevelCompleted(int numberMup)
    {
        if (numberMup == 1)
        {
            return saveInfo.levelIsComplitedMapOne;
        }
        if (numberMup == 2)
        {
            return saveInfo.levelIsComplitedMapTwo;
        }
        if (numberMup == 3)
        {
            return saveInfo.levelIsComplitedMapThree;
        }
        return 0;
    }

    public void AddGold(int gold)
    {
        saveInfo.gold += gold;
        GoldEarnedPerLevel(gold);
        Save();
    }

    public int GetGold()
    {
        return saveInfo.gold;
    }

    private string Serialize(SaveInfo toBeSerialized)
    {
        return JsonUtility.ToJson(toBeSerialized,true);
    }

    private SaveInfo DeSerializer()
    {
        string json = File.ReadAllText(filePath);
        return JsonUtility.FromJson<SaveInfo>(json);
    }

    private void GoldEarnedPerLevel(int priceAsteroid)
    {
        currerntGold += priceAsteroid;
    }

    public int GetGoldEarnedLevel()
    {
        return currerntGold;
    }

    public void ResetCurrentGold()
    {
        currerntGold = 0;
    }
}
