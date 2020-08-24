using System.Collections;
using System;

public class UpdateGameLoader
{
	// Those are the Hashtable's key as constants to make the code more readable
	public const string GAME_DATA_KEY_PLAYER_RESULT = "resultPlayer";
	public const string GAME_DATA_KEY_OPPONENT_RESULT = "resultOpponent";
	public const string GAME_DATA_KEY_COINS_AMOUNT_CHANGE = "coinsAmountChange";
	public const string GAME_DATA_KEY_RESULT = "gameResult";

	public delegate void OnLoadedAction(Hashtable gameUpdateData);
	public event OnLoadedAction OnLoaded;


	public void Load(UseableItem playerChoice, int bet)
	{
		///FIX - Because the first option from the <see cref="UseableItem"/> is none the random range should be from 1 to 4. Not starting from 0
		UseableItem opponentHand = (UseableItem)Enum.GetValues(typeof(UseableItem)).GetValue(UnityEngine.Random.Range(1, 4));

		// I replace the strings with the constants.
		Hashtable mockGameUpdate = new Hashtable();
		mockGameUpdate[GAME_DATA_KEY_PLAYER_RESULT] = playerChoice;
		mockGameUpdate[GAME_DATA_KEY_OPPONENT_RESULT] = opponentHand;
		Result drawResult = ResultAnalyzer.GetResultState(playerChoice, opponentHand);
		mockGameUpdate[GAME_DATA_KEY_COINS_AMOUNT_CHANGE] = GetCoinsAmount(drawResult, bet);
		// Add a result entry to the Hashtable
		mockGameUpdate[GAME_DATA_KEY_RESULT] = drawResult.ToString();


		OnLoaded(mockGameUpdate);
	}

	// I changed the function parameters to get the result and the bet made by the player.
	// This way the coin amount will be increase or decrease by the bet amount.
	private int GetCoinsAmount (Result drawResult, int bet)
	{
		if (drawResult.Equals (Result.Won))
		{
			return bet;
		}
		else if (drawResult.Equals (Result.Lost))
		{
			return -bet;
		}
		else
		{
			return 0;
		}
	}
}