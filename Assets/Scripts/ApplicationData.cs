using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[CreateAssetMenu(fileName = "ApplicationData", menuName = "Custom/ApplicationData", order = 1)]
public class ApplicationData : ScriptableObject
{
    const string playerDataFileName = "playerData";

    public PlayerData playerData;

    public void SavePlayerData()
    {
        string filePath = System.IO.Path.Combine(Application.persistentDataPath, playerDataFileName + ".json");
        File.WriteAllText(filePath, JsonUtility.ToJson(playerData, true));
    }

    public void LoadPlayerData()
    {
        string filePath = System.IO.Path.Combine(Application.persistentDataPath, playerDataFileName + ".json");

        playerData = new PlayerData();

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            playerData = JsonUtility.FromJson<PlayerData>(dataAsJson);
        }

        SavePlayerData();
    }

    public void ClearPlayerData()
    {
        string filePath = System.IO.Path.Combine(Application.persistentDataPath, playerDataFileName + ".json");

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        playerData = new PlayerData();
        SavePlayerData();
    }
}

[System.Serializable]
public class PlayerData
{
    public bool soundsOn = true;
    public bool hapticOn = true;
}