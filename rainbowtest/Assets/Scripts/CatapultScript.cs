using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CatapultScript : NetworkBehaviour {

private Animator anim;

	void Start () {

		anim = GetComponent<Animator>();
	}


	void Update () {

		if (Input.GetButtonDown("Fire1")) {
			anim.SetBool("IsAiming", true);
		}
		if (Input.GetButtonUp("Fire1")) {
			anim.SetBool("IsAiming", false);
		}
	}
}
