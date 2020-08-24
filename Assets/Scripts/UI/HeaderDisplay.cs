using System;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HeaderDisplay : MonoBehaviour
{
    [SerializeField, Header("Labels"), FormerlySerializedAs("MoneyLabel")]
    // Those are UI components 
    // The money label is where I going to display how much money the player currently has
    private Text _moneyLabel = null;
    [SerializeField, FormerlySerializedAs("TimeLabel")]
    // The time label is where I going to display how much time left before the player can get some money
    private Text _timeLabel = null;
    [SerializeField, FormerlySerializedAs("ProgressBar")]
    // The progress bar is just a easier way to visualize the time.
    private Image _progressBar = null;
    
    // The amount to fill the progress bar. It goes from 0 to 1
    private float _progressValue = 0;

    // The countdown timer
    private TimeSpan _timer = new TimeSpan();
    // The when the player should get their money
    private DateTime _saveTime = new DateTime();

    // $2 over time
    public const int MONEY_INCREMENT = 2;
    // Every 2 minutes I will add more money
    public const int MINUTE_INCREMENT = 2;

    void Start()
    {
        // Because Json Utility does not converts DateTime objects, I turn them into longs so they can be saved
        // and then turn them back into DateTime objects to calculate the time.
        _saveTime = DateTime.FromFileTimeUtc(GameInstance.Instance.CurrentGameData.LastTimeRewardReceived);
    }

    // Update is called once per frame
    void Update()
    {
        // Where I calculate how much time is left
        _timer = (DateTime.UtcNow - _saveTime);
        if (_moneyLabel != null)
        {
            _moneyLabel.text = $"Money: ${GameInstance.Instance.CurrentGameData.Player.MoneyAmount}";
        }
        if(_timeLabel != null)
        {
            _timeLabel.text = _timer.ToString(@"hh\:mm\:ss");
        }
        if(_progressBar !=null)
        {
            // The timer gives me a negative value in minutes, thus I negate. I divided by 2 to get the percentage.
            // but because the value is inverted I subtract 1 to the value and I get a scale
            var timespanScale = 1-((float)(_timer.TotalMinutes*-1)/2);
            // Because the progress bar goes from 0 to 1, I am clamping the value
            _progressValue = Mathf.Clamp(timespanScale, 0, 1);
            // Adding the progress to the image to fill
            _progressBar.fillAmount = _progressValue;
            // If the value is 1 then I need to change the time
            if (_progressValue >= 1)
            {
                // This value is a long, that is why I am converting
                GameInstance.Instance.CurrentGameData.LastTimeRewardReceived = DateTime.UtcNow.AddMinutes(MINUTE_INCREMENT).ToFileTimeUtc();
                // I am just updating the time used to calculate the countdown.
                _saveTime = DateTime.UtcNow.AddMinutes(MINUTE_INCREMENT);
                // If I've been away for more then 2 minutes the time scale will be more than 1, which multiplying by the money increment value
                // I get how much money I should be giving to the player.
                GameInstance.Instance.CurrentGameData.Player.MoneyAmount += MONEY_INCREMENT * Mathf.FloorToInt(timespanScale);
            }
        }
    }

    
}
