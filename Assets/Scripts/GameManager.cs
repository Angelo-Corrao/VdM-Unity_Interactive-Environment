using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public static GameManager Instance;
	public Canvas pauseMenu;
	public Text coins;
	public Text death;
	public UnityEvent respawn;
	public bool isGamePaused = false;
	public bool canPause = true;
	public bool isPlayerDead = false;
	public float score = 0;
	public int deathCounter = 0;
	public AudioMixerSnapshot pausedSnapshot;
	public AudioMixerSnapshot unpausedSnapshot;

	private void Awake() {
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	private void Start() {
		DataPersistenceManager.Instance.LoadGame();
		AudioManager.Instance.PlayMusic("In Game");
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Escape) && canPause) {
			if (!isGamePaused)
				Pause();
			else
				Unpause();
		}
	}

	public void Continue() {
		Unpause();
	}

	public void MainMenu() {
		Unpause();
		Cursor.lockState = CursorLockMode.None;
		DataPersistenceManager.Instance.SaveGame();
		AudioManager.Instance.musicSource.Stop();
		AudioManager.Instance.sfxSource.Stop();
		SceneManager.LoadScene("Main Menu");
	}

	public void Respawn() {
		Unpause();
		isPlayerDead = false;
		canPause = true;
		Death();
		respawn?.Invoke();
	}

	public void Pause() {
		pauseMenu.gameObject.SetActive(true);
		Cursor.lockState = CursorLockMode.None;
		isGamePaused = true;
		pausedSnapshot.TransitionTo(0);
		Time.timeScale = 0;
	}

	public void Unpause() {
		pauseMenu.gameObject.SetActive(false);
		Cursor.lockState = CursorLockMode.Locked;
		isGamePaused = false;
		AudioManager.Instance.PlaySFX("Button Click");
		unpausedSnapshot.TransitionTo(0f);
		Time.timeScale = 1;
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
		if (isPlayerDead)
			gameData.deathCounter = deathCounter + 1;
		else
			gameData.deathCounter = deathCounter;
	}
}
