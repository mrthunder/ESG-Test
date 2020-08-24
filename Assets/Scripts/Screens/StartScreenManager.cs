using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class StartScreenManager : MonoBehaviour
{
    [Header("Input Fields"),Header("UI Components"), SerializeField, FormerlySerializedAs("PlayerNameInput")]
    // The player can type their name here and I will forward to the game
    private InputField _playerNameInput = null;
    [Header("Buttons"), SerializeField, FormerlySerializedAs("StartButton")]
    // I am getting the reference to the button to make non interactable once clicked
    private Button _startButton = null;
    [SerializeField, FormerlySerializedAs("ClearDataButton")]
    // This is the button that once clicked will clear the game data
    private Button _clearDataButton = null;
    [Header("Panels"), SerializeField, FormerlySerializedAs("NewUserPanel")]
    // This is the panel where the user can type their name
    private GameObject _newUserPanel = null;
    [Space(10), Header("Event System"), SerializeField, FormerlySerializedAs("CanvasEventSystem")]
    // I am using the event system to set the selected button.
    private EventSystem _canvasEventSystem = null;


#if UNITY_EDITOR
    [ContextMenu("Find UI Objects")]
    // This is to find all the objects that I am asking easily
    private void FindUIObjectsInTheScene()
    {
        const string path = "Canvas/Panel/";
        _newUserPanel = GameObject.Find($"{path}New User Panel");
        _playerNameInput = _newUserPanel.transform.Find($"PlayerName_Input")?.GetComponent<InputField>();
        _startButton = _newUserPanel.transform.Find($"Start Button")?.GetComponent<Button>();
        if (_startButton != null)
        {
            _startButton.onClick.AddListener(StartGame);
        }
        _clearDataButton = GameObject.Find($"{path}Clear Data Button")?.GetComponent<Button>();
        if(_clearDataButton != null)
        {
            _clearDataButton.onClick.AddListener(ClearData);
        }
        _canvasEventSystem = FindObjectOfType<EventSystem>();
    }
#endif

    /// <summary>
    /// This is a method for the Start Game Button. It saves the name and moves the player to the lobby.
    /// </summary>
    public void StartGame()
    {
        if (_startButton != null)
        {
            // This is just so the game does not receive multiple requests to change scene
            _startButton.interactable = false;
        }

        if (_playerNameInput != null && !string.IsNullOrEmpty(_playerNameInput.text))
        {
            GameInstance.Instance.CurrentGameData.Player.PlayerName = _playerNameInput.text;
            GameInstance.Instance.SaveGame();
        }

        SceneManager.LoadScene(SceneHelper.SCENE_NAME_LOBBY_SCREEN);
    }

    /// <summary>
    /// This method is for the x button on the new player panel.
    /// It hides the panel and deselects the input field.
    /// </summary>
    public void HideNewPlayerPanel()
    {
        _newUserPanel.SetActive(false);
        _canvasEventSystem.SetSelectedGameObject(null);
    }

    /// <summary>
    /// This method is for the clear data button.
    /// It clears the save data.
    /// </summary>
    public void ClearData()
    {
        GameInstance.Instance.ClearData();
    }

    void Update()
    {
        if (_newUserPanel.activeInHierarchy && _playerNameInput != null && _startButton != null)
        {
            //The player needs to type something in order to play the game
            _startButton.interactable = (_playerNameInput.text.Length > 0);
        }
        // Once the player press space, either the player goes to the lobby or the new player panel pop up.
        if (!_newUserPanel.activeInHierarchy && Input.GetKeyDown(KeyCode.Space))
        {
            if (GameInstance.Instance.IsNewPlayer)
            {
                _newUserPanel.SetActive(true);
                _canvasEventSystem.SetSelectedGameObject(_playerNameInput.gameObject);
            }
            else
            {
                SceneManager.LoadScene(SceneHelper.SCENE_NAME_LOBBY_SCREEN);
            }
        }
    }

    /// <summary>
    /// This method is for the input field. 
    /// Once player press enter and leaves the input field, I deselects the input field. 
    /// </summary>
    public void DeselecteEverything()
    {
        _canvasEventSystem.SetSelectedGameObject(null);
    }
}
