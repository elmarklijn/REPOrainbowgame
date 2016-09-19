using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PotWithGold : NetworkBehaviour {

	private Transform targetPos;
	private Rigidbody rB;

	public GameObject goldPot;


	// Use this for initialization
	void Start () {
		rB = GetComponent<Rigidbody>();

		rB.useGravity = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.F)){
			rB.velocity = new Vector3 (0,15,15);

			rB.useGravity = true;
		}
	}

	void OnTriggerEnter (Collider collider) {
		if (collider.gameObject.name == "Terrain") {
		Debug.Log ("Ground Collision found with Pot!");
			int amountPots = GameObject.FindGameObjectsWithTag("GoldPot").Length;
			if (amountPots == 0) {
				Instantiate (goldPot, transform.position, transform.rotation);
			}
		}
	}
	
}
