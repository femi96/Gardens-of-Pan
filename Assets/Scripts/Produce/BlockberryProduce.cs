using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockberryProduce : Produce {
  // Blockberry produce

  public bool onVine = true;

  // Unit functions
  public override string GetName() {
    return "Blockberry Seed";
  }

  public override float GetSize() {
    return 0.2f;
  }

  public override float GetWandRadius() {
    return 0.3f;
  }

  // Produce functions
  public override void ProduceBehavior() {

    if (onVine) {
      held = true;

      if (timeActive > 5f)
        Drop();
    }

    if (!onVine) {
      if (timeActive > 90f)
        Break();
    }
  }

  private void Drop() {
    onVine = false;
    held = false;
  }
}