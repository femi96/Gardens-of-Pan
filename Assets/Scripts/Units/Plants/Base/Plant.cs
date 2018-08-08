using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Plant : Unit {
  // Game controller that handles a plant's state and behavior

  [Header("Plant Fields")]
  public bool grown;
  public int growthStage = 0;

  public float timeActive = 0f;
  public float dieTime = 0f;
  public float growTime = 0f;
  public float produceTime = 0f;

  public override void Awake() {
    base.Awake();
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

  public virtual void Die() {
    garden.RemoveUnit(this);
  }

  public bool IsToClose() {

    bool toClose = false;
    List<Unit> plants = garden.GetUnitListOfType(typeof(Plant));

    foreach (Unit u in plants) {
      Plant p = (Plant)u;

      if (p == this)
        continue;

      float pDist = (p.transform.position - transform.position).magnitude;

      if (pDist < PlantRadius() || pDist < p.PlantRadius()) {
        toClose = true;
        break;
      }
    }

    return toClose;
  }

  // ====================
  // SAVING/LOADING plant
  // ====================

  public override UnitSave GetUnitSave() {
    PlantSave save = new PlantSave(this);
    SetPlantSave(save);
    return save;
  }

  public override void SetFromSave(UnitSave save) {
    PlantSave p = (PlantSave)save;
    grown = p.grown;
    growthStage = p.growthStage;
    timeActive = p.timeActive;
    dieTime = p.dieTime;
    growTime = p.growTime;
  }

  public virtual void SetPlantSave(PlantSave save) {}
}