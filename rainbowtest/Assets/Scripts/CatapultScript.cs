using UnityEngine;
using System.Collections;


public class CatapultScript : MonoBehaviour {

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
