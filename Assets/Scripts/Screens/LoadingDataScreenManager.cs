using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// Reasoning: 
/// I create this scene to load the data and have a instance of the Game Instance on the scene.
/// By creating the Game Instance instance here, I prevent any game instance to be created in any other scene.
/// </summary>
public class LoadingDataScreenManager : MonoBehaviour
{
    [Header("Labels"), SerializeField, FormerlySerializedAs("LoadingLabel")]
    // I am going to update the text inside, so people won't think the game is frozen.
    private Text _loadingLabel = null;
    [Header("Images"),SerializeField, FormerlySerializedAs("FillBar")]
    // The progress bar
    private Image _fillBar = null;
    // This is all to save on allocation
    private const string BASE_LABEL = "Loading";
    private const char REPEAT_CHARACTER = '.';
    private const int REPEAT_AMOUNT_MAX = 4;
    private int _repeatIndex = 0;
    private string[] _repeateCharacterArray = new string[REPEAT_AMOUNT_MAX];
    [Header("Intervals"),Min(0), SerializeField, FormerlySerializedAs("LoadingTime")]
    // The loading time is for any one who wants to change the wait for seconds time.
    private float _loadingTime = 0.2f;
    private WaitForSeconds _loadingInterval;
    // Start is called before the first frame update
    void Start()
    {
        // Initializing the wait for seconds with the time exposed
        _loadingInterval = new WaitForSeconds(_loadingTime);
        // making sure the progress bar fill amount is always zero at start
        _fillBar.fillAmount = 0;
        
        // This is just for the loading text animation.
        // NOTE: I did not wanted to add TextMeshPro to this project because it was not here at first,
        // but if TMP_Text was used instead of Text, I would just loop through the visible characters showing from "g"
        // in loading to the 3 dots. It would be a lot easier.
        for (int i = 0; i < REPEAT_AMOUNT_MAX; i++)
        {
            _repeateCharacterArray[i] = new string(REPEAT_CHARACTER, i);
        }

        StartCoroutine(ChangeLoadingText());

        // After the data was loaded I need to show the progress bar going up.
        GameInstance.Instance.LoadData(() =>
        {
            StartCoroutine(FillProgressBar());
        });
    }


    public IEnumerator ChangeLoadingText()
    {
        while (isActiveAndEnabled)
        {
            //changing the Loading... text over time
            _loadingLabel.text = BASE_LABEL + _repeateCharacterArray[_repeatIndex];
            yield return _loadingInterval;
            _repeatIndex = (_repeatIndex + 1) % REPEAT_AMOUNT_MAX;
        }
    }

    public IEnumerator FillProgressBar()
    {
        // Filling the progress bar over time.
        const float fillAmountPerInterval = 0.1f;
        while (_fillBar.fillAmount < 1)
        {
            _fillBar.fillAmount += fillAmountPerInterval;
            yield return _loadingInterval;
        }
        // Once it is finished I move the player to the start screen.
        SceneManager.LoadScene(SceneHelper.SCENE_NAME_START_SCREEN);
    }
}
