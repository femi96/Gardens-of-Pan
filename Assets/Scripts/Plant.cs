using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Plant : Unit {
  // Game controller that handles a plant's state and behavior

  public GardenBoard board;
  public GameObject model;

  public float timeActive = 0f;

  public override void Awake() {
    base.Awake();
    board = garden.GetBoard();
    model = transform.Find("Model").gameObject;
  }

  void Update() {
    timeActive += Time.deltaTime;

    PlantBehavior();
  }

  public abstract void PlantBehavior();

  public void Die() {
    Destroy(gameObject);
  }
}
