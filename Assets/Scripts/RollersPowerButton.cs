using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

// This is the monitor's power button
public class RollersPowerButton : ButtonAnimation, IDataPersistence
{
	public Canvas interactKM;
	public Canvas interactGamepad;
	public PlayerMovement playerMovement;
	public bool areRollersActive = true;
	public UnityEvent<bool> pressed;
	private bool isInRange = false;

	void Update() {
		if (isPressed) {
			base.Animation(minScale, maxScale, scalePerUnit, axesToScale);

			if (isNegativeAnimation) {
				if (transform.localScale.y >= maxScale) {
					if (areRollersActive) {
						pressed?.Invoke(true);
						areRollersActive = false;
					}
					else {
						pressed?.Invoke(false);
						areRollersActive = true;
					}
					isPressed = false;
				}
			}
		}
	}

	private void OnTriggerStay(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
			if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0) {
				isInRange = true;
				if (playerMovement.currentDevice == "KM") {
					interactKM.gameObject.SetActive(true);
					interactGamepad.gameObject.SetActive(false);
				}
				else {
					interactGamepad.gameObject.SetActive(true);
					interactKM.gameObject.SetActive(false);
				}
			}
			else {
				isInRange = false;
				interactGamepad.gameObject.SetActive(false);
				interactKM.gameObject.SetActive(false);
			}
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			isInRange = false;
			interactGamepad.gameObject.SetActive(false);
			interactKM.gameObject.SetActive(false);
		}
	}

	public void ButtonClick() {
		if (isInRange && !GameManager.Instance.isGamePaused) {
			isPressed = true;
			isPositiveAnimation = true;
			AudioManager.Instance.PlaySFX("Button Click");
		}
	}

	public void LoadData(GameData gameData, bool isNewGame) {
		if (isNewGame)
			gameData.areRollersActive = areRollersActive;
		areRollersActive = gameData.areRollersActive;
	}

	public void SaveData(ref GameData gameData) {
		gameData.areRollersActive = areRollersActive;
	}
}
