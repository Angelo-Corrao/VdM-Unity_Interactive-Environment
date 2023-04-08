using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public enum FireState {
	OFF = 0,
	DEFAULT = 1,
	RED = 2,
	GREEN = 3,
}

public class PlayerInteractions : MonoBehaviour
{
	public Canvas interact;
	public Canvas release;
	public Transform verticalStair;
	public Transform campFire;
	public VisualEffect fire_VFX;
	private FireState fireState = 0;
	private bool isInVerticalStairRange = false;
	private bool isInCampfireRange = false;
	private CharacterController controller;
	//private bool gfd = false;

	private void Start() {
		controller = GetComponent<CharacterController>();
		fire_VFX.Stop();
	}

	private void Update() {
		if (isInVerticalStairRange) {
			if (Input.GetKeyDown(KeyCode.E)) {
				if (!PlayerState.isInVerticalStair) {
					transform.SetParent(verticalStair);
					controller.enabled = false;
					if (controller.transform.localPosition.y > 6.84f + 1f)
						controller.transform.localPosition = new Vector3(0f, 6.84f + 1f, -1f);
					else
						controller.transform.localPosition = new Vector3(0f, controller.transform.localPosition.y, -1f);
					controller.enabled = true;
					PlayerState.isInVerticalStair = true;
				}
				else {
					transform.SetParent(null);
					PlayerState.isInVerticalStair = false;
				}
			}
		}

		if (isInCampfireRange) {
			if (Input.GetKeyDown(KeyCode.E)) {
				Vector4 color;
				switch (fireState) {
					case FireState.OFF:
						fire_VFX.Play();
						fireState = FireState.DEFAULT;
						break;

					case FireState.DEFAULT:
						color = new Color(214f / 255, 11f / 255, 11f / 255);
						fire_VFX.SetVector4("FireColor", color);
						fireState = FireState.RED;
						break;

					case FireState.RED:
						color = new Color(20f / 255, 180f / 255, 42f / 255);
						fire_VFX.SetVector4("FireColor", color);
						fireState = FireState.GREEN;
						break;

					case FireState.GREEN:
						fire_VFX.Stop();
						fireState = FireState.OFF;
						break;
				}
			}
		}
	}

	private void OnTriggerStay(Collider other) {
		if (other.gameObject.CompareTag("VerticalStair")) {
			if (!PlayerState.isInVerticalStair) {
				release.gameObject.SetActive(false);
				Vector3 viewPos = Camera.main.WorldToViewportPoint(verticalStair.position);
				if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.z > 0){
					isInVerticalStairRange = true;
					interact.gameObject.SetActive(true);
				}
				else {
					isInVerticalStairRange = false;
					interact.gameObject.SetActive(false);
				}
			}
			else {
				interact.gameObject.SetActive(false);
				release.gameObject.SetActive(true);
			}
		}

		if (other.gameObject.CompareTag("Campfire")) {
			Vector3 viewPos = Camera.main.WorldToViewportPoint(campFire.position);
			if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0) {
				isInCampfireRange = true;
				interact.gameObject.SetActive(true);
			}
			else {
				isInCampfireRange = false;
				interact.gameObject.SetActive(false);
			}
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag("VerticalStair")) {
			isInVerticalStairRange = false;
			interact.gameObject.SetActive(false);
		}

		if (other.gameObject.CompareTag("Campfire")) {
			isInCampfireRange = false;
			interact.gameObject.SetActive(false);
		}
	}
}
