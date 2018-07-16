﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMover : MonoBehaviour {
  // Game controller that handles an entity's movement

  public bool locked = false;
  private bool moving;
  private bool isGrounded;

  public float height = 0.25f;
  public float moveSpeed = 0.5f;

  private Vector3 velocity = new Vector3(0, 0, 0);
  private Vector3 moveDirection = new Vector3(0, 0, 0);
  private static float gravity = 10f;
  private static float rayRange = 1f;

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

  // Applies gravity and normal forces to velocity
  private void ApplyGravity() {
    velocity.y -= gravity * Time.deltaTime;

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

  // Moves based on velocity
  private void ApplyVelocity() {
    Vector3 frameVelocity = velocity * Time.deltaTime;

    // Update transform position
    transform.position += frameVelocity;

    // Check if on ground
    RaycastHit hit;

    if (Physics.Raycast(transform.position + (Vector3.up * rayRange), -Vector3.up, out hit, height + rayRange, LayerConstants.GroundLayer)) {
      Vector3 collide = new Vector3(0, height + rayRange - hit.distance, 0);

      if (collide.y > Time.deltaTime)
        transform.position += Time.deltaTime * Vector3.up;
      else
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

  // Monster movement

  // Starts monster movement
  public void MoveStart(Vector3 destination) {
    moveDirection = destination - transform.position;
    moving = true;
  }

  // Stops monster movement
  public void MoveStop() {
    moving = false;
  }

  // Returns if monster is moving
  public bool IsMoving() {
    return moving;
  }
}