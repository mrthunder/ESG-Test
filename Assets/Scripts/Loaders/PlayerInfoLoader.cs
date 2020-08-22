using UnityEngine;
using System.Collections;
using System;

public class PlayerInfoLoader
{
	public delegate void OnLoadedAction(Hashtable playerData);
	public event OnLoadedAction OnLoaded;

	//FIX - I fixing the methods naming inconsistencies 
	public void Load()
	{
		//TODO - inspect this function
		Hashtable mockPlayerData = new Hashtable();
		mockPlayerData[Player.PLAYER_DATA_KEY_ID] = 1;
		//TODO - Let the player write their own name
		mockPlayerData[Player.PLAYER_DATA_KEY_NAME] = "Player 1";
		mockPlayerData[Player.PLAYER_DATA_KEY_COINS] = 50;

		OnLoaded(mockPlayerData);
	}
}