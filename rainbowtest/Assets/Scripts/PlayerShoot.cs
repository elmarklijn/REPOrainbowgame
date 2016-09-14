using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

	//PROBEERSEL
	public GameObject Ammo;
	public float ammospeed = 50f;

	//vars voor richten en schieten
	private float startPull;
	private float minPull;

	public PlayerWeapon weapon;

	private const string PLAYER_TAG = "Player";
	[SerializeField]
	private Camera cam;
	[SerializeField]
	private LayerMask mask;
	private Animator anim;
	private RaycastHit hit;

	void Start () {

		if (cam == null){
			Debug.LogError ("No camera referenced on shooting script");
			this.enabled = false;
		}

		anim = GetComponentInChildren<Animator>();
	}

	void Update () {

		if (!isLocalPlayer) {
			return;
		}
		if (PlayerController.isHolding == true) {
			return;
		}
		if (Input.GetButtonDown("Fire1")) {
			startPull = Time.time;
			anim.SetBool("PlayerAiming", true);
		}
		minPull = Time.time - startPull;
		if (minPull > 0.5f) {
			if (Input.GetButtonUp("Fire1")) {
				Shoot();
				anim.SetBool("PlayerAiming", false);
			}
		}
		if (minPull < 0.5f) {
			if (Input.GetButtonUp("Fire1")) {
				anim.SetBool("PlayerAiming", false);

			}
		}
	}

	[Command]
	void CmdOnShoot () {
		RpcDoShootEffect();
	}

	[ClientRpc]
	void RpcDoShootEffect () {
		SpawnRockAmmo();
	}

	void SpawnRockAmmo() {
		//create steen ammo
		GameObject newAmmo = Instantiate(Ammo, cam.transform.position, cam.transform.rotation) as GameObject;
		newAmmo.GetComponent<Rigidbody>().velocity = (hit.point - cam.transform.position).normalized * ammospeed;
		//destroy ammo
		Destroy (newAmmo, 5f);
	}

	[Client]
	void Shoot() {
		if (!isLocalPlayer) {
			return;
			}

		CmdOnShoot();

		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, weapon.range, mask)){
			Debug.Log (hit.collider);
			//we hit something with raycast
			if (hit.collider.tag == PLAYER_TAG) {
				CmdPlayerShot(hit.collider.name, weapon.damage);
			}
		}
	}

	[Command]
	void CmdPlayerShot(string playerID, int damage) {
		Debug.Log (playerID + " has been shot.");

		PlayerManager player = GameManager.GetPlayer(playerID);
		player.RpcTakeDamage(damage);
	}


}
