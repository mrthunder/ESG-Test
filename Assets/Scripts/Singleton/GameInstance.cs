using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameInstance : MonoBehaviour
{
    private static GameInstance _instance = null;
    public static GameInstance Instance => _instance ?? (new GameObject("[Game Instance]").AddComponent<GameInstance>());

    // The current data of the game
    private GameData _currentGameData = null;
    public GameData CurrentGameData => _currentGameData;

    private bool _isNewPlayer = true;
    public bool IsNewPlayer => _isNewPlayer;

    void Awake()
    {
        //Only one singleton can exist
        if(this != _instance)
        {
            Destroy(gameObject);
        }
        if(_instance == null)
        {
            _instance = this;
        }

        _currentGameData = GameData.Load();

        if(_currentGameData != null)
        {
            _isNewPlayer = false;
        }
        else
        {
            _currentGameData = new GameData();
        }
        
        DontDestroyOnLoad(gameObject);
    }

    public void SaveGame()
    {
        GameData.Save(_currentGameData);
    }

    public void ClearData()
    {
        GameData.ClearData();
        _isNewPlayer = true;
        _currentGameData = new GameData();
    }
}
