using UnityEngine;
using System.IO;
using System.Threading.Tasks;
using System;

[System.Serializable]
public class GameData
{
    [System.Serializable]
    public class PlayerData
    {
        public int Id = 1;
        public string PlayerName = "Player 1";
        public int MoneyAmount = 100;
    }
    // The player gets money over time. This is the last time that their should received
    // any money
    public long LastTimeRewardReceived;

    public bool ShouldPlayLobbyTutorial;

    public PlayerData Player;

    public GameData()
    {
        
        LastTimeRewardReceived = DateTime.UtcNow.AddMinutes(HeaderDisplay.MINUTE_INCREMENT).ToFileTimeUtc();
        ShouldPlayLobbyTutorial = true;
        Player = new PlayerData();
    }

    #region Save and Load

    private const string FILE_NAME = "Game.sav";
    public static void Save(GameData data)
    {
        string json = JsonUtility.ToJson(data);
        string path = Application.streamingAssetsPath;
        string finalPath = Path.Combine(path, FILE_NAME);
        if(!File.Exists(finalPath))
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (StreamWriter sw = new StreamWriter(File.Create(finalPath))) 
            {
                sw.Write(json);
            }
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
        else
        {
            //I am rewriting the entire file with the new json
            File.WriteAllText(finalPath, json);
        }
        
    }

    public async static Task<GameData> Load()
    {
        string path = Application.streamingAssetsPath;
        string finalPath = Path.Combine(path, FILE_NAME);
        if (File.Exists(finalPath))
        {
            GameData data;
            using(StreamReader sr = new StreamReader(File.OpenRead(finalPath)))
            {
                string json = await sr.ReadToEndAsync();
                data = JsonUtility.FromJson<GameData>(json);
            }
            return data;
        }
        return null;
    }

    public static void ClearData()
    {
        string path = Application.streamingAssetsPath;
        string finalPath = Path.Combine(path, FILE_NAME);
        if (File.Exists(finalPath))
        {
            //Deletes both the file and meta
            File.Delete(finalPath);
            File.Delete($"{finalPath}.meta");
        }
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
        Debug.Log("<color='green'>File deleted!!!</color>");
#endif
    }
    #endregion
}
