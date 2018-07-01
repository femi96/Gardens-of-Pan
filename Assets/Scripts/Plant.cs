using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Plant : Unit {
  // Game controller that handles a plant's state and behavior

  public GardenBoard board;
  public GameObject model;

  public bool grown;

  public float timeActive = 0f;

  public override void Awake() {
    base.Awake();
    board = garden.GetBoard();
    model = transform.Find("Model").gameObject;

    PlantAwake();
  }

  void Update() {
    timeActive += Time.deltaTime;

    PlantBehavior();
  }

  // Plants behavior on load (disabling objects etc)
  public abstract void PlantAwake();

  // Plant behavior per frame
  public abstract void PlantBehavior();

  // Returns a plants width for suffocation
  public abstract float PlantRadius();

  public void Die() {
    garden.RemoveUnit(this);
  }
}