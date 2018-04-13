using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : Unit {
  // Game controller that handles a monster's state and behavior

  private MonsterMover mover;
  private bool owned = false;

  // Monster AI
  public MonsterBehavior currentBehavior;
  public MonsterBehavior[] behaviors;
  private bool currentBehaviorDone = true;

  void Awake() {

    // Awake with components
    mover = gameObject.GetComponent<MonsterMover>();
  }

  void Start() {

    Behaviors();
  }

  void Update() {

    if (currentBehaviorDone)
      StartNewBehavior();
    else
      currentBehavior.BehaviorUpdate();
  }

  // Returns if garden meets visit conditions
  public abstract bool CanVisit(Garden garden);

  // Returns if monster can be owned
  public abstract bool CanOwn(Garden garden);

  // Returns if garden board has a valid spawn
  public abstract bool CanSpawn(GardenBoard board);

  // Returns a valid spawn from garden board, given it exists
  public abstract SpawnPoint GetSpawn(GardenBoard board);

  // Returns if monster is owned
  public bool IsOwned() {
    return owned;
  }

  // Set if monster is owned
  public void SetOwned(bool own) {
    owned = own;
  }

  // Returns monster rmover
  public MonsterMover GetMover() {
    return mover;
  }

  // Sets current state to done
  public void BehaviorDone() {
    currentBehaviorDone = true;
  }

  // Chooses a new state, and starts it
  private void StartNewBehavior() {

    bool newState = false;
    float max = float.MinValue;

    foreach (MonsterBehavior b in behaviors) {

      float f = b.GetFactorTotal();

      if (f > max) {
        max = f;
        currentBehavior = b;
        newState = true;
      }
    }

    if (newState) {
      currentBehavior.StartBehavior();
      currentBehaviorDone = false;
    }
  }

  // Create and set monster's behavior states
  public abstract void Behaviors();

  // Set monster's behavior states to input b
  public void SetBehaviors(MonsterBehavior[] b) {
    behaviors = b;
  }
}
