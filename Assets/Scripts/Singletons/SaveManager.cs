using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    private static string Path => $"{Application.persistentDataPath}/Save/";
    private static string Extension => ".save";

    [SerializeField] private List<SaveSlot> slots = new List<SaveSlot>();

    private void Start()
    {
        //GetSaves();
    }

    public void SetNewSaveAtSlot(int index)
    {
        Instance.slots[index].SetSave(new SaveData());

        SaveFile(index);
    }

    private void SaveFile(int index)
    {
        if (!Directory.Exists(Path))
            Directory.CreateDirectory(Path);

        SaveData data = Instance.slots[index].GetSave();
        string contentJson = JsonUtility.ToJson(data);
        File.WriteAllText(GetSavePath(data.index), contentJson);
    }

    public void LoadSlot(int index)
    {
        Instance.slots[index].Load();
        SceneManager.ChangeScene("Map");
    }

    public bool IsSlotEmpty(int index)
    {
        if (Instance.slots[index] == null)
        {
            return true;
        }

        return false;
    }

    public void DeleteSlot(int index)
    {
        Instance.slots[index].Delete();
    }

    public void RefreshAll()
    {
        foreach (SaveSlot slot in Instance.slots)
        {
            slot.RefreshLook();
        }
    }


    /*public void GetSaves()
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
    }*/

    /*public static void DeleteSave(int index)
    {
        if (File.Exists(GetSavePath(index)))
        {
            File.Delete(GetSavePath(index));
            Instance.slots[index].DeleteSave();
        }
    }*/

    /*public static SaveData Load(int index)
    {
        string fileContent = File.ReadAllText(GetSavePath(index));
        return JsonUtility.FromJson<SaveData>(fileContent);
    }*/

    /*public static void Save(SaveData saveData)
    {
        string contentJson = JsonUtility.ToJson(saveData);
        File.WriteAllText(GetSavePath(saveData.index), contentJson);
        Instance.slots[saveData.index].SetSave(saveData);
    }*/

    /*public static bool HasSaves()
    {
        if (!Directory.Exists(Path)) return false;

        foreach (SaveSlot slot in Instance.slots)
        {
            if (File.Exists(GetSavePath(slot.Index)))
            {
                return true;
            }
        }

        return false;
    }*/

    /*public static void NewGame()
    {
        if (PlayerPrefs.HasKey("LastPlayedSlot"))
        {
            Instance.slots[PlayerPrefs.GetInt("LastPlayedSlot")].LoadGame();
        }
    }*/

    public static string GetSavePath(int indexSlot)
    {
        return $"{Path}Save_{indexSlot}{Extension}";
    }
}
