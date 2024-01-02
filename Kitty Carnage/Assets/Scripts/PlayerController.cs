using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using TMPro;

public class PlayerController : MonoBehaviour, IDamageable
{
    // Player variables
    public float maxHealth = 100f;
	private float currentHealth;

	private HealthBar healthBar;

	public float speed = 2f;

	[SerializeField]
	private float jumpForce = 5f;

    private bool grounded;
    private float groundDistance = 0.2f;    // The radius of the sphere used to check for ground

	[SerializeField]
	private float interactableDistance = 2f;

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

	// Other variables
	Vector3 mouseWorldPosition = new Vector3();
	Vector2 screenCenterPoint = new Vector2();
	[SerializeField]
	private Transform debugTransform;

	private GameObject equippedItemL;
	private GameObject equippedItemR;

	private Weapon weapon;
	private float weaponRange;

	[SerializeField]
	private Transform vfxHitGreen;
	[SerializeField]
	private Transform vfxHitRed;

	//private Transform hitTransform = null;

	// Variables used by Weapon class
	[HideInInspector]
	public Vector3 aimDirection;
	[HideInInspector]
	public Transform spawnProjectilePosition;

	// Ammo visuals
	[SerializeField]
	private TextMeshProUGUI tmpLoadedAmmo;
	[SerializeField]
	private TextMeshProUGUI tmpSpareAmmo;

	// Awake is called before Start
	void Awake()
    {
		// Set health
		currentHealth = maxHealth;
		healthBar = this.GetComponentInChildren<HealthBar>();

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

		// Set other varibles
		mouseWorldPosition = Vector3.zero;
		screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);

		equippedItemL = this.GetComponentInChildren<EquippedItemL>().gameObject;
		equippedItemR = this.GetComponentInChildren<EquippedItemR>().gameObject;

		weapon = equippedItemR.GetComponentInChildren<Weapon>();

		if (weapon != null)
		{
			weaponRange = weapon.range;
		}

		spawnProjectilePosition = this.GetComponentInChildren<SpawnProjectilePosition>().gameObject.transform;
	}

	// Start is called before the first frame update
	void Start()
    {
		GameManager.instance.HideCursor();

		healthBar.SetMaxHealth(maxHealth);

		// Set aimVirtualCamera to false when loaded
		aimVirtualCamera.gameObject.SetActive(false);
		cameraSensitivity = cameraFollowSensitivity / 10;   // Divided by 10 to get the correct value

		tmpLoadedAmmo.text = weapon.loadedAmmo.ToString();
		tmpSpareAmmo.text = weapon.spareAmmo.ToString();
	}

	// Update is called once per frame
	void Update()
	{
        if (currentHealth <= 0)
		{
			Debug.Log($"Player died");
			//Destroy(gameObject);
		}
		// If loaded ammo text is NOT the same as the loaded ammo on the weapon
		if (tmpLoadedAmmo.text != weapon.loadedAmmo.ToString())
		{
			// Update loaded ammo text to be the same as the loaded ammo on the weapon
			tmpLoadedAmmo.text = weapon.loadedAmmo.ToString();
		}

		// If spare ammo text is NOT the same as the spare ammo on the weapon
		if (tmpSpareAmmo.text != weapon.spareAmmo.ToString())
		{
			// Update spare ammo text to be the same as the spare ammo on the weapon
			tmpSpareAmmo.text = weapon.spareAmmo.ToString();
		}
	}

	// FixedUpdate is called at a fixed interval in sync with the physics system
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
			if (newVerticalRotation > 180)
			{
				newVerticalRotation -= 360;
			}

			// Clamp rotation to min and max angles
			verticalRotation = Mathf.Clamp(newVerticalRotation, minVerticalRotation, maxVerticalRotation);

			// Apply rotation
			cameraTarget.transform.localEulerAngles = new Vector3(verticalRotation, 0, 0);
			this.transform.Rotate(Vector3.up, lookInput.x * cameraSensitivity);
        }

		// MOVEMENT//
		// Move the player in the direction the camera is facing
		Vector3 movementDirection = (this.transform.forward * movementInput.y + this.transform.right * movementInput.x).normalized;
       
        // Apply movementDirection to playerRigidbody
        playerRigidbody.velocity = new Vector3(movementDirection.x * speed, playerRigidbody.velocity.y, movementDirection.z * speed);
		playerRigidbody.angularVelocity = Vector3.zero;

		// Check if there is ground directly below the player
		grounded = Physics.CheckSphere(this.transform.position, groundDistance, groundLayer);
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
        if (grounded)  // If the player is grounded
		{
			// Set the vertical velocity directly for a consistent jump height
			playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, jumpForce, playerRigidbody.velocity.z);
		}
	}

	public void OnCrouch(InputAction.CallbackContext context)
	{
		
	}

	public void OnInteract(InputAction.CallbackContext context)
	{
		if (!context.performed)
		{
			return;
		}

		Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
		if (Physics.Raycast(ray, out RaycastHit raycastHit, interactableDistance))
		{
			IInteractable iInteractable = raycastHit.collider.gameObject.GetComponent<IInteractable>();

			if (iInteractable != null)
			{
				iInteractable.Interact();
			}
		}
	}

	public void OnAim(InputAction.CallbackContext context)
	{
		if (context.action.triggered)
		{
			cameraSensitivity = cameraAimSensitivity / 10;  // Divided by 10 to get the correct value
			aimVirtualCamera.gameObject.SetActive(true);
		}
		else
		{
			cameraSensitivity = cameraFollowSensitivity / 10;   // Divided by 10 to get the correct value
			aimVirtualCamera.gameObject.SetActive(false);
		}
	}

	public void OnShoot(InputAction.CallbackContext context)
	{
		if (!context.performed)
		{
			return;
		}

		if (weapon != null) // If there is a weapon equipped
		{
			Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

			if (Physics.Raycast(ray, out RaycastHit raycastHit, weaponRange, aimColliderLayers))
			{
				debugTransform.position = raycastHit.point;
				mouseWorldPosition = raycastHit.point;
				//hitTransform = raycastHit.transform;
			}
			else    // Manually set distance of raycast
			{
				debugTransform.position = Camera.main.transform.position + Camera.main.transform.forward * weaponRange;
				mouseWorldPosition = Camera.main.transform.position + Camera.main.transform.forward * weaponRange;
				//hitTransform = raycastHit.transform;
			}
		}

		// Set aimDirection
		aimDirection = (mouseWorldPosition - spawnProjectilePosition.position).normalized;
		weapon.UseWeapon();
	}

	public void TakeDamage(float amount)
	{
		if (currentHealth > 0)
		{
			currentHealth -= amount;
			healthBar.SetHealth(currentHealth);
		}
	}
}
