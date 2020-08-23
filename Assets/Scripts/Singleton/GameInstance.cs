using System;
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
