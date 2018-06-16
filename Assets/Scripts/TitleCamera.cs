using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCamera : MonoBehaviour {
  // Controller that handles camera inputs and behaviors

  public Transform target;
  public float speed = 0.1f;

  private float radius;
  private float x;
  private float y;

  void Start() {
    radius = new Vector3(transform.position.x - target.position.x, 0, transform.position.z - target.position.z).magnitude;
    x = 0;
    y = 0;
  }

  void Update() {
    transform.LookAt(target);

    x += speed;
    Quaternion rotation = Quaternion.Euler(y, x, 0);
    Vector3 negDistance = new Vector3(0.0f, 0.0f, -radius);
    Vector3 position = rotation * negDistance + target.position + new Vector3(0, transform.position.y, 0);
    transform.position = position;
  }
}
