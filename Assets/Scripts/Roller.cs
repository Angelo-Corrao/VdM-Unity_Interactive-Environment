using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roller : MonoBehaviour
{
    public float moveSpeed = 3f;
	private float baseMoveSpeed;

	private void Start() {
		baseMoveSpeed = moveSpeed;
	}

	void Update()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }

	private void OnCollisionEnter(Collision collision) {
		moveSpeed *= -1;
	}

	public void ChangeState(bool currentState) {
		if (currentState) {
			moveSpeed = 0;
		}
		else
			moveSpeed = baseMoveSpeed;
	}
}
