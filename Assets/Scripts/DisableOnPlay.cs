using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnPlay : MonoBehaviour {
  // Game controller that disables gameobjects on start

  void Start() {
    gameObject.SetActive(false);
  }
}
