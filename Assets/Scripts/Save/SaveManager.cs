using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    private static string Path => $"{Application.persistentDataPath}/Save/";
    private static string Extension => ".save";
    
    [SerializeField] private List<SaveSlot> slots = new List<SaveSlot>();

    public void Start()
    {
        GetSaves();
    }

    public void GetSaves()
    {
        if (!Directory.Exists(Path))
            Directory.CreateDirectory(Path);
        
        for (int i = 0; i < slots.Count; i++)
        {
            string path = GetSavePath(i);
            if (File.Exists(path))
            {
                string fileContent = File.ReadAllText(path);
                SaveData save = JsonUtility.FromJson<SaveData>(fileContent);

                slots[i].SetSave(save);
            }
            else
            {
                slots[i].SetEmpty(i);
            }
        }
    }

    public static void DeleteSave(int index)
    {
        if (File.Exists(GetSavePath(index)))
        {
            File.Delete(GetSavePath(index));
        }
    }

    public static SaveData Load(int index)
    {
        string fileContent = File.ReadAllText(GetSavePath(index));
        return JsonUtility.FromJson<SaveData>(fileContent);
    }

    public static void Save(SaveData saveData)
    {
        string contentJson = JsonUtility.ToJson(saveData);
        File.WriteAllText(GetSavePath(saveData.index),contentJson);
    }

    public static bool HasSaves()
    {
        foreach(SaveSlot slot in Instance.slots)
        {
            if (slot != null && slot.HasData())
            {
                return true;
            }
        }

        return false;
    }

    public static void NewGame()
    {
        
    }

    public static string GetSavePath(int indexSlot)
    {
        return $"{Path}Save_{indexSlot}{Extension}";
    }
}
