using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
	private PlayerController playerController;

	private Animator animator;
    private Rigidbody playerRigidbody;

	const float locomotionAnimationSmoothTime = 0.1f;

	private Vector2 currentAnimationBlend = new Vector2();
	private Vector2 animationVelocity = new Vector2();
	[SerializeField]
	private float animationSmoothTime = 0.1f;

	void Awake()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
		playerRigidbody = GetComponent<Rigidbody>();
	}

    void Update()
    {
		// Calculate speed of player
        float currentSpeed = playerRigidbody.velocity.magnitude;

		// Normalize the speed to a value between 0 and 1
		float normalizedSpeed = currentSpeed / playerController.speed;

		// Set the "speed" parameter in the animator
		animator.SetFloat("speed", normalizedSpeed, locomotionAnimationSmoothTime, Time.deltaTime);

		// Blend animations
		currentAnimationBlend = Vector2.SmoothDamp(currentAnimationBlend, playerController.movementInput,
			ref animationVelocity, animationSmoothTime);

		animator.SetFloat("horizontalInput", currentAnimationBlend.x);
		animator.SetFloat("verticalInput", currentAnimationBlend.y);

		// Walking
		if (currentAnimationBlend.x != 0 || currentAnimationBlend.y != 0)
		{
			animator.SetBool("walking", true);
		}
		else if (currentAnimationBlend.x == 0 || currentAnimationBlend.y == 0)
		{
			animator.SetBool("walking", false);
		}
	}
}
