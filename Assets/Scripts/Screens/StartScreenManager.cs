using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreenManager : MonoBehaviour
{
    // The player can type their name here and I will forward to the game
    public InputField PlayerNameInput = null;
    // I am getting the reference to the button to make non interactable once clicked
    public Button StartButton = null;

#if UNITY_EDITOR
    [ContextMenu("Find UI Objects")]
    private void FindUIObjectsInTheScene()
    {
        const string path = "Canvas/Panel/";
        PlayerNameInput = GameObject.Find($"{path}PlayerName_Input")?.GetComponent<InputField>();
        StartButton = GameObject.Find($"{path}Start Button")?.GetComponent<Button>();
        if(StartButton!=null)
        {
            StartButton.onClick.AddListener(StartGame);
        }
    }
#endif

    public void StartGame()
    {
        if(StartButton != null)
        {
            StartButton.interactable = false;
        }

        if(PlayerNameInput != null && !string.IsNullOrEmpty(PlayerNameInput.text))
        {
            GameInstance.Instance.CurrentGameData.Player.PlayerName = PlayerNameInput.text;
        }

        //TODO - Add game scene name to the string
        SceneManager.LoadScene(SceneHelper.SCENE_NAME_GAME_SCREEN);
    }
}
