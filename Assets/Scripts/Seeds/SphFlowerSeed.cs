using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphFlowerSeed : Seed {
  // Sph flower seed

  public GameObject plant;

  // Unit functions
  public override string GetName() {
    return "Sph Seed";
  }

  public override float GetSize() {
    return 0.1f;
  }

  public override float GetWandRadius() {
    return 0.1f;
  }

  public override float GetHoverHeight() {
    return 0.1f;
  }

  // Seed functions
  public override void SeedBehavior() {
    BlockType surfaceType = board.GetBlock(transform.position).GetBlockType();
    bool validSurface = BlockTypes.InGroup(surfaceType, BlockTypes.DepthGround);

    // If can plant, plant
    if (timeActive > 5f && validSurface)
      Plant();

    // If to old, break
    if (timeActive > 45f)
      Break();
  }

  public override void Plant() {
    bool planted = garden.TryAddUnit(plant, transform.position, Quaternion.identity);

    if (planted)
      Break();
  }
}