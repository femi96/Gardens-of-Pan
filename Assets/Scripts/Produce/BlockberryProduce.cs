using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockberryProduce : Produce {
  // Blockberry produce

  // Unit functions
  public override string GetName() {
    return "Blockberry Seed";
  }

  public override float GetSize() {
    return 0.2f;
  }

  public override float GetWandRadius() {
    return 0.1f;
  }

  // Produce functions
  public override void ProduceBehavior() {

    if (timeActive > 90f)
      Break();
  }
}