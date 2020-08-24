using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	//UI - I changed components exposed to the inspector from public to private
	// there is no need to keep them public, since no other class will access those variables.
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
	// This is the text in the middle that display rock, paper, scissors.
	private Text _resultLabel = null;

	private Player _player = null;
	private PlayerInfoLoader _playerInfoLoader = null;
	private UpdateGameLoader _updateGameLoader = null;

	// I made the bet variable instead of fixed
	private int _bet = 0;

	// I store the result of the game in those variables to show after the coroutine finishes.
	private string _playerHandResult = string.Empty;
	private string _opponentHandResult = string.Empty;
	private string _gameResult = string.Empty;
	[Space(10), SerializeField, Header("Time Intervals"), Tooltip("In seconds")]
	//In seconds
	private float _resultInterval = 2f;
	private WaitForSeconds _resultWaitInterval = null;


	void Start()
	{
		// I move the update game loader initialization to here, to avoid garbage.
		_playerInfoLoader = new PlayerInfoLoader();
		_playerInfoLoader.OnLoaded += OnPlayerInfoLoaded;
		_playerInfoLoader.Load(GameInstance.Instance.CurrentGameData.Player);
        _updateGameLoader = new UpdateGameLoader();
        _updateGameLoader.OnLoaded += OnGameUpdated;

		// I am changing the name label only once instead of every frame.
		//Displaying the player's name
		_nameLabel.text = "Name: " + _player.GetName();

		// I am going to reuse the wait for seconds in my coroutine
		_resultWaitInterval = new WaitForSeconds(_resultInterval);
	}

	

	public void OnPlayerInfoLoaded(Hashtable playerData)
	{
		_player = new Player(playerData);
	}

	// This will display the rock paper scissor text on the screen over time, and then display the result
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

	/// <summary>
	/// This method is for the Quit button. It quits the game or stop playing the game in the editor.
	/// </summary>
	public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

	/// <summary>
	/// This method is for the Lobby Button. Sends the player to the lobby.
	/// </summary>
    public void GotoLobby()
    {
		SceneManager.LoadScene(SceneHelper.SCENE_NAME_LOBBY_SCREEN);
    }

	private void UpdateGame(UseableItem playerChoice)
	{
		// Restore the game to its initial state
        _resultLabel.text = "Make your Bet!!";
        _playerHand.text = string.Empty;
		_enemyHand.text = string.Empty;
        _updateGameLoader.Load(playerChoice, _bet);
	}

	public void OnGameUpdated(Hashtable gameUpdateData)
	{
		// Different from before, I am storing the result into string to display later.
		_playerHandResult = DisplayResultAsText((UseableItem)gameUpdateData[UpdateGameLoader.GAME_DATA_KEY_PLAYER_RESULT]);
		_opponentHandResult = DisplayResultAsText((UseableItem)gameUpdateData[UpdateGameLoader.GAME_DATA_KEY_OPPONENT_RESULT]);
		// I am getting the game result here.
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

	/// <summary>
	/// When you changes scene and this object gets destroy, I am removing the events.
	/// </summary>
    private void OnDestroy()
    {
		_playerInfoLoader.OnLoaded -= OnPlayerInfoLoaded;
		_updateGameLoader.OnLoaded -= OnGameUpdated;
	}


}