using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockberryPlant : Plant {
  // Blockberry plant

  public GameObject produce;

  [Header("Plant Parts")]
  public GameObject trunk;
  public GameObject[] branch;
  public GameObject[] bush;

  public Transform[] producePoints;

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
  public override void PlantAwake() {
    trunk.SetActive(false);
    branch[0].SetActive(false);
    branch[1].SetActive(false);
    bush[0].SetActive(false);
    bush[1].SetActive(false);
  }

  public override void PlantBehavior() {

    // If not grown, grow
    if (!grown)
      NotGrown();

    // If grown, create produce
    if (grown)
      Grown();

    // If to old, break
    if (timeActive > 600f) {
      dieTime += Time.deltaTime;
      // Should look old now
    }

    if (dieTime > 5f + growTime * 0.1f)
      Die();
  }

  public override float PlantRadius() {
    if (grown)
      return 0.5f;

    return growTime * 0.5f / 30f;
  }

  // Plant Behavior when fully grown
  private void Grown() {

    // Create produce
    produceTime += Time.deltaTime;

    if (produceTime > 10f) {
      Transform point = producePoints[Random.Range(0, producePoints.Length)];

      if (garden.TryAddUnit(produce, point.position, point.rotation))
        produceTime -= 10f;
      else {
        produceTime -= 10f;
      }
    }
  }

  // Plant Behavior when not grown
  private void NotGrown() {
    BlockType surfaceType = board.GetBlock(transform.position).GetBlockType();
    bool validSurface = surfaceType == BlockType.Dirt;

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

    if (validSurface && !toClose) {
      growTime += Time.deltaTime;
      dieTime = 0;
    } else {
      dieTime += Time.deltaTime;
      // Should look bad based on surface type
    }

    // Grow based on growTime
    if (growthStage == 0) {
      if (growTime >= 0f) {
        // trunk.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
        trunk.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        trunk.SetActive(true);
        growthStage = 1;
      }
    }

    if (growthStage == 1) {
      float trunkSize = Mathf.Lerp(0.1f, 1f, (growTime - 0f) / 10f);
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