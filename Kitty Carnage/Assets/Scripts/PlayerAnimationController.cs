using System.Collections;
using System.Collections.Generic;
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
		Debug.Log("normalizedSpeed = " + normalizedSpeed);

		// Set the "speed" parameter in the animator
		animator.SetFloat("Speed", normalizedSpeed, locomotionAnimationSmoothTime, Time.deltaTime);
	}
}
