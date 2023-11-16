using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

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
	private LayerMask defaultLayer;
	private LayerMask groundLayer;
	private LayerMask aimColliderLayers;

	// Input actions
	private InputActions inputActions;
	[HideInInspector] public Vector2 movementInput = new Vector2();
	private Vector2 mouseInput = new Vector2();

	// Cinemachine variables
	[SerializeField] private CinemachineVirtualCamera aimVirtualCamera;

	// Other variables
	private Rigidbody playerRigidbody;

	private bool grounded;
	private float groundDistance = 0.2f;    // The radius of the sphere used to check for ground

	private GameObject cameraTarget;

	Vector3 mouseWorldPosition = new Vector3();
	[SerializeField] private Transform debugTransform;

	[SerializeField] private float gunShootDistance = 20f;

	[SerializeField] private Transform vfxHitGreen;
	[SerializeField] private Transform vfxHitRed;

	Transform hitTransform = null;

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
		defaultLayer = 1 << LayerMask.NameToLayer("Default");
		groundLayer = 1 << LayerMask.NameToLayer("Ground");
		aimColliderLayers = defaultLayer | groundLayer;

		// Input actions
		inputActions = new InputActions();

		// Other variables
		playerRigidbody = GetComponent<Rigidbody>();
		cameraTarget = transform.Find("CameraTarget").gameObject;
		aimVirtualCamera = GameObject.Find("PlayerAimCamera").GetComponent<CinemachineVirtualCamera>();

		mouseWorldPosition = Vector3.zero;
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
		Debug.Log($"movementInput: {movementInput}");

		// Check if there is ground directly below the player
		grounded = Physics.CheckSphere(transform.position, groundDistance, groundLayer | defaultLayer);

		// Move player using velocity
		Vector3 movementDirection = new Vector3(movementInput.x, 0, movementInput.y).normalized;
		Vector3 rotatedDirection = cameraTarget.transform.rotation * movementDirection;
		Vector3 movement = new Vector3(rotatedDirection.x * speed, playerRigidbody.velocity.y, rotatedDirection.z * speed);

		playerRigidbody.velocity = movement;

		// Shoot
		Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);

		Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

		if (Physics.Raycast(ray, out RaycastHit raycastHit, gunShootDistance, aimColliderLayers))
		{
			debugTransform.position = raycastHit.point;
			mouseWorldPosition = raycastHit.point;
			hitTransform = raycastHit.transform;
		}
		else	// Manually set distance of raycast
		{
			debugTransform.position = Camera.main.transform.position + Camera.main.transform.forward * gunShootDistance;
			mouseWorldPosition = Camera.main.transform.position + Camera.main.transform.forward * gunShootDistance;
			hitTransform = raycastHit.transform;
		}

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
		newVerticalRotation = Mathf.Clamp(newVerticalRotation, -65f, 65f);

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
			playerRigidbody.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
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
		if (hitTransform != null)
		{	// Hit something
			if (hitTransform.GetComponent<BulletTarget>() != null)
			{
				// Hit target
				Instantiate(vfxHitGreen, debugTransform.position, Quaternion.identity);
			}
			else
			{
				//Hit something else
				Instantiate(vfxHitRed, debugTransform.position, Quaternion.identity);
			}
		}
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
