using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public static GameManager Instance;
	public Canvas pauseMenu;
	public Text coins;
	public Text death;
	public UnityEvent respawn;
	public bool anyUIActive = false;
	public float score = 0;
	public int deathCounter = 0;

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

	public void Respawn() {
		Cursor.lockState = CursorLockMode.Locked;
		Time.timeScale = 1;
		anyUIActive = false;
		Death();
		respawn?.Invoke();
	}

	public void UpdateScore(float value) {
		score += value;
		coins.text = score.ToString();
	}

	public void Death() {
		deathCounter++;
		death.text= deathCounter.ToString();
	}

	public void LoadData(GameData gameData, bool isNewGame) {
		if (isNewGame) {
			gameData.score = score;
			gameData.deathCounter = deathCounter;
		}

		UpdateScore(gameData.score);
		deathCounter = gameData.deathCounter;
		death.text = deathCounter.ToString();
	}

	public void SaveData(ref GameData gameData) {
		gameData.score = score;
		gameData.deathCounter = deathCounter;
	}
}
