using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(PlayerMotor))]

public class PlayerController : MonoBehaviour {

	[SerializeField]
	private float speed = 5f;
	[SerializeField]
	private float defaultSpeed = 5f;
	[SerializeField]
	private float runspeed = 10f;
	[SerializeField]
	private float staminaStopSpeed = 1f;
	[SerializeField]
	private float lookSensitivity = 3f;
	private bool cantSprint = false;
	private PlayerMotor motor;

	private Animator animator;

	[SerializeField]
	private float staminaAmount = 1f;
	private float staminaBurn = 0.3f;
	private float staminaRegen = 0.1f;
	private float staminaStopSec = 3f;


	//getter voor de staminabalk
	public float GetStaminaAmount () {
		Debug.Log (staminaAmount);
		return staminaAmount;	
	}

	void Start () {
		motor = GetComponent<PlayerMotor>();
		animator = GetComponentInChildren<Animator>();
	}

	void Update () {
		//bereken movement als 3d vector
		float xMov = Input.GetAxis("Horizontal");
		float zMov = Input.GetAxis("Vertical");

		Vector3 movHorizontal = transform.right * xMov;
		Vector3 movVertical = transform.forward * zMov;

		//final movement vector
		Vector3 velocity = (movHorizontal + movVertical) * speed;

		//animate movement 
		animator.SetFloat("ForwardVelocity", zMov);

		//apply movement
		motor.Move (velocity);

		//bereken rotatie ALLEEN OM TE DRAAIEN (dus niet omhoog/ beneden kijken, want dat moet in de camera)
		float yRot = Input.GetAxisRaw("Mouse X");

		Vector3 rotation = new Vector3 (0f, yRot, 0f) * lookSensitivity;

		//apply rotation
		motor.Rotate(rotation);


		//bereken CAMERA rotatie
		float xRot = Input.GetAxisRaw("Mouse Y");

		float cameraRotationX = xRot * lookSensitivity;

		//apply camerarotation
		motor.RotateCamera(cameraRotationX);

		//apply run
		if (Input.GetButton ("Sprint") && cantSprint == false && staminaAmount > 0.05f) {
			//stamina burnen in real time, geen framerate afhankelijk
			staminaAmount -= staminaBurn * Time.deltaTime;
			speed = runspeed;
			animator.speed = 2f;
		} else {
			//stamina terug krijgen
			staminaAmount += staminaRegen * Time.deltaTime;
			speed = defaultSpeed;
			animator.speed = 1f;
		}
		//clampen van stamina
		staminaAmount = Mathf.Clamp (staminaAmount, 0f, 1f);
		if (staminaAmount <= 0.05f) {
			cantSprint = true;
			Invoke("StaminaStop", staminaStopSec);
			animator.speed = 0.2f;
			cantSprint = false;
		}
		if (staminaAmount >= 1f) {
			speed = defaultSpeed;
			animator.speed = 1f;
		}
		GetStaminaAmount();

	}


	//stamina invoke method
	void StaminaStop() {
		speed = staminaStopSpeed;
		}



}
