using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

		anyUIActive = true;
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Escape) && !anyUIActive) {
			pauseMenu.gameObject.SetActive(!pauseMenu.gameObject.activeSelf);
			Cursor.lockState = CursorLockMode.None;
			anyUIActive = true;
			Time.timeScale = 0;
		}
	}

	public void NewGame() {
		Cursor.lockState = CursorLockMode.Locked;
		anyUIActive = false;
		DataPersistenceManager.Instance.NewGame();
		DataPersistenceManager.Instance.SaveGame();
		DataPersistenceManager.Instance.LoadGame();
	}

	public void ContinueFromMainMenu() {
		DataPersistenceManager.Instance.LoadGame();
	}

	public void QuitGame() {
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
	}

	public void ContinueFromPause() {
		pauseMenu.gameObject.SetActive(!pauseMenu.gameObject.activeSelf);
		Cursor.lockState = CursorLockMode.Locked;
		anyUIActive = false;
		Time.timeScale = 1;
	}

	public void UpdateScore(float value) {
		score += value;
		coins.text = score.ToString();
	}
}
