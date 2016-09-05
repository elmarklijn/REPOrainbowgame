using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class PlayerMotor : MonoBehaviour {

	private Animator anim;

	[SerializeField]
	private Camera cam;

	private Vector3 velocity = Vector3.zero;
	private Vector3 rotation = Vector3.zero;
	private float cameraRotationX = 0f;
	private float currentCameraRotationX = 0f;


	[SerializeField]
	private float cameraRotationLimit = 85f;

	private Rigidbody rb;

	void Start () {
		rb = GetComponent<Rigidbody>();
		anim = GetComponentInChildren<Animator>();
	}	

	// get movement vector
	public void Move (Vector3 _velocity) {
		velocity = _velocity;
	}

	// get rotation vector
	public void Rotate (Vector3 _rotation) {
		rotation = _rotation;
	}

	// get camerarotation vector
	public void RotateCamera (float _cameraRotationX) {
		cameraRotationX = _cameraRotationX;
	}


	// run physics fixed
	void FixedUpdate () {
		PerformMovement();
		PerformRotation();
	}

	//Perform movement based on velocity var
	void PerformMovement() {
		if (velocity != Vector3.zero) {
			rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
		}
	}

	//perform rotation
	void PerformRotation() {
		rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
		if (cam != null) {
			//set rotation en clampen
			currentCameraRotationX -= cameraRotationX; //GEBRUIK -= of += VOOR INVERSION
			currentCameraRotationX = Mathf.Clamp (currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
			//apply rotatie naar transform van de camera
			cam.transform.localEulerAngles = new Vector3 (currentCameraRotationX, 0f, 0f);
		}
	}
}
