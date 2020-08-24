using System;
using UnityEngine;
using UnityEngine.UI;

public class HeaderDisplay : MonoBehaviour
{
    public Text MoneyLabel = null;
    public Text TimeLabel = null;
    public Image ProgressBar = null;

    public float ProgressValue = 0;

    private TimeSpan _timer = new TimeSpan();
    private DateTime _saveTime = new DateTime();

    // $2 over time
    public const int MONEY_INCREMENT = 2;
    // Every 2 minutes I will add more money
    public const int MINUTE_INCREMENT = 2;

    void Start()
    {
        _saveTime = DateTime.FromFileTimeUtc(GameInstance.Instance.CurrentGameData.LastTimeRewardReceived);
    }

    // Update is called once per frame
    void Update()
    {
        _timer = (DateTime.UtcNow - _saveTime);
        if (MoneyLabel != null)
        {
            MoneyLabel.text = $"Money: ${GameInstance.Instance.CurrentGameData.Player.MoneyAmount}";
        }
        if(TimeLabel != null)
        {
            TimeLabel.text = _timer.ToString(@"hh\:mm\:ss");
        }
        if(ProgressBar !=null)
        {
            var timespanScale = 1-((float)(_timer.TotalMinutes*-1)/2);
            ProgressValue = Mathf.Clamp(timespanScale, 0, 1);
            ProgressBar.fillAmount = ProgressValue;
            if (ProgressValue >= 1)
            {
                GameInstance.Instance.CurrentGameData.LastTimeRewardReceived = DateTime.UtcNow.AddMinutes(MINUTE_INCREMENT).ToFileTimeUtc();
                _saveTime = DateTime.FromFileTimeUtc(GameInstance.Instance.CurrentGameData.LastTimeRewardReceived);
                GameInstance.Instance.CurrentGameData.Player.MoneyAmount += MONEY_INCREMENT * Mathf.FloorToInt(timespanScale);
            }
        }
    }

    
}
