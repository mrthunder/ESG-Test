using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class LobbyScreenManager : MonoBehaviour
{
    // TODO - add similar variable to game data
    private bool _shouldPlayTutorial = false;
    public PlayableDirector TutorialPrefab = null;
    // Start is called before the first frame update
    void Start()
    {
        // TODO - Check with game data the value of the should play tutorial variable
        // _shouldPlayTutorial = GameInstance.CurrentGameData.ShouldPlayLobbyTutorial;
        if(_shouldPlayTutorial && TutorialPrefab != null)
        {
            // Instantiate the tutorial
            var tutorial = Instantiate(TutorialPrefab, Vector3.zero, Quaternion.identity);
            // Play the tutorial
            tutorial.Play();
            // Once the tutorial is finished, destroy the object
            tutorial.stopped += _ =>
             {
                 Destroy(tutorial.gameObject);
             };
        }
    }

    public void PlayGame()
    {
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
