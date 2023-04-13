using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public Vector3 playerPosition;
	public Quaternion playerRotation;
	public bool playerIsInVerticalStair;
	public FireState fireState;
	public Vector3 roller1Position;
	public Vector3 roller2Position;
	public string roller1CurrentDirection;
	public string roller2CurrentDirection;
	public float roller1MoveSpeed;
	public float roller2MoveSpeed;
	public bool areRollersActive;
	public SerializableDictionary<string, bool> coinsCollected;
	public float score;
}
