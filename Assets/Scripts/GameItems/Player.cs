using UnityEngine;
using System.Collections;
using System;

public class Player
{
	public const string PLAYER_DATA_KEY_ID = "userId";
	public const string PLAYER_DATA_KEY_NAME = "name";
	public const string PLAYER_DATA_KEY_COINS = "coins";

	private int _userId;
	private string _name;
	private int _coins;

	public Player(Hashtable playerData)
	{
		_userId = (int)playerData[PLAYER_DATA_KEY_ID];
		_name = playerData[PLAYER_DATA_KEY_NAME].ToString(); 
		_coins = (int)playerData[PLAYER_DATA_KEY_COINS];
	}
	
	public int GetUserId()
	{
		return _userId;
	}
	
	public string GetName()
	{
		return _name;
	}

	public int GetCoins()
	{
		return _coins;
	}

	public void ChangeCoinAmount(int amount)
	{
		_coins += amount;
	}
}