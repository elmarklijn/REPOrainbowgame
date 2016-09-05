using UnityEngine;
using System.Collections;

public class CameraRotation : MonoBehaviour {

	public float yAmount;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0f, yAmount, 0f, Space.World);
	}
}
