using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingDataScreenManager : MonoBehaviour
{
    // I am going to update the text inside, so people won't think the game is frozen.
    public Text LoadingLabel = null;
    public Image FillBar = null;
    private const string BASE_LABEL = "Loading";
    private const char REPEAT_CHARACTER = '.';
    private const int REPEAT_AMOUNT_MAX = 4;
    private int _repeatIndex = 0;
    private string[] _repeateCharacterArray = new string[REPEAT_AMOUNT_MAX];
    [Min(0)]
    public float LoadingTime = 0.2f;
    private WaitForSeconds _loadingInterval;
    // Start is called before the first frame update
    void Start()
    {
        _loadingInterval = new WaitForSeconds(LoadingTime);
        FillBar.fillAmount = 0;
        for (int i = 0; i < REPEAT_AMOUNT_MAX; i++)
        {
            _repeateCharacterArray[i] = new string(REPEAT_CHARACTER, i);
        }

        StartCoroutine(ChangeLoadingText());

        GameInstance.Instance.LoadData(() =>
        {
            StartCoroutine(FillProgressBar());
        });
    }


    public IEnumerator ChangeLoadingText()
    {
        while (isActiveAndEnabled)
        {
            LoadingLabel.text = BASE_LABEL + _repeateCharacterArray[_repeatIndex];
            yield return _loadingInterval;
            _repeatIndex = (_repeatIndex + 1) % REPEAT_AMOUNT_MAX;
        }
    }

    public IEnumerator FillProgressBar()
    {
        const float fillAmountPerInterval = 0.1f;
        while (FillBar.fillAmount < 1)
        {
            FillBar.fillAmount += fillAmountPerInterval;
            yield return _loadingInterval;
        }
        SceneManager.LoadScene(SceneHelper.SCENE_NAME_START_SCREEN);
    }
}
