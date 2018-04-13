using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMover : MonoBehaviour {
  // Game controller that handles a monster's movement

  // Movement variables (prefab):
  private bool moving;
  private bool isGrounded;

  public float radius = 0.25f;
  public float height = 0.25f;
  public float moveSpeed = 0.5f;

  private Vector3 velocity;
  private Vector3 moveDirection;

  private static float gravity = 0.1f;
  private static int gLayer = 1 << 8;
  // private static int wLayer = 1 << 9;

  void Start() {
    velocity = new Vector3(0, 0, 0);
    moveDirection = new Vector3(0, 0, 0);
  }

  void Update() {

    // Update velocity
    ApplyGravity();
    ApplyMoveDirection();

    // Move based on velocity
    ApplyVelocity();
  }

  // Starts monster movement
  public void Move(Vector3 destination) {
    moveDirection = destination - transform.position;
    moving = true;
  }

  // Stops monster movement
  public void Stop() {
    moving = false;
  }

  // Returns if monster is moving
  public bool IsMoving() {
    return moving;
  }

  // Applies gravity and normal forces to velocity
  private void ApplyGravity() {
    velocity.y -= gravity;

    if (isGrounded) {
      velocity.y = 0;
    }
  }

  // Applies a move direction to velocity
  private void ApplyMoveDirection() {
    if (!moving) {
      velocity.x = 0;
      velocity.z = 0;
      return;
    }

    // Turn then walk (turn is instant atm)
    transform.forward = moveDirection;

    moveDirection.y = 0;
    moveDirection = Vector3.Normalize(moveDirection);
    moveDirection *= moveSpeed;

    velocity.x = moveDirection.x;
    velocity.z = moveDirection.z;
  }

  // Moves based on velocity and limits to garden area
  private void ApplyVelocity() {
    ApplyMove(velocity * Time.deltaTime);
  }

  // Applies move vector to transform position
  private void ApplyMove(Vector3 v) {

    // Update transform position
    transform.position += v;

    // Check if on ground
    Vector3 collide = new Vector3(0, 0, 0);
    RaycastHit hit;

    if (Physics.Raycast(transform.position, -Vector3.up, out hit, height, gLayer)) {
      collide.y = height - hit.distance;
      transform.position += collide;
      isGrounded = true;
    } else {
      isGrounded = false;
    }

    // Check if falling off world
    if (transform.position.y < -1) {
      transform.position += 2 * Vector3.up;
      isGrounded = true;
    }
  }
}
