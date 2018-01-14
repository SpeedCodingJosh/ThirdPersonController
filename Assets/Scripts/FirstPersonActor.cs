using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonActor : MonoBehaviour 
{
	[Header("Data")]
	public float moveSpeed = 3; 
	public float runSpeed = 2;
	public int jumpPower = 5, gravityMultiplier = 1;

	private Vector3 movement;
	
	[Header("Ground data")]
	public float groundDistance = 0.1f;
	public LayerMask whatIsGround;

	private bool isGrounded;
	private Vector3 groundNormal;

	[Header("Requirements")]
	private Rigidbody rbody;
	private PlayerControls controls;
	
	private void Start () 
	{
		rbody = GetComponent<Rigidbody>();
		controls = GetComponent<PlayerControls>();
	}
	
	// Update is called once per frame
	private void Update () 
	{
		// Check the ground 
		CheckGroundStatus();

		// Get movement axis
		movement = controls.move;

		// Apply speed to the movement
		movement *= moveSpeed;

		// Apply speed boost 
		if(controls.isRunning)
		{
			movement *= runSpeed;
		}

		// Improvement on ground movement
		movement = Vector3.ProjectOnPlane(movement, groundNormal);
	}

	private void FixedUpdate ()
	{
		// Move rigidbody
		rbody.velocity = new Vector3(movement.x, 
									 rbody.velocity.y, 
									 movement.z);

		// Perform a jump
		if(controls.jumped && isGrounded)
		{
			rbody.velocity = new Vector3(rbody.velocity.x,
										 jumpPower,
										 rbody.velocity.z);
		}
	}

	private void CheckGroundStatus ()
	{
		RaycastHit hit;

		// Check if we are on ground
		if(Physics.Raycast(transform.position, Vector3.down, out hit, groundDistance, whatIsGround))
		{
			// We are on ground
			isGrounded = true;
			groundNormal = hit.normal;
		}
		else
		{
			// We are not on ground
			isGrounded = false;
			groundNormal = Vector3.up;

			// Apply extra gravity if needed (if not set var to 1)
			rbody.AddForce((Physics.gravity * gravityMultiplier) - Physics.gravity);
		}
	}
}
