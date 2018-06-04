using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour {
  // Controller that handles camera inputs and behaviors

  private Transform wand;

  private float x = 0.0f;   // Current camera angles
  private float y = 30.0f;

  private float distance = 3.0f;  // Current distance from wand

  private float xSpeed = 9.0f;  // Angular change rate
  private float ySpeed = 12.0f;
  private float yMinLimit = 5f; // Angle bounds
  private float yMaxLimit = 80f;

  private float distanceSpeed = 0.5f; // Distance change rate
  private float distanceMin = 1f;     // Distance bounds
  private float distanceMax = 5f;

  void Start() {

    // Initialize fields
    wand = transform.parent;

    // Camera depth mode for water shader
    GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;

    // Lock cursor to screen
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;

    // Camera positioning
    Vector3 angles = transform.eulerAngles;
    x = angles.y;
    y = angles.x;
  }

  void Update() {

    // Lock cursor to screen on input
    if (Input.GetAxis(InputConstants.CameraLock) > 0) {
      Cursor.lockState = CursorLockMode.Locked;
    }
  }

  void LateUpdate() {

    // TODO: Frame independent

    // Update camera position based on mouse movement
    x += Input.GetAxis(InputConstants.CameraX) * xSpeed * 0.02f;
    y -= Input.GetAxis(InputConstants.CameraY) * ySpeed * 0.02f;
    x = (x + 360f) % 360f;
    y = Mathf.Clamp(y, yMinLimit, yMaxLimit);

    Quaternion rotation = Quaternion.Euler(y, x, 0);

    distance = Mathf.Clamp(distance - Input.GetAxis(InputConstants.CameraZoom) * distanceSpeed, distanceMin, distanceMax);

    Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
    Vector3 position = rotation * negDistance + wand.position;

    transform.rotation = rotation;
    transform.position = position;
  }
}
