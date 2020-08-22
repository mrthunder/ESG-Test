using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreenManager : MonoBehaviour
{
   
    public void Retry()
    {
        //TODO - Tell the game to retry
        SceneManager.LoadScene(SceneHelper.SCENE_NAME_START_SCREEN);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
