using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Roller : MonoBehaviour, IDataPersistence
{
	public string id;
    public float moveSpeed = 3f;
	public Canvas deathMenu;
	private Rigidbody rb;
	private string currentDirection;
	private float baseMoveSpeed;

	private void Start() {
		rb = GetComponent<Rigidbody>();
		baseMoveSpeed = moveSpeed;
		if (id == "1")
			currentDirection = "Left";
		if (id == "2")
			currentDirection = "Right";
	}

	void Update()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }

	private void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.CompareTag("Player")) {
			Cursor.lockState = CursorLockMode.None;
			deathMenu.gameObject.SetActive(true);
			GameManager.Instance.isGamePaused = true;
			GameManager.Instance.canPause = false;
			GameManager.Instance.isPlayerDead = true;
			rb.velocity = Vector3.zero;
			AudioManager.Instance.PlaySFX("Hit");
			AudioManager.Instance.PlaySFX("Grunt");
			Time.timeScale = 0;
		}
		else {
			moveSpeed *= -1;
			if (currentDirection == "Left")
				currentDirection = "Right";
			else
				currentDirection = "Left";
		}
	}

	public void ChangeState(bool currentState) {
		if (currentState) {
			moveSpeed = 0;
		}
		else {
			if (currentDirection == "Left") {
				if (baseMoveSpeed < 0)
					moveSpeed = baseMoveSpeed * -1;
				else
					moveSpeed = baseMoveSpeed;
			}
			else {
				if (baseMoveSpeed < 0)
					moveSpeed = baseMoveSpeed;
				else
					moveSpeed = baseMoveSpeed * -1;
			}
		}
	}

	public void LoadData(GameData gameData, bool isNewGame) {
		if (isNewGame) {
			if (id == "1") {
				gameData.roller1MoveSpeed = moveSpeed;
				gameData.roller1Position = transform.position;
				gameData.roller1CurrentDirection = currentDirection;
			}
			if (id == "2") {
				gameData.roller2MoveSpeed = moveSpeed;
				gameData.roller2Position = transform.position;
				gameData.roller2CurrentDirection = currentDirection;
			}
		}

		if (id == "1") {
			moveSpeed = gameData.roller1MoveSpeed;
			transform.position = gameData.roller1Position;
			currentDirection = gameData.roller1CurrentDirection;
		}
		if (id == "2") {
			moveSpeed = gameData.roller2MoveSpeed;
			transform.position = gameData.roller2Position;
			currentDirection = gameData.roller2CurrentDirection;
		}
	}

	public void SaveData(ref GameData gameData) {
		if (id == "1") {
			gameData.roller1MoveSpeed = moveSpeed;
			gameData.roller1Position = transform.position;
			gameData.roller1CurrentDirection = currentDirection;
		}
		if (id == "2") {
			gameData.roller2MoveSpeed = moveSpeed;
			gameData.roller2Position = transform.position;
			gameData.roller2CurrentDirection = currentDirection;
		}
	}
}
