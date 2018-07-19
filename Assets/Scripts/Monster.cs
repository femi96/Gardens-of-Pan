using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : Unit {
  // Game controller that handles a monster's state and behaviour

  public GardenBoard board;
  public EntityMover mover;
  public bool owned = false;

  // Monster AI
  public MonsterBehaviour currentBehaviour;
  public MonsterBehaviour[] behaviours;
  public bool currentBehaviourDone = true;

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
    SetBehaviours();
  }

  void Update() {
    if (currentBehaviourDone)
      StartNewBehaviour();

    foreach (MonsterBehaviour behaviour in behaviours)
      behaviour.timeSinceLastEnd += Time.deltaTime;

    currentBehaviour.BehaviourUpdate();
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
  private void StartNewBehaviour() {

    bool newState = false;
    float maxPriority = float.MinValue;

    foreach (MonsterBehaviour behaviour in behaviours) {

      behaviour.behavioursSince += 1;

      // Is behaviour valid
      if (behaviour.IsResticted())
        continue;

      // Find highest factor behaviour
      float priority = behaviour.GetPriority();

      if (priority > maxPriority) {
        maxPriority = priority;
        currentBehaviour = behaviour;
        newState = true;
      }
    }

    if (newState) {
      currentBehaviour.StartBehaviour();
      currentBehaviourDone = false;
    }
  }

  // Set monster's behaviour states
  public void SetBehaviours() {
    behaviours = Behaviours();
  }

  // Get set of monster behaviour states based on monster type
  public abstract MonsterBehaviour[] Behaviours();
}
