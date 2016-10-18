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
	private float lookSensitivity = 3f;
	private PlayerMotor motor;

	private Animator animator;

	[SerializeField]
	private float staminaAmount = 1f;
	private float staminaBurn = 0.3f;
	private float staminaRegen = 0.1f;

	//getter voor de staminabalk
	public float GetStaminaAmount () {
		return staminaAmount;	
	}


	//pickup and carry
	public GameObject carryObject;
	private GameObject GoldPot;
	[HideInInspector]
	public static bool isHolding = false;

	void Start () {
		motor = GetComponent<PlayerMotor>();
		animator = GetComponentInChildren<Animator>();
	}

	void Update () {

		//pause
		if (PauseMenu.IsOn) {

			if (Cursor.lockState != CursorLockMode.None)
				Cursor.lockState = CursorLockMode.None;
			motor.Move (Vector3.zero);
			motor.Rotate (Vector3.zero);
			motor.RotateCamera(0f);

			return;
		}

		if (Cursor.lockState != CursorLockMode.Locked) {
			Cursor.lockState = CursorLockMode.Locked;
		}


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

		//clampen van stamina
		staminaAmount = Mathf.Clamp (staminaAmount, 0f, 1f);

		//apply run
		if (Input.GetButton ("Sprint") && staminaAmount >= 0.02f) {
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

		//goldpot oppakken
		if (Input.GetButtonDown("Use") && isHolding == false) {
			if (GoldPot != null) {
				animator.SetBool("isCarrying", true);
				GetComponent<PlayerManager>().CmdWantToPickUp();
//				GoldPot.GetComponent<NetworkIdentity>().AssignClientAuthority(this.gameObject.GetComponent<NetworkIdentity>().connectionToClient);
//				Debug.Log ("authority voor de goldpot voor: " + this.gameObject.GetComponent<NetworkIdentity>().connectionToClient);
				GoldPot.GetComponent<BoxCollider>().enabled = false;
				GoldPot.GetComponent<Rigidbody>().isKinematic = true;
				GoldPot.transform.SetParent(carryObject.transform);
				GoldPot.transform.localPosition = Vector3.zero;
				GoldPot.transform.localRotation = Quaternion.identity;
				isHolding = true;
				}
			}
		else if (Input.GetButtonDown("Use") && isHolding == true) {
			if (GoldPot != null) {
				animator.SetBool("isCarrying", false);
				GetComponent<PlayerManager>().CmdWantToPutDown();
//				GoldPot.GetComponent<NetworkIdentity>().RemoveClientAuthority(this.gameObject.GetComponent<NetworkIdentity>().connectionToClient);
//				Debug.Log ("lost authority voor de goldpot voor: " + this.gameObject.GetComponent<NetworkIdentity>().connectionToClient);
				GoldPot.GetComponent<BoxCollider>().enabled = true;
				GoldPot.GetComponent<Rigidbody>().isKinematic = false;
				GoldPot.transform.SetParent(null);
				isHolding = false;
				}
			}
		//TIJDELIJK!!
		if (Input.GetKey(KeyCode.K))
			GetComponentInParent<PlayerManager>().RpcTakeDamage(999999, this.name);

	}


		//triggerenter goldpot oppakken
		void OnTriggerEnter (Collider collider) {
			Debug.Log ("close enough to pick up pot");
			GoldPot = collider.gameObject;
			Debug.Log ("collider found: " + GoldPot);
		}
		//triggerenter goldpot weg
		void OnTriggerExit (Collider collider) {
			Debug.Log ("NOT close enough to pick up pot");
			GoldPot = null;
			Debug.Log ("collider found: " + GoldPot);
		}

}
	