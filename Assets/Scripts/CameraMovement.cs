using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    public float mouseSensitivityX = 100f;
    public float mouseSensitivityY = 100f;
    public Transform player;

    private float _xRotationMin = -90;
    private float _xRotationMax = 90;
	private PlayerInputs _playerInputs;
	private Vector2 _rotation;

	float xRotation = 0;

	private void OnEnable() {
		_playerInputs.Enable();
	}

	private void OnDisable() {
		_playerInputs.Disable();
	}

	private void Awake() {
		_playerInputs = new PlayerInputs();

		_playerInputs.Player.Rotate.performed += ctx => Dpi2Cm(ctx);
		_playerInputs.Player.Rotate.canceled += _ => _rotation = Vector2.zero;
	}

	void Update() {
		float mouseX = _rotation.x * mouseSensitivityX * Time.deltaTime;
		float mouseY = _rotation.y * mouseSensitivityY * Time.deltaTime;

		player.Rotate(Vector3.up * mouseX);

		xRotation -= mouseY;
		xRotation = Mathf.Clamp(xRotation, _xRotationMin, _xRotationMax);
		transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
	}

	private void Dpi2Cm(InputAction.CallbackContext ctx) {
		_rotation = _playerInputs.Player.Rotate.ReadValue<Vector2>();
		if (ctx.control.device.name == "Mouse") {
			float dpi = Screen.dpi;

			// Set a minimum value for dpi in case they are 0 so you can always still move
			if (dpi <= 0.0f)
				dpi = 50.0f;

			_rotation /= dpi * 0.393701f;
		}
	}
}
