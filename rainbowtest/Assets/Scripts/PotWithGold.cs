using UnityEngine;
using System.Collections;

public class PotWithGold : MonoBehaviour {

	private Transform targetPos;
	private Rigidbody rB;
	private Rigidbody goldPotrB;

	public GameObject goldPot;


	// Use this for initialization
	void Start () {
		rB = GetComponent<Rigidbody>();
		goldPotrB = goldPot.GetComponent<Rigidbody>();

		rB.useGravity = false;
		goldPotrB.useGravity = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.F)){
			rB.velocity = new Vector3 (0,15,15);
			goldPotrB.velocity = new Vector3 (0,15,15);

			rB.useGravity = true;
			goldPotrB.useGravity = true;
		}
		if (transform.position.y <= 2f) {
			rB.drag = 100;
		}
	}
}
