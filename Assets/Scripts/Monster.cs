using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Unit {
	// Monster: Class for monsters in the garden
	

	// Monster variables:
	[Header("Monster")]
	public bool owned = false;

	[HideInInspector]public Garden garden;

	// Movement variables:
	[Header("Movement")]
	public bool isGrounded;
	public float radius = 0.25f;
	public float height = 0.25f;
	public float moveSpeed = 0.5f;

	private Vector3 velocity = new Vector3(0, 0, 0);
	private static float gravity = 0.1f;
	private int gLayer = 1 << 8;
	// private int wLayer = 1 << 9;

	// MonsterAI variables:
	[Header("MonsterAI")]
	public MonsterState currentState;
	public MonsterState[] states;
	public float stateTime;

	[HideInInspector]public bool stateDone = false;
	[HideInInspector]public Dictionary<MonsterState, int> statesSinceState;
	[HideInInspector]public MonsterState possibleState;
	[HideInInspector]public Vector3 moveDestination;
	[HideInInspector]public Vector3 moveDirection;
	[HideInInspector]public bool moving = false;


	// Unity MonoBehavior Functions:
	void Awake() {

		// Awake with components
		garden = GameObject.Find("Garden").GetComponent<Garden>();
	}

	void Start() {

		// MonsterAI factor repeat
		statesSinceState = new Dictionary<MonsterState, int>();
		foreach(MonsterState state in states) {
			statesSinceState[state] = 0;
		}
	}
	
	void Update() {

		// MonsterAI state updates
		stateTime += Time.deltaTime;
		if(currentState == null || stateDone) { ChangeState(); }
		currentState.UpdateState(this);

		// Movement updates
		ApplyGravity();
		if(moving) { ApplyMoveDirection(moveDirection); }
		else { velocity.x = 0; velocity.z = 0; }
		ApplyVelocity();
	}
	
	void FixedUpdate() {}

	// Returns if garden meets visit conditions
	public virtual bool CanVisit(Garden garden) {
		return false;
	}

	// Returns if garden has enough room to enter
	public virtual bool RoomInGarden(Garden garden) {
		UpdateSize();
		if(garden.FreeRoom() >= size) {
			return true;
		}
		return false;
	}

	// Updates unit size
	public virtual void UpdateSize() {
		size = 0;
	}

	// Applies gravity and normal forces to velocity
	private void ApplyGravity() {
		velocity.y -= gravity;
		if(isGrounded) {
			velocity.y = 0;
		}
	}

	// Applies a move direction to velocity
	private void ApplyMoveDirection(Vector3 move) {
		move.y = 0;
		move = Vector3.Normalize(move);
		move *= moveSpeed;
		transform.forward = move;

		velocity.x = move.x;
		velocity.z = move.z;
	}

	// Moves based on velocity and limits to garden area
	private void ApplyVelocity() {
		Move(velocity * Time.deltaTime);
		LimitMovement();
	}

	// Applies move vector to transform position
	private void Move(Vector3 v) {

		// Update transform position
		transform.position += v;

		// Check if on ground
		Vector3 collide = new Vector3(0,0,0);
		RaycastHit hit;
		if(Physics.Raycast(transform.position, -Vector3.up, out hit, height, gLayer)) {
			collide.y = height - hit.distance;
			transform.position += collide;
			isGrounded = true;
		} else {
			isGrounded = false;
		}

		// Check if falling off world
		if(transform.position.y < -2) {
			transform.position += 4*Vector3.up;
			isGrounded = true;
		}
	}

	// Corrects position to in garden area
	private void LimitMovement() {

		// Get garden limits assuming square garden
		float limit = (garden.gardenSize / 2f) - radius;

		float y = transform.position.y;
		float x = transform.position.x;
		float z = transform.position.z;

		// Correct x
		if(x < -limit) { transform.position = new Vector3(-limit, y, z); }
		if(x > limit) { transform.position = new Vector3(limit, y, z); }
		x = transform.position.x;

		// Correct z
		if(z < -limit) { transform.position = new Vector3(x, y, -limit); }
		if(z > limit) { transform.position = new Vector3(x, y, limit); }
		z = transform.position.z;
	}

	// Weight MonsterAI states and set the best one
	public void ChangeState() {

		// Initial state is null with score -1
		float score = -1f;
		MonsterState nextState = currentState;

		// Get state with highest factor score 
		foreach(MonsterState state in states) {
			possibleState = state;

			float newScore = 0;
			newScore += state.GetScore(this);

			if(newScore > score) { score = newScore; nextState = state; }

			statesSinceState[state] += 1;
		}
		currentState = nextState;

		// Update current state and setup to run
		stateDone = false;
		stateTime = 0f;
		statesSinceState[currentState] = 0;
	}
}
