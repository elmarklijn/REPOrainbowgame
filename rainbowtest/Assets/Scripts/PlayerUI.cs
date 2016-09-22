using UnityEngine;

public class PlayerUI : MonoBehaviour {

	[SerializeField]
	RectTransform StaminaFill;
	[SerializeField]
	GameObject pauseMenu;

	private PlayerController controller;

	public void SetController (PlayerController _controller) {
		controller = _controller;
	}

	void Awake () {
		controller = GetComponent<PlayerController>();
		PauseMenu.IsOn = false;
	}

	void Update () {
		SetStaminaAmount(controller.GetStaminaAmount());

		//pause menu
		if (Input.GetKeyDown(KeyCode.Escape)) {
			TogglePauseMenu();
		}
	}

	public void TogglePauseMenu() {
		pauseMenu.SetActive (!pauseMenu.activeSelf);
		PauseMenu.IsOn = pauseMenu.activeSelf;
	}


	void SetStaminaAmount (float _amount) {
		StaminaFill.localScale = new Vector3 (1f, _amount, 1f);
	}




}
