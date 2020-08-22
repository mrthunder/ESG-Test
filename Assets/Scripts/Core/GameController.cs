using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

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
	private Text _moneyLabel = null;

	private Player _player = null;
	private PlayerInfoLoader _playerInfoLoader = null;
	private UpdateGameLoader _updateGameLoader = null;

	void Awake()
	{
#if UNITY_EDITOR
		Debug.Assert(_playerHand, "The <color='blue'><b>player hand</b></color> is missing check if the text was assigned correctly.");
		Debug.Assert(_enemyHand, "The <color='blue'><b>enemy hand</b></color> is missing check if the text was assigned correctly.");
		Debug.Assert(_nameLabel, "The <color='blue'><b>name label</b></color> is missing check if the text was assigned correctly.");
		Debug.Assert(_moneyLabel, "The <color='blue'><b>money label</b></color> is missing check if the text was assigned correctly.");
#endif
	}

#if UNITY_EDITOR
	[ContextMenu("Find Texts in the Scene")]
	private void FindTextInScene()
    {
		_playerHand = GameObject.Find("Player Hand")?.GetComponent<Text>();
		_enemyHand = GameObject.Find("Enemy Hand")?.GetComponent<Text>();
		_nameLabel = GameObject.Find("Name")?.GetComponent<Text>();
		_moneyLabel = GameObject.Find("Money")?.GetComponent<Text>();
    }
#endif

	void Start()
	{
		_playerInfoLoader = new PlayerInfoLoader();
		_playerInfoLoader.OnLoaded += OnPlayerInfoLoaded;
		_playerInfoLoader.Load(GameInstance.Instance.CurrentGameData.Player);
        _updateGameLoader = new UpdateGameLoader();
        _updateGameLoader.OnLoaded += OnGameUpdated;
    }

	void Update()
	{
		UpdateHud();
	}

	public void OnPlayerInfoLoaded(Hashtable playerData)
	{
		_player = new Player(playerData);
	}

	public void UpdateHud()
	{
		_nameLabel.text = "Name: " + _player.GetName();
		_moneyLabel.text = "Money: $" + _player.GetCoins().ToString();
	}

	public void HandlePlayerInput(int item)
	{
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

	private void UpdateGame(UseableItem playerChoice)
	{
		_updateGameLoader.Load(playerChoice);
	}

	public void OnGameUpdated(Hashtable gameUpdateData)
	{
		_playerHand.text = DisplayResultAsText((UseableItem)gameUpdateData[UpdateGameLoader.GAME_DATA_KEY_PLAYER_RESULT]);
		_enemyHand.text = DisplayResultAsText((UseableItem)gameUpdateData[UpdateGameLoader.GAME_DATA_KEY_OPPONENT_RESULT]);

		_player.ChangeCoinAmount((int)gameUpdateData[UpdateGameLoader.GAME_DATA_KEY_COINS_AMOUNT_CHANGE]);
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