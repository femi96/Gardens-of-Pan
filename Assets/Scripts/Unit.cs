using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour {
  // Game controller that handles a unit's state and behavior

  public Garden garden;

  public virtual void Awake() {
    garden = GameObject.FindWithTag("Garden").GetComponent<Garden>();
  }

  // Returns name of unit
  public abstract string GetName();

  // Returns size of unit
  public abstract float GetSize();

  // Returns radius of wand when selecting unit
  public abstract float GetWandRadius();
}