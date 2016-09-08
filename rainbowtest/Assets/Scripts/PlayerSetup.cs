using UnityEngine;
using UnityEngine.Networking;


[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(PlayerController))]

public class PlayerSetup : NetworkBehaviour {

	[SerializeField]
	Behaviour[] componentsToDisable;
	[SerializeField]
	string remoteLayerName = "RemotePlayer";

	[SerializeField]
	string dontDrawLayerName = "DontDraw";
	[SerializeField]
	GameObject playerGraphics;

	[SerializeField]
	GameObject playerUIPrefab;
	[HideInInspector]
	public GameObject playerUIInstance;


	void Start() {
		if (!isLocalPlayer) {
			DisableComponents();
			AssignRemoteLayer();
		} else {

			//disable player graphics for local player
			SetLayerRecursively (playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));

			//create player UI
			playerUIInstance = Instantiate (playerUIPrefab);
			playerUIInstance.name = playerUIPrefab.name;

			//configure player UI
			PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
			if (ui == null)
				Debug.LogError ("No player UI component on player prefab");
			ui.SetController (GetComponent<PlayerController>());
		}

		GetComponent<PlayerManager>().Setup();
	}

	void SetLayerRecursively (GameObject obj, int newLayer) {
		obj.layer = newLayer;
		foreach (Transform child in obj.transform){
			SetLayerRecursively (child.gameObject, newLayer);
		}
	}

	public override void OnStartClient() {
		base.OnStartClient();

		string netID = GetComponent<NetworkIdentity>().netId.ToString();
		PlayerManager player = GetComponent<PlayerManager>();

		GameManager.RegisterPlayer(netID, player);
	}

	void AssignRemoteLayer() {
		gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
	}

	void DisableComponents () {
		for (int i = 0; i < componentsToDisable.Length; i++) {
			componentsToDisable[i].enabled = false;
			}
	}

	//when destroyed
	void OnDisable () {

		Destroy (playerUIInstance);

		GameManager.instance.SceneCameraActive(true);

		GameManager.UnRegisterPlayer(transform.name);
	}

}
