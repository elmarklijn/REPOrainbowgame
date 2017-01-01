using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SpawnColor : NetworkBehaviour {

	private NetworkStartPosition[] spawnPoints;
	private Transform spawnTrans;
	public Material[] materialColors;
	public SkinnedMeshRenderer playerRenderer;

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

		if (spawnPoints[0].transform.position == transform.position) {
			Debug.Log ("transform van " + spawnPoints[0].gameObject.name + " komt overeen met player spawn");
			playerRenderer.materials[1].CopyPropertiesFromMaterial(materialColors[0]);
		} 
		if (spawnPoints[1].transform.position == transform.position) {
			Debug.Log ("transform van " + spawnPoints[1].gameObject.name + " komt overeen met player spawn");
			playerRenderer.materials[1].CopyPropertiesFromMaterial(materialColors[1]);
		} 
		if (spawnPoints[2].transform.position == transform.position) {
			Debug.Log ("transform van " + spawnPoints[2].gameObject.name + " komt overeen met player spawn");
			playerRenderer.materials[1].CopyPropertiesFromMaterial(materialColors[2]);
		} 
		if (spawnPoints[3].transform.position == transform.position) {
			Debug.Log ("transform van " + spawnPoints[3].gameObject.name + " komt overeen met player spawn");
			playerRenderer.materials[1].CopyPropertiesFromMaterial(materialColors[3]);
		}
	}
}
