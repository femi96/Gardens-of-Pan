using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Seed : Unit {
  // Game controller that handles a seed's state and behavior

  public GardenBoard board;
  public GameObject model;
  public GameObject modelRot;

  public float timeActive = 0f;

  public override void Awake() {
    base.Awake();
    board = garden.GetBoard();
    model = transform.Find("Model").gameObject;
    modelRot = transform.Find("ModelRot").gameObject;
  }

  void Update() {
    timeActive += Time.deltaTime;

    SeedBehavior();
  }

  public abstract void SeedBehavior();

  public abstract void Plant();

  public void Break() {
    Destroy(gameObject);
  }
}