using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimeout : MonoBehaviour {
  // Destroys gameObject after a set time

  public float timeoutSeconds = 3f; // Time to destroy

  private float time; // Current time

  void Start() {
    time = 0f;
  }

  void Update() {

    // Destroy this when time has passed
    time += Time.deltaTime;

    if (timeoutSeconds <= time) {
      Destroy(gameObject);
    }
  }
}
