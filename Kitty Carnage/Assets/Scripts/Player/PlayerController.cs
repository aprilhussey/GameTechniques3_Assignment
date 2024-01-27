using Cinemachine;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamageable
{
	// Root
	private GameObject rootObject;

	[Header("Player")]
	public float maxHealth = 100f;
	private float currentHealth;

	private GameObject playerCanvas;
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

	[Header("Camera / Cinemachine")]
	[SerializeField]
	private GameObject playerCamera;
	private GameObject cameraTarget;

	[SerializeField]
	private CinemachineVirtualCamera followVirtualCamera;
	[SerializeField]
	private CinemachineVirtualCamera zoomVirtualCamera;

	[SerializeField]
	[Tooltip("Minimum vertical rotation of the CameraTarget gameobject")]
	float minVerticalRotation = -80f;   // Define min rotation
	[SerializeField]
	[Tooltip("Maximum vertical rotation of the CameraTarget gameobject")]
	float maxVerticalRotation = 80f;   // Define max rotation

	private float cameraSensitivity;
	[SerializeField]
	[Tooltip("Camera sensitivity when NOT zooming")]
	private float cameraFollowSensitivity = 5f;  // Camera sensitivity when NOT zooming
	[SerializeField]
	[Tooltip("Camera sensitivity when zooming")]
	private float cameraZoomSensitivity = 2.5f;   // Camera sensitivity when zooming

	[Header("Audio listener")]
	[SerializeField]
	private AudioListener playerAudioListener;

	// Layer masks
	private LayerMask defaultLayer;
	private LayerMask groundLayer;
	private LayerMask zoomColliderLayers;

	// Input actions variables
	[HideInInspector]
	public Vector2 movementInput;
	private Vector2 lookInput;

	[Header("Weapon")]
	[HideInInspector]
	public Weapon weapon;
	private float weaponRange;

	[HideInInspector]
	public Vector3 zoomDirection;
	public Transform spawnProjectilePosition;

	[Header("HUD")]
	// Ammo visuals
	[SerializeField]
	private TextMeshProUGUI tmpLoadedAmmo;
	[SerializeField]
	private TextMeshProUGUI tmpSpareAmmo;

	// Popup visuals
	public TextMeshProUGUI tmpPopUp;

	[Header("Other")]
	[SerializeField]
	private Transform debugTransform;
	private Vector3 mouseWorldPosition = new Vector3();
	private Vector2 screenCenterPoint = new Vector2();

	private GameObject equippedItemL;
	private GameObject equippedItemR;

	[Header("VFX")]
	[SerializeField]
	private Transform vfxHitGreen;
	[SerializeField]
	private Transform vfxHitRed;

	//private Transform hitTransform = null;

	// Photon
	private PhotonView photonView;

	void Awake()
	{
		// Set root object
		rootObject = this.transform.root.gameObject;

		// Set Photon view
		photonView = this.gameObject.GetComponent<PhotonView>();

		// Set player canvas
		playerCanvas = rootObject.GetComponentInChildren<HUD>().gameObject;

		// Set health
		currentHealth = maxHealth;
		healthBar = playerCanvas.GetComponentInChildren<HealthBar>();

		// Set rigidbody
		playerRigidbody = this.GetComponent<Rigidbody>();

		// Set camera / cinemachine variables
		cameraTarget = this.gameObject.GetComponentInChildren<CameraTarget>().gameObject;

		// Set layer mask variables
		defaultLayer = 1 << LayerMask.NameToLayer("Default");
		groundLayer = 1 << LayerMask.NameToLayer("Ground");
		zoomColliderLayers = defaultLayer | groundLayer;

		// Set input actions variables
		movementInput = Vector2.zero;
		lookInput = Vector2.zero;

		// Set other varibles
		mouseWorldPosition = Vector3.zero;
		screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);

		equippedItemL = this.GetComponentInChildren<EquippedItemL>().gameObject;
		equippedItemR = this.GetComponentInChildren<EquippedItemR>().gameObject;

		if (equippedItemR != null)
		{
			weapon = equippedItemR.GetComponentInChildren<Weapon>();
			
			if (weapon != null)
			{
				weaponRange = weapon.range;
			}
		}
	}

	void Start()
	{
		GameManager.Instance.HideCursor();

		healthBar.SetMaxHealth(maxHealth);

		// Set zoomVirtualCamera to false when loaded
		zoomVirtualCamera.Follow = this.gameObject.GetComponentInChildren<CameraTarget>().transform;
		zoomVirtualCamera.gameObject.SetActive(false);

		followVirtualCamera.Follow = this.gameObject.GetComponentInChildren<CameraTarget>().transform;
		followVirtualCamera.LookAt = this.gameObject.GetComponentInChildren<CameraTarget>().transform;

		cameraSensitivity = cameraFollowSensitivity / 10;   // Divided by 10 to get the correct value

		if (weapon != null)
		{
			tmpLoadedAmmo.text = weapon.loadedAmmo.ToString();
			tmpSpareAmmo.text = weapon.spareAmmo.ToString();
		}
		else
		{
			tmpLoadedAmmo.text = "";
			tmpSpareAmmo.text = "";
		}

		if (photonView != null)
		{
			if (photonView.IsMine)
			{
				playerCamera.SetActive(true);
				zoomVirtualCamera.enabled = true;
				followVirtualCamera.enabled = true;

				playerAudioListener.enabled = true;
			}
			else
			{
				playerCamera.SetActive(false);
				zoomVirtualCamera.enabled = false;
				followVirtualCamera.enabled = false;

				playerAudioListener.enabled = false;
			}
		}
	}

	void Update()
	{
		if (photonView != null)
		{

			if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
			{
				return;
			}

			if (currentHealth <= 0)
			{
				Debug.Log($"Player died");
				//Destroy(gameObject);
			}

			if (weapon != null)
			{
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
		}
	}

	void FixedUpdate()
	{
		if (photonView != null)
		{
			if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
			{
				return;
			}

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

		Ray ray = playerCamera.GetComponent<Camera>().ScreenPointToRay(screenCenterPoint);
		if (Physics.Raycast(ray, out RaycastHit raycastHit, interactableDistance))
		{
			IInteractable iInteractable = raycastHit.collider.gameObject.GetComponent<IInteractable>();

			if (iInteractable != null)
			{
				iInteractable.Interact(this);
			}
		}
	}

	public void OnZoom(InputAction.CallbackContext context)
	{
		if (context.action.triggered)
		{
			cameraSensitivity = cameraZoomSensitivity / 10;  // Divided by 10 to get the correct value
			zoomVirtualCamera.gameObject.SetActive(true);
		}
		else
		{
			cameraSensitivity = cameraFollowSensitivity / 10;   // Divided by 10 to get the correct value
			zoomVirtualCamera.gameObject.SetActive(false);
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
			Ray ray = playerCamera.GetComponent<Camera>().ScreenPointToRay(screenCenterPoint);

			if (Physics.Raycast(ray, out RaycastHit raycastHit, weaponRange, zoomColliderLayers))
			{
				debugTransform.position = raycastHit.point;
				mouseWorldPosition = raycastHit.point;
				//hitTransform = raycastHit.transform;
			}
			else    // Manually set distance of raycast
			{
				debugTransform.position = playerCamera.transform.position + playerCamera.transform.forward * weaponRange;
				mouseWorldPosition = playerCamera.transform.position + playerCamera.transform.forward * weaponRange;
				//hitTransform = raycastHit.transform;
			}

			// Set zoomDirection
			zoomDirection = (mouseWorldPosition - spawnProjectilePosition.position).normalized;
			weapon.UseWeapon();
		}
	}

	public void TakeDamage(float amount)
	{
		if (currentHealth > 0)
		{
			currentHealth -= amount;
			healthBar.SetHealth(currentHealth);
		}
	}

	public void AddAmmo(int amount)
	{
		if (weapon != null)
		{
			weapon.spareAmmo = weapon.spareAmmo + amount;
		}
	}
}