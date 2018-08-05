using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Produce : Unit {
  // Game controller that handles a produce's state and behavior

  [Header("Produce Fields")]
  public EntityMover entityMover;

  public bool held = false;

  public float timeActive = 0f;

  public override void Awake() {
    base.Awake();
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

  public override UnitSave GetUnitSave() {
    UnitSave save = new ProduceSave(this);
    return save;
  }

  public override void SetFromSave(UnitSave save) {
    ProduceSave p = (ProduceSave)save;
    held = p.held;
    timeActive = p.timeActive;
  }
}
