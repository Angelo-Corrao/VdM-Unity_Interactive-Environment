using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, IDataPersistence {
	public string id;
	public float value = 5;
	private bool isCollected = false;

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			GameManager.Instance.UpdateScore(value);
			isCollected = true;
			Destroy(gameObject);
		}
	}

	public void LoadData(GameData gameData, bool isNewGame) {
		if (isNewGame) {
			gameData.coinsCollected = new SerializableDictionary<string, bool>();
		}
		gameData.coinsCollected.TryGetValue(id, out isCollected);
		if (isCollected)
			Destroy(gameObject);
	}

	public void SaveData(ref GameData gameData) {
		if (isCollected)
			if (!gameData.coinsCollected.ContainsKey(id))
				gameData.coinsCollected.Add(id, isCollected);
	}
}
