using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimeout : MonoBehaviour {
  // DestroyTimeout:
  //		Controller that destroys gameObject after a set time


  // Assigned in Editor:
  public float timeoutSeconds = 3f;

  // Time variables:
  private float time;


  // Unity MonoBehavior Functions:
  void Start() {

    // Start at 0s
    time = 0f;
  }

  void Update() {

    // Destroy when time has passed
    time += Time.deltaTime;

    if (timeoutSeconds < time) {
      Destroy(gameObject);
    }
  }
}
