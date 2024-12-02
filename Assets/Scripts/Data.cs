using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Data : MonoBehaviour
{
    public static Data Instance;
    private string filePath;
    private void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "saveData.json");
        if (Instance == null)
        {
            Instance = this;
            Load();
        }
    }

    private SaveInfo saveInfo;

    public void Save()
    {
        string serializeObj = Serialize(saveInfo);
        File.WriteAllText(filePath, serializeObj);
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

    public void AddGold()
    {
        saveInfo.gold++;
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
}
