using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyScreenManager : MonoBehaviour
{
    private bool _shouldPlayTutorial = false;
    [SerializeField, Tooltip("The one that contains the balloons")]
    private GameObject _tutorialCanvas = null;
    [SerializeField, Tooltip("The one that contains the pop up window")]
    private GameObject _overlayPanel = null;
    // Start is called before the first frame update
    void Start()
    {
        // TODO - Decide if you need to implement a tutorial...
        _shouldPlayTutorial = GameInstance.Instance.CurrentGameData.ShouldPlayLobbyTutorial;
        if (_shouldPlayTutorial && _tutorialCanvas != null)
        {
            GameInstance.Instance.CurrentGameData.ShouldPlayLobbyTutorial = false;
            _tutorialCanvas.SetActive(true);
        }
    }

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

    public void GotoStartMenu()
    {
        SceneManager.LoadScene(SceneHelper.SCENE_NAME_START_SCREEN);
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
