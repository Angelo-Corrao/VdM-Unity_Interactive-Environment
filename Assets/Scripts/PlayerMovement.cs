using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

public class PlayerMovement : MonoBehaviour, IDataPersistence
{
    public float speed = 10f;
	public float sprintSpeedMultiplier = 0.5f;
    public float gravityScale = 1f;
	public float maxJumpHeight = 2f;
	public float jumpBufferTimer = 0.15f;
	public float jumpHangTimer = 0.1f;
	public LayerMask _groundMask;
	public Transform verticalStair;
	public UnityEvent interacted;
	[HideInInspector]
	public bool isInVerticalStair = false;
	[HideInInspector]
	public string currentDevice = "";

	private CharacterController _controller;
	private float gravity;
	private float _maxJumpTime;
	private bool _isMaxJumpHeightReached = false;
	private bool _isDoubleJumpPressed = false;
	private float _jumpBufferCounter = 0f;
	private float _jumpHangCounter = 0f;
	private float _jumpTimeCounter = 0f;
	private float _isGroundedCheckTimer = 0.1f;
	private float _isGroundedCheckCounter = 0f;
	private float baseSpeed;
	private Vector3 respawnPoint;
	private float y = 0f;
	private PlayerInputs _playerInputs;
	private Vector2 _movement;
	private Vector3 _direction;

	private void OnEnable() {
		_playerInputs.Enable();
	}

	private void OnDisable() {
		_playerInputs.Disable();
	}

	private void Awake() {
		_playerInputs = new PlayerInputs();
		
		_playerInputs.Character.Movement.performed += _ => _movement = _playerInputs.Character.Movement.ReadValue<Vector2>();
		_playerInputs.Character.Movement.canceled += _ => _movement = Vector2.zero;

		_playerInputs.Character.Jump.started += _ => JumpPressed();
		_playerInputs.Character.Jump.canceled += _ => JumpReleased();

		_playerInputs.Character.Sprint.started += _ => speed = baseSpeed * sprintSpeedMultiplier;
		_playerInputs.Character.Sprint.canceled += _ => speed = baseSpeed;

		_playerInputs.Character.Interact.started += _ => interacted?.Invoke();
	}

	private void Start() {
		_controller = GetComponent<CharacterController>();
		baseSpeed = speed;
		gravity = -9.81f * gravityScale;
		respawnPoint = _controller.transform.position;
		InputSystem.onEvent.Call(HandleControl);
	}

	private void Update()
    {
		if (GameManager.Instance.isGamePaused) {
			_movement.x = 0;
			_movement.y = 0;
		}

		if (_isGroundedCheckCounter > 0) {
			_isGroundedCheckCounter -= Time.deltaTime;
		}

		if (IsGrounded()) {
			if (_isGroundedCheckCounter <= 0) {
				y = gravity;
				_isDoubleJumpPressed = false;
				_isMaxJumpHeightReached = false;

				if (_jumpBufferCounter > 0) {
					Jump();
					_jumpBufferCounter = 0f;
				}

				_jumpHangCounter = jumpHangTimer;
			}
		}
		else {
			y += gravity * Time.deltaTime;

			if (_jumpBufferCounter > 0) {
				_jumpBufferCounter -= Time.deltaTime;
			}

			if (_jumpHangCounter > 0) {
				_jumpHangCounter -= Time.deltaTime;
			}

			if (_jumpTimeCounter >= _maxJumpTime) {
				_isMaxJumpHeightReached = true;
			}
			else {
				_jumpTimeCounter += Time.deltaTime;
			}
		}

		_direction = transform.right * _movement.x + transform.forward * _movement.y;
		if (_direction.magnitude > 1)
			_direction = _direction.normalized * speed;
		else
			_direction *= speed;

		if (isInVerticalStair) {
			VerticalStairMovement();
		}

		_direction.y = y;
		_controller.Move(_direction * Time.deltaTime);
    }

	private void JumpPressed() {
		if (!GameManager.Instance.isGamePaused) {
			if (IsGrounded()) {
				_isGroundedCheckCounter = _isGroundedCheckTimer;
				Jump();
			}
			else {
				if (_jumpHangCounter <= 0) {
					if (!_isDoubleJumpPressed) {
						Jump();
						_isDoubleJumpPressed = true;
					}
					else {
						_jumpBufferCounter = jumpBufferTimer;
					}
				}
				else {
					Jump();
					_jumpHangCounter = 0f;
				}
			}
		}
	}

	private void JumpReleased() {
		if (!_isMaxJumpHeightReached && !GameManager.Instance.isGamePaused) {
			y = 0f;
			_isMaxJumpHeightReached = true;
		}
	}

	private bool IsGrounded() {
		float spherePositionY = transform.position.y - (_controller.height / 2) + _controller.radius - 0.002f;
		Vector3 spherePosition = new Vector3(transform.position.x, spherePositionY, transform.position.z);
		return Physics.CheckSphere(spherePosition, _controller.radius - 0.001f, _groundMask, QueryTriggerInteraction.Ignore);
	}

	private void Jump() {
		y = Mathf.Sqrt(-2.0f * gravity * maxJumpHeight);
		_maxJumpTime = -(y / gravity);
		_jumpTimeCounter = 0;
		_isMaxJumpHeightReached = false;
	}

	public void SetInVerticalStair() {
		if (!isInVerticalStair) {
			transform.SetParent(verticalStair);
			_controller.enabled = false;
			if (_controller.transform.localPosition.y > 6.84f + 1f)
				_controller.transform.localPosition = new Vector3(0f, 6.84f + 1f, -1f);
			else
				_controller.transform.localPosition = new Vector3(0f, _controller.transform.localPosition.y, -1f);
			_controller.enabled = true;
			isInVerticalStair = true;
		}
		else {
			transform.SetParent(null);
			isInVerticalStair = false;
		}
	}

	private void VerticalStairMovement() {
		if (_controller.transform.localPosition.y <= 6.84f + 1f) { // 6.84 = half high; 1 = offset
			y = _direction.z;
			_direction.x = 0;
			_direction.z = 0;
		}
		else {
			y = 0;
			_direction.x = 0;
			_direction.z = 0;
			_controller.enabled = false;
			_controller.transform.localPosition = new Vector3(0f, 6.84f + 1f, -1f);
			_controller.enabled = true;
		}
	}

	public void Respawn() {
		_controller.enabled = false;
		_controller.transform.position = respawnPoint;
		_controller.enabled = true;
	}

	private void HandleControl(InputEventPtr obj) {
		if (obj.deviceId == 1 || obj.deviceId == 2) {
			currentDevice = "KM";
		}
		else {
			currentDevice = "Gamepad";
		}
	}

	public void LoadData(GameData gameData, bool isNewGame) {
		if (isNewGame) {
			gameData.playerPosition = _controller.transform.position;
			gameData.playerRotation = _controller.transform.rotation;
			gameData.playerIsInVerticalStair = isInVerticalStair;
		}
		_controller.enabled = false;
		_controller.transform.position = gameData.playerPosition;
		_controller.transform.rotation = gameData.playerRotation;
		_controller.enabled = true;
		isInVerticalStair = gameData.playerIsInVerticalStair;
		if (isInVerticalStair) {
			isInVerticalStair = false;
			SetInVerticalStair();
		}
	}

	public void SaveData(ref GameData gameData) {
		if (GameManager.Instance.isPlayerDead)
			gameData.playerPosition = respawnPoint;
		else
			gameData.playerPosition = _controller.transform.position;
		gameData.playerRotation = _controller.transform.rotation;
		gameData.playerIsInVerticalStair = isInVerticalStair;
	}
}
