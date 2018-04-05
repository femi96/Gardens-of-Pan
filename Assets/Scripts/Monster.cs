using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : Unit {
  // Game controller that handles a monster's state and behavior

  // Monster variables:
  [Header("Monster")]
  private Garden garden;
  private MonsterMover mover;
  private bool owned = false;

  void Awake() {

    // Awake with components
    garden = GameObject.Find("Garden").GetComponent<Garden>();
    mover = gameObject.GetComponent<MonsterMover>();
  }

  void Start() {}

  void Update() {}

  // Returns if garden meets visit conditions
  public abstract bool CanVisit(Garden garden);

  // Returns if garden board has a valid spawn
  public abstract bool CanSpawn(GardenBoard board);

  // Returns a valid spawn from garden board, given it exists
  public abstract SpawnPoint GetSpawn(GardenBoard board);

  // Returns if monster is owned
  public bool Owned() {
    return owned;
  }
}
