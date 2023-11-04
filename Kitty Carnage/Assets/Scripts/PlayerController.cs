using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	public PlayableCharacter playerData;

	// Character.cs variables
	private string id;
	private float health;
	private float speed;

	// PlayableCharacter.cs varaibles
	private float lookSensitivity;
	private float jumpForce;
	private int ammoAmount;

	// Other variables
	private Rigidbody rb;

	private InputActions inputActions;
	private Vector2 movementInput = new Vector2();
	private bool jumpInput;

	private bool grounded;
	private LayerMask groundLayer;
	private float groundDistance = 0.2f;    // The radius of the sphere used to check for ground

	private Vector2 mouseInput;
	public Transform cameraTransform;

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

	/*// Start is called before the first frame update
	void Start()
	{

	}*/

	// Update is called once per frame
	void Update()
	{
		Debug.Log("grounded value: " + grounded);

		// Check if there is ground directly below the player
		grounded = Physics.CheckSphere(transform.position, groundDistance, groundLayer);

		if (grounded && inputActions.Player.Jump.triggered)
		{
			Jump();
		}

		// Move camera based on mouse input
		cameraTransform.Rotate(-mouseInput.y * lookSensitivity, mouseInput.x * lookSensitivity, 0);
		cameraTransform.localEulerAngles = new Vector3(cameraTransform.localEulerAngles.x, cameraTransform.localEulerAngles.y, 0);

		// Move player using velocity
		//rb.velocity = new Vector3(movementInput.x * speed, rb.velocity.y, movementInput.y * speed);
		Vector3 movementDirection = new Vector3(movementInput.x, 0, movementInput.y).normalized;
		Vector3 rotatedDirection = cameraTransform.rotation * movementDirection;
		Vector3 movement = new Vector3(rotatedDirection.x * speed, rb.velocity.y, rotatedDirection.z * speed);

		rb.velocity = movement;
	}

	void Jump()
	{
		// Add upward force to rigidboday
		rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
	}

	void OnDrawGizmos()
	{
		// Draw a red sphere at the player's position with a radius of groundDistance
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, groundDistance);
	}
}
