using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SpawnColor : NetworkBehaviour {

	private NetworkStartPosition[] spawnPoints;

	// Use this for initialization
	void Awake () {
		spawnPoints = FindObjectsOfType<NetworkStartPosition>();
	}
	
	public void GetStartColor () {
	Debug.Log ("kleuren vinden");
	Debug.Log ("gevonden: " + spawnPoints);

		Debug.Log ("case 0: spawnpuntnaam " + spawnPoints[0].gameObject.name);
		Debug.Log ("case 1: spawnpuntnaam " + spawnPoints[1].gameObject.name);
		Debug.Log ("case 2: spawnpuntnaam " + spawnPoints[2].gameObject.name);
		Debug.Log ("case 3: spawnpuntnaam " + spawnPoints[3].gameObject.name);


		switch (spawnPoints.Length) {
		default:
			break;
		case 0:
			Debug.Log ("case 0: spawnpuntnaam " + spawnPoints[0].gameObject.name);
			break;
		case 1:
			Debug.Log ("case 1: spawnpuntnaam " + spawnPoints[1].gameObject.name);
			break;
		case 2:
			Debug.Log ("case 2: spawnpuntnaam " + spawnPoints[2].gameObject.name);
			break;
		case 3:
			Debug.Log ("case 3: spawnpuntnaam " + spawnPoints[3].gameObject.name);
			break;
		}

	}
}
