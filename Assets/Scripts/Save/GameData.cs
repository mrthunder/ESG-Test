using UnityEngine;
using System.IO;
using System.Threading.Tasks;
using System;

[System.Serializable]
//This class contains all the important data of this game
public class GameData
{
    [System.Serializable]
    //This class is to store the player information
    public class PlayerData
    {
        public int Id = 1;
        public string PlayerName = "Player 1";
        public int MoneyAmount = 100;
    }
    // The player gets money over time. This is the last time that their should received
    // any money. Because unity's json class does not serialize DateTime objects, I am using a long
    public long LastTimeRewardReceived;
    //If this is a new player, then the lobby tutorial will be shown, after that there is no need.
    public bool ShouldPlayLobbyTutorial;
    //The information of the current player
    public PlayerData Player;

    //Ctor
    public GameData()
    {
        // The player will always receive the money reward every two minutes, so if this the first time a player is playing
        // the time the player should receive the money reward should be two minutes from now (when the player started).
        LastTimeRewardReceived = DateTime.UtcNow.AddMinutes(HeaderDisplay.MINUTE_INCREMENT).ToFileTimeUtc();
        ShouldPlayLobbyTutorial = true;
        Player = new PlayerData();
    }

    #region Save and Load
    // The name of the file containing the game save data
    private const string FILE_NAME = "Game.sav";

    /// <summary>
    /// Saves the game into a file at the streaming assets folder
    /// </summary>
    /// <param name="data">data to be save</param>
    public static void Save(GameData data)
    {
        // I am turning the game data into a json string
        string json = JsonUtility.ToJson(data);
        // making the path to the file
        string path = Application.streamingAssetsPath;
        string finalPath = Path.Combine(path, FILE_NAME);
        //if the file does not exits, I need to create
        if(!File.Exists(finalPath))
        {
            // If the folder does not exist I need to create the folder
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            // Writing the json string into the save file
            using (StreamWriter sw = new StreamWriter(File.Create(finalPath))) 
            {
                sw.Write(json);
            }
#if UNITY_EDITOR
            // If I am in the editor I want to refresh the project section to see the folder and file
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
        else
        {
            //I am rewriting the entire file with the new json
            File.WriteAllText(finalPath, json);
        }
        
    }

    /// <summary>
    /// Load asynchronously the save data from a file at the streaming assets folder
    /// </summary>
    /// <returns>save data</returns>
    public async static Task<GameData> Load()
    {
        // Making the path to the file
        string path = Application.streamingAssetsPath;
        string finalPath = Path.Combine(path, FILE_NAME);
        // Checking if the file exist
        if (File.Exists(finalPath))
        {
            // create the object
            GameData data;
            using(StreamReader sr = new StreamReader(File.OpenRead(finalPath)))
            {
                // transferring the file contents to the json string
                string json = await sr.ReadToEndAsync();
                // assigning the data to the object
                data = JsonUtility.FromJson<GameData>(json);
            }
            return data;
        }
        // if the file does not exist I just return a null
        return null;
    }

    /// <summary>
    /// Delete the save file and creates a new save for the player
    /// </summary>
    public static void ClearData()
    {
        // Making the path
        string path = Application.streamingAssetsPath;
        string finalPath = Path.Combine(path, FILE_NAME);
        if (File.Exists(finalPath))
        {
            //Deletes both the file and meta
            File.Delete(finalPath);
            File.Delete($"{finalPath}.meta");
        }
#if UNITY_EDITOR
        // If I am in the editor I want to refresh the project section to see if the file was deleted
        UnityEditor.AssetDatabase.Refresh();
        Debug.Log("<color='green'>File deleted!!!</color>");
#endif
    }
    #endregion
}
