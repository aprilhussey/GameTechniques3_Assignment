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
	private int lookSpeed;
	private int ammoAmount;

	// Other variables
	private InputActions inputActions;
	private Vector2 movementInput = new Vector2();

    private Rigidbody rb;

    // Awake is called before Start
    void Awake()
    {
		// Access character data - Character.cs
		id = playerData.id;
        health = playerData.health;
        speed = playerData.speed;

		// Access character data - PlayableCharacter.cs
        lookSpeed = playerData.lookSpeed;
        ammoAmount = playerData.ammoAmount;

        inputActions = new InputActions();
		inputActions.Player.Movement.performed += context => movementInput = context.ReadValue<Vector2>();
		inputActions.Player.Movement.canceled += context => movementInput = Vector2.zero;

        rb = GetComponent<Rigidbody>();
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
        
    }

    // Update is called once per frame
    void Update()
    {
		// Move player using velocity
		rb.velocity = new Vector3(movementInput.x * speed, rb.velocity.y, movementInput.y * speed);
	}
}
