using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreenManager : MonoBehaviour
{
   
    public void Retry()
    {
        SceneManager.LoadScene(SceneHelper.SCENE_NAME_LOBBY_SCREEN);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
