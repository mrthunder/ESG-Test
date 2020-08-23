using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	//UI
	[Space(10), SerializeField, Header("UI Components")]
	private Text _playerHand = null;
	[SerializeField]
	private Text _enemyHand = null;
	[Space(10),SerializeField, Header("Labels")]
	private Text _nameLabel = null;
	[SerializeField]
	private Text _playerBetLabel = null;
	[SerializeField]
	private Text _enemyBetLabel = null;
	[SerializeField]
	private Text _resultLabel = null;

	private Player _player = null;
	private PlayerInfoLoader _playerInfoLoader = null;
	private UpdateGameLoader _updateGameLoader = null;

	private int _bet = 0;

	private string _playerHandResult = string.Empty;
	private string _opponentHandResult = string.Empty;
	private string _gameResult = string.Empty;
	[Space(10), SerializeField, Header("Time Intervals")]
	//In seconds
	private float _resultInterval = 2f;
	private WaitForSeconds _resultWaitInterval = null;



	void Start()
	{
		_playerInfoLoader = new PlayerInfoLoader();
		_playerInfoLoader.OnLoaded += OnPlayerInfoLoaded;
		_playerInfoLoader.Load(GameInstance.Instance.CurrentGameData.Player);
        _updateGameLoader = new UpdateGameLoader();
        _updateGameLoader.OnLoaded += OnGameUpdated;

		//Displaying the player's name
		_nameLabel.text = "Name: " + _player.GetName();

		_resultWaitInterval = new WaitForSeconds(_resultInterval);
	}

	

	public void OnPlayerInfoLoaded(Hashtable playerData)
	{
		_player = new Player(playerData);
	}

	public IEnumerator GameLoop()
    {
		_resultLabel.text = "Rock!!";
		yield return _resultWaitInterval;
		_resultLabel.text = "Paper!!";
		yield return _resultWaitInterval;
		_resultLabel.text = "Scissors!!";
		yield return _resultWaitInterval;
		_resultLabel.text = "Go!!";
		_playerHand.text = _playerHandResult;
		_enemyHand.text = _opponentHandResult;
		yield return _resultWaitInterval;
		_resultLabel.text = _gameResult;
		yield return null;
        GameInstance.Instance.CurrentGameData.Player = _player;
        if (_player.GetCoins() == 0)
        {
            //GAME OVER
            SceneManager.LoadScene(SceneHelper.SCENE_NAME_GAME_OVER_SCREEN);
        }
    }
	

	public void HandlePlayerInput(int item)
	{
		// To play you need to bet
		if (_bet == 0)
        {
			_resultLabel.text = "Make a bet to play!";
			return;
        }

		UseableItem playerChoice = UseableItem.None;

		switch (item)
		{
			case 1:
				playerChoice = UseableItem.Rock;
				break;
			case 2:
				playerChoice = UseableItem.Paper;
				break;
			case 3:
				playerChoice = UseableItem.Scissors;
				break;
		}

		UpdateGame(playerChoice);
	}

	public void ChangeBet(int amount)
    {
		// The player cannot bet more than it has
		_bet = Mathf.Clamp(_bet + amount, 0, GameInstance.Instance.CurrentGameData.Player.MoneyAmount);
		if(_playerBetLabel != null)
        {
			_playerBetLabel.text = $"${_bet}";
        }
		if(_enemyBetLabel != null)
        {
			//The enemy will always match your bet.
			_enemyBetLabel.text = $"${_bet}";
		}
    }

	public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void GotoLobby()
    {
		SceneManager.LoadScene(SceneHelper.SCENE_NAME_LOBBY_SCREEN);
    }

	private void UpdateGame(UseableItem playerChoice)
	{
        _resultLabel.text = "Make your Bet!!";
        _playerHand.text = string.Empty;
		_enemyHand.text = string.Empty;
        _updateGameLoader.Load(playerChoice, _bet);
	}

	public void OnGameUpdated(Hashtable gameUpdateData)
	{
		_playerHandResult = DisplayResultAsText((UseableItem)gameUpdateData[UpdateGameLoader.GAME_DATA_KEY_PLAYER_RESULT]);
		_opponentHandResult = DisplayResultAsText((UseableItem)gameUpdateData[UpdateGameLoader.GAME_DATA_KEY_OPPONENT_RESULT]);
		_gameResult = $"You {gameUpdateData[UpdateGameLoader.GAME_DATA_KEY_RESULT]}";
		_player.ChangeCoinAmount((int)gameUpdateData[UpdateGameLoader.GAME_DATA_KEY_COINS_AMOUNT_CHANGE]);

		StartCoroutine(GameLoop());
	}


    private string DisplayResultAsText (UseableItem result)
	{
		switch (result)
		{
			case UseableItem.Rock:
				return "Rock";
			case UseableItem.Paper:
				return "Paper";
			case UseableItem.Scissors:
				return "Scissors";
		}

		return "Nothing";
	}

    private void OnDestroy()
    {
		_playerInfoLoader.OnLoaded -= OnPlayerInfoLoaded;
		_updateGameLoader.OnLoaded -= OnGameUpdated;
	}


}