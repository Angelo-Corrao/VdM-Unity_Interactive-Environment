using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
	public Text coins;
	private float score = 0;

	private void Awake() {
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	public void NewGame() {
		Debug.Log("New Game");
	}

	public void Continue() {

	}

	public void Quit() {

	}

	public void UpdateScore(float value) {
		score += value;
		coins.text = score.ToString();
	}
}
