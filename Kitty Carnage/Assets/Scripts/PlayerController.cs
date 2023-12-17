using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    // Player variables
    //[HideInInspector]
    public float health = 100f;
    //[HideInInspector]
    public float speed = 2f;

	[SerializeField]
	private float jumpForce = 5f;

	private int ammoAmount = 0;

    private bool grounded;
    private float groundDistance = 0.2f;    // The radius of the sphere used to check for ground
	
    // Rigidbody
	private Rigidbody playerRigidbody;

	// Camera / Cinemachine variables
	private GameObject cameraTarget;

	[SerializeField]
    private CinemachineVirtualCamera aimVirtualCamera;

	private float cameraSensitivity;
	[SerializeField]
	[Tooltip("Camera sensitivity when NOT aiming")]
	private float cameraFollowSensitivity = 5f;  // Camera sensitivity when NOT aiming
	[SerializeField]
	[Tooltip("Camera sensitivity when aiming")]
	private float cameraAimSensitivity = 2.5f;   // Camera sensitivity when aiming

	[SerializeField]
	[Tooltip("Minimum vertical rotation of the CameraTarget gameobject")]
	float minVerticalRotation = -80f;	// Define min rotation
	[SerializeField]
	[Tooltip("Maximum vertical rotation of the CameraTarget gameobject")]
	float maxVerticalRotation = 80f;   // Define max rotation

	// Layer masks
	private LayerMask defaultLayer;
	private LayerMask groundLayer;
	private LayerMask aimColliderLayers;

    // Input actions variables
    [HideInInspector]
    public Vector2 movementInput;

    private Vector2 lookInput;
	private bool jumpInput;
	private bool crouchInput;
	private bool shootInput;
    private bool aimInput;

	// Awake is called before Start
	void Awake()
    {
        ammoAmount = 0;

        // Set rigidbody
        playerRigidbody = GetComponent<Rigidbody>();

		// Set camera / cinemachine variables
		cameraTarget = transform.Find("CameraTarget").gameObject;
		
        aimVirtualCamera = GameObject.Find("PlayerAimCamera").GetComponent<CinemachineVirtualCamera>();

        // Set layer mask variables
        defaultLayer = 1 << LayerMask.NameToLayer("Default");
        groundLayer = 1 << LayerMask.NameToLayer("Ground");
        aimColliderLayers = defaultLayer | groundLayer;

		// Set input actions variables
		movementInput = Vector2.zero;
	    lookInput = Vector2.zero;
        jumpInput = false;
        crouchInput = false;
	    shootInput = false;
        aimInput = false;
    }

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		// LOOK //
		// Keep track of current rotation
		float verticalRotation = cameraTarget.transform.localEulerAngles.x;

		// Rotate the player and camera based on lookInput
		if (lookInput != Vector2.zero)
        {
			// Calculate new rotation
			float newVerticalRotation = verticalRotation - lookInput.y * cameraSensitivity;

			// Adjust for 360 degree system
			if (newVerticalRotation > 180) newVerticalRotation -= 360;

			// Clamp rotation to min and max angles
			verticalRotation = Mathf.Clamp(newVerticalRotation, minVerticalRotation, maxVerticalRotation);

			// Apply rotation
			cameraTarget.transform.localEulerAngles = new Vector3(verticalRotation, 0, 0);

			this.transform.Rotate(Vector3.up, lookInput.x * cameraSensitivity);
            //cameraTarget.transform.Rotate(Vector3.right, -lookInput.y * cameraSensitivity);
        }
		// LOOK //

		// MOVEMENT //
		// Move the player in the direction the camera is facing
		Vector3 movementDirection = (this.transform.forward * movementInput.y + this.transform.right * movementInput.x).normalized;
       
        // Apply movementDirection to playerRigidbody
        playerRigidbody.velocity = new Vector3(movementDirection.x * speed, playerRigidbody.velocity.y, movementDirection.z * speed);
		playerRigidbody.angularVelocity = Vector3.zero;
		// MOVEMENT //

		// JUMP //
		// Check if there is ground directly below the player
		grounded = Physics.CheckSphere(this.transform.position, groundDistance, groundLayer);

        if (jumpInput && grounded)  // If the jump action has been triggered and the player is grounded
        {
			// Add upward force to rigidboday
			//playerRigidbody.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            
            // Set the vertical velocity directly for a consistent jump height
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, jumpForce, playerRigidbody.velocity.z);
        }
        // JUMP //

        if (aimInput)
        {
            cameraSensitivity = cameraAimSensitivity / 10;  // Divided by 10 to get the correct value
		}
        else
        {
            cameraSensitivity = cameraFollowSensitivity / 10;	// Divided by 10 to get the correct value
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started || context.phase == InputActionPhase.Performed)
        {
            movementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            movementInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
		if (context.phase == InputActionPhase.Started || context.phase == InputActionPhase.Performed)
		{
			lookInput = context.ReadValue<Vector2>();
		}
		else if (context.phase == InputActionPhase.Canceled)
		{
			lookInput = Vector2.zero;
		}
	}

    public void OnJump(InputAction.CallbackContext context)
    {
        jumpInput = context.action.triggered;
	}

	public void OnCrouch(InputAction.CallbackContext context)
	{
		crouchInput = context.action.triggered;
	}

	public void OnShoot(InputAction.CallbackContext context)
    {
        shootInput = context.action.triggered;
    }

	public void OnAim(InputAction.CallbackContext context)
	{
		aimInput = context.action.triggered;
	}
}
