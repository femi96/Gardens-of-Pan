using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDrop : MonoBehaviour {
  // Game controller that handles an entity's movement

  private bool isGrounded;
  public float height = 0.25f;
  private Vector3 velocity;
  private static float gravity = 10f;

  void Start() {
    velocity = new Vector3(0, 0, 0);
  }

  void Update() {
    ApplyGravity();
    ApplyVelocity();
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
    Vector3 collide = new Vector3(0, 0, 0);
    RaycastHit hit;

    if (Physics.Raycast(transform.position, -Vector3.up, out hit, height, LayerConstants.GroundLayer)) {
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
