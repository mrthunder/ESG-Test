using System;
using UnityEngine;

/// <summary>
/// This is a singleton. This class contains the game data, and it is the interface between the other classes to the game data
/// </summary>
public class GameInstance : MonoBehaviour
{
    // singleton instance
    private static GameInstance _instance = null;
    // Singleton instance property. If I call the instance and their is no one in the scene, I create a new instance object.
    public static GameInstance Instance => _instance ?? (_instance = new GameObject("[Game Instance]").AddComponent<GameInstance>());

    // The current data of the game
    private GameData _currentGameData = null;
    // I use a property so the game data cannot be changed.
    public GameData CurrentGameData => _currentGameData;

    //If is a new player, then it needs to create a game data
    private bool _isNewPlayer = true;
    public bool IsNewPlayer => _isNewPlayer;

    void Awake()
    {
        //Only one singleton can exist
        if (_instance == null)
        {
            _instance = this;
        }else if (this != _instance)
        {
#if UNITY_EDITOR
            Debug.Log($"Game Object: {gameObject.name} was deleted!!!");
#endif
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Loads the game data, and call the callback once finished.
    /// </summary>
    /// <param name="onComplete">callback</param>
    public async void LoadData(Action onComplete)
    {
        _currentGameData = await GameData.Load();
        if (_currentGameData != null)
        {
            _isNewPlayer = false;
        }
        else
        {
            _currentGameData = new GameData();
        }
        onComplete.Invoke();

    }

    /// <summary>
    /// Saves the game data into a file
    /// </summary>
    public void SaveGame()
    {
        GameData.Save(_currentGameData);
    }

    /// <summary>
    /// Delete the game data file and creates a new game data
    /// </summary>
    public void ClearData()
    {
        GameData.ClearData();
        _isNewPlayer = true;
        _currentGameData = new GameData();
    }

    /// <summary>
    /// When the games exits, I save the game to the file
    /// </summary>
    private void OnApplicationQuit()
    {
        SaveGame();
    }


}
