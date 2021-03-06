﻿using System.Collections;

public class PlayerInfoLoader
{
	public delegate void OnLoadedAction(Hashtable playerData);
	public event OnLoadedAction OnLoaded;

	// I remove the constructor and gave the Load function the Player Data object to update the Hashtable, based on the save data

	//FIX - I fixing the methods naming inconsistencies 
	public void Load(GameData.PlayerData data)
	{
		Hashtable mockPlayerData = new Hashtable();
		mockPlayerData[Player.PLAYER_DATA_KEY_ID] = data.Id;
		mockPlayerData[Player.PLAYER_DATA_KEY_NAME] = data.PlayerName;
		mockPlayerData[Player.PLAYER_DATA_KEY_COINS] = data.MoneyAmount;

		OnLoaded(mockPlayerData);
	}
}