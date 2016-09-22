using UnityEngine;
using System.Collections;


[RequireComponent (typeof(PlayerManager))]
public class PlayerScore : MonoBehaviour {

	PlayerManager player;

	// Use this for initialization
	void Start () {
		player = GetComponent<PlayerManager>();
		StartCoroutine (SyncScoreLoop());
	}

	void OnDestroy () {
		if (player != null)
		SyncNow();
	}


	//sync death en kill
	IEnumerator SyncScoreLoop () {
		while (true){
			yield return new WaitForSeconds(10f);

			SyncNow();
		}

	}

	void SyncNow () {
		if (UserAccountManager.IsLoggedIn) {
				UserAccountManager.instance.GetData(OnDataReceived);
			}
	}

	void OnDataReceived(string data){

		if (player.kills == 0 && player.deaths == 0)
			return;

		int kills = DataTranslator.DataToKills(data);
		int deaths = DataTranslator.DataToDeaths(data);

		int newKills = player.kills + kills;
		int newDeaths = player.deaths + deaths;

		string newData = DataTranslator.ValuesToData(newKills, newDeaths);

		Debug.Log ("Syncing: " + newData);

		player.kills = 0;
		player.deaths = 0;

		UserAccountManager.instance.SendData(newData);
	}


}
