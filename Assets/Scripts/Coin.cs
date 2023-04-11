using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
	public float value = 5;

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			GameManager.Instance.UpdateScore(value);
			Destroy(gameObject);
		}
	}
}
