using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Pickupable : NetworkBehaviour {

	private PlayerController player;

	public bool isHolding = false;

//	void Update() {
//
////	//goldpot oppakken
////		if (Input.GetButtonDown("Use") && isHolding == false) {
////	//			player.animator.SetBool("isCarrying", true);
////				GetComponent<BoxCollider>().enabled = false;
////				GetComponent<Rigidbody>().isKinematic = true;
////
////				transform.SetParent(carryObject.transform);
////				transform.localPosition = Vector3.zero;
////				transform.localRotation = Quaternion.identity;
////				isHolding = true;
////			}
////		else if (Input.GetButtonDown("Use") && isHolding == true) {
////			if (GoldPot != null) {
////				animator.SetBool("isCarrying", false);
////				GoldPot.GetComponent<BoxCollider>().enabled = true;
////				GoldPot.GetComponent<Rigidbody>().isKinematic = false;
////				GoldPot.transform.SetParent(null);
////				isHolding = false;
////				}
////			}
//	}
//
//		//triggerenter goldpot oppakken
//		void OnTriggerEnter (Collider collider) {
//			Debug.Log ("close enough to pick up pot");
//			player = collider.gameObject.GetComponent<PlayerController>();
//			Debug.Log ("collider found: " + player);
//		}
//		//triggerenter goldpot weg
//		void OnTriggerExit (Collider collider) {
//			Debug.Log ("NOT close enough to pick up pot");
//			player = null;
//			Debug.Log ("collider found: " + player);
//		}
//
}
