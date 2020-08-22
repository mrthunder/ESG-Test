using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class GameData
{
    [System.Serializable]
    public class PlayerData
    {
        public string PlayerName = "Player 1";
        public int MoneyAmount = 50;
    }

    public PlayerData Player = new PlayerData();

    #region Save and Load

    private const string FILE_NAME = "Game.sav";
    public static void Save(GameData data)
    {
        string json = JsonUtility.ToJson(data);
        string path = Application.streamingAssetsPath;
        string finalPath = Path.Combine(path, FILE_NAME);
        if(!File.Exists(finalPath))
        {
            File.Create(finalPath);
        }
        using(StreamWriter sw = new StreamWriter(File.OpenWrite(finalPath)))
        {
            sw.Write(json);
        }
    }

    public static GameData Load()
    {
        string path = Application.streamingAssetsPath;
        string finalPath = Path.Combine(path, FILE_NAME);
        if (File.Exists(finalPath))
        {
            GameData data;
            using(StreamReader sr = new StreamReader(File.OpenRead(finalPath)))
            {
                data = JsonUtility.FromJson<GameData>(sr.ReadToEnd());
            }
            return data;
        }
        return new GameData();
    }
    #endregion
}
