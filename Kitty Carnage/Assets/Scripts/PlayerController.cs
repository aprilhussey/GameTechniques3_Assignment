using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	public PlayableCharacter playerData;

	// Character.cs variables
	private string id;
	[HideInInspector] public float health;
	[HideInInspector] public float speed;

	// PlayableCharacter.cs varaibles
	private float lookSensitivity;
	private float jumpForce;
	private int ammoAmount;

	// Other variables
	private Rigidbody rb;

	private InputActions inputActions;
	[HideInInspector] public Vector2 movementInput = new Vector2();
	private bool jumpInput;

	private bool grounded;
	private LayerMask groundLayer;
	private float groundDistance = 0.2f;    // The radius of the sphere used to check for ground

	private Vector2 mouseInput = new Vector2();
	private GameObject cameraTarget;

	// Awake is called before Start
	void Awake()
	{
		// Access character data - Character.cs
		id = playerData.id;
		health = playerData.health;
		speed = playerData.speed;

		// Access character data - PlayableCharacter.cs
		lookSensitivity = playerData.lookSensitivity;
		jumpForce = playerData.jumpForce;
		ammoAmount = playerData.ammoAmount;

		// Other variables
		rb = GetComponent<Rigidbody>();
		cameraTarget = transform.Find("Camera Target").gameObject;

		// Input actions
		inputActions = new InputActions();

		// Subscribe to Movement action
		inputActions.Player.Movement.performed += context => movementInput = context.ReadValue<Vector2>();
		inputActions.Player.Movement.canceled += context => movementInput = Vector2.zero;

		// Subscribe to Look action
		inputActions.Player.Look.performed += context => mouseInput = context.ReadValue<Vector2>();
		inputActions.Player.Look.canceled += context => mouseInput = Vector2.zero;

		// Set groundLayer to the Ground layer mask
		groundLayer = 1 << LayerMask.NameToLayer("Ground");
	}

	void OnEnable()
	{
		inputActions.Enable();
	}

	void OnDisable()
	{
		inputActions.Disable();
	}

	// Start is called before the first frame update
	void Start()
	{
		GameManager.instance.HideCursor();
	}

	// Update is called once per frame
	void Update()
	{
		Debug.Log("grounded value: " + grounded);
		Debug.Log("movementInput: " + movementInput);

		// Check if there is ground directly below the player
		grounded = Physics.CheckSphere(transform.position, groundDistance, groundLayer);

		if (grounded && inputActions.Player.Jump.triggered)
		{
			Jump();
		}

		// Move player using velocity
		Vector3 movementDirection = new Vector3(movementInput.x, 0, movementInput.y).normalized;
		Vector3 rotatedDirection = cameraTarget.transform.rotation * movementDirection;
		Vector3 movement = new Vector3(rotatedDirection.x * speed, rb.velocity.y, rotatedDirection.z * speed);

		rb.velocity = movement;

		if (inputActions.Player.Shoot.triggered)
		{
			Shoot();
		}
	}

	void LateUpdate()
	{
		// Rotate player horizontally to match mouseInput
		transform.Rotate(0, mouseInput.x * lookSensitivity, 0);

		// Get current camera rotation
		Vector3 currentCameraRotation = cameraTarget.transform.localRotation.eulerAngles;

		// Calculate new vertical rotation
		float newVerticalRotation = currentCameraRotation.x - mouseInput.y * lookSensitivity;

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

	void Jump()
	{
		// Add upward force to rigidboday
		rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
	}

	void Shoot()
	{
		// Spawn ammo
		// Ammo moves in direction of mouse direction
	}

	void OnDrawGizmos()
	{
		// Draw a red sphere at the player's position with a radius of groundDistance
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, groundDistance);
	}
}
