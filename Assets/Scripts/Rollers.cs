using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rollers : MonoBehaviour
{
    public float moveSpeed = 3f;

	void Update()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }

	private void OnCollisionEnter(Collision collision) {
		moveSpeed *= -1;
	}
}
