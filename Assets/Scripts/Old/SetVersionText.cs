using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetVersionText : MonoBehaviour {

  public string prefix;

  void Start() {
    Text text = GetComponent<Text>();
    text.text = prefix + VersionConstants.GameVersion;
  }
}