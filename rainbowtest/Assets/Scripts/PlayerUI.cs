using UnityEngine;

public class PlayerUI : MonoBehaviour {

	[SerializeField]
	RectTransform StaminaFill;

	private PlayerController controller;

	public void SetController (PlayerController _controller) {
		controller = _controller;
	}

	void Awake () {
		controller = GetComponent<PlayerController>();
	}

	void Update () {
		SetStaminaAmount(controller.GetStaminaAmount());
	}

	void SetStaminaAmount (float _amount) {
		StaminaFill.localScale = new Vector3 (1f, _amount, 1f);
	}


}
