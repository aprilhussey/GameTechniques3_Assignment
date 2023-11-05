using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
	private PlayerController playerController;

	private Animator animator;
    private Rigidbody rb;

	const float locomotionAnimationSmoothTime = 0.1f;

	// Awake is called before Start
	void Awake()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody>();
	}
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate speed of player
        float currentSpeed = rb.velocity.magnitude;

		// Normalize the speed to a value between 0 and 1
		float normalizedSpeed = currentSpeed / playerController.speed;

		// Set the "speed" parameter in the animator
		animator.SetFloat("Speed", normalizedSpeed, locomotionAnimationSmoothTime, Time.deltaTime);

        Direction(playerController.movementInput);
	}

	void Direction(Vector2 movementInput)
    {
		// Forward movement input
        if (movementInput.y > 0)
        {
            animator.SetBool("Forward", true);
        }
		else if (movementInput.y == 0)
		{
			animator.SetBool("Forward", false);
		}

		// Forward right movement input
		if (movementInput.y > 0 && movementInput.x > 0)
		{
			animator.SetBool("Forward", true);
			animator.SetBool("Right", true);
		}
		else if (movementInput.y == 0 && movementInput.x == 0)
		{
			animator.SetBool("Forward", false);
			animator.SetBool("Right", false);
		}

		// Right movement input
		if (movementInput.x > 0)
		{
			animator.SetBool("Right", true);
		}
		else if (movementInput.x == 0)
		{
			animator.SetBool("Right", false);
		}

		// Backward right movement input
		if (movementInput.y < 0 && movementInput.x > 0)
		{
			animator.SetBool("Backward", true);
			animator.SetBool("Right", true);
		}
		else if (movementInput.y == 0 && movementInput.x == 0)
		{
			animator.SetBool("Backward", false);
			animator.SetBool("Right", false);
		}

		// Backward movement input
		if (movementInput.y < 0)
		{
			animator.SetBool("Backward", true);
		}
		else if (movementInput.y == 0)
		{
			animator.SetBool("Backward", false);
		}

		// Backward left movement input
		if (movementInput.y < 0 && movementInput.x < 0)
		{
			animator.SetBool("Backward", true);
			animator.SetBool("Left", true);
		}
		else if (movementInput.y == 0 && movementInput.x == 0)
		{
			animator.SetBool("Backward", false);
			animator.SetBool("Left", false);
		}

		// Left movement input
		if (movementInput.x < 0)
		{
			animator.SetBool("Left", true);
		}
		else if (movementInput.x == 0)
		{
			animator.SetBool("Left", false);
		}

		// Forward left movement input
		if (movementInput.y > 0 && movementInput.x < 0)
		{
			animator.SetBool("Forward", true);
			animator.SetBool("Left", true);
		}
		else if (movementInput.y == 0 && movementInput.x == 0)
		{
			animator.SetBool("Forward", false);
			animator.SetBool("Left", false);
		}
	}
}
