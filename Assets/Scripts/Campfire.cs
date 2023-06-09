using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public enum FireState {
	OFF = 0,
	DEFAULT = 1,
	RED = 2,
	GREEN = 3,
}

public class Campfire : MonoBehaviour, IDataPersistence
{
	public Canvas interact;
	public VisualEffect fire_VFX;
	private bool isInRange = false;
	private FireState fireState = 0;
	private AudioSource fire_SFX;

	private void Update() {
		if (isInRange) {
			if (Input.GetKeyDown(KeyCode.E) && !GameManager.Instance.anyUIActive) {
				Vector4 fireColor;
				Vector4 smokeColor;
				switch (fireState) {
					case FireState.OFF:
						AudioManager.Instance.PlaySFXAtPoint("Fire", transform.position);
						fire_SFX = GameObject.Find("One shot audio").gameObject.GetComponent<AudioSource>();
						fire_SFX.loop = true;
						fire_VFX.SendEvent("FirePlay");
						StartCoroutine(SmokeEvent(false));
						fireColor = new Color(191f / 255, 191f / 255, 191f / 255);
						fire_VFX.SetVector4("FireColor", fireColor);
						smokeColor = new Color(51f / 255, 51f / 255, 51f / 255);
						StartCoroutine(ChangeSmokeColor(smokeColor));
						fireState = FireState.DEFAULT;
						break;

					case FireState.DEFAULT:
						fireColor = new Color(214f / 255, 11f / 255, 11f / 255);
						fire_VFX.SetVector4("FireColor", fireColor);
						smokeColor = new Color(241f / 255, 158f / 255, 101f / 255);
						StartCoroutine(ChangeSmokeColor(smokeColor));
						fireState = FireState.RED;
						break;

					case FireState.RED:
						fireColor = new Color(20f / 255, 180f / 255, 42f / 255);
						fire_VFX.SetVector4("FireColor", fireColor);
						smokeColor = new Color(7f / 255, 63f / 255, 2f / 255);
						StartCoroutine(ChangeSmokeColor(smokeColor));
						fireState = FireState.GREEN;
						break;

					case FireState.GREEN:
						if (fire_SFX != null) {
							fire_SFX.Stop();
							Destroy(fire_SFX.gameObject);
						}
						fire_VFX.SendEvent("FireStop");
						StartCoroutine(SmokeEvent(true));
						fireState = FireState.OFF;
						break;
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

	private IEnumerator SmokeEvent(bool isSmokeActive) {
		yield return new WaitForSeconds(3);
		if (!isSmokeActive)
			fire_VFX.SendEvent("SmokePlay");
		else
			fire_VFX.SendEvent("SmokeStop");
	}

	private IEnumerator ChangeSmokeColor(Vector4 smokeColor) {
		yield return new WaitForSeconds(3);
		fire_VFX.SetVector4("SmokeColor", smokeColor);
	}

	private void InitializeFireState() {
		Vector4 fireColor;
		Vector4 smokeColor;
		switch (fireState) {
			case FireState.DEFAULT:
				AudioManager.Instance.PlaySFXAtPoint("Fire", transform.position);
				fire_SFX = GameObject.Find("One shot audio").gameObject.GetComponent<AudioSource>();
				fire_SFX.loop = true;
				fire_VFX.SendEvent("FirePlay");
				StartCoroutine(SmokeEvent(false));
				fireColor = new Color(191f / 255, 191f / 255, 191f / 255);
				fire_VFX.SetVector4("FireColor", fireColor);
				smokeColor = new Color(51f / 255, 51f / 255, 51f / 255);
				StartCoroutine(ChangeSmokeColor(smokeColor));
				fireState = FireState.DEFAULT;
				break;

			case FireState.RED:
				AudioManager.Instance.PlaySFXAtPoint("Fire", transform.position);
				fire_SFX = GameObject.Find("One shot audio").gameObject.GetComponent<AudioSource>();
				fire_SFX.loop = true;
				fire_VFX.SendEvent("FirePlay");
				StartCoroutine(SmokeEvent(false));
				fireColor = new Color(214f / 255, 11f / 255, 11f / 255);
				fire_VFX.SetVector4("FireColor", fireColor);
				smokeColor = new Color(241f / 255, 158f / 255, 101f / 255);
				StartCoroutine(ChangeSmokeColor(smokeColor));
				fireState = FireState.RED;
				break;

			case FireState.GREEN:
				AudioManager.Instance.PlaySFXAtPoint("Fire", transform.position);
				fire_SFX = GameObject.Find("One shot audio").gameObject.GetComponent<AudioSource>();
				fire_SFX.loop = true;
				fire_VFX.SendEvent("FirePlay");
				StartCoroutine(SmokeEvent(false));
				fireColor = new Color(20f / 255, 180f / 255, 42f / 255);
				fire_VFX.SetVector4("FireColor", fireColor);
				smokeColor = new Color(7f / 255, 63f / 255, 2f / 255);
				StartCoroutine(ChangeSmokeColor(smokeColor));
				fireState = FireState.GREEN;
				break;
		}
	}

	public void LoadData(GameData gameData, bool isNewGame) {
		if (isNewGame)
			gameData.fireState = FireState.OFF;
		fireState = gameData.fireState;
		InitializeFireState();
	}

	public void SaveData(ref GameData gameData) {
		gameData.fireState = fireState;
	}
}
