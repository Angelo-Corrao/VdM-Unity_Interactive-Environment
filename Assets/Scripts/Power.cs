using System.Threading;
using UnityEngine;
using UnityEngine.Events;

// This is the monitor's power button
public class Power : ButtonAnimation
{
	public Canvas interact;
	public bool areRollersActive = true;
	public UnityEvent<bool> pressed;
	private bool isInRange = false;


	void Update() {
		if (isInRange) {
			if (Input.GetKeyDown(KeyCode.E)) {
				isPressed = true;
				isPositiveAnimation = true;
			}
		}

		//base.Animation(minScale, maxScale, scalePerUnit, axesToScale);
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
				}
			}
		}
	}

	private void OnTriggerStay(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
			if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0) {
				isInRange = true;
				interact.gameObject.SetActive(true);
			}
			else {
				isInRange = false;
				interact.gameObject.SetActive(false);
			}
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			isInRange = false;
			interact.gameObject.SetActive(false);
		}
	}

	// This is nedeed when the game restart, so if a game end with the monitor turned off the next game it will be on again
	/*public void setMonitorOn() {
		monitor.gameObject.SetActive(true);
		if (main)
			lines.SetActive(true);
		isPcOn = true;
		mesh.material = materialsOn;
	}*/
}
