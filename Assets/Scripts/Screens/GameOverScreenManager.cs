using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreenManager : MonoBehaviour
{

   /// <summary>
   /// This method is for the Retry Button. It send the player to the lobby.
   /// </summary>
    public void Retry()
    {
        SceneManager.LoadScene(SceneHelper.SCENE_NAME_LOBBY_SCREEN);
    }

    /// <summary>
    /// This method is for the Quit button. It quits the game or stop playing the game in the editor.
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
