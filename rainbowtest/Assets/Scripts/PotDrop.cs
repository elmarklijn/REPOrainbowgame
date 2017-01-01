using UnityEngine;
using System.Collections;

public class PotDrop : MonoBehaviour {

	private GameObject GoldPot;
	private GameObject Rainbow;

	// Use this for initialization
	void Start () {
		GoldPot = GameObject.FindWithTag("GoldPot");
		Rainbow = GameObject.FindWithTag("Rainbow");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider collider) {
		if (collider.gameObject.name == "GoldPot") {
		Debug.Log ("POT BIJ BASE");
			int amountPots = GameObject.FindGameObjectsWithTag("GoldPot").Length;
			Destroy(GoldPot);
			Destroy(Rainbow);
		}

	}



}
