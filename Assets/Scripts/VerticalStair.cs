using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VerticalStair : MonoBehaviour
{
	public Canvas interact;
	public Canvas release;
	public PlayerMovement playerMovement;
	public UnityEvent interacted;
	private bool isInRange = false;

	private void Update() {
		if (isInRange) {
			if (Input.GetKeyDown(KeyCode.E) && !GameManager.Instance.anyUIActive) {
				interacted?.Invoke();
			}
		}
	}

	private void OnTriggerStay(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			if (!playerMovement.isInVerticalStair) {
				release.gameObject.SetActive(false);
				Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
				if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.z > 0) {
					isInRange = true;
					interact.gameObject.SetActive(true);
				}
				else {
					isInRange = false;
					interact.gameObject.SetActive(false);
				}
			}
			else {
				isInRange = true;
				interact.gameObject.SetActive(false);
				release.gameObject.SetActive(true);
			}
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			isInRange = false;
			interact.gameObject.SetActive(false);
		}
	}
}
