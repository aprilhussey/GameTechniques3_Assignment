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

	private Vector2 currentAnimationBlend = new Vector2();
	private Vector2 animationVelocity = new Vector2();
	[SerializeField] private float animationSmoothTime = 0.1f;

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

		// Blend animations
		currentAnimationBlend = Vector2.SmoothDamp(currentAnimationBlend, playerController.movementInput,
			ref animationVelocity, animationSmoothTime);

		animator.SetFloat("Horizontal Input", currentAnimationBlend.x);
		animator.SetFloat("Vertical Input", currentAnimationBlend.y);

		// Walking
		if (currentAnimationBlend.x != 0 || currentAnimationBlend.y != 0)
		{
			animator.SetBool("Walking", true);
		}
		else if (currentAnimationBlend.x == 0 || currentAnimationBlend.y == 0)
		{
			animator.SetBool("Walking", false);
		}
	}
}
