using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
	public Canvas pauseMenu;
	public Text coins;
	public bool anyUIActive = false;
	public float score = 0;

	private void Awake() {
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	private void Start() {
		DataPersistenceManager.Instance.LoadGame();
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Escape) && !anyUIActive) {
			pauseMenu.gameObject.SetActive(true);
			Cursor.lockState = CursorLockMode.None;
			anyUIActive = true;
			Time.timeScale = 0;
		}
	}

	public void InizializeNewGame() {
		DataPersistenceManager.Instance.NewGame();
	}

	public void Continue() {
		pauseMenu.gameObject.SetActive(!pauseMenu.gameObject.activeSelf);
		Cursor.lockState = CursorLockMode.Locked;
		anyUIActive = false;
		Time.timeScale = 1;
	}

	public void MainMenu() {
		Cursor.lockState = CursorLockMode.None;
		Time.timeScale = 1;
		DataPersistenceManager.Instance.SaveGame();
		SceneManager.LoadScene("Main Menu");
	}

	public void UpdateScore(float value) {
		score += value;
		coins.text = score.ToString();
	}
}
