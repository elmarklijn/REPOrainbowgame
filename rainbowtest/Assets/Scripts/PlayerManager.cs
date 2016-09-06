using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

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

	[SerializeField]
	private Behaviour[] disableOnDeath;
	private bool[] wasEnabled;

	public void Setup () {

		wasEnabled = new bool[disableOnDeath.Length];
		for (int i = 0; i < wasEnabled.Length; i++) {
			wasEnabled[i] = disableOnDeath[i].enabled;
		}

		SetDefaults();
	}

	[ClientRpc]
	public void RpcTakeDamage (int amount) {
		if (isDead)
			return;

		currentHealth -= amount;

		Debug.Log (transform.name + " now has " + currentHealth + " health.");

		if (currentHealth <= 0) {
			Die();
		}
	}

	private void Die() {
		isDead = true;

		//disable components
		for (int i = 0; i < disableOnDeath.Length; i++) {
			disableOnDeath[i].enabled = false;
		}
//		Collider col = GetComponent<Collider>();
//		if (col != null)
//			col.enabled = false;

		Debug.Log (transform.name + " is Dead!");

		// call respawn
		StartCoroutine(Respawn());

	}

	private IEnumerator Respawn () {
		
		yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);
		SetDefaults();
		Transform startPoint = NetworkManager.singleton.GetStartPosition();
		transform.position = startPoint.position;
		transform.rotation = startPoint.rotation;
	}


	public void SetDefaults() {

		isDead = false;

		currentHealth = maxHealth;

		for (int i = 0; i < disableOnDeath.Length; i++) {
			disableOnDeath[i].enabled = wasEnabled[i];
		}

		Collider col = GetComponent<Collider>();
		if (col != null)
			col.enabled = true;
	}

}
