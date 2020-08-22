using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameInstance : MonoBehaviour
{
    private static GameInstance _instance = null;
    public static GameInstance Instance => _instance ?? (new GameObject("[Game Instance]").AddComponent<GameInstance>());

    public GameData CurrentGameData = new GameData();

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
        DontDestroyOnLoad(gameObject);
    }    
}
