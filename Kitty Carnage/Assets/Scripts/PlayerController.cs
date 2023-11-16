using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PlayerController : MonoBehaviour
{
	public PlayableCharacter playerData;

	// Character.cs variables
	private string id;
	[HideInInspector] public float health;
	[HideInInspector] public float speed;

	// PlayableCharacter.cs varaibles
	private float cameraSensitivity;
	private float followSensitivity;  // Camera sensitivity when NOT aiming
	private float aimSensitivity;   // Camera sensitivity when aiming
	private float jumpForce;
	private int ammoAmount;

	// Layer masks
	private LayerMask groundLayer;

	// Other variables
	private Rigidbody rb;

	private InputActions inputActions;
	[HideInInspector] public Vector2 movementInput = new Vector2();

	private bool grounded;
	private float groundDistance = 0.2f;    // The radius of the sphere used to check for ground

	private Vector2 mouseInput = new Vector2();
	private GameObject cameraTarget;

	// Cinemachine variables
	[SerializeField] private CinemachineVirtualCamera aimVirtualCamera;

	// Awake is called before Start
	void Awake()
	{
		// Access character data - Character.cs
		id = playerData.id;
		health = playerData.health;
		speed = playerData.speed;

		// Access character data - PlayableCharacter.cs
		followSensitivity = playerData.followSensitivity;	// Camera sensitivity when NOT aiming
		aimSensitivity = playerData.aimSensitivity;	// Camera sensitivity when aiming
		jumpForce = playerData.jumpForce;

		// Set layer masks
		groundLayer = 1 << LayerMask.NameToLayer("Ground");

		// Input actions
		inputActions = new InputActions();

		// Other variables
		rb = GetComponent<Rigidbody>();
		cameraTarget = transform.Find("CameraTarget").gameObject;
		aimVirtualCamera = GameObject.Find("PlayerAimCamera").GetComponent<CinemachineVirtualCamera>();
	}

	// Start is called before the first frame update
	void Start()
	{
		GameManager.instance.HideCursor();

		// Set aimVirtualCamera to false when loaded
		aimVirtualCamera.gameObject.SetActive(false);
		cameraSensitivity = followSensitivity;
	}

	// Update is called once per frame
	void Update()
	{
		Debug.Log($"grounded value: {grounded}");
		Debug.Log($"movementInput: { movementInput}");

		// Check if there is ground directly below the player
		grounded = Physics.CheckSphere(transform.position, groundDistance, groundLayer);

		// Move player using velocity
		Vector3 movementDirection = new Vector3(movementInput.x, 0, movementInput.y).normalized;
		Vector3 rotatedDirection = cameraTarget.transform.rotation * movementDirection;
		Vector3 movement = new Vector3(rotatedDirection.x * speed, rb.velocity.y, rotatedDirection.z * speed);

		rb.velocity = movement;
	}

	void LateUpdate()
	{
		// Rotate player horizontally to match mouseInput
		transform.Rotate(0, mouseInput.x * cameraSensitivity, 0);

		// Get current camera rotation
		Vector3 currentCameraRotation = cameraTarget.transform.localRotation.eulerAngles;

		// Calculate new vertical rotation
		float newVerticalRotation = currentCameraRotation.x - mouseInput.y * cameraSensitivity;

		// Adjust the rotation value to prevent snapping
		if (newVerticalRotation > 180)
		{
			newVerticalRotation -= 360;
		}

		// Clamp the vertical rotation to prevent flipping
		newVerticalRotation = Mathf.Clamp(newVerticalRotation, -90f, 90f);

		// Set camera rotation to match mouseInput
		cameraTarget.transform.localRotation = Quaternion.Euler(newVerticalRotation, currentCameraRotation.y, currentCameraRotation.z);
	}

	void OnMovementPerformed(InputAction.CallbackContext context)
	{
		movementInput = context.ReadValue<Vector2>();
	}

	void OnMovementCanceled()
	{
		movementInput = Vector2.zero;
	}

	void OnLookPerformed(InputAction.CallbackContext context)
	{
		mouseInput = context.ReadValue<Vector2>();
	}

	void OnLookCanceled()
	{
		mouseInput = Vector2.zero;
	}

	void OnJumpPerformed()
	{
		if (grounded)
		{
			// Add upward force to rigidboday
			rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
		}
	}

	void OnAimPerformed()
	{
		aimVirtualCamera.gameObject.SetActive(true);
		cameraSensitivity = aimSensitivity;

	}

	void OnAimCanceled()
	{
		aimVirtualCamera.gameObject.SetActive(false);
		cameraSensitivity = followSensitivity;
	}

	void OnShootPerformed()
	{
		// Spawn ammo
		// Ammo moves in direction of mouse direction
	}

	void OnEnable()
	{
		inputActions.Enable();

		// Add listeners for the 'performed' and 'canceled' events //

		// Movement action
		inputActions.Player.Movement.performed += context => OnMovementPerformed(context);
		inputActions.Player.Movement.canceled += context => OnMovementCanceled();

		// Look action
		inputActions.Player.Look.performed += context => OnLookPerformed(context);
		inputActions.Player.Look.canceled += context => OnLookCanceled();

		// Jump action
		inputActions.Player.Jump.performed += context => OnJumpPerformed();
		//inputActions.Player.Jump.canceled += context => OnJumpCanceled();

		// Aim action
		inputActions.Player.Aim.performed += context => OnAimPerformed();
		inputActions.Player.Aim.canceled += context => OnAimCanceled();

		// Shoot action
		inputActions.Player.Shoot.performed += context => OnShootPerformed();
		//inputActions.Player.Shoot.canceled += context => OnShootCanceled();
	}

	void OnDisable()
	{
		inputActions.Disable();

		// Remove listeners for the 'performed' and 'canceled' events //

		// Movement action
		inputActions.Player.Movement.performed += context => OnMovementPerformed(context);
		inputActions.Player.Movement.canceled += context => OnMovementCanceled();

		// Look action
		inputActions.Player.Look.performed += context => OnLookPerformed(context);
		inputActions.Player.Look.canceled += context => OnLookCanceled();

		// Jump action
		inputActions.Player.Jump.performed += context => OnJumpPerformed();
		//inputActions.Player.Jump.canceled += context => OnJumpCanceled();

		// Aim action
		inputActions.Player.Aim.performed += context => OnAimPerformed();
		inputActions.Player.Aim.canceled += context => OnAimCanceled();

		// Shoot action
		inputActions.Player.Shoot.performed += context => OnShootPerformed();
		//inputActions.Player.Shoot.canceled += context => OnShootCanceled();
	}

	void OnDrawGizmos()
	{
		// Draw a red sphere at the player's position with a radius of groundDistance
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, groundDistance);
	}
}
