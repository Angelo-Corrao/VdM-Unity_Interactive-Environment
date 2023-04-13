using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {
	public static MainMenuManager Instance;

	private void Awake() {
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	public void NewGame() {
		Cursor.lockState = CursorLockMode.Locked;
		DataPersistenceManager.Instance.NewGame();
		DataPersistenceManager.Instance.SaveGame();
		SceneManager.LoadScene("VdM_Unity");
	}

	public void Continue() {
		Cursor.lockState = CursorLockMode.Locked;
		SceneManager.LoadScene("VdM_Unity");
	}

	public void QuitGame() {
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
	}
}
