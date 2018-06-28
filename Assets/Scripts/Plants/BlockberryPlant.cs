using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockberryPlant : Plant {
  // Blockberry plant

  public GameObject produce;
  // public Transform producePoint;

  [Header("Plant Parts")]
  public GameObject trunk;
  public GameObject[] branch;
  public GameObject[] bush;

  private int growthStage = 0;

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

    // If not grown, grow
    if (!grown) {
      NotGrown();
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

  // Plant Behavior when not grown
  private void NotGrown() {
    BlockType surfaceType = board.GetBlock(transform.position).GetBlockType();
    bool validSurface = surfaceType == BlockType.Dirt;

    if (validSurface) {
      growTime += Time.deltaTime;
      dieTime = 0;
    } else {
      dieTime += Time.deltaTime;
      // Should look bad based on surface type
    }

    // Grow based on growTime
    if (growthStage == 0) {
      if (growTime >= 1f) {
        // trunk.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
        trunk.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        trunk.SetActive(true);
        growthStage = 1;
      }
    }

    if (growthStage == 1) {
      float trunkSize = Mathf.Lerp(0.1f, 1f, (growTime - 1f) / 9f);
      trunk.transform.localScale = new Vector3(trunkSize, trunkSize, trunkSize);

      if (growTime >= 10f) {
        branch[0].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        branch[1].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        branch[0].SetActive(true);
        branch[1].SetActive(true);
        growthStage = 2;
      }
    }

    if (growthStage == 2) {
      float branchSize = Mathf.Lerp(0.1f, 1f, (growTime - 10f) / 10f);
      branch[0].transform.localScale = new Vector3(branchSize, branchSize, branchSize);
      branch[1].transform.localScale = new Vector3(branchSize, branchSize, branchSize);

      if (growTime >= 20f) {
        bush[0].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        bush[1].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        bush[0].SetActive(true);
        bush[1].SetActive(true);
        growthStage = 3;
      }
    }

    if (growthStage == 3) {
      float bushSize = Mathf.Lerp(0.1f, 1f, (growTime - 20f) / 10f);
      bush[0].transform.localScale = new Vector3(bushSize, bushSize, bushSize);
      bush[1].transform.localScale = new Vector3(bushSize, bushSize, bushSize);

      if (growTime >= 30f) {
        grown = true;
      }
    }
  }
}