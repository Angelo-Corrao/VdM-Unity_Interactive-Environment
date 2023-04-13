using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class DataPersistenceManager : MonoBehaviour
{
    public static DataPersistenceManager Instance { get; private set; }
	public string fileName = "SaveData.json";
	public GameData gameData;
	public List<IDataPersistence> dataPersistenceObjects;
	private FileDataHandler fileDataHandler;

	private void Awake() {
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	private void Start() {
		dataPersistenceObjects = FindAllDataPersistenceObjects();
		fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
	}

	public void NewGame() {
		gameData = new GameData();
	}

	public void SaveGame() {
		foreach (IDataPersistence dataPersistenceObjcet in dataPersistenceObjects) {
			dataPersistenceObjcet.SaveData(ref gameData);
		}

		fileDataHandler.Save(gameData);
	}

	public void LoadGame() {
		gameData = fileDataHandler.Load();

		if (gameData == null) {
			NewGame();
		}

		foreach (IDataPersistence dataPersistenceObjcet in dataPersistenceObjects) {
			dataPersistenceObjcet.LoadData(gameData);
		}
	}

	private void OnApplicationQuit() {
		SaveGame();
	}

	private List<IDataPersistence> FindAllDataPersistenceObjects() {
		IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
		return dataPersistenceObjects.ToList();
	}
}
