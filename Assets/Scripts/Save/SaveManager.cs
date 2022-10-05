using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
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

    public static void DeleteSave(int _index)
    {
        if (File.Exists(GetSavePath(_index)))
        {
            File.Delete(GetSavePath(_index));
        }
    }

    public static SaveData Load(int _index)
    {
        string fileContent = File.ReadAllText(GetSavePath(_index));
        return JsonUtility.FromJson<SaveData>(fileContent);
    }

    public static void Save(SaveData _saveData)
    {
        string contentJson = JsonUtility.ToJson(_saveData);
        File.WriteAllText(GetSavePath(_saveData.index),contentJson);
    }

    public static string GetSavePath(int _indexSlot)
    {
        return $"{Path}Save_{_indexSlot}{Extension}";
    }
}
