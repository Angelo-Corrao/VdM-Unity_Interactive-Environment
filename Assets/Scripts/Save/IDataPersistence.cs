using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistence
{
    public void LoadData(GameData gameData, bool isNewGame);
	public void SaveData(ref GameData gameData);
}
