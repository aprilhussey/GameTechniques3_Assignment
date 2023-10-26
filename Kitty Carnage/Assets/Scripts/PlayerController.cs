using System.Collections;
using System.Collections.Generic;
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
	private float lookSpeed;
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

	// Awake is called before Start
	void Awake()
	{
		// Access character data - Character.cs
		id = playerData.id;
		health = playerData.health;
		speed = playerData.speed;

		// Access character data - PlayableCharacter.cs
		lookSpeed = playerData.lookSpeed;
		jumpForce = playerData.jumpForce;
		ammoAmount = playerData.ammoAmount;

		// Other variables
		rb = GetComponent<Rigidbody>();

		// Input actions
		inputActions = new InputActions();

		// Subscribe to Movement action
		inputActions.Player.Movement.performed += context => movementInput = context.ReadValue<Vector2>();
		inputActions.Player.Movement.canceled += context => movementInput = Vector2.zero;

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

		// Move player using velocity
		rb.velocity = new Vector3(movementInput.x * speed, rb.velocity.y, movementInput.y * speed);
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
