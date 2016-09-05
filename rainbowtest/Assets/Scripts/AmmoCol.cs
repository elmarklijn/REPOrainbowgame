using UnityEngine;
using System.Collections;

public class AmmoCol : MonoBehaviour {

public GameObject AmmoSmoke;


	// Use this for initialization
	void Start () {
	
	}

	void OnCollisionEnter (Collision ammoCollider) {
		foreach (ContactPoint contact in ammoCollider.contacts) {
            	Debug.DrawRay(contact.point, contact.normal, Color.white);
				Instantiate (AmmoSmoke, contact.point, Quaternion.identity);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}