﻿using UnityEngine;
using System.Collections;
using System;

public class UpdateGameLoader
{

	public const string GAME_DATA_KEY_PLAYER_RESULT = "resultPlayer";
	public const string GAME_DATA_KEY_OPPONENT_RESULT = "resultOpponent";
	public const string GAME_DATA_KEY_COINS_AMOUNT_CHANGE = "coinsAmountChange";

	public delegate void OnLoadedAction(Hashtable gameUpdateData);
	public event OnLoadedAction OnLoaded;


	public void Load(UseableItem playerChoice)
	{
		///FIX - Because the first option from the <see cref="UseableItem"/> is none the random range should be from 1 to 4. Not starting from 0
		UseableItem opponentHand = (UseableItem)Enum.GetValues(typeof(UseableItem)).GetValue(UnityEngine.Random.Range(1, 4));

		Hashtable mockGameUpdate = new Hashtable();
		mockGameUpdate[GAME_DATA_KEY_PLAYER_RESULT] = playerChoice;
		mockGameUpdate[GAME_DATA_KEY_OPPONENT_RESULT] = opponentHand;
		mockGameUpdate[GAME_DATA_KEY_COINS_AMOUNT_CHANGE] = GetCoinsAmount(playerChoice, opponentHand);
		
		OnLoaded(mockGameUpdate);
	}

	private int GetCoinsAmount (UseableItem playerHand, UseableItem opponentHand)
	{
		Result drawResult = ResultAnalyzer.GetResultState(playerHand, opponentHand);

		if (drawResult.Equals (Result.Won))
		{
			return 10;
		}
		else if (drawResult.Equals (Result.Lost))
		{
			return -10;
		}
		else
		{
			return 0;
		}
	}
}