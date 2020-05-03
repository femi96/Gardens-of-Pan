using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMover : MonoBehaviour {
  // Handles entity's movement

  public bool locked = false;
  private bool moving;
  private bool isGrounded;

  public float height = 0.25f;
  public float moveSpeed = 0.5f;

  private Vector3 velocity = new Vector3(0, 0, 0);
  private Vector3 moveDirection = new Vector3(0, 0, 0);

  private const float Gravity = 10f;
  private const float HeightRayRange = 1f;
  private const float HeightError = 0.02f;

  void Update() {
    if (locked)
      return;

    // Update velocity
    ApplyGravity();
    ApplyMoveDirection();

    // Move based on velocity
    ApplyVelocity();
  }

  // Basic velocity methods
  // =========================

  // Applies gravity and normal forces to velocity
  private void ApplyGravity() {
    velocity.y -= Gravity * Time.deltaTime;

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
    } else {
      moving = false;
    }

    moveDirection.y = 0;
    moveDirection = Vector3.Normalize(moveDirection);
    moveDirection *= moveSpeed;

    // Turn then walk (turn is instant atm)
    transform.forward = moveDirection;

    velocity.x = moveDirection.x;
    velocity.z = moveDirection.z;
  }

  // Moves based on velocity
  private void ApplyVelocity() {
    Vector3 frameVelocity = velocity * Time.deltaTime;

    // Check if on ground
    RaycastHit hit;

    if (Physics.Raycast(transform.position + (Vector3.up * HeightRayRange), -Vector3.up, out hit, height + HeightRayRange, LayerConstants.GroundLayer)) {
      float heightDelta = height + HeightRayRange - hit.distance;

      if (heightDelta >= HeightError) {
        velocity += 20f * heightDelta * Vector3.up;

        if (moveSpeed == 0f)
          velocity = velocity.normalized;
        else
          velocity = velocity.normalized * moveSpeed;

        frameVelocity = velocity * Time.deltaTime;
      } else if (heightDelta >= HeightError) {
        frameVelocity += new Vector3(0, heightDelta, 0);
      }

      isGrounded = true;
    } else {
      isGrounded = false;
    }

    // Update transform position
    transform.position += frameVelocity;

    // Check if falling off world
    if (transform.position.y < -1) {
      transform.position += 2 * Vector3.up;
      isGrounded = true;
    }
  }

  // Monster movement
  // ===================

  // Causes monster to move in direction
  public void MoveTo(Vector3 destination) {
    moveDirection = destination - transform.position;
    moving = true;
  }
}
