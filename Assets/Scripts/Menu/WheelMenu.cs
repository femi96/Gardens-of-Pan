using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelMenu : MonoBehaviour {
  // Game controller that handles wheel menu

  public Garden garden;

  // All public variables are assigned in editor

  public void Shovel() {
    garden.SetGardenMode(GardenMode.Play);
  }
}