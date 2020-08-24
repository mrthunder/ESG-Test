using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyScreenManager : MonoBehaviour
{
    //This tells if the tutorial window should be displayed
    private bool _shouldPlayTutorial = false;
    [SerializeField, Tooltip("The one that contains the balloons")]
    // The canvas that contain the tutorial information
    private GameObject _tutorialCanvas = null;
    [SerializeField, Tooltip("The one that contains the pop up window")]
    // The panel with the information that shows up when the player, out of money, tries to play the game.
    private GameObject _overlayPanel = null;
    // Start is called before the first frame update
    void Start()
    {
        // Gets from the save data if should show or not the tutorial
        _shouldPlayTutorial = GameInstance.Instance.CurrentGameData.ShouldPlayLobbyTutorial;
        if (_shouldPlayTutorial && _tutorialCanvas != null)
        {
            // After showing the tutorial, updates the save data, telling to not show that anymore
            GameInstance.Instance.CurrentGameData.ShouldPlayLobbyTutorial = false;
            _tutorialCanvas.SetActive(true);
        }
    }

    /// <summary>
    /// This is a method for the play button. It changes the scene from the lobby to the game.
    /// if the player does not have any money, it shows the overlay panel
    /// </summary>
    public void PlayGame()
    {
        if (GameInstance.Instance.CurrentGameData.Player.MoneyAmount == 0)
        {
            if (_overlayPanel !=null)
            {
                _overlayPanel.SetActive(true); 
            }
            return;
        }
        SceneManager.LoadScene(SceneHelper.SCENE_NAME_GAME_SCREEN);
    }

    /// <summary>
    /// This is a method for the start menu button. It moves the player from the lobby to the start menu scene.
    /// </summary>
    public void GotoStartMenu()
    {
        SceneManager.LoadScene(SceneHelper.SCENE_NAME_START_SCREEN);
    }

    /// <summary>
    /// This is a method for the quit button. It quits the game or stop playing the game if on the editor.
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
