using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveLoadManager
{
    private static readonly string FileName = "map_progress.json";
    private static string Path => System.IO.Path.Combine(Application.persistentDataPath, FileName);

    public static void SaveGame(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Path, json);
        Debug.Log($"[SaveLoad]progress saved to {Path}:\n{json}");
    }

    public static SaveData LoadGame()
    {
        if (!File.Exists(Path))
        {
            Debug.Log("[SaveLoad]No save file, generate new map");
            return null;
        }

        string json = File.ReadAllText(Path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        Debug.Log($"[SaveLoad]Progress loaded from {Path}:\n{json}");
        return data;
    }

    public static void DeleteSave()
    {
        if (File.Exists(Path))
            File.Delete(Path);
    }
}
