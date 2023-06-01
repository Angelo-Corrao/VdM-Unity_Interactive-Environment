using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VerticalStair : MonoBehaviour
{
	public Canvas interactKM;
	public Canvas interactGamepad;
	public Canvas releaseKM;
	public Canvas releaseGamepad;
	public PlayerMovement playerMovement;
	public UnityEvent interacted;
	private bool isInRange = false;

	private void OnTriggerStay(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			if (!playerMovement.isInVerticalStair) {
				releaseKM.gameObject.SetActive(false);
				releaseGamepad.gameObject.SetActive(false);
				Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
				if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.z > 0) {
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
					interactKM.gameObject.SetActive(false);
					interactGamepad.gameObject.SetActive(false);
				}
			}
			else {
				isInRange = true;
				interactKM.gameObject.SetActive(false);
				interactGamepad.gameObject.SetActive(false);
				if (playerMovement.currentDevice == "KM") {
					interactKM.gameObject.SetActive(true);
					interactGamepad.gameObject.SetActive(false);
				}
				else {
					interactGamepad.gameObject.SetActive(true);
					interactKM.gameObject.SetActive(false);
				}
			}
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			isInRange = false;
			interactKM.gameObject.SetActive(false);
			interactGamepad.gameObject.SetActive(false);
		}
	}

	public void PlayerHasInteracted() {
		if (isInRange && !GameManager.Instance.isGamePaused) {
			interacted?.Invoke();
		}
	}
}
