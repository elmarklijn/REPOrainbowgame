﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {

	public Text killCount;
	public Text deathCount;

	// Use this for initialization
	void Start () {
		if (UserAccountManager.IsLoggedIn)
		UserAccountManager.instance.GetData(OnRecievedData);
	}
	
	void OnRecievedData(string data){
		killCount.text = DataTranslator.DataToKills(data).ToString() + " KILLS";
		deathCount.text = DataTranslator.DataToDeaths(data).ToString() + " DEATHS";
	} 
}
