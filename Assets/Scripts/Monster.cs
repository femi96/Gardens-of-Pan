using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : Unit {
  // Game controller that handles a monster's state and behavior

  public GardenBoard board;
  public EntityMover mover;
  public bool owned = false;

  // Monster AI
  public MonsterBehavior currentBehavior;
  public MonsterBehavior[] behaviors;
  public bool currentBehaviorDone = true;

  // Monster models
  private GameObject modelOwned;
  private GameObject modelWild;

  public override void Awake() {
    base.Awake();
    board = garden.GetBoard();
    mover = gameObject.GetComponent<EntityMover>();
    modelOwned = transform.Find("ModelOwned").gameObject;
    modelWild = transform.Find("ModelWild").gameObject;
  }

  void Start() {
    SetBehaviors();
  }

  void Update() {
    if (currentBehaviorDone)
      StartNewBehavior();

    foreach (MonsterBehavior behavior in behaviors)
      behavior.timeSinceLastEnd += Time.deltaTime;

    currentBehavior.BehaviorUpdate();
  }

  // Returns if garden meets visit conditions
  public abstract bool CanVisit();

  // Returns if monster can be owned
  public abstract bool CanOwn();

  // Returns if garden board has a valid spawn
  public abstract bool CanSpawn();

  // Returns a valid spawn from garden board, given it exists
  public abstract SpawnPoint GetSpawn();

  // Set if monster is owned
  public void SetOwned(bool own) {

    owned = own;
    modelOwned.SetActive(owned);
    modelWild.SetActive(!owned);
  }

  // Chooses a new state, and starts it
  private void StartNewBehavior() {

    bool newState = false;
    float maxPriority = float.MinValue;

    foreach (MonsterBehavior behavior in behaviors) {

      behavior.behaviorsSince += 1;

      // Is behavior valid
      if (behavior.IsResticted())
        continue;

      // Find highest factor behavior
      float priority = behavior.GetPriority();

      if (priority > maxPriority) {
        maxPriority = priority;
        currentBehavior = behavior;
        newState = true;
      }
    }

    if (newState) {
      currentBehavior.StartBehavior();
      currentBehaviorDone = false;
    }
  }

  // Set monster's behavior states
  public void SetBehaviors() {
    behaviors = Behaviors();
  }

  // Get set of monster behavior states based on monster type
  public abstract MonsterBehavior[] Behaviors();
}
