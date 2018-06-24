using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Produce : Unit {
  // Game controller that handles a produce's state and behavior

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

    ProduceBehavior();
  }

  public abstract void ProduceBehavior();

  public void Break() {
    Destroy(gameObject);
  }
}
