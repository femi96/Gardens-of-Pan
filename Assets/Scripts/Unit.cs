using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour {
  // Game controller that handles a unit's state and behavior

  // Returns size of unit
  public abstract float GetSize();

  // Returns radius of wand when selecting unit
  public abstract float GetWandRadius();

  // Returns if garden has enough room to add
  public bool RoomInGarden(Garden garden) {
    return garden.FreeRoom() >= GetSize();
  }
}