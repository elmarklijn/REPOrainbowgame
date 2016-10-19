using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PotWithGold : NetworkBehaviour {

//	private Transform startPos;
	private Rigidbody rB;

	public GameObject goldPot;
//	public GameObject rainBow;

	// Use this for initialization
	void Start () {
		rB = GetComponent<Rigidbody>();
////		rB.useGravity = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y <= 0) {
			rB.constraints = RigidbodyConstraints.FreezeAll;
		}

	}

//	[Command]
//	void CmdRequestRainbowSpawn () {
//		RpcRainbow();
//	}
//
//	[ClientRpc]
//	void RpcRainbow () {
//		SpawnRainbow();
//	}
//
//	void SpawnRainbow() {
//		Instantiate(rainBow, transform.position, transform.rotation);
//		rB.velocity = new Vector3 (0,15,15);
//		rB.useGravity = true;
//	}

	void OnTriggerEnter (Collider collider) {
		if (collider.gameObject.name == "Terrain") {
		Debug.Log ("Ground Collision found: spawning Pot on server!");
			int amountPots = GameObject.FindGameObjectsWithTag("GoldPot").Length;
			if (isServer && amountPots == 0) {
				var goldPotSpawn = (GameObject)Instantiate (goldPot, transform.position, transform.rotation);
				NetworkServer.Spawn(goldPotSpawn);
			}
		}
	}
	
}