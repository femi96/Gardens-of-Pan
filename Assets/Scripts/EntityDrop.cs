using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDrop : MonoBehaviour {
  // Game controller that handles an entity's movement

  public bool locked = false;

  private bool isGrounded;
  public float height = 0.25f;
  private Vector3 velocity;
  private static float gravity = 10f;
  private static float rayRange = 0.2f;

  void Start() {
    velocity = new Vector3(0, 0, 0);
  }

  void Update() {
    if (!locked) {
      ApplyGravity();
      ApplyVelocity();
    }
  }

  // Applies gravity and normal forces to velocity
  private void ApplyGravity() {
    velocity.y -= gravity * Time.deltaTime;

    if (isGrounded) {
      velocity.y = 0;
    }
  }

  // Moves based on velocity and limits to garden area
  private void ApplyVelocity() {
    Vector3 frameVelocity = velocity * Time.deltaTime;

    // Update transform position
    transform.position += frameVelocity;

    // Check if on ground
    RaycastHit hit;

    if (Physics.Raycast(transform.position + (Vector3.up * rayRange), -Vector3.up, out hit, height + rayRange, LayerConstants.GroundLayer)) {
      Vector3 collide = new Vector3(0, height + rayRange - hit.distance, 0);
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
