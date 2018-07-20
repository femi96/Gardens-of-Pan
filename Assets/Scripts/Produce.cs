using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Produce : Unit {
  // Game controller that handles a produce's state and behavior

  public GardenBoard board;
  public EntityMover entityMover;

  public bool held = false;

  public float timeActive = 0f;

  public override void Awake() {
    base.Awake();
    board = garden.GetBoard();
    entityMover = GetComponent<EntityMover>();
  }

  void Update() {
    timeActive += Time.deltaTime;
    entityMover.locked = held;

    ProduceBehavior();
  }

  public abstract void ProduceBehavior();

  public void Break() {
    garden.RemoveUnit(this);
  }
}
