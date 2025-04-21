using System.ComponentModel;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : NetworkBehaviour
{
	private CharacterController characterController;

	[Header("Move Speed Settings")]
	[SerializeField] private float runSpeed = 12f;
	[SerializeField] private float sprintSpeed = 18f;
	[SerializeField] private float walkSpeed = 6f;
	[SerializeField] private float crouchSpeed = 3f;
	[SerializeField] private float currentSpeed = 0f;

	[Header("Jump Settings")]
	[SerializeField] private float standingJumpHeight = 3f;
	[SerializeField] private float crouchedJumpHeight = 1.5f;

	[Header("Other Settings")]
	[SerializeField] private Transform groundCheck;
	[SerializeField] private LayerMask groundMask;

	[Header("Bool Settings Dynamic")]
	[ReadOnly(true)]
	[SerializeField] private bool isGrounded;
	[ReadOnly(true)]
	[SerializeField] private bool isRunning;
	[ReadOnly(true)]
	[SerializeField] private bool isSprinting;
	[ReadOnly(true)]
	[SerializeField] private bool isCrouching;
	[ReadOnly(true)]
	[SerializeField] private bool isWalking;
	[ReadOnly(true)]
	[SerializeField] private bool isStill;
	[ReadOnly(true)]
	[SerializeField] private bool canMove;

	private float xMovement;
	private float yMovement;
	private float gravity = -30f;
	private float goundCheckRadious = 0.4f;

	private Vector3 velocity;

	private float last_clicked_time;

	private void Start()
	{
		NetworkManager.OnClientStarted += NetworkManager_OnClientStarted;
	}

	private void NetworkManager_OnClientStarted()
	{
		enabled = IsOwner;

		characterController = GetComponent<CharacterController>();
		canMove = true;
	}

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();

		enabled = IsOwner;

		characterController = GetComponent<CharacterController>();
		canMove = true;
	}

	private void Update()
	{
		if (characterController != null)
		{
			if (canMove)
				GetInputs();
			CheckIsGrounded();
			Move();
		}
	}

	private void FixedUpdate()
	{
		HandleMoveSpeed();
	}

	private void GetInputs()
	{
		xMovement = Input.GetAxis("Horizontal");
		yMovement = Input.GetAxis("Vertical");

		if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
		{
			if (isCrouching)
				velocity.y = Mathf.Sqrt(crouchedJumpHeight * -2f * gravity);
			else
				velocity.y = Mathf.Sqrt(standingJumpHeight * -2f * gravity);
		}

		//Not Moving at all
		if (!Input.GetKey(KeyCode.W) || (!Input.GetKey(KeyCode.A) || !Input.GetKey(KeyCode.S) || !Input.GetKey(KeyCode.D)))
			HandleMoveBools(true, false, false, false, false);

		//Walking
		if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
			HandleMoveBools(false, true, false, false, false);

		//Runing
		if (Input.GetKey(KeyCode.LeftControl) && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
		{
			//Sprinting
			if (Input.GetKey(KeyCode.CapsLock))
				HandleMoveBools(false, false, false, false, true);
			//Runing
			else
				HandleMoveBools(false, false, false, true, false);
		}

		//Crouching
		if (Input.GetKey(KeyCode.LeftShift) && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
		{
			HandleMoveBools(false, false, true, false, false);
			transform.localScale = new Vector3(transform.localScale.x, 0.5f, transform.localScale.z);
		}
		else
			transform.localScale = new Vector3(transform.localScale.x, 1f, transform.localScale.z);

		if (Input.GetKey(KeyCode.LeftShift))
			transform.localScale = new Vector3(transform.localScale.x, 0.5f, transform.localScale.z);
		else
			transform.localScale = new Vector3(transform.localScale.x, 1f, transform.localScale.z);
	}

	public void HandleMoveBools(bool _is_still, bool _is_walking, bool _is_crouching, bool _is_running, bool _is_sprinting)
	{
		isSprinting = _is_sprinting;
		isRunning = _is_running;
		isStill = _is_still;
		isCrouching = _is_crouching;
		isWalking = _is_walking;
	}

	public void HandleMoveSpeed()
	{
		if (isStill)
			currentSpeed = walkSpeed;
		if (isWalking)
			currentSpeed = walkSpeed;
		if (isCrouching)
			currentSpeed = crouchSpeed;
		if (isRunning)
			currentSpeed = runSpeed;
		if (isSprinting)
			currentSpeed = sprintSpeed;
	}

	public void Move()
	{
		Vector3 movement = transform.right * xMovement + transform.forward * yMovement;
		movement.Normalize();

		characterController.Move(movement * currentSpeed * Time.deltaTime);

		velocity.y += gravity * Time.deltaTime;
		characterController.Move(velocity * Time.deltaTime);
	}

	public void CheckIsGrounded()
	{
		isGrounded = Physics.CheckSphere(groundCheck.position, goundCheckRadious, groundMask);

		if (isGrounded && velocity.y < 0f)
			velocity.y = -2f;
	}
}
