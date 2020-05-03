using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Seed : Unit {
  // Handles a seed's state and behavior

  [Header("Seed Fields")]
  public GameObject model;

  public float timeActive = 0f;

  public override void Awake() {
    base.Awake();
    model = transform.Find("Model").gameObject;
  }

  void Update() {
    timeActive += Time.deltaTime;

    SeedUpdate();
  }

  public abstract void SeedUpdate();

  public abstract void Plant();

  public void Break() {
    garden.RemoveUnit(this);
  }

  public override UnitSave GetUnitSave() {
    UnitSave save = new SeedSave(this);
    return save;
  }

  public override void SetFromSave(UnitSave save) {
    SeedSave s = (SeedSave)save;
    timeActive = s.timeActive;
  }
}