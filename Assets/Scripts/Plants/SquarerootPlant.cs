using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquarerootPlant : Plant {
  // Squareroot plant

  public GameObject produce;

  [Header("Plant Parts")]
  public GameObject root;

  private int growthStage = 0;

  private float dieTime = 0f;
  private float growTime = 0f;

  // Unit functions
  public override string GetName() {
    return "Squareroot";
  }

  public override float GetSize() {
    return 0.3f;
  }

  public override float GetWandRadius() {
    return 0.15f;
  }

  // Plant functions
  public override void PlantAwake() {
    root.SetActive(false);
  }

  public override void PlantBehavior() {

    // If not grown, grow
    if (!grown)
      NotGrown();

    // If grown, create produce
    if (grown) {
      if (garden.TryAddUnit(produce, transform.position + 0.1f * Vector3.up + 0.02f * Random.onUnitSphere, root.transform.rotation))
        Die();
    }

    if (dieTime > 5f + growTime * 0.1f)
      Die();
  }

  public override float PlantRadius() {
    return growTime * 0.2f / 15f;
  }

  // Plant Behavior when not grown
  private void NotGrown() {
    BlockType surfaceType = board.GetBlock(transform.position).GetBlockType();
    bool validSurface = BlockTypes.InGroup(surfaceType, BlockTypes.DepthGround);
    bool toClose = IsToClose();

    if (validSurface && !toClose) {
      growTime += Time.deltaTime;
      dieTime = 0;
    } else {
      dieTime += Time.deltaTime;
      // Should change color based on surface type
    }

    // Grow based on growTime
    if (growthStage == 0) {
      if (growTime >= 0f) {
        root.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        root.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        root.SetActive(true);
        growthStage = 1;
      }
    }

    if (growthStage == 1) {
      float rootSize = Mathf.Lerp(0.1f, 1f, (growTime - 0f) / 15f);
      root.transform.localScale = new Vector3(rootSize, rootSize, rootSize);

      if (growTime >= 20f) {
        grown = true;
      }
    }
  }
}
