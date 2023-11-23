using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    // Player variables
    [HideInInspector]
    public float health;
    [HideInInspector]
    public float speed;

	[SerializeField]
	private float jumpForce;

	private int ammoAmount;

    private bool grounded;
    private float groundDistance = 0.2f;    // The radius of the sphere used to check for ground
	
    // Rigidbody
	private Rigidbody playerRigidbody;

	// Camera / Cinemachine variables
	private GameObject cameraTarget;

	[SerializeField]
    private CinemachineVirtualCamera aimVirtualCamera;

	private float cameraSensitivity;
	private float cameraFollowSensitivity;  // Camera sensitivity when NOT aiming
	private float cameraAimSensitivity;   // Camera sensitivity when aiming

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
        // Set player variables
        health = 100f;
        speed = 2f;

        jumpForce = 5f;
        ammoAmount = 0;

        // Set rigidbody
        playerRigidbody = GetComponent<Rigidbody>();

		// Set camera / cinemachine variables
		cameraTarget = transform.Find("CameraTarget").gameObject;
		
        aimVirtualCamera = GameObject.Find("PlayerAimCamera").GetComponent<CinemachineVirtualCamera>();

		cameraFollowSensitivity = 0.5f;
		cameraAimSensitivity = 0.25f;

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
        // MOVEMENT //

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
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
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
