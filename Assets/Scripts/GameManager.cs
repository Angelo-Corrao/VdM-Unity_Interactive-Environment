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
		AudioManager.Instance.PlayMusic("In Game", 0.8f);
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Escape) && !anyUIActive) {
			pauseMenu.gameObject.SetActive(true);
			anyUIActive = true;
			AudioManager.Instance.musicSource.volume = 0.2f;
			AudioManager.Instance.sfxSource.volume = 0.2f;
			Time.timeScale = 0;
		}
	}

	public void InizializeNewGame() {
		DataPersistenceManager.Instance.NewGame();
	}

	public void Continue() {
		pauseMenu.gameObject.SetActive(!pauseMenu.gameObject.activeSelf);
		anyUIActive = false;
		AudioManager.Instance.musicSource.volume = 1;
		AudioManager.Instance.sfxSource.volume = 1;
		AudioManager.Instance.PlaySFX("Button Click");
		Time.timeScale = 1;
	}

	public void MainMenu() {
		Time.timeScale = 1;
		DataPersistenceManager.Instance.SaveGame();
		AudioManager.Instance.PlaySFX("Button Click");
		AudioManager.Instance.musicSource.volume = 1;
		AudioManager.Instance.sfxSource.volume = 1;
		AudioManager.Instance.musicSource.Stop();
		AudioManager.Instance.sfxSource.Stop();
		SceneManager.LoadScene("Main Menu");
	}

	public void Respawn() {
		Time.timeScale = 1;
		anyUIActive = false;
		Death();
		AudioManager.Instance.musicSource.volume = 1;
		AudioManager.Instance.sfxSource.volume = 1;
		AudioManager.Instance.PlaySFX("Button Click");
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
