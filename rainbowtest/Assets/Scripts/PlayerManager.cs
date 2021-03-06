﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


[RequireComponent(typeof(PlayerSetup))]

public class PlayerManager : NetworkBehaviour {

	[SyncVar]
	private bool _isDead = false;
	public bool isDead
	{
		get { return _isDead; }
		protected set { _isDead = value; }
	}

	[SerializeField]
	private int maxHealth = 100;
	[SyncVar]
	private int currentHealth;

	public int kills;
	public int deaths;

	[SerializeField]
	private Behaviour[] disableOnDeath;
	private bool[] wasEnabled;
	[SerializeField]
	private GameObject[] disableGameObjectsOnDeath;
	[SerializeField]
	private GameObject spawnEffect;
	private bool firstSetup = true;

	public Animator anim;
	private GameObject GoldPot;




	public void SetupPlayer () {

		if (isLocalPlayer) {
			//switch camera en enable UI
			GameManager.instance.SceneCameraActive(false);
			GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
		}

		CmdBroadCastNewPlayerSetup();
	}

	[Command]
	private void CmdBroadCastNewPlayerSetup() {
		RpcSetupPlayerOnAllClients();
	}

	[ClientRpc]
	private void RpcSetupPlayerOnAllClients () {
		if (firstSetup) {
			wasEnabled = new bool[disableOnDeath.Length];
			for (int i = 0; i < wasEnabled.Length; i++) {
				wasEnabled[i] = disableOnDeath[i].enabled;
			}
								//probeersel !KAN ALLEN VERWIJDERT WORDEN ALS HET NIET WERKT!//
								GetComponent<SpawnColor>().GetStartColor();
			firstSetup = false;
		}

		SetDefaults();
	}



	[ClientRpc]
	public void RpcTakeDamage (int amount, string _sourceID) {
		if (isDead)
			return;

		currentHealth -= amount;

		Debug.Log (transform.name + " now has " + currentHealth + " health.");

		if (currentHealth <= 0) {

			Die(_sourceID);
		}
	}

	private void Die(string _sourceID) {
		isDead = true;

		PlayerManager sourcePlayer = GameManager.GetPlayer(_sourceID);
		if (sourcePlayer != null) {
			sourcePlayer.kills++;
		}

		deaths++;
		//drop pot if carrying
		if (GameObject.FindGameObjectWithTag("GoldPot") != null) {
			if (GetComponentInChildren<Pickupable>()) {
			GoldPot = GameObject.FindGameObjectWithTag("GoldPot");
			GoldPot.GetComponent<BoxCollider>().enabled = true;
			GoldPot.GetComponent<Rigidbody>().isKinematic = false;
			GoldPot.transform.SetParent(null);
			PlayerController.isHolding = false;
			anim.SetBool("isCarrying", false);
		}
		}
		//disable components
		for (int i = 0; i < disableOnDeath.Length; i++) {
			disableOnDeath[i].enabled = false;
		}

		//disable gameobjects
		for (int i = 0; i < disableGameObjectsOnDeath.Length; i++) {
			disableGameObjectsOnDeath[i].SetActive(false);
		}

//		Collider col = GetComponent<Collider>();
//		if (col != null)
//			col.enabled = false;

		//animatie
		anim.SetBool("isDead", true);
		// switch camera and disable UI																						
		if (isLocalPlayer){
			GameManager.instance.SceneCameraActive(true);
			GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
		}

		Debug.Log (transform.name + " is Dead!");

		// call respawn
		StartCoroutine(Respawn());

	}

	private IEnumerator Respawn () {
		
		yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);

		Transform startPoint = NetworkManager.singleton.GetStartPosition();
		transform.position = startPoint.position;
		transform.rotation = startPoint.rotation;

		yield return new WaitForSeconds(0.1f);

		SetupPlayer();
	}


	public void SetDefaults() {

		isDead = false;
		anim.SetBool("isDead", false);
		currentHealth = maxHealth;

		//enable components
		for (int i = 0; i < disableOnDeath.Length; i++) {
			disableOnDeath[i].enabled = wasEnabled[i];
		}

		//enable gameobjects
		for (int i = 0; i < disableGameObjectsOnDeath.Length; i++) {
			disableGameObjectsOnDeath[i].SetActive(true);
		}

		//enable collider
		Collider col = GetComponent<Collider>();
		if (col != null)
			col.enabled = true;


		//create spawneffect
		GameObject _gfxIns = (GameObject)Instantiate(spawnEffect, transform.position, Quaternion.identity);
		Destroy(_gfxIns, 3f);
	}

	[Command]
		public void CmdWantToPickUp () {
			GoldPot = GameObject.FindGameObjectWithTag("GoldPot");
			GoldPot.GetComponent<NetworkIdentity>().AssignClientAuthority(this.gameObject.GetComponent<NetworkIdentity>().connectionToClient);
//			Debug.Log ("authority voor de goldpot voor: " + this.gameObject.GetComponent<NetworkIdentity>().connectionToClient);
		}

	[Command]
		public void CmdWantToPutDown() {
		GoldPot = GameObject.FindGameObjectWithTag("GoldPot");
			if (GoldPot.GetComponent<NetworkIdentity>().clientAuthorityOwner != null) {
				Debug.Log ("found a KUTMORON! proberen te verwijderen:");
				GoldPot.GetComponent<NetworkIdentity>().RemoveClientAuthority(GoldPot.GetComponent<NetworkIdentity>().clientAuthorityOwner);
//			Debug.Log ("lost authority voor de goldpot voor: " + this.gameObject.GetComponent<NetworkIdentity>().connectionToClient);
			} else { return; }
		}
}
