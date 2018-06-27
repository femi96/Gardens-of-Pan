using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockberryPlant : Plant {
  // Blockberry plant

  public GameObject produce;
  // public Transform producePoint;

  private float dieTime = 0f;
  private float growTime = 0f;
  private float produceTime = 0f;

  // Unit functions
  public override string GetName() {
    return "Blockberry Bush";
  }

  public override float GetSize() {
    return 1f;
  }

  public override float GetWandRadius() {
    return 0.3f;
  }

  // Plant functions
  public override void PlantBehavior() {
    BlockType surfaceType = board.GetBlock(transform.position).GetBlockType();
    bool validSurface = surfaceType == BlockType.Dirt;

    // If not grown, grow
    if (!grown) {
      if (validSurface) {
        growTime += Time.deltaTime;
        dieTime = 0;
      } else {
        dieTime += Time.deltaTime;
        // Should look bad based on surface type
      }

      // Grow based on growTime
    }

    // If grown, create produce
    if (grown) {
      // Create produce
      produceTime += Time.deltaTime;

      if (produceTime > 20f) {
        produceTime -= 20f;
        garden.TryAddProduce(produce);
      }
    }

    // If to old, break
    if (timeActive > 600f) {
      dieTime += Time.deltaTime;
      // Should look old now
    }

    if (dieTime > 30f)
      Die();
  }
}