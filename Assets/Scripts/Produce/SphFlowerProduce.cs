using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphFlowerProduce : Produce {
  // Sph flower produce

  // Unit functions
  public override string GetName() {
    return "Sph Flower";
  }

  public override float GetSize() {
    return 0.3f;
  }

  public override float GetWandRadius() {
    return 0.1f;
  }

  public override float GetHoverHeight() {
    return 0.1f;
  }

  // Produce functions
  public override void ProduceBehavior() {

    if (timeActive > 30f)
      Break();
  }
}