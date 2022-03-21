using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnPlay : MonoBehaviour {
  // Disables gameObject on start

  void Start() {
    gameObject.SetActive(false);
  }
}
