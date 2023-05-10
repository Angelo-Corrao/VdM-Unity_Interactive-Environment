using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {
	public static MainMenuManager Instance;
	private bool saveAlreadyExists = false;

	private void Awake() {
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	private void Start() {
		AudioManager.Instance.PlayMusic("Main Menu");
		if (File.Exists(Path.Combine(Application.persistentDataPath, DataPersistenceManager.Instance.fileName))) {
			saveAlreadyExists = true;
		}
	}

	public void NewGame() {
		DataPersistenceManager.Instance.NewGame();
		DataPersistenceManager.Instance.SaveGame();
		DataPersistenceManager.Instance.isNewGame = true;
		AudioManager.Instance.PlaySFX("Menu Click");
		SceneManager.LoadScene("VdM_Unity");
	}

	public void Continue() {
		if (saveAlreadyExists) {
			DataPersistenceManager.Instance.isNewGame = false;
			AudioManager.Instance.PlaySFX("Menu Click");
			SceneManager.LoadScene("VdM_Unity");
		}
	}

	public void QuitGame() {
		AudioManager.Instance.PlaySFX("Menu Click");
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
	}
}
