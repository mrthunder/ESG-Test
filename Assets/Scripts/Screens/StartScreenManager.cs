using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StartScreenManager : MonoBehaviour
{
    [Header("Input Fields"),Header("UI Components")]
    // The player can type their name here and I will forward to the game
    public InputField PlayerNameInput = null;
    [Header("Buttons")]
    // I am getting the reference to the button to make non interactable once clicked
    public Button StartButton = null;
    public Button ClearDataButton = null;
    [Header("Panels")]
    public GameObject NewUserPanel = null;
    [Space(10), Header("Event System")]
    public EventSystem CanvasEventSystem = null;


#if UNITY_EDITOR
    [ContextMenu("Find UI Objects")]
    private void FindUIObjectsInTheScene()
    {
        const string path = "Canvas/Panel/";
        NewUserPanel = GameObject.Find($"{path}New User Panel");
        PlayerNameInput = NewUserPanel.transform.Find($"PlayerName_Input")?.GetComponent<InputField>();
        StartButton = NewUserPanel.transform.Find($"Start Button")?.GetComponent<Button>();
        if (StartButton != null)
        {
            StartButton.onClick.AddListener(StartGame);
        }
        ClearDataButton = GameObject.Find($"{path}Clear Data Button")?.GetComponent<Button>();
        if(ClearDataButton != null)
        {
            ClearDataButton.onClick.AddListener(ClearData);
        }
        CanvasEventSystem = FindObjectOfType<EventSystem>();
    }
#endif

    public void StartGame()
    {
        if (StartButton != null)
        {
            StartButton.interactable = false;
        }

        if (PlayerNameInput != null && !string.IsNullOrEmpty(PlayerNameInput.text))
        {
            GameInstance.Instance.CurrentGameData.Player.PlayerName = PlayerNameInput.text;
            GameInstance.Instance.SaveGame();
        }

        SceneManager.LoadScene(SceneHelper.SCENE_NAME_LOBBY_SCREEN);
    }

    public void HideNewPlayerPanel()
    {
        NewUserPanel.SetActive(false);
        CanvasEventSystem.SetSelectedGameObject(null);
    }

    public void ClearData()
    {
        GameInstance.Instance.ClearData();
    }

    void Update()
    {
        if (NewUserPanel.activeInHierarchy && PlayerNameInput != null && StartButton != null)
        {
            //The player needs to type something in order to play the game
            StartButton.interactable = (PlayerNameInput.text.Length > 0);
        }
        if (!NewUserPanel.activeInHierarchy && Input.GetKeyDown(KeyCode.Space))
        {
            if (GameInstance.Instance.IsNewPlayer)
            {
                NewUserPanel.SetActive(true);
                CanvasEventSystem.SetSelectedGameObject(PlayerNameInput.gameObject);
            }
            else
            {
                SceneManager.LoadScene(SceneHelper.SCENE_NAME_LOBBY_SCREEN);
            }
        }
    }
}
