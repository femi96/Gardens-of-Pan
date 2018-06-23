using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Seed : Unit {
  // Game controller that handles a seed's state and behavior

  public GameObject model;

  public float timeActive = 0f;

  void Awake() {
    base.Awake();
    model = transform.Find("Model").gameObject;
  }

  void Update() {
    timeActive += Time.deltaTime;

    SeedBehavior();
  }

  public abstract bool SeedBehavior();

  public abstract void Plant();

  public void Break() {
    Destroy(gameObject);
  }
}